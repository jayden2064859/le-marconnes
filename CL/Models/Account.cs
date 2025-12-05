using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Models
{
    public class Account
    {
        public int AccountId { get; set; } 
        public string Gebruikersnaam { get; set; }
        public string WachtwoordHash { get; set; }
        public string Rol { get; set; }
        public int KlantId { get; set; }
        public bool IsActief { get; set; }
        public DateTime RegistratieDatum { get; set; } = DateTime.Now;

    }
}
