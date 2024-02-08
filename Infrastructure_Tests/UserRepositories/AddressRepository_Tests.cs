using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure_Tests.UserRepositories;

public class AddressRepository_Tests
{
    private readonly DataContext _context = new(new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);

    [Fact]
    public async Task CreateAsync_ShouldCreateSaveRecordToDatabase_ReturnAddressEntity()
    {
        //Arrange
        var addressRepository = new AddressRepository(_context);
        var addressEntity = new UserAddressEntity 
        { 
            UserId = Guid.NewGuid(),
            StreetName = "streetname", 
            PostalCode = "123456",
            City = "city"
        };

        //Act
        var result = await addressRepository.CreateAsync(addressEntity);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldNotSaveRecordToDatabase_ReturnNull()
    {
        //Arrange
        var addressRepository = new AddressRepository(_context);
        var addressEntity = new UserAddressEntity
        {
            StreetName = "streetname",
            PostalCode = "postalcode",
            City = "city"
        };

        //Act
        var result = await addressRepository.CreateAsync(addressEntity);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeAddressEntity()
    {
        //Arrange
        var addressRepository = new AddressRepository(_context);
        var addressEntity = new UserAddressEntity 
        { 
            UserId = Guid.NewGuid(),
            StreetName = "streetname",
            PostalCode = "123456",
            City = "city"
        };
        await addressRepository.CreateAsync(addressEntity);

        //Act
        var result = await addressRepository.GetAllAsync();

        //Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IEnumerable<UserAddressEntity>>(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetAsync_ShouldGetOneUserAddress_ReturnOneAddressEntity()
    {
        //Arrange
        var addressRepository = new AddressRepository(_context);
        var addressEntity = new UserAddressEntity 
        { 
            UserId = Guid.NewGuid() ,
            StreetName = "streetname",
            PostalCode = "123456",
            City = "city"
        };
        _context.UserAddresses.Add(addressEntity);
        _context.SaveChanges();

        Expression<Func<UserAddressEntity, bool>> validExpression = entity => entity.StreetName == "streetname";

        //Act
        var result = await addressRepository.GetAsync(validExpression);

        //Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetAsync_ShouldNotGetOneUserAddress_ReturnNull()
    {
        //Arrange
        var addressRepository = new AddressRepository(_context);
        Expression<Func<UserAddressEntity, bool>> invalidExpression = entity => entity.StreetName == "hej";

        //Act
        var result = await addressRepository.GetAsync(invalidExpression);

        //Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveOneAddressEntity_ReturnTrue()
    {
        //Arrange
        var addressRepository = new AddressRepository(_context);
        var addressEntity = new UserAddressEntity 
        { 
            UserId = Guid.NewGuid(),
            StreetName = "streetname",
            PostalCode = "123456",
            City = "city"
        };
        await addressRepository.CreateAsync(addressEntity);

        //Act
        var result = await addressRepository.DeleteAsync(x => x.StreetName == addressEntity.StreetName);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldNotFindUserAddressAndRemoveIt_ReturnFalse()
    {
        //Arrange
        var addressRepository = new AddressRepository(_context);
        var addressEntity = new UserAddressEntity { StreetName = "Tester" };

        //Act
        var result = await addressRepository.DeleteAsync(x => x.StreetName == addressEntity.StreetName);

        //Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateUserAddress_ReturnUpdatedEntity()
    {
        //Arrange
        var addressRepository = new AddressRepository(_context);
        var addressEntity = await addressRepository.CreateAsync(new UserAddressEntity
        {
            UserId = Guid.NewGuid(),
            StreetName = "streetname",
            PostalCode = "123456",
            City = "city"
        });

        //Act
        addressEntity.StreetName = "annat";
        var updatedAddress = await addressRepository.UpdateAsync(x => x.StreetName == "streetname", addressEntity);

        //Assert
        Assert.NotNull(updatedAddress);
        Assert.Equal("annat", updatedAddress.StreetName);


    }

    [Fact]
    public async Task UpdateAsync_ShouldNotUpdateUserAddressIfNotExists_ReturnNull()
    {
        //Arrange
        var addressRepository = new AddressRepository(_context);
        var addressEntity = await addressRepository.CreateAsync(new UserAddressEntity
        {
            UserId = Guid.NewGuid(),
            StreetName = "streetname",
            PostalCode = "123456",
            City = "city"
        });

        //Act
        addressEntity.StreetName = "annat";
        var updatedAddress = await addressRepository.UpdateAsync(x => x.StreetName == "annat", addressEntity);

        //Assert
        Assert.Null(updatedAddress);


    }
}
