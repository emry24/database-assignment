using Infrastructure.Dtos;
using Infrastructure.Entities;
using Infrastructure.Repositories;
using System.Diagnostics;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository, RoleRepository roleRepository, ProfileRepository profileRepository, AddressRepository addressRepository, AuthRepository authRepository)
{
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly UserRepository _userRepository = userRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly AuthRepository _authRepository = authRepository;

   public bool CreateUser(CreateUserDto createUserDto)
    {
        try
        {
            if (!_authRepository.Exists(x => x.Email == createUserDto.Email))
            {
                var roleEntity = new RoleEntity
                {
                    RoleName = createUserDto.RoleName,
                };

               var createdRole = _roleRepository.Create(roleEntity);
            
                //if (createdRole != null)
                //    return true;
            }

            var userAddressEntity = new UserAddressEntity
            {
                StreetName = createUserDto.StreetName,
                PostalCode = createUserDto.PostalCode,
                City = createUserDto.City,
            };

            var createdAddress = _addressRepository.Create(userAddressEntity);

            var userEntity = new UserEntity
            {
                Id = createUserDto.Id,
                Created = createUserDto.Created,
                RoleId = createUserDto.RoleId,
            };

            var createdUser = _userRepository.Create(userEntity);

            var profileEntity = new ProfileEntity
            {
                FirstName = createUserDto.FirstName,
                LastName = createUserDto.LastName,
            };

            var createdProfile = _profileRepository.Create(profileEntity);

            return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

}
