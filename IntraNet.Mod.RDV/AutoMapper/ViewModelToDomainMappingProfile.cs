using AutoMapper;
using IntraNet.Domain.Entities;
using IntraNet.Mod.RDV.Models.ViewModel;

namespace IntraNet.Mod.RDV.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappingProfile"; }
        }
        protected virtual void Configure()
        {
            base.CreateMap<DadosBancarios,DadosBancariosViewModel>();
            base.CreateMap<Setor,SetorViewModel>();
            base.CreateMap<Despesas,DespesasViewModel>();
            base.CreateMap<DespesaAnexo, DespesaAnexoViewModel>();
            base.CreateMap<EmpCCusto,EmpCCustoViewModel>();
            base.CreateMap<SetorEmp,SetorEmpViewModel>();
            base.CreateMap<Empresa,EmpresaViewModel>();
            base.CreateMap<Relatorio, RelatorioViewModel>();
            base.CreateMap<Unidade,UnidadeViewModel>();
            base.CreateMap<RateioItem,RateioItemViewModel>();
            base.CreateMap<Adiantamento, AdiantamentoViewModel>();
        }
    }
}