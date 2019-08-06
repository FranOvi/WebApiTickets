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
    public class CompraController : ApiController
    {
        private PticketsEntities dbContext = new PticketsEntities();

        //visualiza todos los registros api/compra
        [HttpGet]
        public IEnumerable<compra> Get()
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                return pticketsEntities.compra.ToList();
            }
        }

        [HttpGet]
        public compra Get(int id)
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                return pticketsEntities.compra.FirstOrDefault(e => e.compra_id == id);
            }
        }

        [HttpPost]
        public IHttpActionResult AgregaCompra([FromBody]compra com)
        {
            if (ModelState.IsValid)
            {
                dbContext.compra.Add(com);
                dbContext.SaveChanges();
                return Ok(com);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IHttpActionResult ActualizarCompra(int id, [FromBody]compra com)
        {
            if (ModelState.IsValid)
            {
                var CompraExiste = dbContext.compra.Count(c => c.compra_id == id) > 0;

                if (CompraExiste)
                {
                    dbContext.Entry(com).State = EntityState.Modified;
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
        public IHttpActionResult EliminarCompra(int id)
        {

            var com = dbContext.compra.Find(id);

            if (com != null)
            {
                dbContext.compra.Remove(com);
                dbContext.SaveChanges();
                return Ok(com);
            }
            else
            {
                return NotFound();
            }

        }
    }
}
