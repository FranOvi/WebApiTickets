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
    public class FilaController : ApiController
    {
        private PticketsEntities dbContext = new PticketsEntities();

        //visualiza todos los registros api/fila
        [HttpGet]
        public IEnumerable<fila> Get()
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                return pticketsEntities.fila.ToList();
            }
        }

        [HttpGet]
        public fila Get(int id)
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                return pticketsEntities.fila.FirstOrDefault(e => e.fila_id == id);
            }
        }

        [HttpPost]
        public IHttpActionResult AgregaFila([FromBody]fila fi)
        {
            if (ModelState.IsValid)
            {
                dbContext.fila.Add(fi);
                dbContext.SaveChanges();
                return Ok(fi);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IHttpActionResult ActualizarFila(int id, [FromBody]fila fi)
        {
            if (ModelState.IsValid)
            {
                var FilaExiste = dbContext.fila.Count(c => c.fila_id == id) > 0;

                if (FilaExiste)
                {
                    dbContext.Entry(fi).State = EntityState.Modified;
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
        public IHttpActionResult EliminarFila(int id)
        {

            var fi = dbContext.fila.Find(id);

            if (fi != null)
            {
                dbContext.fila.Remove(fi);
                dbContext.SaveChanges();
                return Ok(fi);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
