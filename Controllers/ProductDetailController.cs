using Data.Entity.ProductEntity;
using Microsoft.AspNetCore.Mvc;

namespace certeficates.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDetailController : ControllerBase
    {
        private readonly IProductDetailService _productDetailService;

        public ProductDetailController(IProductDetailService productDetailService)
        {
            _productDetailService = productDetailService;
        }

        [HttpGet("{productId}")]
        public async Task<ActionResult<ProductDetail>> GetDetailsByProductIdAsync(Guid productId)
        {
            var detail = await _productDetailService.GetDetailsByProductIdAsync(productId);
            if (detail == null)
            {
                return NotFound();
            }
            return Ok(detail);
        }

        [HttpPost("{productId}")]
        public async Task<ActionResult<ProductDetail>> CreateProductDetailAsync(Guid productId, [FromBody] ProductDetail detail)
        {
            var createdDetail = await _productDetailService.CreateProductDetailAsync(productId, detail);
            return CreatedAtAction(nameof(GetDetailsByProductIdAsync), new { productId = productId }, createdDetail);
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult<ProductDetail>> UpdateProductDetailAsync(Guid productId, [FromBody] ProductDetail detail)
        {
            var updatedDetail = await _productDetailService.UpdateProductDetailAsync(productId, detail);
            if (updatedDetail == null)
            {
                return NotFound();
            }
            return Ok(updatedDetail);
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProductDetailAsync(Guid productId)
        {
            var deleted = await _productDetailService.DeleteProductDetailAsync(productId);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
