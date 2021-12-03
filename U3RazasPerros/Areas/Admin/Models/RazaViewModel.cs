using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace U3RazasPerros.Areas.Admin.Models
{
    public class RazaViewModel
    {
        public IEnumerable<U3RazasPerros.Models.Paises> Paises { get; set; }
        public IEnumerable<U3RazasPerros.Models.Razas> Razas { get; set; }
    }
}
