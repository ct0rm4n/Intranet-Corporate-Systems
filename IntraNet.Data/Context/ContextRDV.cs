using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntraNet.Domain.Entities;

namespace IntraNet.Data.Context
{
    public class ContextRDV : DbContext
    {
        public ContextRDV() : base("con_RDV")
        {

        }
        public DbSet<Adiantamento> Adiantamento { get; set; }
        public DbSet<SolicitaEmpresa> SolicitaEmpresas { get; set; }
        public DbSet<DadosBancarios> DadosBancarios { get; set; }
        public DbSet<Despesas> Despesas { get; set; }
        public DbSet<Relatorio> Relatorio { get; set; }
        public DbSet<Empresa> Empresa { get; set; }
        public DbSet<Unidade> Unidade { get; set; }
        public DbSet<Setor> Setor { get; set; }
        public DbSet<EmpCCusto> EmpresaCC { get; set; }
        public DbSet<SetorEmp> SetorEmp { get; set; }
        public DbSet<SolicitaCCusto> SolicitaCusto { get; set; }
        public DbSet<Aprovador> Aprovador { get; set; }
        public DbSet<RateioItem> RateioItems { get; set; }
        public DbSet<Financeiro> Financeiro { get; set; }
        public DbSet<DespesaAnexo> DespesaAnexo { get; set; }
        public DbSet<TipoDespesa> TipoDespesa { get; set; }
        public DbSet<KMValor> KMValor { get; set; }
        

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingEntitySetNameConvention>();
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>;
            modelBuilder.Properties<DateTime>().Configure(p => p.HasColumnType("date"));
            modelBuilder.Properties<string>().Configure(p => p.HasColumnType("varchar"));
            modelBuilder.Properties<string>().Configure(p => p.HasMaxLength(100));            
            base.OnModelCreating(modelBuilder);
        }
        
        public static ContextRDV Create()
        {
            return new ContextRDV();
        }

    }
}
