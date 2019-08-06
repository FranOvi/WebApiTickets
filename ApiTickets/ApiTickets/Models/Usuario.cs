using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiTickets.Models
{
    public class Usuario
    {
        public int usuario_id { get; set; }
        public string email { get; set; }
        public string nombres { get; set; }
        public string apellidos { get; set; }
    }
}