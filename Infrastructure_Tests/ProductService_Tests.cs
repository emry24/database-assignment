using Infrastructure.Contexts;
using Infrastructure.Repositories.ProductRepositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Dtos;
using Infrastructure.Entities.ProductEntities;


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
            ManufactureName = "Manufacture",
            Ingress = "Ingress",
            Description = "Description",
            Specification = "Specification",
            Price = 99
        });
        // Assert
        Assert.True(result);
    }




    //[Fact]
    //public async Task GetProductByArticleNrAsync_ProductExists_ReturnsProductDto()
    //{
    //    // Arrange
    //    string articleNumber = "12345";
    //    var expectedProductDto = new ProductDto
    //    {
    //        ProductTitle = "Title",
    //        ArticleNumber = "12345",
    //        ManufactureName = "Manufacture",
    //        Ingress = "Ingress",
    //        Description = "Description",
    //        Price = 100.00m,
    //        Specification = "Specification",
    //        CategoryName = "Category"
    //    };

    //    _dbContext.Products.Add(new Product
    //    {
    //        ArticleNumber = "12345",
    //        ProductInformation = new ProductInformation
    //        {
    //            ProductTitle = "Title",
    //            Ingress = "Ingress",
    //            Description = "Description",
    //            Specification = "Specification"
    //        },
    //        Manufacture = new Manufacture
    //        {
    //            ManufactureName = "Manufacture"
    //        },
    //        ProductPrice = new ProductPrice
    //        {
    //            Price = 100.00m
    //        },
    //        Category = new Category
    //        {
    //            CategoryName = "Category"
    //        }
    //    });
    //    await _dbContext.SaveChangesAsync();

    //    // Act
    //    var result = await _productService.GetProductByArticleNrAsync(articleNumber);

    //    // Assert
    //    Assert.IsNotNull(result);
    //    Assert.AreEqual(expectedProductDto.ProductTitle, result.ProductTitle);
    //    Assert.AreEqual(expectedProductDto.ArticleNumber, result.ArticleNumber);
    //    Assert.AreEqual(expectedProductDto.ManufactureName, result.ManufactureName);
    //    Assert.AreEqual(expectedProductDto.Ingress, result.Ingress);
    //    Assert.AreEqual(expectedProductDto.Description, result.Description);
    //    Assert.AreEqual(expectedProductDto.Price, result.Price);
    //    Assert.AreEqual(expectedProductDto.Specification, result.Specification);
    //    Assert.AreEqual(expectedProductDto.CategoryName, result.CategoryName);
    //}


}
