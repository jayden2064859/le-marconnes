using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Models
{
    public class Klant
    {
        public int Id { get; set; }
        public string Voornaam { get; set; } = "";
        public string Tussenvoegsel { get; set; } = "";
        public string Achternaam { get; set; } = "";
        public string Email{ get; set; } = "";
        public string Telefoonnummer { get; set; } = "";
        public string Adres { get; set; } = "";
        public DateTime RegistratieDatum {  get; set; } = DateTime.Now;
    }
}
