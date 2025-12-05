using CL.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Xml;

namespace LeMarconnes
{
    class Program
    {
        static HttpClient client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7279/")
        };

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== LE MARCONNÈS ===");

            bool doorgaan = true;
            while (doorgaan)
            {
                Console.WriteLine("\n1. Toon alle reserveringen");
                Console.WriteLine("2. Zoek reservering op id");
                Console.WriteLine("3. Nieuwe reservering maken");
                Console.WriteLine("4. Reservering updaten");
                Console.WriteLine("5. Reservering verwijderen");
                Console.WriteLine("6. Afsluiten");
                Console.Write("\nKeuze: ");

                string keuze = Console.ReadLine();

                switch (keuze)
                {
                    case "1":
                        await ToonReserveringen();
                        break;
                    case "2": 
                        await ZoekReserveringOpId();
                        break;
                    case "3":
                        await MaakReservering();
                        break;
                    case "4":
                        await UpdateReservering();
                        break;
                    case "5":
                        await VerwijderReservering();
                        break;
                    case "6":
                        doorgaan = false;
                        break;
                }
            }
        }

        static async Task ToonReserveringen()
        {
            Console.WriteLine("\n--- RESERVERINGEN ---");

            try
            {
                var response = await client.GetAsync("api/Reserveringen");

                if (response.IsSuccessStatusCode)
                {
                    var reserveringen = await response.Content.ReadFromJsonAsync<List<Reservering>>();

                    Console.WriteLine($"Totaal aantal: {reserveringen.Count}\n");

                    foreach (var res in reserveringen)
                    {
                        Console.WriteLine($"ID: {res.ReserveringId}");
                        Console.WriteLine($"Klant: {res.KlantId}, Accommodatie: {res.AccommodatieId}");
                        Console.WriteLine($"Van: {res.StartDatum:dd-MM} tot {res.EindDatum:dd-MM}");
                        Console.WriteLine($"Aantal volwassenen: {res.AantalVolwassenen}");
                        Console.WriteLine($"Aantal kinderen 0-7: {res.AantalKinderen0_7}");
                        Console.WriteLine($"Aantal kinderen 7-12: {res.AantalKinderen7_12}");
                        Console.WriteLine($"Prijs: {res.TotaalPrijs},-");
                        Console.WriteLine(new string('-', 30));
                        Console.WriteLine("\n");
                    }
                }

            }
            catch
            {
                Console.WriteLine("API niet bereikbaar");
            }
        }

        static async Task ZoekReserveringOpId()
        {

            bool finished = false;
            while (!finished)
            {
                Console.WriteLine("\nVoer id in van reservering die je wil ophalen: ");
                int id = int.Parse(Console.ReadLine());


                try
                {
                    var response = await client.GetAsync($"api/Reserveringen/{id}");

                    if (response.IsSuccessStatusCode)
                    {
                        var res = await response.Content.ReadFromJsonAsync<Reservering>();

                        Console.WriteLine($"\n--- RESERVERING {id} GEVONDEN ---");
                        Console.WriteLine($"ID: {res.ReserveringId}");
                        Console.WriteLine($"Klant: {res.KlantId}");
                        Console.WriteLine($"Accommodatie: {res.AccommodatieId}");
                        Console.WriteLine($"Van: {res.StartDatum:dd-MM-yyyy} tot {res.EindDatum:dd-MM-yyyy}");
                        Console.WriteLine($"Volwassenen: {res.AantalVolwassenen}");
                        Console.WriteLine($"Kinderen 0-7: {res.AantalKinderen0_7}");
                        Console.WriteLine($"Kinderen 7-12: {res.AantalKinderen7_12}");
                        Console.WriteLine($"Hond: {res.AantalHonden}");
                        Console.WriteLine($"Elektriciteit: {(res.HeeftElectriciteit ? "Ja" : "Nee")}");
                        Console.WriteLine($"Prijs: {res.TotaalPrijs},-");
                        Console.WriteLine($"Status: {res.Status}");
                        Console.WriteLine(new string('-', 30));
                        Console.WriteLine("\n");

                        finished = true;
                    }
                    else
                    {
                        Console.WriteLine($"Reservering met id {id} niet gevonden");
                    }
                }
                catch
                {
                    Console.WriteLine("\nEr is iets misgegaan\n");
                }
            }
           
        }

        static async Task MaakReservering()
        {
            Console.WriteLine("\n--- NIEUWE RESERVERING ---");

            Reservering nieuweReservering = new Reservering();

             Console.Write("Klant ID: ");
            int klantId = int.Parse(Console.ReadLine());

            nieuweReservering.KlantId = klantId;

               
            nieuweReservering.AccommodatieId = 1; // alleen camping word voor nu uitgewerkt, dus accommodatietype is altijd 1 (1=camping)

            Console.Write("Startdatum (dd-mm-jjjj): ");
            string startInput = Console.ReadLine();
            nieuweReservering.StartDatum = DateTime.ParseExact(startInput, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            Console.Write("Einddatum (dd-mm-jjjj): ");
            string eindInput = Console.ReadLine();
            nieuweReservering.EindDatum = DateTime.ParseExact(eindInput, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            Console.Write("Aantal volwassenen: ");
            nieuweReservering.AantalVolwassenen = int.Parse(Console.ReadLine());

            Console.Write("Aantal kinderen 0-7 jaar: ");
            nieuweReservering.AantalKinderen0_7 = int.Parse(Console.ReadLine());

            Console.Write("Aantal kinderen 7-12 jaar: ");
            nieuweReservering.AantalKinderen7_12 = int.Parse(Console.ReadLine());

            Console.Write("Hond mee? (j/n): ");
            nieuweReservering.AantalHonden = int.Parse(Console.ReadLine());

            Console.Write("Elektriciteit gewenst? (j/n): ");
            nieuweReservering.HeeftElectriciteit = Console.ReadLine().ToLower() == "j";

            if (nieuweReservering.HeeftElectriciteit)
            {
                Console.Write($"Voor hoeveel nachten elektriciteit? (max {nieuweReservering.AantalNachten}): ");
                nieuweReservering.AantalDagenElectriciteit = int.Parse(Console.ReadLine());
            }

            nieuweReservering.Status = "Gereserveerd";

            try
            {
                var response = await client.PostAsJsonAsync("api/Reserveringen", nieuweReservering);

                if (response.IsSuccessStatusCode)
                {
                    var gemaakt = await response.Content.ReadFromJsonAsync<Reservering>();
                    Console.WriteLine($"\nAangemaakt! Prijs: {gemaakt.TotaalPrijs},-\n");
                }
            }
            catch
            {
                Console.WriteLine("\nFout bij aanmaken\n");
            }
        }


        static async Task VerwijderReservering()
        {
            Console.WriteLine("\nVoer Id in van reservering die je wil verwijderen: ");
            int id = int.Parse(Console.ReadLine());
           
            try
            {
                var response = await client.DeleteAsync($"api/Reserveringen/{id}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"\nReservering met id {id} succesvol verwijderd\n");
                }
                else
                {
                    Console.WriteLine($"\nReservering met id {id} niet gevonden\n");
                }
            }
            catch
            {
                Console.WriteLine("\nEr is iets misgegaan\n");
            }
        }

        static async Task UpdateReservering()
        {
            Console.Write("\nVoer Id in van reservering die je wil wijzigen: ");
            int id = int.Parse(Console.ReadLine());

            try
            {
                var responseGet = await client.GetAsync($"api/Reserveringen/{id}");

                if (!responseGet.IsSuccessStatusCode)
                {
                    Console.WriteLine($"\nReservering met id {id} niet gevonden.\n");
                    return;
                }

                var res = await responseGet.Content.ReadFromJsonAsync<Reservering>();

                bool finished = false;
                while (!finished)
                {
                    Console.WriteLine("\nWelke waarde wil je wijzigen?");
                    Console.WriteLine(new string('-', 30));
                    Console.WriteLine($"1.  KlantId: {res.KlantId}");
                    Console.WriteLine($"2.  AccommodatieId: {res.AccommodatieId}");
                    Console.WriteLine($"3.  StartDatum: {res.StartDatum:yyyy-MM-dd}");
                    Console.WriteLine($"4.  EindDatum: {res.EindDatum:yyyy-MM-dd}");
                    Console.WriteLine($"5.  AantalVolwassenen: {res.AantalVolwassenen}");
                    Console.WriteLine($"6.  AantalKinderen0_7: {res.AantalKinderen0_7}");
                    Console.WriteLine($"7.  AantalKinderen7_12: {res.AantalKinderen7_12}");
                    Console.WriteLine($"8.  Hond: {res.AantalHonden}");
                    Console.WriteLine($"9.  Elektriciteit: {res.HeeftElectriciteit}");
                    Console.WriteLine($"10. AantalDagenElektriciteit: {res.AantalDagenElectriciteit}");
                    Console.WriteLine($"11. Status: {res.Status}");
                    Console.WriteLine(new string('-', 30));

                    Console.WriteLine("0. Terug naar menu");


                    Console.Write("\nKeuze: ");
                    string keuze = Console.ReadLine();

                    switch (keuze)
                    {
                        case "1":
                            Console.Write("Nieuwe KlantId: ");
                            res.KlantId = int.Parse(Console.ReadLine());
                            break;
                        case "2":
                            Console.Write("Nieuwe AccommodatieId: ");
                            res.AccommodatieId = int.Parse(Console.ReadLine());
                            break;
                        case "3":
                            Console.Write("Nieuwe StartDatum (yyyy-mm-dd): ");
                            res.StartDatum = DateTime.Parse(Console.ReadLine());
                            break;
                        case "4":
                            Console.Write("Nieuwe EindDatum (yyyy-mm-dd): ");
                            res.EindDatum = DateTime.Parse(Console.ReadLine());
                            break;
                        case "5":
                            Console.Write("Nieuwe AantalVolwassenen: ");
                            res.AantalVolwassenen = int.Parse(Console.ReadLine());
                            break;
                        case "6":
                            Console.Write("Nieuwe AantalKinderen0_7: ");
                            res.AantalKinderen0_7 = int.Parse(Console.ReadLine());
                            break;
                        case "7":
                            Console.Write("Nieuwe AantalKinderen7_12: ");
                            res.AantalKinderen7_12 = int.Parse(Console.ReadLine());
                            break;
                        case "8":
                            Console.Write("Hond (true/false): ");
                            res.AantalHonden = int.Parse(Console.ReadLine());
                            break;
                        case "9":
                            Console.Write("Elektriciteit (true/false): ");
                            res.HeeftElectriciteit = bool.Parse(Console.ReadLine());
                            break;
                        case "10":
                            Console.Write("AantalDagenElektriciteit: ");
                            res.AantalDagenElectriciteit = int.Parse(Console.ReadLine());
                            break;
                        case "11":
                            Console.Write("Nieuwe Status: ");
                            res.Status = Console.ReadLine();
                            break;
                        case "0":
                            finished = true;
                           
                            break;
                        default:
                            Console.WriteLine("Ongeldige keuze.");
                            break;
                    }
                }

                var responsePut = await client.PutAsJsonAsync($"api/Reserveringen/{id}", res);

                if (responsePut.IsSuccessStatusCode && finished != true)
                {
                    Console.WriteLine($"\nReservering met id {id} succesvol bijgewerkt!\n");
                }
                else if (!responsePut.IsSuccessStatusCode && finished != true)
                {                    
                    Console.WriteLine("\nEr is een fout opgetreden bij het bijwerken.\n");
                }
            }
            catch
            {
                Console.WriteLine("\nFout bij verbinden met API.\n");
            }
        }


    }
}