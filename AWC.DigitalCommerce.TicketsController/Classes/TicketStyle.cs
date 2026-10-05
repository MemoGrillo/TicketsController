using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWC.DigitalCommerce.TicketsController.Classes
{
    public class TicketStyle
    {
        public int MarginLeft { get; set; } = 10;

        public int MarginRight { get; set; } = 10;

        public int TopMargin { get; set; } = 10;

        public int LineSpacing { get; set; } = 4;

        public int Width { get; }

        public int ContentWidth => Width - MarginLeft - MarginRight;

        public TicketStyle(int paperWidth)
        {
            Width = paperWidth;
        }
    }
}
