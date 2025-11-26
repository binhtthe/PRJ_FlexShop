using PRJ_MKS_BTT.Model;

namespace PRJ_MKS_BTT.IRepository
{
    public interface IProductRepository
    {
        Task AddProductAsync(Product product);
        Task AddRangeSkusAsync(IEnumerable<Sku> skus);
        Task AddRangeImagesAsync(IEnumerable<ProductImage> images);
        Task<bool> ExistsSkuCodeAsync(string skuCode);
        Task UpdateProductAsync(Product product);
        Task<Product> GetProductByIdAsync(int productId);
        Task<Product> GetProductByNameAsync(string productName);
        Task<IEnumerable<Sku>> GetSkusByProductIdAsync(int productId);
        Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId);
        Task UpdateRangeSkusAsync(IEnumerable<Sku> skus);
        Task UpdateRangeImagesAsync(IEnumerable<ProductImage> images);
    }
}
