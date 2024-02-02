using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Repositories.ProductRepositories
{
    public class CategoryRepository(ProductDataContext context) : ProductBaseRepository<Category>(context)
    {
        private readonly ProductDataContext _context = context;
    }
}
