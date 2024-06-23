using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using U3ActRegistroDeActividadesMaui.Models.Entities;

namespace U3ActRegistroDeActividadesMaui.Helpers
{
    public static class ActividadesSerializerHelper
    {
        public static void Serializar(IEnumerable<Actividades> list)
        {
            var json = JsonSerializer.Serialize(list);
            string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "acts.json");
            File.WriteAllText(filePath, json);
        }
        public static IEnumerable<Actividades> Deserializar()
        {
            string filePath = Path.Combine(FileSystem.Current.AppDataDirectory, "acts.json");
            string jsonStringFromFile = File.ReadAllText(filePath);
            var des = JsonSerializer.Deserialize<IEnumerable<Actividades>>(jsonStringFromFile, new JsonSerializerOptions { PropertyNameCaseInsensitive=true});
            return des??Enumerable.Empty<Actividades>();
        }

    }
}
