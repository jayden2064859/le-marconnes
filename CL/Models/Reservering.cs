using System;

namespace CL.Models
{
    public class Reservering
    {
        public int Id { get; set; }
        public int KlantId { get; set; }
        public int AccommodatieId { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime EindDatum { get; set; }
        public int AantalVolwassenen { get; set; }
        public int AantalKinderen0_7 { get; set; }
        public int AantalKinderen7_12 { get; set; }

        public int AantalHonden { get; set; } = 0; 
        public bool HeeftElectriciteit { get; set; } = false; 
        public int AantalDagenElectriciteit { get; set; } = 0; 

        public decimal TotaalPrijs { get; set; }
        public string Status { get; set; } = "Bevestigd"; 
        public DateTime DatumAangemaakt { get; set; } = DateTime.Now;

        public int AantalNachten => (EindDatum - StartDatum).Days;
    }
}
