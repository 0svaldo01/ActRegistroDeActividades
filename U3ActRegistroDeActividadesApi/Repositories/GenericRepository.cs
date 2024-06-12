using Microsoft.EntityFrameworkCore;
using U3ActRegistroDeActividadesApi.Models.Entities;

namespace U3ActRegistroDeActividadesApi.Repositories
{
    public class GenericRepository<T>(ItesrcneActividadesContext Context) : IGenericRepository<T> where T : class
    {
        public virtual DbSet<T> GetAll()
        {
            return Context.Set<T>();
        }
        public virtual T? GetById(int id)
        {
            return Context.Find<T>(id);
        }
        public virtual void Insert(T entidad)
        {
            Context.Add(entidad);
            Context.SaveChanges();
        }
        public virtual void Update(T entidad)
        {
            Context.Update(entidad);
            Context.SaveChanges();
        }
        public virtual void Delete(T entidad)
        {
            Context.Remove(entidad);
            Context.SaveChanges();
        }
    }
}
