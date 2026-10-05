using AWC.DigitalCommerce.TicketsController.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWC.DigitalCommerce.TicketsController.Classes
{
    public abstract class CatalogoBase
    {
        public int Id { get; set; }

        public string Descripcion { get; set; } = string.Empty;

        public override string ToString()
        {
            return Descripcion;
        }
    }

    public class TipoIdentificacion : CatalogoBase
    {
    }
    public class Provincia : CatalogoBase
    {
    }

    public class Canton : CatalogoBase
    {
        public int ProvinciaId { get; set; }
    }
    public class Distrito : CatalogoBase
    {
        public int CantonId { get; set; }
    }
}
