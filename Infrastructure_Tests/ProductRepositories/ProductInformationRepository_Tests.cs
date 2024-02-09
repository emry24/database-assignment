using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure_Tests.ProductRepositories;

public class ProductInformationRepository_Tests
{
    private readonly ProductDataContext _context = new(new DbContextOptionsBuilder<ProductDataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public async Task CreateAsync_ShouldCreateSaveRecordToDatabase_ReturnProductInformationEntity()
    {
        //Arrange
        var productInformationRepository = new ProductInformationRepository(_context);
        var productInformationEntity = new ProductInformation
        {
            ArticleNumber = "123456",
            ProductTitle = "productTitle",
            Ingress = "ingress",
            Description = "description", 
            Specification = "specification" 
        };

        //Act
        var result = await productInformationRepository.CreateAsync(productInformationEntity);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotSaveRecordToDatabase_ReturnNull()
    {
        //Arrange
        var productInformationRepository = new ProductInformationRepository(_context);
        var productInformationEntity = new ProductInformation
        {
            ProductTitle = "productTitle",
            Ingress = "ingress",
            Description = "description",
            Specification = "specification"
        };

        //Act
        var result = await productInformationRepository.CreateAsync(productInformationEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeProductInformationEntity()
    {
        //Arrange
        var productInformationRepository = new ProductInformationRepository(_context);
        var productInformationEntity = new ProductInformation
        {
            ArticleNumber = "123456",
            ProductTitle = "productTitle",
            Ingress = "ingress",
            Description = "description",
            Specification = "specification"
        };
        await productInformationRepository.CreateAsync(productInformationEntity);

        //Act
        var result = await productInformationRepository.GetAllAsync();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ProductInformation>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAsync_ShouldGetOneProductInformationEntity_ReturnOneProductInformationEntity()
    {
        //Arrange
        var productInformationRepository = new ProductInformationRepository(_context);
        var productInformationEntity = new ProductInformation
        {
            ArticleNumber = "123456",
            ProductTitle = "productTitle",
            Ingress = "ingress",
            Description = "description",
            Specification = "specification"
        };
        _context.ProductInformations.Add(productInformationEntity);
        _context.SaveChanges();

        Expression<Func<ProductInformation, bool>> validExpression = entity => entity.ArticleNumber == "123456";

        //Act
        var result = await productInformationRepository.GetAsync(validExpression);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_ShouldNotGetOneProductInformationEntity_ReturnNull()
    {
        //Arrange
        var productInformationRepository = new ProductInformationRepository(_context);
        Expression<Func<ProductInformation, bool>> invalidExpression = entity => entity.ArticleNumber == "hej";

        //Act
        var result = await productInformationRepository.GetAsync(invalidExpression);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveOneProductInformationEntity_ReturnTrue()
    {
        //Arrange
        var productInformationRepository = new ProductInformationRepository(_context);
        var productInformationEntity = new ProductInformation
        {
            ArticleNumber = "123456",
            ProductTitle = "productTitle",
            Ingress = "ingress",
            Description = "description",
            Specification = "specification"
        };
        await productInformationRepository.CreateAsync(productInformationEntity);

        //Act
        var result = await productInformationRepository.DeleteAsync(x => x.ArticleNumber == productInformationEntity.ArticleNumber);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotFindProductInformationAndRemoveIt_ReturnFalse()
    {
        //Arrange
        var productInformationRepository = new ProductInformationRepository(_context);
        var productInformationEntity = new ProductInformation { ArticleNumber = "tester" };

        //Act
        var result = await productInformationRepository.DeleteAsync(x => x.ArticleNumber == productInformationEntity.ArticleNumber);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateProductInformation_ReturnUpdatedEntity()
    {
        //Arrange
        var productInformationRepository = new ProductInformationRepository(_context);
        var productInformationEntity = await productInformationRepository.CreateAsync(new ProductInformation
        {
            ArticleNumber = "123456",
            ProductTitle = "productTitle",
            Ingress = "ingress",
            Description = "description",
            Specification = "specification"
        });

        //Act
        productInformationEntity.ProductTitle = "updatedProductTitle";
        var updatedProductInformation = await productInformationRepository.UpdateAsync(x => x.ProductTitle == "productTitle", productInformationEntity);

        //Assert
        Assert.NotNull(updatedProductInformation);
        Assert.Equal("updatedProductTitle", updatedProductInformation.ProductTitle);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateProductInformationIfNotExists_ReturnNull()
    {
        //Arrange
        var productInformationRepository = new ProductInformationRepository(_context);
        var productInformationEntity = await productInformationRepository.CreateAsync(new ProductInformation
        {
            ArticleNumber = "123456",
            ProductTitle = "productTitle",
            Ingress = "ingress",
            Description = "description",
            Specification = "specification"
        });

        //Act
        productInformationEntity.ArticleNumber = "annat";
        var updatedProductInformation = await productInformationRepository.UpdateAsync(x => x.ArticleNumber == "annat", productInformationEntity);

        //Assert
        Assert.Null(updatedProductInformation);
    }
}
