using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWC.DigitalCommerce.TicketsController.Classes
{
    public class FECustomer
    {
        [Key]
        public int Id { get; set; }

        public int SSNType { get; set; }

        public string SSN{ get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int State { get; set; }

        public int County { get; set; }

        public int District { get; set; }

        public string EconomicActivity { get; set; }
    }
}
