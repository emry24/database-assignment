using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository, RoleRepository roleRepository, ProfileRepository profileRepository, AddressRepository addressRepository, AuthRepository authRepository)
{
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserRepository _userRepository = userRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly AuthRepository _authRepository = authRepository;

   public async Task<bool> CreateUser(UserRegistrationDto userRegistrationDto)
    {
        try
        {
            if (await _authRepository.ExistingAsync(x => x.Email == userRegistrationDto.Email))
            {
                return false;
            }

            var roleExists = await _roleRepository.ExistingAsync(x => x.RoleName == userRegistrationDto.RoleName);
            int roleId;

            if (roleExists)
            {
                
                var existsRoleId = await _roleRepository.GetAsync(x => x.RoleName == userRegistrationDto.RoleName);
                roleId = existsRoleId.Id;
            }

            else
            {
                var roleentity = new RoleEntity
                {
                    RoleName = userRegistrationDto.RoleName,
                };

                var newRole = await _roleRepository.Create(roleentity);
                roleId = newRole.Id;
            }


            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                RoleId = roleId,
            };

            var createdUser = await _userRepository.Create(userEntity);


            // result = generera password skall ta in userRegistartionDTO.password

            var authEntity = new UserAuthEntity
            {
                UserId = createdUser.Id,
                Email = userRegistrationDto.Email,
                Password = userRegistrationDto.Password,
            };

            var createdAuth = await _authRepository.Create(authEntity);

            var userAddressEntity = new UserAddressEntity
            {
                UserId = createdUser.Id,
                StreetName = userRegistrationDto.StreetName,
                PostalCode = userRegistrationDto.PostalCode,
                City = userRegistrationDto.City,
                
            };

            var createdAddress = await _addressRepository.Create(userAddressEntity);  

            var profileEntity = new ProfileEntity
            {
                UserId = createdUser.Id,
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
            };

            var createdProfile = await _profileRepository.Create(profileEntity);

            return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }


    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        try
        {
            var userEntities = await _userRepository.GetAllAsync();

            if (userEntities != null)
            {
                var userDtos = userEntities.Select(userEntity => new UserDto
                {
                    FirstName = userEntity.Profile.FirstName,
                    LastName = userEntity.Profile.LastName,
                    RoleName = userEntity.Role.RoleName,

                }).ToList();

                return userDtos;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }




    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        try
        {
            var user = await _userRepository.GetAsync(x => x.UserAuth.Email == email);
            if (user != null)
            {
                var userDto = new UserDto
                {
                    FirstName = user.Profile.FirstName,
                    LastName = user.Profile.LastName,
                    Email = user.UserAuth.Email,
                    StreetName = user.UserAddress?.StreetName,
                    PostalCode = user.UserAddress?.PostalCode,
                    City = user.UserAddress?.City,
                    RoleName = user.Role.RoleName
                };

                return userDto;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return null!;
    }



    public async Task<bool> UpdateUserAddress(UserRegistrationDto updatedAddressDto)
    {
        try
        {
            var user = await _authRepository.GetAsync(u => u.Email == updatedAddressDto.Email);
            if (user != null)
            {
                var userAddressEntity = await _addressRepository.GetAsync(a => a.UserId == user.UserId);
                userAddressEntity.StreetName = updatedAddressDto.StreetName;
                userAddressEntity.PostalCode = updatedAddressDto.PostalCode;
                userAddressEntity.City = updatedAddressDto.City;

                var updatedAddress = await _addressRepository.UpdateAsync(a => a.UserId == userAddressEntity.UserId, userAddressEntity);

                return updatedAddress != null;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;        
    }






    public async Task<bool> DeleteUserByEmail(string email)
    {
        try
        {
            var user = await _authRepository.GetAsync(u => u.Email == email);
            if (user != null)
            {
                await _userRepository.DeleteAsync(x => x.Id == user.UserId);
                return true;
            }

            else
            {
                return false;
            }

        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }

        return false;
    }

}
