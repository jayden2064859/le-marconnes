# Le Marconnes - Casus blok 2 Backend Development

## Project Beschrijving
backend systeem voor reserveringen van camping Le Marconnes

## Tools
- C# .NET 8
- ASP.NET Core Web API
- SQL Server Management Studio 22 


## Projectstructuur
- 'LeMarconnes' - Console applicatie
- 'CL' - Class Library met models en DAL
- 'API' - Web API project (Swagger)


## Team
- Jayden Rerimassie (2064859)
- Yassir Merini (2405743)
- Jannes van den Berg (2206808)
- Othman Azouzal (2404269)

## Setup

1. Clone repo: `git clone https://github.com/jayden2064859/le-marconnes.git`
2. Run DBscript in SQL Server Management Studio 
3. Connection string in DAL.cs aanpassen naar eigen servernaam (Server=[naam]\SQLEXPRESS;Database=LeMarconnesDB;Trusted_Connection=True;TrustServerCertificate=True;) 
4. Rechtermuisklik op solution -> Configure Startup projects -> Multiple Startup Projects -> selecteer LeMarconnes & API
