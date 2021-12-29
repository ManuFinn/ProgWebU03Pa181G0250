using System.Collections.Generic;
using U3RazasPerros.Models;

namespace U3RazasPerros.Areas.Admin.Models
{
    public class AgregarRazaViewModel
    {
        public IEnumerable<Paises> Paises { get; set; }

        public Razas Razas { get; set; }

    }
}
