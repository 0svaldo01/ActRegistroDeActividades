namespace U3ActRegistroDeActividadesApi.Models.DTOs
{
    public class DeptoDTO
    {
        public int Id { get; set; }
        public string Departamento { get; set; } = string.Empty;
        public IEnumerable<ActividadDTO> Actividades { get; set; } = null!;
        public IEnumerable<DeptoDTO> Subordinados { get; set; } = null!;
    }
}
