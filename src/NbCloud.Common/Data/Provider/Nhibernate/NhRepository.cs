using System;
using System.Linq;
using NbCloud.Common.Data.Model;
using NHibernate.Linq;

namespace NbCloud.Common.Data.Provider.Nhibernate
{
    public class NhRepository<T, TId> : RepositoryBase<T, TId> where T : class, INbEntity<TId>
    {
        private readonly NhRepositoryContext _repositoryContext;
        public NhRepository(IRepositoryContext repositoryContext) : base(repositoryContext)
        {
            _repositoryContext = (NhRepositoryContext)repositoryContext;
        }

        public override T Get(TId id)
        {
            return _repositoryContext.DbContext.Get<T>(id);
        }

        public override T Get(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _repositoryContext.DbContext.Query<T>().Where(predicate).FirstOrDefault();
        }

        public override IQueryable<T> GetAll()
        {
            return _repositoryContext.DbContext.Query<T>();
        }

        public override void Delete(TId id)
        {
            var entity = Get(id);
            if (entity != null)
            {
                _repositoryContext.DbContext.Delete(entity);
            }
        }

        public override void Flush()
        {
            _repositoryContext.DbContext.Flush();
        }

        public override T Create(T entity, TId id)
        {
            _repositoryContext.DbContext.Save(entity, id);
            return entity;
        }
    }
}
