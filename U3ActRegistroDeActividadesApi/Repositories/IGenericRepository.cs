using Microsoft.EntityFrameworkCore;

namespace U3ActRegistroDeActividadesApi.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        void Delete(T entidad);
        DbSet<T> GetAll();
        T? GetById(int id);
        void Insert(T entidad);
        void Update(T entidad);
    }
}