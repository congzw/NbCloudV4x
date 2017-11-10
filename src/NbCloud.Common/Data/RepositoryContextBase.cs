using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NbCloud.Common.Data.Model;

namespace NbCloud.Common.Data
{
    public abstract class RepositoryContextBase : IRepositoryContext
    {
        /// <summary>
        /// 位置标记
        /// 用于在集合中标志自己
        /// EX:items[RepositoryContextBase.ContextKey]
        /// </summary>
        public const string ContextKey = "NbCloud.Common.Data.RepositoryContextBase.ContextKey";
        protected RepositoryContextBase()
        {
            _repositoryTable = new Dictionary<string, IRepository>();
            IsolationLevel = IsolationLevel.ReadCommitted;
        }

        #region Private Fields
        private readonly Guid _id = Guid.NewGuid();
        private readonly IDictionary<string, IRepository> _repositoryTable;
        #endregion
        
        public Guid Id
        {
            get { return this._id; }
        }

        public IDictionary<string, IRepository> RepositoryTable
        {
            get { return _repositoryTable; }
        }

        public virtual IRepository<T, TKey> GetRepository<T, TKey>() where T : class, INbEntity<TKey>
        {
            IRepository<T, TKey> repository = null;
            string key = typeof(T) + typeof(TKey).ToString();

            if (_repositoryTable.ContainsKey(key))
                repository = _repositoryTable[key] as IRepository<T, TKey>;
            else
            {
                repository = CreateRepository<T, TKey>();
                if (!_repositoryTable.ContainsKey(key))
                {
                    _repositoryTable.Add(key, repository);
                }
            }

            return repository;
        }

        public abstract IRepository<T, TKey> CreateRepository<T, TKey>() where T : class,INbEntity<TKey>;
        
        public abstract T Insert<T>(T entity) where T : class;

        public abstract T Update<T>(T entity) where T : class;

        public abstract void Deleted<T>(T entity) where T : class;

        public abstract IQueryable<T> Query<T>() where T : class;

        public abstract T Get<T>(object id) where T : class;

        #region tx
        
        public IsolationLevel IsolationLevel { get; set; }

        public virtual void RequireNew()
        {
            this.RequireNew(IsolationLevel);
        }

        public abstract void RequireNew(System.Data.IsolationLevel level);

        public abstract void Cancel();

        public abstract void Dispose();

        public abstract void Commit();

        #endregion

    }
}
