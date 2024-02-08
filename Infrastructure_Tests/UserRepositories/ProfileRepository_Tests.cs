using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Tests.UserRepositories;

public class ProfileRepository_Tests
{
    private readonly DataContext _context = new(new DbContextOptionsBuilder<DataContext>()
.UseInMemoryDatabase($"{Guid.NewGuid()}")
.Options);

    [Fact]
    public async Task CreateAsync_ShouldCreateSaveRecordToDatabase_ReturnProfileEntity()
    {
        //Arrange
        var profileRepository = new ProfileRepository(_context);
        var profileEntity = new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "firstname",
            LastName = "lastname",
        };

        //Act
        var result = await profileRepository.CreateAsync(profileEntity);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotSaveRecordToDatabase_ReturnNull()
    {
        //Arrange
        var profileRepository = new ProfileRepository(_context);
        var profileEntity = new ProfileEntity
        {
            FirstName = "firstname",
            LastName = "lastname",
        };

        //Act
        var result = await profileRepository.CreateAsync(profileEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeProfileEntity()
    {
        //Arrange
        var profileRepository = new ProfileRepository(_context);
        var profileEntity = new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "firstname",
            LastName = "lastname",

        };
        await profileRepository.CreateAsync(profileEntity);

        //Act
        var result = await profileRepository.GetAllAsync();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<ProfileEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAsync_ShouldGetOneUserProfile_ReturnOneProfileEntity()
    {
        //Arrange
        var profileRepository = new ProfileRepository(_context);
        var profileEntity = new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "firstname",
            LastName = "lastname",
        };
        _context.Profiles.Add(profileEntity);
        _context.SaveChanges();

        Expression<Func<ProfileEntity, bool>> validExpression = entity => entity.FirstName == "firstname";

        //Act
        var result = await profileRepository.GetAsync(validExpression);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_ShouldNotGetOneUserProfile_ReturnNull()
    {
        //Arrange
        var profileRepository = new ProfileRepository(_context);
        Expression<Func<ProfileEntity, bool>> invalidExpression = entity => entity.FirstName == "hej";

        //Act
        var result = await profileRepository.GetAsync(invalidExpression);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveOneProfileEntity_ReturnTrue()
    {
        //Arrange
        var profileRepository = new ProfileRepository(_context);
        var profileEntity = new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "firstname",
            LastName = "lastname",
        };
        await profileRepository.CreateAsync(profileEntity);

        //Act
        var result = await profileRepository.DeleteAsync(x => x.FirstName == profileEntity.FirstName);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotFindUserProfileAndRemoveIt_ReturnFalse()
    {
        //Arrange
        var profileRepository = new ProfileRepository(_context);
        var profileEntity = new ProfileEntity { FirstName = "Tester" };

        //Act
        var result = await profileRepository.DeleteAsync(x => x.FirstName == profileEntity.FirstName);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUserProfile_ReturnUpdatedEntity()
    {
        //Arrange
        var profileRepository = new ProfileRepository(_context);
        var profileEntity = await profileRepository.CreateAsync(new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "firstname",
            LastName = "lastname",
        });

        //Act
        profileEntity.FirstName = "annat";
        var updatedProfile = await profileRepository.UpdateAsync(x => x.FirstName == "firstname", profileEntity);

        //Assert
        Assert.NotNull(updatedProfile);
        Assert.Equal("annat", updatedProfile.FirstName);
    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateUserProfileIfNotExists_ReturnNull()
    {
        //Arrange
        var profileRepository = new ProfileRepository(_context);
        var profileEntity = await profileRepository.CreateAsync(new ProfileEntity
        {
            UserId = Guid.NewGuid(),
            FirstName = "firstname",
            LastName = "lastname",
        });

        //Act
        profileEntity.FirstName = "annat";
        var updatedProfile = await profileRepository.UpdateAsync(x => x.FirstName == "annat", profileEntity);

        //Assert
        Assert.Null(updatedProfile);
    }
}

