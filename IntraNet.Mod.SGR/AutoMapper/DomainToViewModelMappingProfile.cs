using AutoMapper;
using IntraNet.Domain.Entities.SGR;
using IntraNet.Mod.SGR.Models.Identity;
using IntraNet.Mod.SGR.Models.ViewModel;
using IntraNet.Security.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public override string ProfileName
        {
            get { return "ViewModelToDomainMappingProfile"; }
        }
        protected virtual void Configure()
        {

            base.CreateMap< CreateReuniaoViewModel ,Reuniao>();
            base.CreateMap<DemandaViewModel, Demanda>();
            base.CreateMap<EditDemandaViewModel, Demanda>();
            base.CreateMap<EnderecoViewModel, Endereco>();
            base.CreateMap<ItemAssuntoViewModel,ItemAssunto>();
        }
    }
}