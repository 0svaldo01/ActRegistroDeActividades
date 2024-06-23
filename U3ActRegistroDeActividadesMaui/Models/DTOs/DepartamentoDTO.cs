using Newtonsoft.Json;
using TableAttribute = SQLite.TableAttribute;

namespace U3ActRegistroDeActividadesMaui.Models.DTOs
{
    [Table("Departamentos")]
    public class DepartamentoDTO
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nombre")]
        public string Departamento { get; set; } = null!;

        [JsonProperty("username")]
        public string Username { get; set; } = null!;

        [JsonProperty("password")]
        public string Password { get; set; } = null!;

        [JsonProperty("idSuperior")]
        public int? IdSuperior { get; set; }

        [JsonProperty("actividades")]
        public IEnumerable<ActividadDTO> Actividades { get; set; } = null!;

        [JsonProperty("subordinados")]
        public IEnumerable<DepartamentoDTO> Subordinados { get; set; } = null!;
    }
}