using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class AuthRepository(DataContext context) : BaseRepository<UserAuthEntity>(context)
{
    private readonly DataContext _context = context;


}


