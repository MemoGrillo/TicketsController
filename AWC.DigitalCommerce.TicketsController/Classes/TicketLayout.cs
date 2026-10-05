using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWC.DigitalCommerce.TicketsController.Classes
{
    public class TicketLayout
    {
        public float QtyColumn { get; set; }

        public float DescriptionColumn { get; set; }

        public float PriceColumn { get; set; }

        public TicketLayout(int printableWidth)
        {
            QtyColumn = printableWidth * 0.08f;
            DescriptionColumn = printableWidth * 0.18f;
            PriceColumn = printableWidth * 0.98f;
        }
    }
}
