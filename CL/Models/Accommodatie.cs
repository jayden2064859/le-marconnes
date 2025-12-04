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
        public int AccommodatieTypeId { get; set; }
        public string PlaatsNummer { get; set; } = "";
        public int Capaciteit { get; set; } 
        public string Status { get; set; } = "Beschikbaar";
    }

    public class AccommodatieType
    {
        public int Id { get; set; }
        public string Naam { get; set; } = "";
    }
}
