using CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CL.Services
{
    public static class TariefCalculator
{

    public static decimal TotaalPrijs(Reservering reservering, List<Tarief> tarieven)
    {
        int aantalNachten = reservering.AantalNachten;
        decimal totaal = 0;


        var campingTarieven = tarieven.Where(t => t.AccommodatieTypeId == 1).ToList();

 
        var campingplaats = campingTarieven.First(t => t.Type == "Campingplaats").Prijs;
        var volwassene = campingTarieven.First(t => t.Type == "Volwassene").Prijs;
        var kind07 = campingTarieven.First(t => t.Type == "Kind_0_7").Prijs;
        var kind712 = campingTarieven.First(t => t.Type == "Kind_7_12").Prijs;
        var hond = campingTarieven.First(t => t.Type == "Hond").Prijs;
        var electriciteit = campingTarieven.First(t => t.Type == "Electriciteit").Prijs;
        var toeristenbelasting = campingTarieven.First(t => t.Type == "Toeristenbelasting").Prijs;

        // prijsberekening campingplaats per nacht
        totaal = campingplaats * aantalNachten;

        // per persoon per nacht
        totaal += volwassene * reservering.AantalVolwassenen * aantalNachten;
        totaal += kind07 * reservering.AantalKinderen0_7 * aantalNachten;
        totaal += kind712 * reservering.AantalKinderen7_12 * aantalNachten;


        int totaalPersonen = reservering.AantalVolwassenen +
                             reservering.AantalKinderen0_7 +
                             reservering.AantalKinderen7_12;
        totaal += toeristenbelasting * totaalPersonen * aantalNachten;

        
       // per hond per nacht
       if (reservering.AantalHonden > 0)
       {
            totaal += hond * reservering.AantalHonden * aantalNachten;

       }
                

        // electriciteit per nacht (hoeft niet de hele verblijfperiode te zijn)
        if (reservering.HeeftElectriciteit && reservering.AantalDagenElectriciteit > 0)
        {
            totaal += electriciteit * reservering.AantalDagenElectriciteit;
        }

        return Math.Round(totaal, 2);
    }
}
}
