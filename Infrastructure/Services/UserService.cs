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

   public bool CreateUser(UserRegistrationDto userRegistrationDto)
    {
        try
        {
            if (_authRepository.Exists(x => x.Email == userRegistrationDto.Email))
            {
                return false;
            }

           
            var roleExists = _roleRepository.Exists(x => x.RoleName == userRegistrationDto.RoleName);
            int roleId;

            if (roleExists)
            {
                
                var existsRoleId = _roleRepository.GetOne(x => x.RoleName == userRegistrationDto.RoleName);
                roleId = existsRoleId.Id;
            }

            else
            {
                var roleentity = new RoleEntity
                {
                    RoleName = userRegistrationDto.RoleName,
                };

                var newRole = _roleRepository.Create(roleentity);
                roleId = newRole.Id;
            }





            //var roleEntity = new RoleEntity
            //{
            //    RoleName = userRegistrationDto.RoleName,
            //};

            //var createdRole = _roleRepository.Create(roleEntity);

            //if (createdRole != null)
            //    return true;

            var userEntity = new UserEntity
            {
                Id = Guid.NewGuid(),
                Created = DateTime.Now,
                RoleId = roleId,
            };

            var createdUser = _userRepository.Create(userEntity);


            // result = generera password skall ta in userRegistartionDTO.password

            var authEntity = new UserAuthEntity
            {
                UserId = createdUser.Id,
                Email = userRegistrationDto.Email,
                Password = userRegistrationDto.Password, //result
            };

            var createdAuth = _authRepository.Create(authEntity);

            var userAddressEntity = new UserAddressEntity
            {
                UserId = createdUser.Id,
                StreetName = userRegistrationDto.StreetName,
                PostalCode = userRegistrationDto.PostalCode,
                City = userRegistrationDto.City,
                
            };

            var createdAddress = _addressRepository.Create(userAddressEntity);

           

            var profileEntity = new ProfileEntity
            {
                UserId = createdUser.Id,
                FirstName = userRegistrationDto.FirstName,
                LastName = userRegistrationDto.LastName,
            };

            var createdProfile = _profileRepository.Create(profileEntity);

            return true;
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return false;
    }

    //public bool GetAllUsers(UserDto userDto)
    //{

    //}

    //public bool GetOneUser(UserDto userDto)
    //{

    //}

    //public bool UpdateUser(UserRegistrationDto userRegistrationDto)
    //{

    //}

    //public bool DeleteUser(UserRegistrationDto userRegistrationDto)
    //{

    //}

}
