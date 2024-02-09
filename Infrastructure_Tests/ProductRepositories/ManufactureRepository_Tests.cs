using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure_Tests.ProductRepositories;

public class ManufactureRepository_Tests
{
    private readonly ProductDataContext _context = new(new DbContextOptionsBuilder<ProductDataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public async Task CreateAsync_ShouldCreateSaveRecordToDatabase_ReturnManufactureEntityWithId_1()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_context);
        var manufactureEntity = new Manufacture { ManufactureName = "manufacture" };

        //Act
        var result = await manufactureRepository.CreateAsync(manufactureEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotSaveRecordToDatabase_ReturnNull()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_context);
        var manufactureEntity = new Manufacture();

        //Act
        var result = await manufactureRepository.CreateAsync(manufactureEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeManufactureEntity()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_context);
        var manufactureEntity = new Manufacture { ManufactureName = "manufacture" };
        await manufactureRepository.CreateAsync(manufactureEntity);

        //Act
        var result = await manufactureRepository.GetAllAsync();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Manufacture>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAsync_ShouldGetOneManufactureByName_ReturnOneManufacture()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_context);
        var manufactureWithId1 = new Manufacture { Id = 1, ManufactureName = "Apple" };
        _context.Manufactures.Add(manufactureWithId1);
        _context.SaveChanges();

        Expression<Func<Manufacture, bool>> validExpression = entity => entity.Id == 1;

        //Act
        var result = await manufactureRepository.GetAsync(validExpression);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_ShouldNotGetOneManufactureByName_ReturnNull()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_context);
        Expression<Func<Manufacture, bool>> invalidExpression = entity => entity.Id == 1;

        //Act
        var result = await manufactureRepository.GetAsync(invalidExpression);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveOneManufacture_ReturnTrue()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_context);
        var manufactureEntity = new Manufacture { ManufactureName = "manufacture" };
        await manufactureRepository.CreateAsync(manufactureEntity);

        //Act
        var result = await manufactureRepository.DeleteAsync(x => x.ManufactureName == manufactureEntity.ManufactureName);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotFindManufactureAndRemoveIt_ReturnFalse()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_context);
        var manufactureEntity = new Manufacture { ManufactureName = "manufacture" };

        //Act
        var result = await manufactureRepository.DeleteAsync(x => x.ManufactureName == manufactureEntity.ManufactureName);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateManufacture_ReturnUpdatedEntity()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_context);
        var manufactureEntity = await manufactureRepository.CreateAsync(new Manufacture
        {
            ManufactureName = "Apple"
        });

        //Act
        manufactureEntity.ManufactureName = "Annat";
        var updatedManufacture = await manufactureRepository.UpdateAsync(x => x.Id == manufactureEntity.Id, manufactureEntity);

        //Assert
        Assert.NotNull(updatedManufacture);
        Assert.Equal("Annat", updatedManufacture.ManufactureName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateManufactureIfNotExists_ReturnNull()
    {
        //Arrange
        var manufactureRepository = new ManufactureRepository(_context);
        var manufactureEntity = await manufactureRepository.CreateAsync(new Manufacture
        {
            ManufactureName = "Apple"
        });

        //Act
        manufactureEntity.ManufactureName = "Annat";
        var updatedManufacture = await manufactureRepository.UpdateAsync(x => x.Id == 999, manufactureEntity);

        //Assert
        Assert.Null(updatedManufacture);
    }
}
