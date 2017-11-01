using System.Collections.Generic;
using NbCloud.Common;

namespace NbCloud.BaseLib.Tentants
{
    public interface ITenantRepository : IDependency
    {
        IList<Tenant> GetAll();
        void Add(Tenant tenant);
        void Add(IList<Tenant> tenants);
        void Remove(Tenant tenant);
        void Update(Tenant tenant);
    }

    ////todo
    //public class TenantRepository : ITenantRepository
    //{
        
    //}
}