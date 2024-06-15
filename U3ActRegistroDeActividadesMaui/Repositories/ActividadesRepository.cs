using SQLite;
using U3ActRegistroDeActividadesMaui.Models.Entities;

namespace U3ActRegistroDeActividadesMaui.Repositories
{
    public class ActividadesRepository
    {
        private readonly SQLiteConnection context;

        public ActividadesRepository()
        {
            string ruta = FileSystem.AppDataDirectory + "/actividades.db3";
            context = new SQLiteConnection(ruta);
            //context.CreateTable<Actividades>();
        }

        public void Insert(Actividades A)
        {
            context.Insert(A);
        }

        public IEnumerable<Actividades> GetAll()
        {
            return context.Table<Actividades>()
                .OrderBy(x => x.FechaCreacion);
        }

        public Actividades? Get(int id)
        {
            return context.Find<Actividades>(id);
        }

        public void InsertOrReplace(Actividades A)
        {
            context.InsertOrReplace(A);
        }

        public void Update(Actividades A)
        {
            context.Update(A);
        }

        public void Delete(Actividades A)
        {
            context.Delete(A);
        }
        public void DeleteAll()
        {
            context.DeleteAll<Actividades>();
        }

    }
}
