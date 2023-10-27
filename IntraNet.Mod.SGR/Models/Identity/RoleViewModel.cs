﻿using System.ComponentModel.DataAnnotations;

namespace IntraNet.Mod.SGR.Models.Identity
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Nome da Role")]
        public string Name { get; set; }
    }
}