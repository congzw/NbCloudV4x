using System;
using System.Data;
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace NbCloud.Common.Data.Provider.Nhibernate
{
    public class NhRepositoryContext : RepositoryContextBase, IRepositoryContext
    {
        private ISession _dbContext;
        private ITransaction _transaction;
        private bool _cancelled;
        public NhRepositoryContext(ISession session)
        {
            _dbContext = session;
        }

        public override T Insert<T>(T entity)
        {
            DbContext.Save(entity);
            return entity;
        }

        public override T Update<T>(T entity)
        {
            DbContext.Update(entity);
            return entity;
        }

        public override T Get<T>(object id)
        {
            return DbContext.Get<T>(id);
        }

        public override void Deleted<T>(T entity)
        {
            DbContext.Delete(entity);
        }

        public override IQueryable<T> Query<T>()
        {
            return DbContext.Query<T>();
        }

        public override void Cancel()
        {
            this._cancelled = true;
        }

        public override void Commit()
        {
            this._cancelled = false;
        }

        public ISession DbContext
        {
            get
            {
                LogMessage("CTOR() => NhRepositoryContext => _transaction.BeginTransaction()");
                _transaction = _dbContext.BeginTransaction();
                return this._dbContext;
            }
        }

        public override IRepository<T, TKey> CreateRepository<T, TKey>()
        {
            return new NhRepository<T, TKey>(this);
        }

        public override void RequireNew(IsolationLevel level)
        {
            string message = "";
            if (_transaction != null)
            {
                try
                {
                    if (!_cancelled)
                    {
                        //LogMessage("RequireNew() => _transaction.Commit()");
                        message = "RequireNew() => _transaction.Commit(); ";
                        _transaction.Commit();
                    }
                    else
                    {
                        //LogMessage("RequireNew() => _transaction.Rollback()");
                        message = "RequireNew() => _transaction.Rollback(); ";
                        _transaction.Rollback();
                    }
                }
                catch (Exception e)
                {
                    LogMessage("RequireNew() => Error: " + e.Message);
                    throw;
                }
                finally
                {
                    _transaction.Dispose();
                    _transaction = null;
                    _cancelled = false;
                }
            }

            if (_dbContext != null)
            {
                message = message + "=> BeginTransaction()";
                this._transaction = _dbContext.BeginTransaction(level);
            }
            LogMessage(message);
        }

        private bool _disposed = false;
        protected override void Dispose(bool disposing)
        {
            //本类内部没有封装任何托管或非托管资源
            if (!_disposed)
            {
                if (disposing)
                {
                    //处理托管资源
                    DisposeIt();
                }
                //处理非托管资源
            }
            _disposed = true;
        }

        private void DisposeIt()
        {
            if (_transaction != null)
            {
                try
                {
                    if (!_cancelled)
                    {
                        LogMessage("Dispose() => _transaction.Commit()");
                        _transaction.Commit();
                    }
                    else
                    {
                        LogMessage("Dispose() => _transaction.Rollback()");
                        _transaction.Rollback();
                    }
                }
                catch (Exception e)
                {
                    if (!RepositoryContextDisposeHelper.HideDisposingException)
                    {
                        LogMessage("Dispose() => Error: " + e.Message);
                        RepositoryContextDisposeHelper.HideDisposingException = false;
                        throw;
                    }
                    else
                    {
                        LogMessage("Dispose() => Error Hide: " + e.Message);
                    }
                }
                finally
                {
                    _transaction.Dispose();
                    _transaction = null;
                    _cancelled = false;
                }
            }

            if (ShouldReleaseByHand || ShouldReleaseThisByHand)
            {
                if (_dbContext != null)
                {
                    LogMessage(string.Format(">>>> {0} Session Dispose() ByHand. ", _dbContext.GetHashCode()));
                    _dbContext.Connection.Close();
                    _dbContext.Connection.Dispose();
                    _dbContext.Close();
                    _dbContext.Dispose();
                    _dbContext = null;
                }
            }
        }

        private void LogMessage(string message)
        {
            //if (MyDebugHelper.IsDebugMode())
            //{
            //    var myHttpContextHelper = new MyHttpContextHelper();
            //    string url;
            //    myHttpContextHelper.IsRequestAvailable(out url);
            //    UtilsLogger.LogMessage(string.Format("[{0}] > {1} > {2}", url, this.GetHashCode(), message));
            //}
        }

        #region for di setting

        /// <summary>
        /// 是否应该自己负责释放
        /// </summary>
        public bool ShouldReleaseThisByHand { get; set; }
        /// <summary>
        /// 是否应该自己负责释放
        /// </summary>
        public static bool ShouldReleaseByHand { get; set; }

        static NhRepositoryContext()
        {
            ShouldReleaseByHand = false;
        }

        #endregion
    }
}
