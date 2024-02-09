using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace Infrastructure_Tests.ProductRepositories;

public class ProductRepository_Tests
{
    private readonly ProductDataContext _context = new(new DbContextOptionsBuilder<ProductDataContext>()
.UseInMemoryDatabase($"{Guid.NewGuid()}")
.Options);

    [Fact]
    public async Task CreateAsync_ShouldCreateSaveRecordToDatabase_ReturnProduct()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);

        var productEntity = new Product
        {
            ArticleNumber = "123456",
            CategoryId = 1,
            ManufactureId = 1,
        };

        //Act
        var result = await productRepository.CreateAsync(productEntity);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotSaveRecordToDatabase_ReturnNull()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);

        var initialEntity = new Product
        {
            CategoryId = 1,
            ManufactureId = 1,
        };

        await productRepository.CreateAsync(initialEntity);

        var duplicateEntity = new Product
        {
            ArticleNumber = "123456",
            CategoryId = 1,
            ManufactureId = 1,
        };

        // Act
        var result = await productRepository.CreateAsync(duplicateEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeProduct()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var productEntity = new Product
        {
            ArticleNumber = "123456",
            CategoryId = 1,
            ManufactureId = 1,
        };
        await productRepository.CreateAsync(productEntity);

        //Act
        var result = await productRepository.GetAllAsync();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Product>>(result);
    }

    [Fact]
    public async Task GetAsync_ShouldGetOneProduct_ReturnOneProduct()
    {
        //Arrange
        _context.Products.Add(new Product
        {
            ArticleNumber = "123456",
            CategoryId = 1,
            ManufactureId = 1,
            Category = new Category { Id = 1, CategoryName = "category" },
            Manufacture = new Manufacture { Id = 1, ManufactureName = "manufacture" },
            ProductInformation = new ProductInformation { ProductTitle = "productTitle", Description = "description" },
            ProductPrice = new ProductPrice { Price = 100 }
        });
        await _context.SaveChangesAsync();

        var productRepository = new ProductRepository(_context);

        // Act
        Expression<Func<Product, bool>> expression = p => p.ArticleNumber == "123456";
        var result = await productRepository.GetAsync(expression);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("123456", result.ArticleNumber);
    }


    [Fact]
    public async Task GetAsync_ShouldNotGetOneProduct_ReturnNull()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        Expression<Func<Product, bool>> invalidExpression = entity => entity.ArticleNumber == "annat";

        //Act
        var result = await productRepository.GetAsync(invalidExpression);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveOneProduct_ReturnTrue()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var productEntity = new Product
        {
            ArticleNumber = "123456",
            CategoryId = 1,
            ManufactureId = 1,
        };
        await productRepository.CreateAsync(productEntity);

        //Act
        var result = await productRepository.DeleteAsync(x => x.ArticleNumber == productEntity.ArticleNumber);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotFindProductAndRemoveIt_ReturnFalse()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var productEntity = new Product { ArticleNumber = "annat" };

        //Act
        var result = await productRepository.DeleteAsync(x => x.ArticleNumber == productEntity.ArticleNumber);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProduct_ReturnUpdatedEntity()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var productEntity = await productRepository.CreateAsync(new Product
        {
            ArticleNumber = "123456",
            CategoryId = 1,
            ManufactureId = 1,
        });

        //Act
        productEntity.CategoryId = 2;
        var updatedProduct = await productRepository.UpdateAsync(x => x.CategoryId == 1, productEntity);

        //Assert
        Assert.NotNull(updatedProduct);
        Assert.Equal(2, updatedProduct.CategoryId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateProductIfNotExists_ReturnNull()
    {
        //Arrange
        var productRepository = new ProductRepository(_context);
        var productEntity = await productRepository.CreateAsync(new Product
        {
            ArticleNumber = "123456",
            CategoryId = 1,
            ManufactureId = 1,
        });

        //Act
        productEntity.CategoryId = 2;
        var updatedProduct = await productRepository.UpdateAsync(x => x.CategoryId == 2, productEntity);

        //Assert
        Assert.Null(updatedProduct);
    }
}
