using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Tests;

public class UserService_Tests
{
    private readonly DataContext _context = new(new DbContextOptionsBuilder<DataContext>()
    .UseInMemoryDatabase($"{Guid.NewGuid()}")
    .Options);


    [Fact]
    public async Task CreateUserAsync_ShouldCreateOneUser_ReturnTrue()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var authRepository = new AuthRepository(_context);
        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        // Act
        var result = await userService.CreateUserAsync(new UserRegistrationDto
        {
            FirstName = "firstname",
            LastName = "lastname",
            StreetName = "streetname",
            PostalCode = "12345",
            City = "city",
            RoleName = "rolename",
            Email = "email",
            Password = "password"
        });

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldNotCreateOneUser_ReturnFalse()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var authRepository = new AuthRepository(_context);
        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        // Act
        await userService.CreateUserAsync(new UserRegistrationDto
        {
            FirstName = "firstname",
            LastName = "lastname",
            StreetName = "streetname",
            PostalCode = "12345",
            City = "city",
            RoleName = "rolename",
            Email = "email",
            Password = "password"
        });

        var result = await userService.CreateUserAsync(new UserRegistrationDto
        {
            FirstName = "firstname",
            LastName = "lastname",
            StreetName = "streetname",
            PostalCode = "12345",
            City = "city",
            RoleName = "rolename",
            Email = "email",
            Password = "password"
        });

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldGetAllRecords_ReturnIEnumerableOfTypeUser()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);

        var roleEntity = new RoleEntity { RoleName = "role" };
        await roleRepository.CreateAsync(roleEntity);

        var profileEntity = new ProfileEntity 
        { 
            FirstName = "firstname", 
            LastName = "lastname" 
        };
        await profileRepository.CreateAsync(profileEntity);

        var userEntity = new UserEntity 
        { 
            RoleId = roleEntity.Id, 
            Id = profileEntity.UserId 
        };
        await userRepository.CreateAsync(userEntity);

        var userService = new UserService(userRepository, roleRepository, profileRepository, null!, null!);

        // Act
        var result = await userService.GetAllUsersAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldGetOneUserByEmail_ReturnOneUser()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var authRepository = new AuthRepository(_context);

        var roleEntity = new RoleEntity { RoleName = "rolename" };
        await roleRepository.CreateAsync(roleEntity);

        var profileEntity = new ProfileEntity 
        { 
            FirstName = "firstname", 
            LastName = "lastname" 
        };
        await profileRepository.CreateAsync(profileEntity);

        var userAuthEntity = new UserAuthEntity 
        { 
            Email = "email", 
            Password = "password" 
        };
        await authRepository.CreateAsync(userAuthEntity);

        var userAddressEntity = new UserAddressEntity 
        { 
            StreetName = "streetname", 
            PostalCode = "12345", 
            City = "city" 
        };
        await addressRepository.CreateAsync(userAddressEntity);

        var userEntity = new UserEntity { RoleId = roleEntity.Id };
        userEntity.Profile = profileEntity;
        userEntity.UserAddress = userAddressEntity;
        userEntity.UserAuth = userAuthEntity;
        await userRepository.CreateAsync(userEntity);
        await userRepository.CreateAsync(userEntity);

        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        // Act
        var userEmail = "email";
        var result = await userService.GetUserByEmailAsync(userEmail);

        // Act

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldNotGetOneUserByEmail_ReturnNull()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var authRepository = new AuthRepository(_context);

        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        // Act
        var userEmail = "nonExisting";
        var result = await userService.GetUserByEmailAsync(userEmail);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateUserAddressAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var authRepository = new AuthRepository(_context);
        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        _ = await userService.CreateUserAsync(new UserRegistrationDto
        {
            FirstName = "firstname",
            LastName = "lastname",
            StreetName = "streetname",
            PostalCode = "12345",
            City = "city",
            RoleName = "rolename",
            Email = "email",
            Password = "password"
        });


        // Act
        var updatedAddressDto = new UserDto
        {
            Email = "email", 
            FirstName = "updatedFirstName",
            LastName = "updatedLastName",
            StreetName = "updatedStreetName",
            PostalCode = "updatedPostalCode",
            City = "updatedCity"
        };

        var updatedAddressResult = await userService.UpdateUserAddressAsync(updatedAddressDto);


        // Assert
        Assert.True(updatedAddressResult);
    }

    [Fact]
    public async Task UpdateUserAddressAsync_ShouldReturnFalse_WhenUserNotExists()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var authRepository = new AuthRepository(_context);
        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        // Act
        var updatedAddressDto = new UserDto
        {
            Email = "nonexistent@email.com",
            FirstName = "updatedFirstName",
            LastName = "updatedLastName",
            StreetName = "updatedStreetName",
            PostalCode = "updatedPostalCode",
            City = "updatedCity"
        };

        var updatedAddressResult = await userService.UpdateUserAddressAsync(updatedAddressDto);

        // Assert
        Assert.False(updatedAddressResult);
    }

    [Fact]
    public async Task UpdateUserRoleAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var authRepository = new AuthRepository(_context);
        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        var registrationDto = new UserRegistrationDto
        {
            FirstName = "firstname",
            LastName = "lastname",
            StreetName = "streetname",
            PostalCode = "12345",
            City = "city",
            RoleName = "rolename",
            Email = "email",
            Password = "password"
        };
        await userService.CreateUserAsync(registrationDto);

        var updatedRoleEntity = new RoleEntity
        {
            RoleName = "updatedRoleName"
        };
        await roleRepository.CreateAsync(updatedRoleEntity);

        // Act
        var updatedRoleResult = await userService.UpdateUserRoleAsync(registrationDto.Email, updatedRoleEntity.RoleName);

        // Assert
        Assert.True(updatedRoleResult);
    }

    [Fact]
    public async Task UpdateUserRoleAsync_ShouldReturnFalse_WhenUserNotExists()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var authRepository = new AuthRepository(_context);
        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        // Act
        var updatedRoleEntity = new RoleEntity
        {
            RoleName = "updatedRoleName"
        };
        await roleRepository.CreateAsync(updatedRoleEntity);

        var updatedRoleResult = await userService.UpdateUserRoleAsync("nonExisting", updatedRoleEntity.RoleName);

        // Assert
        Assert.False(updatedRoleResult);
    }

    [Fact]
    public async Task UpdateUserAuthAsync_ShouldReturnTrue_WhenUserExists()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var authRepository = new AuthRepository(_context);
        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        var registrationDto = new UserRegistrationDto
        {
            FirstName = "firstname",
            LastName = "lastname",
            StreetName = "streetname",
            PostalCode = "12345",
            City = "city",
            RoleName = "rolename",
            Email = "email",
            Password = "password"
        };
        await userService.CreateUserAsync(registrationDto);

        // Act
        var updatedUserDto = new UserRegistrationDto
        {
            Email = "updatedEmail",
            Password = "updatedPassword"
        };
        var updatedAuthResult = await userService.UpdateUserAuthAsync(registrationDto.Email, updatedUserDto);

        // Assert
        Assert.True(updatedAuthResult);
    }

    [Fact]
    public async Task UpdateUserAuthAsync_ShouldReturnFalse_WhenUserNotExists()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var authRepository = new AuthRepository(_context);
        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        // Act
        var updatedUserDto = new UserRegistrationDto
        {
            Email = "updatedEmail",
            Password = "updatedPassword"
        };
        var updatedAuthResult = await userService.UpdateUserAuthAsync("nonExisting", updatedUserDto);

        // Assert
        Assert.False(updatedAuthResult);
    }

    [Fact]
    public async Task DeleteUserByEmailAsync_ShouldDeleteUser_WhenUserExists()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var authRepository = new AuthRepository(_context);
        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        var registrationDto = new UserRegistrationDto
        {
            FirstName = "firstname",
            LastName = "lastname",
            StreetName = "streetname",
            PostalCode = "12345",
            City = "city",
            RoleName = "rolename",
            Email = "email",
            Password = "password"
        };
        await userService.CreateUserAsync(registrationDto);

        // Act
        var isDeleted = await userService.DeleteUserByEmailAsync(registrationDto.Email);

        // Assert
        Assert.True(isDeleted);
        var deletedUser = await userService.GetUserByEmailAsync(registrationDto.Email);
        Assert.Null(deletedUser);
    }

    [Fact]
    public async Task DeleteUserByEmailAsync_ShouldReturnFalse_WhenUserDoesNotExist()
    {
        // Arrange
        var userRepository = new UserRepository(_context);
        var roleRepository = new RoleRepository(_context);
        var profileRepository = new ProfileRepository(_context);
        var addressRepository = new AddressRepository(_context);
        var authRepository = new AuthRepository(_context);
        var userService = new UserService(userRepository, roleRepository, profileRepository, addressRepository, authRepository);

        // Act
        var isDeleted = await userService.DeleteUserByEmailAsync("nonExisting");

        // Assert
        Assert.False(isDeleted);
    }

}
