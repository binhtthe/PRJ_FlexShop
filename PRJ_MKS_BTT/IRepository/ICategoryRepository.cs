using PRJ_MKS_BTT.Model;

namespace PRJ_MKS_BTT.IRepository
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> CreateCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(Category categoryId);
        public Task<bool> HasChildrenAsync(int categoryId);
        public Task<bool> HasProductsAsync(int categoryId);
        Task<List<Category>> GetCategoriesByParentIdAsync(int parentId);
        Task<List<Category>> GetRootCategoriesAsync();
        Task<bool> ExistsByNameInParentAsync(string name, int? parentId);
        Task<bool> ExistsByNameIdInParentAsync(string name, int? parentId, int categoryId);


    }
}
