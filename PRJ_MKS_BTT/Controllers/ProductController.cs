using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PRJ_MKS_BTT.IService;
using PRJ_MKS_BTT.Request;

namespace PRJ_MKS_BTT.Controllers
{
    [Route("api/seller/products")]
    [ApiController]
    [Authorize(Roles = "Seller")] // chỉ seller được truy cập
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductRequest request)
        {
            if (request == null) return BadRequest(new { message = "Invalid request body" });

            try
            {
                // Lấy sellerId từ JWT claim "UserId"
                var userIdClaim = User.FindFirst("UserId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out var sellerId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var created = await _productService.CreateProductAsync(sellerId, request);
                if (created == null)
                {
                    return BadRequest(new { message = "Failed to create product" });
                }

                // Giả sử bạn có endpoint GET /api/products/{id}
                return CreatedAtAction("GetProductById", "Products", new { id = created.ProductId }, created);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error while creating product");
                return BadRequest(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "Unauthorized create attempt");
                return Forbid();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while creating product");
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An unexpected error occurred." });
            }
        }
    }
}
