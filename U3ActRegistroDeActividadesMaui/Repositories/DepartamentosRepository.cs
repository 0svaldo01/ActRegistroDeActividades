﻿using SQLite;
using U3ActRegistroDeActividadesMaui.Models.Entities;

namespace U3ActRegistroDeActividadesMaui.Repositories
{
    public class DepartamentosRepository
    {
        //Conexión Sqlite
        private readonly SQLiteConnection context;
        public DepartamentosRepository()
        {
            //Base de datos local
            string ruta = FileSystem.AppDataDirectory + "/actividades.db3";
            context = new SQLiteConnection(ruta);
            //Creacion de Tablas
            context.CreateTable<Actividades>();
            context.CreateTable<Departamentos>();
        }
        public void Insert(Departamentos D)
        {
            context.Insert(D);
        }

        public IEnumerable<Departamentos> GetAll()
        {
            return context.Table<Departamentos>()
                .OrderBy(x => x.Nombre);
        }
        public Departamentos? Get(int id)
        {
            return context.Find<Departamentos>(id);
        }
        public void InsertOrReplace(Departamentos D)
        {
            context.InsertOrReplace(D);
        }
        public void Update(Departamentos D)
        {
            context.Update(D);
        }
        public void Delete(int id)
        {
            var departamento = context.Find<Departamentos>(id);
            if (departamento != null)
            {
                context.Delete(departamento);
            }
        }
        //public void Delete(int id)
        //{
        //    context.Delete(id);
        //}
        public void DeleteAll()
        {
            context.DeleteAll<Departamentos>();
        }
    }
}