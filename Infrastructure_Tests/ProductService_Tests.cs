using Infrastructure.Contexts;
using Infrastructure.Repositories.ProductRepositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Dtos;

namespace Infrastructure_Tests;

public class ProductService_Tests
{
    private readonly ProductDataContext _context = new(new DbContextOptionsBuilder<ProductDataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);


    [Fact]
    public async Task CreateOneProduct_ReturnTrue()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var categoryRepository = new CategoryRepository(_context);
        var manufactureRepository = new ManufactureRepository(_context);
        var productInfoRepository = new ProductInformationRepository(_context);
        var productPriceRepository = new ProductPriceRepository(_context);
        var productService = new ProductService(productRepository, categoryRepository, manufactureRepository, productInfoRepository, productPriceRepository);
        // Act
        var result = await productService.CreateProductAsync(new ProductDto
        {
            ArticleNumber = "1234",
            ProductTitle = "Title",
            CategoryName = "Category",
            ManufactureName = "Name",
            Ingress = "Ingress",
            Description = "desc",
            Specification = "spec",
            Price = 99
        });
        // Assert
        Assert.True(result);
    }
}
