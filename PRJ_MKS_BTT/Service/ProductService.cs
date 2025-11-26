using Microsoft.EntityFrameworkCore;
using PRJ_MKS_BTT.IRepository;
using PRJ_MKS_BTT.IService;
using PRJ_MKS_BTT.Model;
using PRJ_MKS_BTT.Request;
using PRJ_MKS_BTT.Response;
using System.Text.Json;


namespace PRJ_MKS_BTT.Service
{
    // Services/ProductService.cs
   
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepo;
        private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository productRepo,
            ICategoryRepository categoryRepo,
            IUnitOfWork uow,
            ILogger<ProductService> logger)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _uow = uow;
            _logger = logger;
        }

        public async Task<ProductResponse> CreateProductAsync(int sellerId, ProductRequest request)
        {
            // 1. Validate request (throw ArgumentException on invalid)
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new ArgumentException("Title is required");

            if (request.CategoryId <= 0)
                throw new ArgumentException("Invalid categoryId");

            if (request.Weight < 0)
                throw new ArgumentException("Weight must be >= 0");

            if (request.Skus == null || !request.Skus.Any())
                throw new ArgumentException("At least one SKU is required");

            foreach (var s in request.Skus)
            {
                if (s.Price <= 0) throw new ArgumentException("SKU price must be > 0");
                if (s.Stock < 0) throw new ArgumentException("SKU stock must be >= 0");
                // attributes presence
                if (s.Attributes == null || !s.Attributes.Any())
                    throw new ArgumentException("SKU attributes are required");
            }

            // 2. Check category exists & active
            var category = await _categoryRepo.GetCategoryByIdAsync(request.CategoryId);
            if (category == null || !category.IsActive)
                throw new ArgumentException("Category not found or inactive");

            // 3. Start transaction via unit of work / DbContext
            await using var tx = await _uow.BeginTransactionAsync();

            try
            {
                // 4. Create product entity
                var productEntity = new Product
                {
                    SellerId = sellerId,
                    CategoryId = request.CategoryId,
                    Title = request.Title,
                    Description = request.Description,
                    Brand = request.Brand,
                    Weight = request.Weight,
                    IsFeatured = request.IsFeatured,
                    IsFreeShipping = request.IsFreeShipping,
                    Status = "Active",
                    ViewCount = 0,
                    SoldCount = 0,
                    Rating = 0,
                    ReviewCount = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _productRepo.AddProductAsync(productEntity);
                await _uow.SaveChangesAsync(); // to get ProductId

                // 5. Insert SKUs
                var skuEntities = new List<Sku>();
                int skuIndex = 1;
                foreach (var s in request.Skus)
                {
                    var skuCode = !string.IsNullOrWhiteSpace(s.SkuCode)
                        ? s.SkuCode
                        : GenerateSkuCode(productEntity.ProductId, skuIndex++);

                    // ensure uniqueness of skuCode (simple check)
                    var exists = await _productRepo.ExistsSkuCodeAsync(skuCode);
                    if (exists)
                    {
                        // if generated collides (rare), regenerate a random one
                        skuCode = GenerateSkuCode(productEntity.ProductId, skuIndex++) + "-" + RandomString(4);
                    }

                    var skuEntity = new Sku
                    {
                        ProductId = productEntity.ProductId,
                        SkuCode = skuCode,
                        Price = s.Price,
                        CompareAtPrice = s.CompareAtPrice ?? 0m,
                        Stock = s.Stock,
                        AttributesJson = JsonSerializer.Serialize(s.Attributes),
                        ImageUrl = s.ImageUrl,
                        IsActive = true
                    };

                    skuEntities.Add(skuEntity);
                }

                await _productRepo.AddRangeSkusAsync(skuEntities);
                await _uow.SaveChangesAsync();

                // 6. Insert product images
                var imageEntities = new List<ProductImage>();
                foreach (var img in request.Images ?? Enumerable.Empty<ImageRequest>())
                {
                    imageEntities.Add(new ProductImage
                    {
                        ProductId = productEntity.ProductId,
                        Url = img.Url,
                        SortOrder = img.SortOrder
                    });
                }

                if (imageEntities.Any())
                {
                    await _productRepo.AddRangeImagesAsync(imageEntities);
                    await _uow.SaveChangesAsync();
                }

                await tx.CommitAsync();

                // 7. Prepare response (map)
                var response = new ProductResponse
                {
                    ProductId = productEntity.ProductId,
                    SellerId = productEntity.SellerId,
                    CategoryId = productEntity.CategoryId,
                    Title = productEntity.Title,
                    Description = productEntity.Description,
                    Brand = productEntity.Brand,
                    Weight = productEntity.Weight,
                    IsFeatured = productEntity.IsFeatured,
                    IsFreeShipping = productEntity.IsFreeShipping,
                    CreatedAt = productEntity.CreatedAt,
                    Images = imageEntities.Select(i => new ImageResponse { Id = i.Id, Url = i.Url, SortOrder = i.SortOrder }).ToList(),
                    Skus = skuEntities.Select(k => new SkuResponse
                    {
                        SkuId = k.SkuId,
                        SkuCode = k.SkuCode,
                        Price = k.Price,
                        CompareAtPrice = k.CompareAtPrice,
                        Stock = k.Stock,
                        ImageUrl = k.ImageUrl,
                        Attributes = JsonSerializer.Deserialize<Dictionary<string, string>>(k.AttributesJson ?? "{}")
                    }).ToList()
                };

                return response;
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        // helpers
        private string GenerateSkuCode(int productId, int index)
        {
            return $"P{productId}-{index:D3}";
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var rng = new Random();
            return new string(Enumerable.Range(0, length).Select(_ => chars[rng.Next(chars.Length)]).ToArray());
        }

        public async Task<ProductResponse> UpdateProductAsync(int productId, ProductRequest request)
        {
            // 1. Lấy sản phẩm
            var product = await _productRepo.GetProductByIdAsync(productId);
            if (product == null)
                throw new ArgumentException("Product not found");

            await using var tx = await _uow.BeginTransactionAsync();

            try
            {
                // ---------------------------
                // UPDATE PRODUCT (chỉ update những trường được gửi)
                // ---------------------------

                if (!string.IsNullOrWhiteSpace(request.Title))
                    product.Title = request.Title;

                if (request.Description != null)
                    product.Description = request.Description;

                if (request.Brand != null)
                    product.Brand = request.Brand;

                if (request.CategoryId > 0)
                {
                    var category = await _categoryRepo.GetCategoryByIdAsync(request.CategoryId);
                    if (category == null || !category.IsActive)
                        throw new ArgumentException("Category not found or inactive");

                    product.CategoryId = request.CategoryId;
                }

                if (request.Weight >= 0)
                    product.Weight = request.Weight;

                product.IsFeatured = request.IsFeatured;
                product.IsFreeShipping = request.IsFreeShipping;
                product.UpdatedAt = DateTime.UtcNow;

                await _productRepo.UpdateProductAsync(product);

                // -----------------------------------
                // UPDATE SKU (nếu request.Skus != null)
                // -----------------------------------

                var existingSkus = (await _productRepo.GetSkusByProductIdAsync(productId)).ToList();
                var updatedSkuEntities = new List<Sku>();

                if (request.Skus != null)
                {
                    foreach (var skuReq in request.Skus)
                    {
                        var exist = existingSkus.FirstOrDefault(x => x.SkuCode == skuReq.SkuCode);

                        if (exist != null)
                        {
                            // SKU cũ → update
                            exist.Price = skuReq.Price > 0 ? skuReq.Price : exist.Price;
                            exist.CompareAtPrice = skuReq.CompareAtPrice ?? exist.CompareAtPrice;
                            exist.Stock = skuReq.Stock >= 0 ? skuReq.Stock : exist.Stock;

                            if (!string.IsNullOrEmpty(skuReq.ImageUrl))
                                exist.ImageUrl = skuReq.ImageUrl;

                            if (skuReq.Attributes != null)
                                exist.AttributesJson = JsonSerializer.Serialize(skuReq.Attributes);

                            updatedSkuEntities.Add(exist);
                        }
                        else
                        {
                            // SKU mới → thêm
                            var newCode = !string.IsNullOrWhiteSpace(skuReq.SkuCode)
                                ? skuReq.SkuCode
                                : GenerateSkuCode(productId, existingSkus.Count + 1);

                            var newSku = new Sku
                            {
                                ProductId = productId,
                                SkuCode = newCode,
                                Price = skuReq.Price,
                                CompareAtPrice = skuReq.CompareAtPrice ?? 0,
                                Stock = skuReq.Stock,
                                ImageUrl = skuReq.ImageUrl,
                                AttributesJson = JsonSerializer.Serialize(skuReq.Attributes ?? new Dictionary<string, string>()),
                                IsActive = true
                            };

                            updatedSkuEntities.Add(newSku);
                        }
                    }

                    await _productRepo.UpdateRangeSkusAsync(updatedSkuEntities);
                }

                // -----------------------------------
                // UPDATE IMAGES (nếu request.Images != null)
                // -----------------------------------

                if (request.Images != null)
                {
                    var existingImages = (await _productRepo.GetImagesByProductIdAsync(productId)).ToList();

                    // Chỉ update những ảnh được gửi, ảnh nào không gửi → giữ nguyên
                    var updatedImages = new List<ProductImage>();

                    foreach (var img in request.Images)
                    {
                        var exist = existingImages.FirstOrDefault(x => x.Url == img.Url);

                        if (exist != null)
                        {
                            exist.SortOrder = img.SortOrder;
                            updatedImages.Add(exist);
                        }
                        else
                        {
                            updatedImages.Add(new ProductImage
                            {
                                ProductId = productId,
                                Url = img.Url,
                                SortOrder = img.SortOrder
                            });
                        }
                    }

                    await _productRepo.UpdateRangeImagesAsync(updatedImages);
                }

                await tx.CommitAsync();

                return new ProductResponse
                {
                    ProductId = product.ProductId,
                    SellerId = product.SellerId,
                    CategoryId = product.CategoryId,
                    Title = product.Title,
                    Description = product.Description,
                    Brand = product.Brand,
                    Weight = product.Weight,
                    IsFeatured = product.IsFeatured,
                    IsFreeShipping = product.IsFreeShipping,
                    CreatedAt = product.CreatedAt,
                    Images = (await _productRepo.GetImagesByProductIdAsync(productId))
                        .Select(i => new ImageResponse { Id = i.Id, Url = i.Url, SortOrder = i.SortOrder }).ToList(),
                    Skus = (await _productRepo.GetSkusByProductIdAsync(productId))
                        .Select(s => new SkuResponse
                        {
                            SkuId = s.SkuId,
                            SkuCode = s.SkuCode,
                            Price = s.Price,
                            CompareAtPrice = s.CompareAtPrice,
                            Stock = s.Stock,
                            ImageUrl = s.ImageUrl,
                            Attributes = JsonSerializer.Deserialize<Dictionary<string, string>>(s.AttributesJson)
                        }).ToList()
                };
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

    }

}
