using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRJ_MKS_BTT.IRepository;
using PRJ_MKS_BTT.IService;

namespace PRJ_MKS_BTT.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        // GET /api/categories?parentId=1
        [HttpGet]
        public async Task<IActionResult> GetCategories([FromQuery] int? parentId)
        {
            if (parentId == null)
            {
                // ROOT categories
                return Ok(await _categoryService.GetRootCategoriesAsync());
            }

            // Sub categories
            return Ok(await _categoryService.GetCategoriesByParentIdAsync(parentId.Value));
        }


        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

    }
}