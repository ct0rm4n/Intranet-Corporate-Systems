using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using IntraNet.Domain.Entities.SGR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntraNet.Data.Context
{
    public class ContextSGR : DbContext
    {
        public ContextSGR() : base("con_SGR")
        {

        }

        public DbSet<AcaoDemanda> AcaoDemanda { get; set; }
        public DbSet<Assunto> Assunto { get; set; }
        public DbSet<Demanda> Demanda { get; set; }
        public DbSet<AnexoItem> AnexoItem { get; set; }
        public DbSet<ItemAssunto> itemassunto { get; set; }
        public DbSet<ItemReuniao> itemreuniao { get; set; }
        public DbSet<Reuniao> reuniao { get; set; }
        public DbSet<UserReuniao> userreuniao { get; set; }
        public DbSet<UserDemanda> userdemanda { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>;
            modelBuilder.Properties<DateTime>().Configure(p => p.HasColumnType("date"));
            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(250));
            base.OnModelCreating(modelBuilder);
        }

        public static ContextSGR Create()
        {
            return new ContextSGR();
        }
    }
}
