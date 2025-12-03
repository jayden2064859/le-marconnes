using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Models
{
    public class Accommodatie
    {
        public int Id { get; set; }
        public string TypeId { get; set; } // 1 = camping, 2 = hotel, etc
        public int MaxPersonen { get; set; }
        public bool IsBeschikbaar { get; set; } = true;
    }

    public class AccommodatieType
    {
        public int Id { get; set; }
        public string Naam { get; set; } = "";
    }
}
