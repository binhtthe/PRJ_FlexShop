using Microsoft.EntityFrameworkCore;
using PRJ_MKS_BTT.Data;
using PRJ_MKS_BTT.IRepository;
using PRJ_MKS_BTT.Model;

namespace PRJ_MKS_BTT.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
            Console.WriteLine("🔍 CONNECTED DB = " + _context.Database.GetDbConnection().Database);
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
           category.CreatedAt = DateTime.UtcNow;
            category.UpdatedAt = DateTime.UtcNow;
            _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return category;

        }

        public async Task<bool> DeleteCategoryAsync(Category categoryId)
        {
           _context.Categories.Remove(categoryId);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        }



        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var CategoryToUD  = await _context.Categories.FirstOrDefaultAsync(c => c.Id == category.Id);
          if (CategoryToUD == null)
            {
                return null;
            }
            CategoryToUD.Name = category.Name;
            CategoryToUD.ParentId = category.ParentId;
            CategoryToUD.Slug = category.Slug;
            CategoryToUD.IconUrl = category.IconUrl;
            CategoryToUD.IsActive = category.IsActive;
            CategoryToUD.UpdatedAt = DateTime.UtcNow;
            _context.Categories.Update(CategoryToUD);
            await _context.SaveChangesAsync();
            return CategoryToUD;

        }
        public async Task<List<Category>> GetRootCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.ParentId == null)
                .ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesByParentIdAsync(int parentId)
        {
            return await _context.Categories
               .Where(c => c.ParentId == parentId)
               .ToListAsync();
        }

        public async Task<bool> ExistsByNameInParentAsync(string name, int? parentId)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name == name && c.ParentId == parentId);
        }
         public async Task<bool> ExistsByNameIdInParentAsync(string name, int? parentId, int categoryId)
        {
            return await _context.Categories
                .AnyAsync(c => c.Name == name && c.ParentId == parentId && c.Id != categoryId );
        }

        public async Task<bool> HasChildrenAsync(int categoryId)
        {
          return await _context.Categories
                .AnyAsync(c => c.ParentId == categoryId);
        }

        public async Task<bool> HasProductsAsync(int categoryId)
        {
            return await _context.Products
                .AnyAsync(p => p.CategoryId == categoryId);
        }
    }
}
