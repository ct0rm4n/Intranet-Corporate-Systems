using AutoMapper;
using IntraNet.Domain.Entities;
using IntraNet.Mod.RDV.Models.ViewModel;

namespace IntraNet.Mod.RDV.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappingProfile"; }
        }
        protected virtual void Configure()
        {
            base.CreateMap<DadosBancariosViewModel, DadosBancarios>();
            base.CreateMap<SetorViewModel, Setor>();
            base.CreateMap<DespesasViewModel, Despesas>();
            base.CreateMap<DespesaAnexoViewModel, DespesaAnexo>();
            base.CreateMap<EmpCCustoViewModel, EmpCCusto>();
            base.CreateMap<SetorEmpViewModel, SetorEmp>();
            base.CreateMap<EmpresaViewModel, Empresa>();
            base.CreateMap<RelatorioViewModel, Relatorio>();
            base.CreateMap<UnidadeViewModel, Unidade>();
            base.CreateMap<RateioItemViewModel, RateioItem>();
            base.CreateMap<AdiantamentoViewModel, Adiantamento>();
        }
    }
}