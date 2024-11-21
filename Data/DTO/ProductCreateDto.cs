//using ProductCrudApi.Entities;

namespace certeficates.DTOs
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public EProductStatus Status { get; set; }
    }
}
