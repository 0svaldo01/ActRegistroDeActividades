using TableAttribute = SQLite.TableAttribute;

namespace U3ActRegistroDeActividadesMaui.Models.DTOs
{
    [Table("Departamentos")]
    public class DepartamentoDTO
    {
        public int Id { get; set; }
        public string Departamento { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? IdSuperior { get; set; }
        public IEnumerable<ActividadDTO> Actividades { get; set; } = null!;
        public IEnumerable<DepartamentoDTO> Subordinados { get; set; } = null!;
    }
}