using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using vecihi.database;
using vecihi.infrastructure.entity.models;

namespace vecihi.infrastructure
{
    public interface IRepository<Entity, Type>
        where Entity : ModelBase<Type>
        where Type : struct
    {
        VecihiDbContext Context { get; set; }
        Task Add(Entity entity);
        Task<Entity> GetById(Type id, bool isDeleted = false);
        IQueryable<Entity> Get(bool isDeleted = false);
        IQueryable<Entity> Query(bool isDeleted = false);
    }

    public class Repository<Entity, Type> : IRepository<Entity, Type>
        where Entity : ModelBase<Type>
        where Type : struct
    {
        public VecihiDbContext Context { get; set; }

        public Repository(VecihiDbContext context)
        {
            Context = context;
        }

        public virtual async Task Add(Entity entity)
        {
            await Context
                .Set<Entity>()
                .AddAsync(entity);
        }

        public virtual async Task<Entity> GetById(Type id, bool isDeleted = false)
        {
            return await Context
                .Set<Entity>()
                .Equal("Id", id)
                .Where(x => x.IsDeleted == isDeleted)
                .FirstOrDefaultAsync();
        }

        public IQueryable<Entity> Get(bool isDeleted = false)
        {
            return Context
                .Set<Entity>()
                .Where(x => x.IsDeleted == isDeleted)
                .AsQueryable();
        }

        public IQueryable<Entity> Query(bool isDeleted = false)
        {
            return Context
                .Set<Entity>()
                .AsNoTracking()
                .Where(x => x.IsDeleted == isDeleted);
        }
    }
}