﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class PticketsEntities : DbContext
    {
        public PticketsEntities()
            : base("name=PticketsEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<compra> compra { get; set; }
        public virtual DbSet<fila> fila { get; set; }
        public virtual DbSet<sala> sala { get; set; }
        public virtual DbSet<ticket> ticket { get; set; }
        public virtual DbSet<usuario> usuario { get; set; }
    }
}
