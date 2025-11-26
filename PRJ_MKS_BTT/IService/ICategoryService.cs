using PRJ_MKS_BTT.Request;
using PRJ_MKS_BTT.Response;

namespace PRJ_MKS_BTT.IService
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> GetCategoriesByParentIdAsync(int parentId);
        Task<List<CategoryResponse>> GetRootCategoriesAsync();
        Task<CategoryResponse> GetCategoryByIdAsync(int categoryId);
        Task<CategoryResponse> CreateCategory(CategoryRequest request);
        Task<CategoryResponse> UpdateCategory(int categoryId, CategoryRequest request);
        Task<bool> DeleteCategoryAsync(int categoryId);

    }
}
