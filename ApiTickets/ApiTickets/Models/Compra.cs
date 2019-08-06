using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTickets.Models
{
    public class Compra
    {
        public int compras_id { get; set; }
        public int cliente_id { get; set; }
        public DateTime fecha { get; set; }
    }
}