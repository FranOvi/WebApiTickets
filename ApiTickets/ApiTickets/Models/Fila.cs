using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTickets.Models
{
    public class Fila
    {
        public int fila_id { get; set; }
        public int sala_id { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
    }
}