using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace Infrastructure_Tests.UserRepositories;

public class RoleRepository_Tests
{
    private readonly DataContext _context = new(new DbContextOptionsBuilder<DataContext>()
        .UseInMemoryDatabase($"{Guid.NewGuid()}")
        .Options);

    [Fact]
    public async Task CreateAsync_Should_Add_One_RoleEntity_To_Database_And_Return_Updated_RoleEntity()
    {
        //Arrange
        var roleRepository = new RoleRepository(_context);
        var roleEntity = new RoleEntity { RoleName = "Tester" };

        //Act
        var result = await roleRepository.CreateAsync(roleEntity);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task CreateAsync_Should_Not_Add_One_RoleEntity_To_Database_And_ReturnNull()
    {
        //Arrange
        var roleRepository = new RoleRepository(_context);
        var roleEntity = new RoleEntity();

        //Act
        var result = await roleRepository.CreateAsync(roleEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeRoleEntity()
    {
        //Arrange
        var roleRepository = new RoleRepository(_context);
        var roleWithId1 = new RoleEntity { Id = 1, RoleName = "Admin" };
        _context.Roles.Add(roleWithId1);
        _context.SaveChanges();

        Expression<Func<RoleEntity, bool>> validExpression = entity => entity.Id == 1;

        //Act
        var result = await roleRepository.GetAsync(validExpression);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_ShouldNotGetAllRecords_ReturnNull()
    {
        //Arrange
        var roleRepository = new RoleRepository(_context);
        Expression<Func<RoleEntity, bool>> invalidExpression = entity => entity.Id == 1;

        //Act
        var result = await roleRepository.GetAsync(invalidExpression);

        //Assert
        Assert.Null(result);
    }
}
