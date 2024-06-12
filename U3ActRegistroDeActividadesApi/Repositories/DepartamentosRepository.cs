using Microsoft.EntityFrameworkCore;
using U3ActRegistroDeActividadesApi.Models.DTOs;
using U3ActRegistroDeActividadesApi.Models.Entities;

namespace U3ActRegistroDeActividadesApi.Repositories
{
    public class DepartamentosRepository(ItesrcneActividadesContext Context) : GenericRepository<Departamentos>(Context)
    {
        public ItesrcneActividadesContext Context = Context;
        public IEnumerable<Departamentos> GetDepartamentos()
        {
            var depas = Context.Departamentos
                .Include(x => x.Actividades)
                .Include(x => x.InverseIdSuperiorNavigation);
            return depas;
        }
        public DeptoDTO GetActividadesRecursivasPorDepartamento(int departamentoId)
        {
            var depa = GetDepartamento(departamentoId);
            if (depa != null)
            {

                DeptoDTO depto = new()
                {
                    Id = depa.Id,
                    Departamento = depa.Nombre,
                    Actividades = depa.Actividades.Select(a => new ActividadDTO
                    {
                        Id = a.Id,
                        Descripcion = a.Descripcion,
                        FechaActualizacion = a.FechaActualizacion,
                        Estado = a.Estado,
                        FechaCreacion = a.FechaCreacion,
                        FechaRealizacion = a.FechaRealizacion,
                        IdDepartamento = a.IdDepartamento,
                        Titulo = a.Titulo,
                    }),
                    Subordinados = depa.InverseIdSuperiorNavigation.Select(subdep => GetActividadesRecursivasPorDepartamento(subdep.Id))
                };
                return depto;
            }
            //Es necesario regresar un nuevo objeto debido a que los subordinados no puede ser null
            //En el body del response se veria asi []
            return new();
        }
        public Departamentos? GetDepartamento(int id)
        {
            return Context.Departamentos
                .Include(x => x.Actividades)
                .Include(x => x.InverseIdSuperiorNavigation).FirstOrDefault(x => x.Id == id);
        }
        public void EliminarDepartamento(Departamentos departamento)
        {
            if (departamento != null)
            {
                /** 
                 * Se eliminaran todos los departamentos subordinados al departamento qu se desea eliminar
                 */
                foreach (var subdepto in departamento.InverseIdSuperiorNavigation)
                {
                    if (subdepto != null)
                    {
                        //if (subdepto.IdSuperiorNavigation != null)
                        //{
                        //  subdepto.IdSuperiorNavigation = null;
                        //}
                        //subdepto.IdSuperior = 0;
                        //Si se desea editar el departamento
                        // Context.Departamentos.Update(subdepto);

                        Context.Departamentos.Remove(subdepto);
                        Context.SaveChanges();
                    }
                }
                /** 
                 * Ahora que el departamento no tiene subordinados se puede eliminar
                 */
                if (departamento.InverseIdSuperiorNavigation.Count == 0)
                {
                    Delete(departamento);
                }
            }
        }
    }
}