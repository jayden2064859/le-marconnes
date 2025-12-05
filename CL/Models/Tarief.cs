using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Models
{
    public class Tarief
    {
        public int TariefId { get; set; }
        public int AccommodatieTypeId { get; set; }
        public string Type { get; set; }
        public decimal Prijs { get; set; }
        
    }
}
