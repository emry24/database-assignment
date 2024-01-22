using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class UserService(UserRepository userRepository, RoleRepository roleRepository, ProfileRepository profileRepository, AddressRepository addressRepository, AuthRepository authRepository)
{
    private readonly UserRepository _userRepository = userRepository;
    private readonly RoleRepository _roleRepository = roleRepository;
    private readonly ProfileRepository _profileRepository = profileRepository;
    private readonly AddressRepository _addressRepository = addressRepository;
    private readonly AuthRepository _authRepository = authRepository;

   
}
