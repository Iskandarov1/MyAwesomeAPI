using Microsoft.AspNetCore.Mvc;
using certeficates.Interfaces;
using certeficates.DTOs;


namespace certeficates.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
         private readonly ILogger<ProductsController> _logger; // For logging

        public ProductsController(IProductService productService , ILogger<ProductsController> logger)
        {
            _productService = productService;
             _logger = logger;
        }

        /// <summary>
        /// Get all products
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetAllProducts()
        {
            _logger.LogInformation("Fetching all products from the database.");

            var products = await _productService.GetAllProductsAsync();

            if(!products.Any())
            {
                _logger.LogWarning("No products found in the database.");
                return NotFound();
            }   


            var productDtos = products.Select(p => new ProductReadDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CreatedAt = p.CreatedAt,
                ModifiedAt = p.ModifiedAt,
                Status = p.Status
            });
            
            _logger.LogInformation("{ProductCount} products retrieved from the database.", products.Count());
            return Ok(productDtos);
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReadDto>> GetProductById(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            _logger.LogInformation("Fetching product with ID: {ProductId} from the database.", id);
            if (product == null){
                _logger.LogWarning("Product with ID: {ProductId} was not found in the database.", id);
                return NotFound();
            } 

            var productDto = new ProductReadDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CreatedAt = product.CreatedAt,
                ModifiedAt = product.ModifiedAt,
                Status = product.Status
            };
            _logger.LogInformation("Product with ID: {ProductId} was found in the database.", id);
            return Ok(productDto);
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductReadDto>> CreateProduct( ProductCreateDto dto)
        {
            _logger.LogInformation("Creating a new product in the database.{ProductName}", dto.Name);
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Status = dto.Status
            };

            var createdProduct = await _productService.CreateProductAsync(product);
            _logger.LogInformation("Product with ID: {ProductId} was created in the database.", createdProduct.Id);

            var productDto = new ProductReadDto
            {
                Id = createdProduct.Id,
                Name = createdProduct.Name,
                Price = createdProduct.Price,
                CreatedAt = createdProduct.CreatedAt,
                ModifiedAt = createdProduct.ModifiedAt,
                Status = createdProduct.Status
            };
            
            return CreatedAtAction(nameof(GetProductById), new { id = createdProduct.Id }, productDto);
        }

        /// <summary>
        /// Update a product
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ProductReadDto>> UpdateProduct(Guid id, [FromBody] ProductUpdateDto dto)
        {
            _logger.LogInformation("Updating product with ID: {ProductId} in the database.", id);
            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                Status = (EProductStatus)dto.Status
            };

            var updatedProduct = await _productService.UpdateProductAsync(id, product);
            
            if (updatedProduct == null){ 
                _logger.LogWarning("Product with ID: {ProductId} was not found in the database.", id);
                 return NotFound();}

            var productDto = new ProductReadDto
            {
                Id = updatedProduct.Id,
                Name = updatedProduct.Name,
                Price = updatedProduct.Price,
                CreatedAt = updatedProduct.CreatedAt,
                ModifiedAt = updatedProduct.ModifiedAt,
                Status =updatedProduct.Status
            };
            _logger.LogInformation("Product with ID: {ProductId} was updated in the database.", id);

            return Ok(productDto);
        }

        /// <summary>
        /// Delete a product
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(Guid id)
        {
            _logger.LogInformation("Deleting product with ID: {ProductId} from the database.", id);
            var deleted = await _productService.DeleteProductAsync(id);
            if (!deleted) { 
                _logger.LogWarning("Product with ID: {ProductId} was not found in the database.", id);
             return NotFound();
            }
            _logger.LogInformation("Product with ID: {ProductId} was deleted from the database.", id);
            return NoContent();
        }
    }
}
