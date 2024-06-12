using Microsoft.EntityFrameworkCore;
using U3ActRegistroDeActividadesApi.Models.Entities;

namespace U3ActRegistroDeActividadesApi.Repositories
{
    public class ActividadesRepository(ItesrcneActividadesContext context) : GenericRepository<Actividades>(context)
    {
        private readonly ItesrcneActividadesContext Context = context;
        public IEnumerable<Actividades> GetActividades()
        {
            return Context.Actividades
                .Include(act => act.IdDepartamentoNavigation)
                .OrderBy(act => act.Titulo);
        }
        public Actividades? GetActividad(int id)
        {
            return GetActividades().FirstOrDefault(act => act.Id == id);
        }
    }
}
