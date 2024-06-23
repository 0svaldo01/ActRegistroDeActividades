using Microsoft.AspNetCore.Http;

namespace U3ActRegistroDeActividadesMaui.Models.DTOs
{
    public class AgregarActividadDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateOnly? FechaRealizacion { get; set; }
        public int IdDepartamento { get; set; }
        public IFormFile Archivo { get; set; } = null!;
    }
}
