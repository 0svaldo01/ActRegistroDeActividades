﻿

using SQLite;
using SQLiteNetExtensions.Attributes;

namespace U3ActRegistroDeActividadesMaui.Models.Entities
{
    [Table("Actividades")]
    public class Actividades
    {
        [PrimaryKey]
        public int Id { get; set; }
        [NotNull]
        public string Titulo { get; set; } = null!;
        [NotNull]
        public string? Descripcion { get; set; }
        [NotNull]
        public DateTime FechaRealizacion { get; set; }
        public int IdDepartamento { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int Estado { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.None)]
        public virtual Departamentos? IdDepartamentoNavigation { get; set; }
        public Uri Imagen
        {
            get
            {
                var uri= new Uri("https://u3eqpo1actapi.labsystec.net/Images/" + Id.ToString() +".jpg?fecha=" + FechaActualizacion.ToString());
                return uri;
            }
        }
    }
}
