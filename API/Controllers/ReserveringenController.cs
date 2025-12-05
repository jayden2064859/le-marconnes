using Microsoft.AspNetCore.Mvc;
using CL.Models;
using CL.Data;
using CL.Services;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReserveringenController : ControllerBase
    {
        // GET: api/reserveringen
        [HttpGet]
        public ActionResult<List<Reservering>> GetAll()
        {
            var reserveringen = DAL.GetAllReserveringen();
            return Ok(reserveringen);
        }


        [HttpGet("{id}")]
        public ActionResult<Reservering> GetById(int id)
        {
            var reservering = DAL.GetReserveringById(id);

            if (reservering == null)
                return NotFound($"Reservering met id {id} niet gevonden.");

            return Ok(reservering);
        }


        // POST: api/reserveringen
        [HttpPost]
        public ActionResult<Reservering> Create([FromBody] Reservering reservering)
        {
             
            var tarieven = DAL.TarievenOphalen(); 
            reservering.TotaalPrijs = TariefCalculator.TotaalPrijs(reservering, tarieven);

            // standaardwaarden initialisen 
            reservering.Status = "Gereserveerd";
            reservering.RegistratieDatum = DateTime.Now;


            bool inserted = DAL.InsertReservering(reservering);
            if (inserted)
            {
                return Ok(reservering);
            }
            else
            {
                return BadRequest("Kan reservering niet toevoegen");
            }
        }


        // DELETE: api/reserveringen/{id}
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool deleted = DAL.DeleteReservering(id);
            if (deleted)
            {
                return Ok($"Reservering met Id {id} verwijderd");
            }
            return NotFound($"Reservering met Id {id} niet gevonden");
        }


        // PUT: api/reseveringen/{id}
        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Reservering res)
        {
            if (res == null || res.ReserveringId != id)
                return BadRequest("ReserveringId ongeldig");

            bool succes = DAL.UpdateReservering(res);

            if (!succes)
                return NotFound($"Reservering met id {id} niet gevonden.");

            return Ok(res);
        }


    }
}