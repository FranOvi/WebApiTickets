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
    public class SalaController : ApiController
    {
        private PticketsEntities dbContext = new PticketsEntities();

        //visualiza todos los registros api/sala
        [HttpGet]
        public IEnumerable<sala> Get()
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                return pticketsEntities.sala.ToList();
            }
        }

        [HttpGet]
        public sala Get(int id)
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                return pticketsEntities.sala.FirstOrDefault(e => e.sala_id == id);
            }
        }

        [HttpPost]
        public IHttpActionResult AgregaSala([FromBody]sala sa)
        {
            if (ModelState.IsValid)
            {
                dbContext.sala.Add(sa);
                dbContext.SaveChanges();
                return Ok(sa);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IHttpActionResult ActualizarSala(int id, [FromBody]sala sa)
        {
            if (ModelState.IsValid)
            {
                var SalaExiste = dbContext.sala.Count(c => c.sala_id == id) > 0;

                if (SalaExiste)
                {
                    dbContext.Entry(sa).State = EntityState.Modified;
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
        public IHttpActionResult EliminarSala(int id)
        {

            var sa = dbContext.sala.Find(id);

            if (sa != null)
            {
                dbContext.sala.Remove(sa);
                dbContext.SaveChanges();
                return Ok(sa);
            }
            else
            {
                return NotFound();
            }

        }

    }
}
