using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NbCloud.Common.Data
{
    public interface IRepository
    {
        Guid Id { get; }
        IRepositoryContext Context { get; }
        void Flush();
    }

    public interface IRepository<T, in TKey> : IRepository
    {
        T Create(T entity);
        T Create(T entity, TKey id);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        void Delete(TKey id);
        T Update(T entity);
        T Get(TKey id);
        T Get(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();
        /// <summary>
        /// 所有数据
        /// </summary>
        IQueryable<T> Table { get; }
        int Count(Expression<Func<T, bool>> predicate);
        IEnumerable<T> Fetch(Expression<Func<T, bool>> predicate);
    }
}