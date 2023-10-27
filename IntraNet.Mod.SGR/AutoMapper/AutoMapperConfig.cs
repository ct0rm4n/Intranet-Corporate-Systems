using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IntraNet.Mod.SGR.AutoMapper
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {            
            Mapper.Initialize(x =>
            {
                x.AddProfile<DomainToViewModelMappingProfile>();
                x.AddProfile<ViewModelToDomainMappingProfile>();
                x.ValidateInlineMaps = false;
            });
        }
    }
}