using SQLite;
using SQLiteNetExtensions.Attributes;

namespace U3ActRegistroDeActividadesMaui.Models.Entities
{
    [Table("Departamentos")]
    public class Departamentos
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [NotNull]
        public string Nombre { get; set; } = null!;
        [NotNull]
        public string Username { get; set; } = null!;
        public string? Password { get; set; } = null!;

        public int? IdSuperior { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.None)]
        public virtual List<Actividades> Actividades { get; set; } = [];

        [OneToMany(CascadeOperations = CascadeOperation.None)]
        public virtual Departamentos? IdSuperiorNavigation { get; set; }

        [OneToMany(CascadeOperations = CascadeOperation.None)]
        public virtual List<Departamentos> InverseIdSuperiorNavigation { get; set; } = [];
    }
}
