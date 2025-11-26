using PRJ_MKS_BTT.Request;
using PRJ_MKS_BTT.Response;

namespace PRJ_MKS_BTT.IService
{
    // IService/IProductService.cs
    public interface IProductService
    {
        Task<ProductResponse> CreateProductAsync(int sellerId, ProductRequest request);
        Task<ProductResponse> UpdateProductAsync(int productId, ProductRequest request);
    }

}
