using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;


namespace Infrastructure.Repositories.ProductRepositories
{
    public abstract class ProductBaseRepository<TEntity> where TEntity : class
    {

        private readonly ProductDataContext _context;

        protected ProductBaseRepository(ProductDataContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity> Create(TEntity entity)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);
                await _context.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return null!;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                var entities = await _context.Set<TEntity>().ToListAsync();
                if (entities != null)
                {
                    return entities;
                }
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return null!;
        }

        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(expression);
                if (entity != null)
                {
                    return entity;
                }
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return null!;
        }

        public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updatedEntity)
        {
            try
            {
                var entityToUpdate = await _context.Set<TEntity>().FirstOrDefaultAsync(expression);
                if (entityToUpdate != null)
                {
                    _context.Entry(entityToUpdate).CurrentValues.SetValues(updatedEntity);
                    await _context.SaveChangesAsync();
                    return entityToUpdate;
                }
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return null!;
        }

        public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(expression);
                if (entity != null)
                {
                    _context.Set<TEntity>().Remove(entity);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return false;
        }

        public virtual async Task<bool> ExistingAsync(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                var existing = await _context.Set<TEntity>().AnyAsync(expression);
                return existing;
                //return _context.Set<TEntity>().Any(expression);
            }
            catch (Exception ex) { Debug.WriteLine("ERROR :: " + ex.Message); }
            return false;
        }
    }
}
