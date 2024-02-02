using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Repositories.ProductRepositories
{
    public class ProductPriceRepository(ProductDataContext context) : ProductBaseRepository<ProductPrice>(context)
    {
        private readonly ProductDataContext _context = context;
    }
}
