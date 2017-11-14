using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NbCloud.Common.Data.Model;

namespace NbCloud.Common.Data
{
    public abstract class RepositoryBase<T, TId> : IRepository<T, TId> where T : class,INbEntity<TId>
    {
        #region 私有字段
        private readonly IRepositoryContext _context;
        private readonly Guid _id = Guid.NewGuid();
        #endregion

        #region 构造函数

        protected RepositoryBase(IRepositoryContext context)
        {
            this._context = context;
            var key = typeof(T).ToString() + typeof(TId);
            if (!context.RepositoryTable.ContainsKey(key))
            {
                context.RepositoryTable.Add(key, this);
            }
        }

        #endregion

        public IRepositoryContext Context
        {
            get { return _context; }
        }

        public virtual T Create(T entity)
        {
            return _context.Insert(entity);
        }

        public virtual T Update(T entity)
        {
            return _context.Update(entity);
        }

        public virtual void Delete(T entity)
        {
            _context.Deleted(entity);
        }

        public abstract T Get(TId id);

        public abstract T Get(Expression<Func<T, bool>> predicate);

        public abstract IQueryable<T> GetAll();

        #region 主流框架都实现了延迟加载 只需实现 GetAll 下面这些都不需要自己实现

        public virtual IQueryable<T> Table
        {
            get { return GetAll(); }
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return Table.Count(predicate);
        }

        public virtual IEnumerable<T> Fetch(Expression<Func<T, bool>> predicate)
        {
            return Table.Where(predicate);
        }

        #endregion

        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            var items = Table.Where(predicate);
            foreach (var item in items)
            {
                Delete(item);
            }
        }

        public abstract void Delete(TId id);

        public Guid Id
        {
            get { return _id; }
        }

        public abstract void Flush();

        public abstract T Create(T entity, TId id);

    }
}
