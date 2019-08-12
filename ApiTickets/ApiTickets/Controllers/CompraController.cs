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
                return dbContext.compra.ToList();
            }
        }

        [HttpGet]
        public compra Get(int id)
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                return dbContext.compra.FirstOrDefault(e => e.compra_id == id);
            }
        }

        [HttpPost]
        public IHttpActionResult AgregaCompra([FromBody]compra com)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = System.Web.HttpContext.Current.User.Identity as System.Security.Claims.ClaimsIdentity;
                var userEmail = claimsIdentity.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                com.cliente_id = dbContext.usuario.FirstOrDefault(e => e.email == userEmail).usuario_id;
                com.fecha = DateTime.Now;

                //Chequeamos que existe y que esta dentro del rango de numero de asiento
                foreach (var tck in com.ticket)
                {
                    //Chequeo que el numero de asiento que se quiere comprar este dentro del rango de asientos de la fila
                    var fila = dbContext.fila.Where(f => f.fila_id == tck.fila_id).FirstOrDefault();
                    if (tck.num_asiento <= 0 || tck.num_asiento > fila.cantidad) return BadRequest("Numero de asiento fuera de la capacidad de la fila");
                    if(dbContext.ticket.Where(t => t.num_asiento == tck.num_asiento && t.fila_id == tck.fila_id).ToList().Count() > 0) return BadRequest("Asiento ya comprado");
                }

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
