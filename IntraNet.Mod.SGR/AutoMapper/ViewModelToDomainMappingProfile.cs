using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using IntraNet.Domain.Entities;
using IntraNet.Domain.Entities.SGR;
using IntraNet.Mod.SGR.Models.Identity;
using IntraNet.Mod.SGR.Models.ViewModel;
using IntraNet.Security.Models;

namespace IntraNet.Mod.SGR.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "DomainToViewModelMappingProfile"; }
        }
        protected virtual void Configure()
        {
            base.CreateMap<Reuniao, ReuniaoViewModel>();
            base.CreateMap<Demanda, DemandaViewModel>();
            base.CreateMap<Demanda, EditDemandaViewModel>();
            base.CreateMap<Endereco, EnderecoViewModel>();
            base.CreateMap<ItemAssunto, ItemAssuntoViewModel>();
        }
    }
}