using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace FirePuckStore.DAL
{
    public class DataConfig
    {
        public static void Configure()
        {
            Database.SetInitializer(new SampleDataInitializer());
        }
    }
}
