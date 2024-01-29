using Infrastructure.Contexts;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories;

public class AuthRepository(DataContext context) : BaseRepository<UserAuthEntity>(context)
{
    private readonly DataContext _context = context;


}


