using System;
using System.Collections.Generic;
using System.Linq;
using NbCloud.Common.Data.Model;

namespace NbCloud.Common.Data
{
    public interface IRepositoryContext : ITransactionManager, IDisposable
    {
        /// <summary>
        /// Gets the unique-identifier of the repository context.
        /// </summary>
        Guid Id { get; }

        IDictionary<string, IRepository> RepositoryTable { get; }
        IRepository<T, TKey> GetRepository<T, TKey>() where T : class ,INbEntity<TKey>;
        IRepository<T, TKey> CreateRepository<T, TKey>() where T :class, INbEntity<TKey>;

        T Insert<T>(T entity) where T : class;
        void Deleted<T>(T entity) where T : class;
        T Update<T>(T entity) where T : class;
        T Get<T>(object id) where T : class; 
        IQueryable<T> Query<T>() where T : class;
    }
}
