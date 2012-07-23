using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace FirePuckStore.DAL.Repositories.Implementation
{
    public class AbstractRepository : IDisposable
    {
        protected PuckStoreDbContext DbContext { get; private set; }

        protected AbstractRepository()
        {
            DbContext = new PuckStoreDbContext();
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
