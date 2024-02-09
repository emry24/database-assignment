using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Infrastructure.Repositories.ProductRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure_Tests.ProductRepositories;

public class CategoryRepository_Tests
{
    private readonly ProductDataContext _context = new(new DbContextOptionsBuilder<ProductDataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public async Task CreateAsync_ShouldCreateSaveRecordToDatabase_ReturnCategoryEntityWithId_1()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_context);
        var categoryEntity = new Category { CategoryName = "category" };

        //Act
        var result = await categoryRepository.CreateAsync(categoryEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotSaveRecordToDatabase_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_context);
        var categoryEntity = new Category();

        //Act
        var result = await categoryRepository.CreateAsync(categoryEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeCategoryEntity()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_context);
        var categoryEntity = new Category { CategoryName = "category" };
        await categoryRepository.CreateAsync(categoryEntity);

        //Act
        var result = await categoryRepository.GetAllAsync();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<Category>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAsync_ShouldGetOneCategoryByName_ReturnOneCategory()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_context);
        var categoryWithId1 = new Category { Id = 1, CategoryName = "TV" };
        _context.Categories.Add(categoryWithId1);
        _context.SaveChanges();

        Expression<Func<Category, bool>> validExpression = entity => entity.Id == 1;

        //Act
        var result = await categoryRepository.GetAsync(validExpression);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_ShouldNotGetOneCategoryByName_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_context);
        Expression<Func<Category, bool>> invalidExpression = entity => entity.Id == 1;

        //Act
        var result = await categoryRepository.GetAsync(invalidExpression);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveOneCategory_ReturnTrue()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_context);
        var categoryEntity = new Category { CategoryName = "category" };
        await categoryRepository.CreateAsync(categoryEntity);

        //Act
        var result = await categoryRepository.DeleteAsync(x => x.CategoryName == categoryEntity.CategoryName);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotFindCategoryAndRemoveIt_ReturnFalse()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_context);
        var categoryEntity = new Category { CategoryName = "category" };

        //Act
        var result = await categoryRepository.DeleteAsync(x => x.CategoryName == categoryEntity.CategoryName);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateCategory_ReturnUpdatedEntity()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_context);
        var categoryEntity = await categoryRepository.CreateAsync(new Category
        {
            CategoryName = "TV"
        });

        //Act
        categoryEntity.CategoryName = "Annat";
        var updatedCategory = await categoryRepository.UpdateAsync(x => x.Id == categoryEntity.Id, categoryEntity);

        //Assert
        Assert.NotNull(updatedCategory);
        Assert.Equal("Annat", updatedCategory.CategoryName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateCategoryIfNotExists_ReturnNull()
    {
        //Arrange
        var categoryRepository = new CategoryRepository(_context);
        var categoryEntity = await categoryRepository.CreateAsync(new Category
        {
            CategoryName = "TV"
        });

        //Act
        categoryEntity.CategoryName = "Annat";
        var updatedCategory = await categoryRepository.UpdateAsync(x => x.Id == 999, categoryEntity);

        //Assert
        Assert.Null(updatedCategory);
    }
}
