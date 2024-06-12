namespace U3ActRegistroDeActividadesApi.Models.DTOs
{
    public class GetActividadDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string? Descripcion { get; set; }
        public DateOnly? FechaRealizacion { get; set; }
        public string Departamento { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public int Estado { get; set; }

    }
}
