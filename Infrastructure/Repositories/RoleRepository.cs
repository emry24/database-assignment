

using Infrastructure.Contexts;
using Infrastructure.Dtos;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class RoleRepository(DataContext context) : BaseRepository<RoleEntity>(context)
{
    private readonly DataContext _context = context;

    internal Task AddAsync(UserDto newRole)
    {
        throw new NotImplementedException();
    }
}


