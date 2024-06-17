﻿using SQLite;
using U3ActRegistroDeActividadesMaui.Models.Entities;

namespace U3ActRegistroDeActividadesMaui.Repositories
{
    public class DepartamentosRepository
    {
        //Conexion Sqlite
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

        public void Delete(Departamentos D)
        {
            context.Delete(D);
        }
        public void DeleteAll()
        {
            context.DeleteAll<Departamentos>();
        }
    }
}
