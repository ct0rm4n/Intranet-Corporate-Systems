using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntraNet.Security.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IntraNet.Security.ContextIdentity
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("con_Infra", throwIfV1Schema: false)
        {

        }

        public DbSet<Client> Client { get; set; }
        public DbSet<Claims> Claims { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Unidade> Unidade { get; set; }
        public DbSet<Setor> Setor { get; set; }
        public DbSet<Endereco> Endereco { get; set; }
        public DbSet<Funcao> Funcao { get; set; }
        public static ApplicationDbContext Create()
        {
            //return OnModelCreating();
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>;
            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(p=>p.HasMaxLength(100));
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
