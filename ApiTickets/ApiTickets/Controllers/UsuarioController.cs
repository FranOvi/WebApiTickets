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
    public class UsuarioController : ApiController
    {
        private PticketsEntities dbContext = new PticketsEntities();

        //visualiza todos los registros api/usuario
        [HttpGet]
        public IEnumerable<usuario> Get()
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                return pticketsEntities.usuario.ToList();
            }
        }

        [HttpGet]
        public usuario Get(int id)
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                return pticketsEntities.usuario.FirstOrDefault(e => e.usuario_id == id);
            }
        }

        [HttpPost]
        public IHttpActionResult AgregaUsuario([FromBody]usuario usu)
        {
            if (ModelState.IsValid)
            {
                dbContext.usuario.Add(usu);
                dbContext.SaveChanges();
                return Ok(usu);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public IHttpActionResult ActualizarUsuario(int id, [FromBody]usuario usu)
        {
            if (ModelState.IsValid)
            {
                var UsuarioExiste = dbContext.usuario.Count(c => c.usuario_id == id) > 0;

                if (UsuarioExiste)
                {
                    dbContext.Entry(usu).State = EntityState.Modified;
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
        public IHttpActionResult EliminarUsuario(int id)
        {

            var usu = dbContext.usuario.Find(id);

            if (usu != null)
            {
                dbContext.usuario.Remove(usu);
                dbContext.SaveChanges();
                return Ok(usu);
            }
            else
            {
                return NotFound();
            }

        }

    }
}
