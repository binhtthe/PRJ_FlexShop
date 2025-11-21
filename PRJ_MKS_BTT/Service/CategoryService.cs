using PRJ_MKS_BTT.IRepository;
using PRJ_MKS_BTT.IService;
using PRJ_MKS_BTT.Model;
using PRJ_MKS_BTT.Request;
using PRJ_MKS_BTT.Response;

namespace PRJ_MKS_BTT.Service
{
    public class CategoryService : ICategoryService

    {
     
        private readonly ICategoryRepository _categoryRepository;
        private readonly IConfiguration _configuration;

        public CategoryService(ICategoryRepository categoryRepository, IConfiguration configuration)
        {
            _categoryRepository = categoryRepository;
            _configuration = configuration;
           
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest request)
        {
            try
            {
                // 1. Validate name
                if (string.IsNullOrWhiteSpace(request.Name))
                    throw new ArgumentException("Name is required");

                // 2. Validate parentId (nếu có)
                if (request.ParentId.HasValue)
                {
                    var parent = await _categoryRepository.GetCategoryByIdAsync(request.ParentId.Value);
                    if (parent == null)
                        throw new ArgumentException("Parent category does not exist");
                }

                // 3. Kiểm tra name trùng trong cùng parentId
                bool isDuplicate = await _categoryRepository.ExistsByNameInParentAsync(
                    request.Name,
                    request.ParentId
                );

                if (isDuplicate)
                    throw new ArgumentException("Category name already exists in this level");

                // 4. Slug tự tạo nếu không truyền
                string slug = !string.IsNullOrWhiteSpace(request.Slug)
                    ? request.Slug
                    : request.Name.ToLower().Replace(" ", "-");

                // 5. Tạo entity
                var newCategory = new Category
                {
                    Name = request.Name,
                    ParentId = request.ParentId,
                    Slug = slug,
                    IconUrl = request.IconUrl,
                    IsActive = request.IsActive,
                    CreatedAt = DateTime.UtcNow
                };

                // 6. Lưu vào DB
                var createdCategory = await _categoryRepository.CreateCategoryAsync(newCategory);

                // 7. Trả response
                return new CategoryResponse
                {
                    Id = createdCategory.Id,
                    Name = createdCategory.Name,
                    ParentId = createdCategory.ParentId,
                    Slug = createdCategory.Slug,
                    IconUrl = createdCategory.IconUrl,
                    IsActive = createdCategory.IsActive
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<CategoryResponse>> GetCategoriesByParentIdAsync(int parentId)
        {
            try
            {
                var list = await _categoryRepository.GetCategoriesByParentIdAsync(parentId);
             
                return list.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    ParentId = c.ParentId,
                    Name = c.Name,
                }).ToList();


            }
            catch (Exception ex)
            {
                return new List<CategoryResponse>();
            }
        }

        public async Task<CategoryResponse> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var category =  await _categoryRepository.GetCategoryByIdAsync(categoryId);
                if (category == null)
                {
                    return null;
                }
                return new CategoryResponse
                {
                    Id = category.Id,
                    Name = category.Name,
                    ParentId = category.ParentId,
                    Slug = category.Slug,
                    IconUrl = category.IconUrl,
                    IsActive = category.IsActive
                };
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<CategoryResponse>> GetRootCategoriesAsync()
        {
            try
            {
                var list = await _categoryRepository.GetRootCategoriesAsync();

                return list.Select(c => new CategoryResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    ParentId = c.ParentId
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<CategoryResponse>();
            }
        }

        public Task<CategoryResponse> UpdateCategory(int categoryId, CategoryRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
