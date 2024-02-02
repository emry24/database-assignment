using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;

namespace Infrastructure.Repositories.ProductRepositories
{
    public class ProductInformationRepository(ProductDataContext context) : ProductBaseRepository<ProductInformation>(context)
    {
        private readonly ProductDataContext _context = context;
    }
}
