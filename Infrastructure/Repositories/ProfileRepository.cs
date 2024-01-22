using Infrastructure.Contexts;
using Infrastructure.Entities;

namespace Infrastructure.Repositories;

public class ProfileRepository(DataContext context) : BaseRepository<ProfileEntity>(context)
{
    private readonly DataContext _context = context;
}
