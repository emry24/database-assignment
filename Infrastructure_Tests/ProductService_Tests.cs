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
    public async Task CreateProductAsync_ShouldCreateOneProduct_ReturnTrue()
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

    [Fact]
    public async Task CreateProductAsync_ShouldNotCreateOneProduct_ReturnFalse()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var categoryRepository = new CategoryRepository(_context);
        var manufactureRepository = new ManufactureRepository(_context);
        var productInfoRepository = new ProductInformationRepository(_context);
        var productPriceRepository = new ProductPriceRepository(_context);
        var productService = new ProductService(productRepository, categoryRepository, manufactureRepository, productInfoRepository, productPriceRepository);

        // Act
        await productService.CreateProductAsync(new ProductDto
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
        Assert.False(result);
    }

    // Kontollera denna
    [Fact]
    public async Task GetAllProductsAsync_ShouldGetAllProducts_ReturnIEnumerableOfTypeProduct()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var productInformationRepository = new ProductInformationRepository(_context);
        var categoryRepository = new CategoryRepository(_context);
        var manufactureRepository = new ManufactureRepository(_context);
        var productService = new ProductService(productRepository, categoryRepository, manufactureRepository, productInformationRepository, null!);

        var productDto = new ProductDto
        {
            ProductTitle = "producttitle",
            CategoryName = "category",
            ManufactureName = "manufacture"
        };
        await productService.CreateProductAsync(productDto);

        var productDto2 = new ProductDto
        {
            ProductTitle = "producttitle2",
            CategoryName = "category2",
            ManufactureName = "manufacture2"
        };
        await productService.CreateProductAsync(productDto2);

        // Act
        var result = await productService.GetAllProductsAsync();

        // Assert
        Assert.NotNull(result);
        //        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetProductByArticleNrAsync_ShouldGetOneProductByArticleNr_ReturnOneProduct()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var categoryRepository = new CategoryRepository(_context);
        var manufactureRepository = new ManufactureRepository(_context);
        var productInformationRepository = new ProductInformationRepository(_context);
        var productPriceRepository = new ProductPriceRepository(_context);
        var productService = new ProductService(productRepository, categoryRepository, manufactureRepository, productInformationRepository, productPriceRepository);

        var productDto = new ProductDto
        {
            ProductTitle = "title",
            ArticleNumber = "123456",
            ManufactureName = "manufacture",
            Ingress = "ingress",
            Description = "description",
            Price = 100,
            Specification = "specification",
            CategoryName = "category",
        };
        await productService.CreateProductAsync(productDto);

        // Act
        var articleNumber = "123456";
        var result = await productService.GetProductByArticleNrAsync(articleNumber);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetProductByArticleNrAsync_ShouldNotGetOneProductByArticleNr_ReturnNull()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var categoryRepository = new CategoryRepository(_context);
        var manufactureRepository = new ManufactureRepository(_context);
        var productInformationRepository = new ProductInformationRepository(_context);
        var productPriceRepository = new ProductPriceRepository(_context);
        var productService = new ProductService(productRepository, categoryRepository, manufactureRepository, productInformationRepository, productPriceRepository);

        // Act
        var articleNumber = "nonExisting";
        var result = await productService.GetProductByArticleNrAsync(articleNumber);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateProductAsync_ShouldReturnTrue_WhenProductExists()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var categoryRepository = new CategoryRepository(_context);
        var manufactureRepository = new ManufactureRepository(_context);
        var productInformationRepository = new ProductInformationRepository(_context);
        var productPriceRepository = new ProductPriceRepository(_context);
        var productService = new ProductService(productRepository, categoryRepository, manufactureRepository, productInformationRepository, productPriceRepository);

        await productService.CreateProductAsync(new ProductDto
        {
            ProductTitle = "title",
            ArticleNumber = "123456",
            ManufactureName = "manufacture",
            Ingress = "ingress",
            Description = "description",
            Price = 100,
            Specification = "specification",
            CategoryName = "category",
        });

        // Act
        var updatedProductDto = new ProductDto
        {
            ProductTitle = "updatedTitle",
            ArticleNumber = "123456",
            ManufactureName = "updateMmanufacture",
            Ingress = "updatedIngress",
            Description = "updatedDescription",
            Price = 200,
            Specification = "updatedSpecification",
            CategoryName = "updatedCategory",
        };
        var updatedProductResult = await productService.UpdateProductAsync(updatedProductDto.ArticleNumber, updatedProductDto);

        // Assert
        Assert.True(updatedProductResult);
    }

    [Fact]
    public async Task UpdateUserAddressAsync_ShouldReturnFalse_WhenUserNotExists()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var categoryRepository = new CategoryRepository(_context);
        var manufactureRepository = new ManufactureRepository(_context);
        var productInformationRepository = new ProductInformationRepository(_context);
        var productPriceRepository = new ProductPriceRepository(_context);
        var productService = new ProductService(productRepository, categoryRepository, manufactureRepository, productInformationRepository, productPriceRepository);

        // Act
        var updatedProductDto = new ProductDto
        {
            ProductTitle = "updatedTitle",
            ArticleNumber = "123456",
            ManufactureName = "updateMmanufacture",
            Ingress = "updatedIngress",
            Description = "updatedDescription",
            Price = 200,
            Specification = "updatedSpecification",
            CategoryName = "updatedCategory",
        };
        var updatedProductResult = await productService.UpdateProductAsync(updatedProductDto.ArticleNumber, updatedProductDto);

        // Assert
        Assert.False(updatedProductResult);
    }

    [Fact]
    public async Task DeleteProductByArticleNrAsync_ShouldDeleteProduct_WhenProductExists()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var categoryRepository = new CategoryRepository(_context);
        var manufactureRepository = new ManufactureRepository(_context);
        var productInformationRepository = new ProductInformationRepository(_context);
        var productPriceRepository = new ProductPriceRepository(_context);
        var productService = new ProductService(productRepository, categoryRepository, manufactureRepository, productInformationRepository, productPriceRepository);

        var productDto = new ProductDto
        {
            ProductTitle = "title",
            ArticleNumber = "123456",
            ManufactureName = "manufacture",
            Ingress = "ingress",
            Description = "description",
            Price = 100,
            Specification = "specification",
            CategoryName = "category",
        };
        await productService.CreateProductAsync(productDto);

        // Act
        var isDeleted = await productService.DeleteProductByArticleNrAsync(productDto.ArticleNumber);

        // Assert
        Assert.True(isDeleted);
        var deletedProduct = await productService.GetProductByArticleNrAsync(productDto.ArticleNumber);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task DeleteProductByArticleNrAsync_ShouldReturnFalse_WhenProductDoesNotExist()
    {
        // Arrange
        var productRepository = new ProductRepository(_context);
        var categoryRepository = new CategoryRepository(_context);
        var manufactureRepository = new ManufactureRepository(_context);
        var productInformationRepository = new ProductInformationRepository(_context);
        var productPriceRepository = new ProductPriceRepository(_context);
        var productService = new ProductService(productRepository, categoryRepository, manufactureRepository, productInformationRepository, productPriceRepository);

        // Act
        var isDeleted = await productService.DeleteProductByArticleNrAsync("nonExisting");

        // Assert
        Assert.False(isDeleted);
    }
}
