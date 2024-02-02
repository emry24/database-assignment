namespace Infrastructure.Dtos
{
    public class ProductDto
    {
        public string ArticleNumber { get; set; } = null!;
        public string ProductTitle { get; set; } = null!;
        public string? Ingress { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? Specification { get; set; }
        public string CategoryName { get; set; } = null!;
        public string ManufactureName { get; set; } = null!;
    }
}
