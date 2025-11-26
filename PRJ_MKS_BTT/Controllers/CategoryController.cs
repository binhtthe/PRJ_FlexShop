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

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] PRJ_MKS_BTT.Request.CategoryRequest request)
        {
            try
            {
                var createdCategory = await _categoryService.CreateCategory(request);
                if (createdCategory == null)
                {
                    return BadRequest(new { message = "Failed to create category" });
                }

                return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating category");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating category");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
            }
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> UpdateCategory([FromRoute] int id, [FromBody] PRJ_MKS_BTT.Request.CategoryRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { message = "Invalid body" });
                }

                var updatedCategory = await _categoryService.UpdateCategory(id, request);
                if (updatedCategory == null)
                {
                    return NotFound(new { message = "Category not found" });
                }
                return Ok(updatedCategory);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while updating category");
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while updating category");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
            }
        }

    }
}