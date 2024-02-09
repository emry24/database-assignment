using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure_Tests.ProductRepositories;

public class ProductPriceRepository_Tests
{
    private readonly ProductDataContext _context = new(new DbContextOptionsBuilder<ProductDataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public async Task CreateAsync_ShouldCreateSaveRecordToDatabase_ReturnProductPriceEntity()
    {
        //Arrange
        var productPriceRepository = new ProductPriceRepository(_context);
        var productPriceEntity = new ProductPrice
        {
            ArticleNumber = "123456",
            Price = 100
        };

        //Act
        var result = await productPriceRepository.CreateAsync(productPriceEntity);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotSaveRecordToDatabase_ReturnNull()
    {
        //Arrange
        var productPriceRepository = new ProductPriceRepository(_context);
        var productPriceEntity = new ProductPrice
        {
            Price = 100
        };

        //Act
        var result = await productPriceRepository.CreateAsync(productPriceEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeProductPriceEntity()
    {
        //Arrange
        var productPriceRepository = new ProductPriceRepository(_context);
        var productPriceEntity = new ProductPrice
        {
            ArticleNumber = "123456",
            Price = 100
        };
        await productPriceRepository.CreateAsync(productPriceEntity);

        //Act
        var result = await productPriceRepository.GetAllAsync();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ProductPrice>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAsync_ShouldGetOneProductPriceEntity_ReturnOneProductPriceEntity()
    {
        //Arrange
        var productPriceRepository = new ProductPriceRepository(_context);
        var productPriceEntity = new ProductPrice
        {
            ArticleNumber = "123456",
            Price = 100
        };
        _context.ProductPrices.Add(productPriceEntity);
        _context.SaveChanges();

        Expression<Func<ProductPrice, bool>> validExpression = entity => entity.ArticleNumber == "123456";

        //Act
        var result = await productPriceRepository.GetAsync(validExpression);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_ShouldNotGetOneProductPriceEntity_ReturnNull()
    {
        //Arrange
        var productPriceRepository = new ProductPriceRepository(_context);
        Expression<Func<ProductPrice, bool>> invalidExpression = entity => entity.ArticleNumber == "hej";

        //Act
        var result = await productPriceRepository.GetAsync(invalidExpression);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveOneProductPriceEntity_ReturnTrue()
    {
        //Arrange
        var productPriceRepository = new ProductPriceRepository(_context);
        var productPriceEntity = new ProductPrice
        {
            ArticleNumber = "123456",
            Price = 100
        };
        await productPriceRepository.CreateAsync(productPriceEntity);

        //Act
        var result = await productPriceRepository.DeleteAsync(x => x.ArticleNumber == productPriceEntity.ArticleNumber);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotFindProductPriceAndRemoveIt_ReturnFalse()
    {
        //Arrange
        var productPriceRepository = new ProductPriceRepository(_context);
        var productPriceEntity = new ProductPrice { ArticleNumber = "tester" };

        //Act
        var result = await productPriceRepository.DeleteAsync(x => x.ArticleNumber == productPriceEntity.ArticleNumber);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProductPrice_ReturnUpdatedEntity()
    {
        //Arrange
        var productPriceRepository = new ProductPriceRepository(_context);
        var productPriceEntity = await productPriceRepository.CreateAsync(new ProductPrice
        {
            ArticleNumber = "123456",
            Price = 100
        });

        //Act
        productPriceEntity.Price = 200;
        var updatedProductPrice = await productPriceRepository.UpdateAsync(x => x.Price == 100, productPriceEntity);

        //Assert
        Assert.NotNull(updatedProductPrice);
        Assert.Equal(200, updatedProductPrice.Price);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateProductPriceIfNotExists_ReturnNull()
    {
        //Arrange
        var productPriceRepository = new ProductPriceRepository(_context);
        var productPriceEntity = await productPriceRepository.CreateAsync(new ProductPrice
        {
            ArticleNumber = "123456",
            Price = 100
        });

        //Act
        productPriceEntity.Price = 200;
        var updatedProductPrice = await productPriceRepository.UpdateAsync(x => x.Price == 200, productPriceEntity);

        //Assert
        Assert.Null(updatedProductPrice);
    }
}
