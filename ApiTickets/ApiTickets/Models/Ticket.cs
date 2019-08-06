using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTickets.Models
{
    public class Ticket
    {
        public int ticket_id { get; set; }
        public int fila_id { get; set; }
        public int compra_id { get; set; }
        public int num_asiento { get; set; }

    }
}