using certeficates.Data.Entity.ProductEntity;
//using ProductCrudApi.Entities;

namespace certeficates.DTOs
{
    public class ProductUpdateDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public EProductStatus Status { get; set; }
    }
}
