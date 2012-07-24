using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirePuckStore.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<T> : IDisposable
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int entityId);
        T GetByIdAsNoTracking(int playerId);
    }
}
