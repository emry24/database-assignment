using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Data;
using System.Diagnostics;


namespace Infrastructure.Services;

public class UserService(UserRepository userRepository, RoleRepository roleRepository, ProfileRepository profileRepository, AddressRepository addressRepository, AuthRepository authRepository)
{
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserRepository _userRepository = userRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly AuthRepository _authRepository = authRepository;

   public async Task<bool> CreateUserAsync(UserRegistrationDto userRegistrationDto)
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

                var newRole = await _roleRepository.CreateAsync(roleentity);
                roleId = newRole.Id;
            }

            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                RoleId = roleId,
            };

            var createdUser = await _userRepository.CreateAsync(userEntity);

            // result = generera password skall ta in userRegistartionDTO.password

            var authEntity = new UserAuthEntity
            {
                UserId = createdUser.Id,
                Email = userRegistrationDto.Email,
                Password = userRegistrationDto.Password,
            };

            var createdAuth = await _authRepository.CreateAsync(authEntity);

            var userAddressEntity = new UserAddressEntity
            {
                UserId = createdUser.Id,
                StreetName = userRegistrationDto.StreetName,
                PostalCode = userRegistrationDto.PostalCode,
                City = userRegistrationDto.City,   
            };

            var createdAddress = await _addressRepository.CreateAsync(userAddressEntity);  

            var profileEntity = new ProfileEntity
            {
                UserId = createdUser.Id,
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
            };

            var createdProfile = await _profileRepository.CreateAsync(profileEntity);

            return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
   }

    public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
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

    public async Task<bool> UpdateUserAddressAsync(UserDto updatedAddressDto)
    {
        try
        {
            var user = await _authRepository.GetAsync(u => u.Email == updatedAddressDto.Email);
            if (user != null)
            {
                var profileEntity = await _profileRepository.GetAsync(p => p.UserId == user.UserId);
                profileEntity.FirstName = updatedAddressDto.FirstName;
                profileEntity.LastName = updatedAddressDto.LastName;

                var updatedProfile = await _profileRepository.UpdateAsync(p => p.UserId == profileEntity.UserId, profileEntity);

                var userAddressEntity = await _addressRepository.GetAsync(a => a.UserId == user.UserId);
                userAddressEntity.StreetName = updatedAddressDto.StreetName;
                userAddressEntity.PostalCode = updatedAddressDto.PostalCode;
                userAddressEntity.City = updatedAddressDto.City;

                var updatedAddress = await _addressRepository.UpdateAsync(a => a.UserId == userAddressEntity.UserId, userAddressEntity);

                return updatedProfile != null && updatedAddress != null;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;        
    }

    public async Task<bool> UpdateUserRoleAsync(string userEmail, string roleName)
    {
        try
        {
            var userAuth = await _authRepository.GetAsync(u => u.Email == userEmail);
            if (userAuth != null)
            {
                var user = await _userRepository.GetAsync(u => u.Id == userAuth.UserId);

                if (user != null)
                {
                    var role = await _roleRepository.GetAsync(r => r.RoleName == roleName);
                    if (role != null)
                    {
                        user.RoleId = role.Id;
                        var updatedUser = await _userRepository.UpdateAsync(u => u.Id == user.Id, user);

                        return updatedUser != null;
                    }
                }
            }

            return false;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false; 
    }

    public async Task<bool> UpdateUserAuthAsync(string userEmail, UserRegistrationDto updatedUserDto)
    {
        try
        {
            var user = await _authRepository.GetAsync(u => u.Email == userEmail);
            if (user != null)
            {
                user.Email = updatedUserDto.Email;
                user.Password = updatedUserDto.Password;

                var updatedAuth = await _authRepository.UpdateAsync(p => p.UserId == user.UserId, user);

                return updatedAuth != null;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false; 
    }

    public async Task<bool> DeleteUserByEmailAsync(string email)
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
