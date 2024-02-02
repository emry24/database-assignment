using Infrastructure.Contexts;
using Infrastructure.Entities.ProductEntities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.ProductRepositories
{
    public class ProductRepository(ProductDataContext context) : ProductBaseRepository<Product>(context)
    {
        private readonly ProductDataContext _context = context;

        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var entities = await _context.Products.Include(i => i.Category).Include(i => i.Manufacture).Include(i => i.ProductInformation).Include(i => i.ProductPrice).ToListAsync();
                if (entities != null)
                {
                    return entities;
                }
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return null!;
        }

        public override async Task<Product> GetAsync(Expression<Func<Product, bool>> expression)
        {
            try
            {
                var entity = await _context.Products.Include(i => i.Category).Include(i => i.Manufacture).Include(i => i.ProductInformation).Include(i => i.ProductPrice).FirstOrDefaultAsync(expression);
                if (entity != null)
                {
                    return entity;
                }
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return null!;
        }
    }
}
