//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConectarDatos
{
    using System;
    using System.Collections.Generic;
    
    public partial class compra
    {
        public int compra_id { get; set; }
        public Nullable<int> cliente_id { get; set; }
        public Nullable<System.DateTime> fecha { get; set; }
    
        public virtual ticket ticket { get; set; }
        public virtual usuario usuario { get; set; }
    }
}
