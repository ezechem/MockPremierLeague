using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MockPremierLeague.API.Contracts
{
    public interface IBaseRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void BulkDelete<T>(IList<T> entities) where T : class;
        Task<bool> SaveAll();
    }
}
