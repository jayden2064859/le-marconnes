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
                Console.WriteLine("1. Toon alle reserveringen");
                Console.WriteLine("2. Zoek reservering op id");
                Console.WriteLine("3. Nieuwe reservering maken");
                Console.WriteLine("4. Reservering verwijderen");
                Console.WriteLine("5. Afsluiten");
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
                        await VerwijderReservering();
                        break;
                    case "5":
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
                        Console.WriteLine($"ID: {res.Id}");
                        Console.WriteLine($"Klant: {res.KlantId}, Accommodatie: {res.AccommodatieId}");
                        Console.WriteLine($"Van: {res.StartDatum:dd-MM} tot {res.EindDatum:dd-MM}");
                        Console.WriteLine($"Aantal volwassenen: {res.AantalVolwassenen}");
                        Console.WriteLine($"Aantal kinderen 0-7: {res.AantalKinderen0_7}");
                        Console.WriteLine($"Aantal kinderen 7-12: {res.AantalKinderen7_12}");
                        Console.WriteLine($"Prijs: {res.TotaalPrijs},-");
                        Console.WriteLine(new string('-', 30));
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
                        Console.WriteLine($"ID: {res.Id}");
                        Console.WriteLine($"Klant: {res.KlantId}");
                        Console.WriteLine($"Accommodatie: {res.AccommodatieId}");
                        Console.WriteLine($"Van: {res.StartDatum:dd-MM-yyyy} tot {res.EindDatum:dd-MM-yyyy}");
                        Console.WriteLine($"Volwassenen: {res.AantalVolwassenen}");
                        Console.WriteLine($"Kinderen 0-7: {res.AantalKinderen0_7}");
                        Console.WriteLine($"Kinderen 7-12: {res.AantalKinderen7_12}");
                        Console.WriteLine($"Hond: {(res.Hond ? "Ja" : "Nee")}");
                        Console.WriteLine($"Elektriciteit: {(res.Elektriciteit ? "Ja" : "Nee")}");
                        Console.WriteLine($"Prijs: {res.TotaalPrijs},-");
                        Console.WriteLine($"Status: {res.Status}");
                        Console.WriteLine(new string('-', 30));

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
            nieuweReservering.Hond = Console.ReadLine().ToLower() == "j";

            Console.Write("Elektriciteit gewenst? (j/n): ");
            nieuweReservering.Elektriciteit = Console.ReadLine().ToLower() == "j";

            if (nieuweReservering.Elektriciteit)
            {
                Console.Write($"Voor hoeveel nachten elektriciteit? (max {nieuweReservering.AantalNachten}): ");
                nieuweReservering.AantalDagenElektriciteit = int.Parse(Console.ReadLine());
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
    }
}