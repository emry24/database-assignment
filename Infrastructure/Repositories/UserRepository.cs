using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class UserRepository(DataContext context) : BaseRepository<UserEntity>(context)
{
    private readonly DataContext _context = context;

    public override async Task<IEnumerable<UserEntity>> GetAllAsync()
    {
        try
        {
            var entities = await _context.Users.Include(i => i.Role).Include(i => i.Profile).Include(i => i.UserAddress).Include(i => i.UserAuth).ToListAsync();
            if (entities != null)
            {
                return entities;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }

    public override async Task<UserEntity> GetAsync(Expression<Func<UserEntity, bool>> expression)
    {
        try
        {
            var entity = await _context.Users.Include(i => i.Role).Include(i => i.Profile).Include(i => i.UserAddress). Include(i => i.UserAuth).FirstOrDefaultAsync(expression);
            if (entity != null)
            {
                return entity;
            }
        }
        catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
        return null!;
    }
}


