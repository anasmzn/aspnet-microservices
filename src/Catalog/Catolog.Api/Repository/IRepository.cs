using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<T>> GetAll<T>();
        Task<T> Get<T>(string id);
    }
}
