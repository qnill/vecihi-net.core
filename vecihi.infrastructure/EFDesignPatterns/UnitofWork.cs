using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vecihi.database;
using vecihi.infrastructure.entity.models;

namespace vecihi.infrastructure
{
    public class UnitOfWork<Type> : IDisposable
        where Type : struct
    {
        private readonly Dictionary<System.Type, object> _repositories = new Dictionary<System.Type, object>();

        public VecihiDbContext Context { get; set; }

        public UnitOfWork(VecihiDbContext context)
        {
            Context = context;
        }

        public IRepository<Entity, Type> Repository<Entity>()
            where Entity : ModelBase<Type>
        {
            if (_repositories.Keys.Contains(typeof(Entity)) == true)
                return _repositories[typeof(Entity)] as IRepository<Entity, Type>;

            IRepository<Entity, Type> repository = new Repository<Entity, Type>(Context);
            _repositories.Add(typeof(Entity), repository);

            return repository;
        }

        public virtual async Task<int> SaveChangesAsync()
        {
            return await Context.SaveChangesAsync();
        }

        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
                if (disposing)
                    Context.Dispose();

            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}