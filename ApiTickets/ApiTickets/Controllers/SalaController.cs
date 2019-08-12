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
        public object Get(int id)
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                //return pticketsEntities.sala.Include(s => s.fila.Select(f => f.ticket)).FirstOrDefault(s => s.sala_id == id);

                return pticketsEntities.sala
                    .Select(s => new
                    {
                        sala_id = s.sala_id,
                        nombre = s.nombre,
                        fila = s.fila.Select(f => new { f.fila_id, f.nombre, f.cantidad, ticket = f.ticket.Select(t => new { t.ticket_id, t.num_asiento }) }).OrderBy(f => f.nombre),
                        //ticket = s.fila.Select(f => f.ticket.Select(t => new { t.ticket_id, t.num_asiento }))
                    }).FirstOrDefault(s => s.sala_id == id);

                //return pticketsEntities.sala
                //    .Select(s => new {
                //        sala_id = s.sala_id,
                //        nombre = s.nombre,
                //        fila = s.fila.Select(f => new { f.fila_id, f.nombre, f.cantidad }).OrderBy(f => f.nombre),
                //        ticket = s.fila.Select(f => f.ticket.Select(t => new { t.ticket_id, t.num_asiento }))
                //    }).FirstOrDefault(s => s.sala_id == id);
            }
        }

        [HttpGet]
        [ActionName("GetExistencias")]
        public IEnumerable<object> GetExistencias()
        {
            using (PticketsEntities pticketsEntities = new PticketsEntities())
            {
                var salaIni = from s in dbContext.sala
                              join f in dbContext.fila on s.sala_id equals f.sala_id into fLeft
                              from fl in fLeft.DefaultIfEmpty()
                              join t in dbContext.ticket on fl.fila_id equals t.fila_id into tLeft
                              from ti in tLeft.DefaultIfEmpty()
                              group new { fl.cantidad, fl.fila_id, ti.ticket_id } by new { s.sala_id, s.nombre } into grp
                              select new
                              {
                                  sala_id = grp.Key.sala_id,
                                  nombre = grp.Key.nombre,
                                  numTotal = grp.Select(fil => new { fil.cantidad , fil.fila_id }).Distinct().Sum(fil => (int?)fil.cantidad ?? 0),
                                  //numTotal = grp.Sum(fil => (int?)fil.cantidad ?? 0),
                                  numOcupado = grp.Count(tck => ((int?)tck.ticket_id ?? 0) > 0)
                              };
                return salaIni.ToList();
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
