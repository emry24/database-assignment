using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infrastructure_Tests.UserRepositories;

public class UserRepository_Tests
{
    private readonly DataContext _context = new(new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public async Task CreateAsync_ShouldCreateSaveRecordToDatabase_ReturnUserEntity()
    {
        //Arrange
        var userRepository = new UserRepository(_context);

        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(),
            Modified = new DateTime(),
            RoleId = 1
        };

        //Act
        var result = await userRepository.CreateAsync(userEntity);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotSaveRecordToDatabase_ReturnNull()
    {
        //Arrange
        var userRepository = new UserRepository(_context);

        var initialEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow,
            RoleId = 1
        };

        await userRepository.CreateAsync(initialEntity);

        var duplicateEntity = new UserEntity
        {
            Id = initialEntity.Id, 
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow,
            RoleId = 1
        };

        // Act
        var result = await userRepository.CreateAsync(duplicateEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeUserEntity()
    {
        //Arrange
        var userRepository = new UserRepository(_context);
        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(),
            Modified = new DateTime(),
            RoleId = 1

        };
        await userRepository.CreateAsync(userEntity);

        //Act
        var result = await userRepository.GetAllAsync();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<UserEntity>>(result);
    }

    [Fact]
    public async Task GetAsync_ShouldGetOneUser_ReturnOneUserEntity()
    {
        //Arrange
        var userRepository = new UserRepository(_context);

        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Created = new DateTime(),
            Modified = new DateTime(),
            RoleId = 1
        };

        //var userEntity = new UserEntity { RoleId = roleEntity.Id };
        //userEntity.Profile = profileEntity;
        //userEntity.UserAddress = userAddressEntity;
        //userEntity.UserAuth = userAuthEntity;
        //await userRepository.CreateAsync(userEntity);
        //await userRepository.CreateAsync(userEntity);

        _context.Users.Add(userEntity);
        _context.SaveChanges();

        Expression<Func<UserEntity, bool>> validExpression = entity => entity.Id == userEntity.Id;

        //Act
        var result = await userRepository.GetAsync(validExpression);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_ShouldNotGetOneUser_ReturnNull()
    {
        //Arrange
        var userRepository = new UserRepository(_context);
        Expression<Func<UserEntity, bool>> invalidExpression = entity => entity.RoleId == 99;

        //Act
        var result = await userRepository.GetAsync(invalidExpression);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveOneUserEntity_ReturnTrue()
    {
        //Arrange
        var userRepository = new UserRepository(_context);
        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow,
            RoleId = 2
        };
        await userRepository.CreateAsync(userEntity);

        //Act
        var result = await userRepository.DeleteAsync(x => x.RoleId == userEntity.RoleId);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotFindUserAndRemoveIt_ReturnFalse()
    {
        //Arrange
        var userRepository = new UserRepository(_context);
        var userEntity = new UserEntity { RoleId = 99 };

        //Act
        var result = await userRepository.DeleteAsync(x => x.RoleId == userEntity.RoleId);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUser_ReturnUpdatedEntity()
    {
        //Arrange
        var userRepository = new UserRepository(_context);
        var userEntity = await userRepository.CreateAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow,
            RoleId = 2
        });

        //Act
        userEntity.RoleId = 3;
        var updatedUser = await userRepository.UpdateAsync(x => x.RoleId == 2, userEntity);

        //Assert
        Assert.NotNull(updatedUser);
        Assert.Equal(3, updatedUser.RoleId);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateUserIfNotExists_ReturnNull()
    {
        //Arrange
        var userRepository = new UserRepository(_context);
        var userEntity = await userRepository.CreateAsync(new UserEntity
        {
            Id = Guid.NewGuid(),
            Created = DateTime.UtcNow,
            Modified = DateTime.UtcNow,
            RoleId = 2
        });

        //Act
        userEntity.RoleId = 3;
        var updatedUser = await userRepository.UpdateAsync(x => x.RoleId == 3, userEntity);

        //Assert
        Assert.Null(updatedUser);
    }
}
