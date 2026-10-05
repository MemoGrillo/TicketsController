using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWC.DigitalCommerce.TicketsController.Classes
{
    public static class Catalogos
    {
        public static List<TipoIdentificacion> TiposIdentificacion()
        {
            return new List<TipoIdentificacion>
            {
                new TipoIdentificacion { Id = 1, Descripcion = "Física" },
                new TipoIdentificacion { Id = 2, Descripcion = "Jurídica" },
                new TipoIdentificacion { Id = 3, Descripcion = "DIMEX" },
                new TipoIdentificacion { Id = 4, Descripcion = "NITE" },
                new TipoIdentificacion { Id = 5, Descripcion = "Extranjero" }
            };
        }
    }
}
