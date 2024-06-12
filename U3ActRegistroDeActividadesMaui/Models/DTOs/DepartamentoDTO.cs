using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using TableAttribute = SQLite.TableAttribute;

namespace U3ActRegistroDeActividadesMaui.Models.DTOs
{
    [Table("Departamentos")]
    public class DepartamentoDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int? IdSuperior { get; set; }
    }
}
