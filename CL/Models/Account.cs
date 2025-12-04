using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Gebruikersnaam { get; set; } = "";
        public string WachtwoordHash { get; set; } = "";
        public string Rol { get; set; } = "Klant";
        public int? KlantId { get; set; }
        public bool IsActief { get; set; } = true;
        public DateTime DatumAangemaakt { get; set; } = DateTime.Now;
    }
}
