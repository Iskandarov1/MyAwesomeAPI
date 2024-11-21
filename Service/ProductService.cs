using certeficates.Interfaces;
using Microsoft.EntityFrameworkCore;

//using ProductCrudApi.Data;
//using ProductCrudApi.Entities;
//using ProductCrudApi.Interfaces;

namespace certeficates.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(AppDbContext dbContext, ILogger<ProductService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public ProductService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

         public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        _logger.LogInformation("Fetching all products from the database.");
        var products = await _dbContext.Products.Include(p => p.ProductDetail).ToListAsync();
        _logger.LogInformation("{ProductCount} products retrieved from the database.", products.Count);
        return products;
    }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching product with ID: {ProductId} from the database.", id);
            var product = await _dbContext.Products
            .Include(p => p.ProductDetail)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                _logger.LogWarning("Product with ID: {ProductId} was not found in the database.", id);
            }
            else
            {
                _logger.LogInformation("Product with ID: {ProductId} was found in the database.", id);
            }
         
            return product;
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _logger.LogInformation("Creating a new product in the database.{ProductName}", product.Name);
            product.Id = Guid.NewGuid();
            product.CreatedAt = DateTime.UtcNow;
            product.ModifiedAt = DateTime.UtcNow;


            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Product with ID: {ProductId} was created in the database.", product.Id);

            return product;
        }

        public async Task<Product> UpdateProductAsync(Guid id, Product product)
        {
            _logger .LogInformation("Updating product with ID: {ProductId} in the database.", id);
            var existingProduct = await _dbContext.Products.FindAsync(id);
            if (existingProduct == null) return null;
            _logger.LogInformation("Product with ID: {ProductId} was found in the database.", id);

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.Status = product.Status;
            existingProduct.ModifiedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();
            _logger.LogInformation("Product with ID: {ProductId} was updated in the database.", id);
            return existingProduct;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            _logger.LogInformation("Deleting product with ID: {ProductId} from the database.", id);
            var product = await _dbContext.Products.FindAsync(id);
            if (product == null) return false;
            _logger.LogInformation("Product with ID: {ProductId} was found in the database.", id);

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();
            
            _logger.LogInformation("Product with ID: {ProductId} was deleted from the database.", id);
            return true;
        }
    }
}
