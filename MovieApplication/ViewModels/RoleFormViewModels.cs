using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApplication.ViewModels
{
    public class RoleFormViewModels
    {
        [Required, StringLength(265) ]
        public string Name { get; set; }
    }
}
