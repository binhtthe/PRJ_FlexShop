using PRJ_MKS_BTT.Data;
using PRJ_MKS_BTT.IRepository;
using PRJ_MKS_BTT.Model;
using Microsoft.EntityFrameworkCore;

namespace PRJ_MKS_BTT.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddProductAsync(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeSkusAsync(IEnumerable<Sku> skus)
        {
            var list = skus.ToList();

            await _context.Skus.AddRangeAsync(list);
            await _context.SaveChangesAsync();
        }


        public async Task AddRangeImagesAsync(IEnumerable<ProductImage> images)
        {
            var list = images.ToList();
            await _context.ProductImages.AddRangeAsync(list);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsSkuCodeAsync(string skuCode)
        {
            return await _context.Skus.AnyAsync(s => s.SkuCode == skuCode);
        }

        public async Task UpdateProductAsync(Product product)
        {
           product.UpdatedAt = DateTime.UtcNow;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<Product> GetProductByNameAsync(string productName)
        {
           return await _context.Products.FirstOrDefaultAsync(p => p.Title == productName);
        }

        public async Task<IEnumerable<Sku>> GetSkusByProductIdAsync(int productId)
        {
           return await _context.Skus.Where(s => s.ProductId == productId).ToListAsync();
        }

        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
           return await _context.ProductImages.Where(i => i.ProductId == productId).ToListAsync();
        }

        public async Task UpdateRangeSkusAsync(IEnumerable<Sku> skus)
        {
            _context.Skus.UpdateRange(skus);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateRangeImagesAsync(IEnumerable<ProductImage> images)
        {
          _context.ProductImages.UpdateRange(images);
            await _context.SaveChangesAsync();
        }
    }
}