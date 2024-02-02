using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Repositories.ProductRepositories
{
    public class ManufactureRepository(ProductDataContext context) : ProductBaseRepository<Manufacture>(context)
    {
        private readonly ProductDataContext _context = context;
    }
}
