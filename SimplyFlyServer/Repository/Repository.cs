using SimplyFlyServer.Context;
using SimplyFlyServer.Interface;

namespace SimplyFlyServer.Repository
{
    public abstract class  Repository<K, T> : IRepository<K, T> where T : class
    {
        protected readonly SimplyFlyContext _context;
        public Repository(SimplyFlyContext context)
        {
            _context = context;
        }
        public abstract Task<IEnumerable<T>> GetAll();


        public abstract Task<T> GetById(K key);

        public async Task<T> Add(T entity)
        {
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return entity;

        }

        public async Task<T> Update(K key, T entity)
        {
            var existingEntity = await GetById(key);
            if (existingEntity == null)
                throw new Exception("Entity not found");

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
            return existingEntity;

        }

        public async Task<T> Delete(K id)
        {
            var entity = await GetById(id);
            _context.Remove(entity);
            await _context.SaveChangesAsync();
            return entity;

        }
    }
}
