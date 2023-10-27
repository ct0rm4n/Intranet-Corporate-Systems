using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Data.Context
{
    public class HangfireContext : DbContext
    {
        public HangfireContext() : base("HangfireContext") // Remove "name="
        {
            Database.SetInitializer<HangfireContext>(null);
            //Database.CreateIfNotExists();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
        }
        public static HangfireContext Create()
        {
            return new HangfireContext();
        }

    }
}
