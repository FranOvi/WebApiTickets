using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConectarDatos;
using System.Data.Entity;

namespace ApiTickets.Controllers
{
    [Authorize]
    public class TicketController : ApiController
    {
        private PticketsEntities dbContext = new PticketsEntities();

        //visualiza todos los registros api/tickets
        [HttpGet]
        public IEnumerable<ticket> Get()
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                return pticketsEntities.ticket.ToList();
            }
        }

        [HttpGet]
        public ticket Get(int id)
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                return pticketsEntities.ticket.FirstOrDefault(e => e.ticket_id == id);
            }
        }

        [HttpPost]
        public IHttpActionResult AgregaTicket([FromBody]ticket tick)
        {
            if (ModelState.IsValid)
            {
                dbContext.ticket.Add(tick);
                dbContext.SaveChanges();
                return Ok(tick);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IHttpActionResult ActualizarTicket(int id, [FromBody]ticket tick)
        {
            if (ModelState.IsValid)
            {
                var TicketExiste = dbContext.ticket.Count(c => c.ticket_id == id) > 0;

                if (TicketExiste)
                {
                    dbContext.Entry(tick).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IHttpActionResult EliminarTicket(int id)
        {

            var tick = dbContext.ticket.Find(id);

            if (tick != null)
            {
                dbContext.ticket.Remove(tick);
                dbContext.SaveChanges();
                return Ok(tick);
            }
            else
            {
                return NotFound();
            }

        }

    }
}
