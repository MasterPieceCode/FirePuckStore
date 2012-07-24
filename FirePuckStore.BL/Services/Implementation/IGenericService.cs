using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FirePuckStore.BL.Services.Implementation
{
    public interface IGenericService<T> : IDisposable
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int entityId);
    }
}
