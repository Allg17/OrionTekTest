using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrionTekTestALG.Model.DataBase
{
    public class DireccionesDataBase
    {
        public readonly SQLiteAsyncConnection database;
        public DireccionesDataBase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Direcciones>().Wait();
            database.ExecuteScalarAsync<string>("PRAGMA auto_vacuum = FULL");
        }

        public async Task<int> Save(Direcciones direcciones)
        {
            if (direcciones.Predeterminada)
            {
                if (await database.Table<Direcciones>().Where(x => x.Predeterminada && x.ClienteID == direcciones.ClienteID).CountAsync() > 0)
                {
                    var res = await database.Table<Direcciones>().FirstAsync(x => x.Predeterminada && x.ClienteID == direcciones.ClienteID);
                    if (res != null)
                    {
                        res.Predeterminada = false;
                        await database.UpdateAsync(res);
                    }
                }
            }
            if (direcciones.ID > 0)
            {
                return await database.UpdateAsync(direcciones);
            }
            else
            {
                return await database.InsertAsync(direcciones);
            }
        }

        public async Task<List<Direcciones>> GetDirecciones(int cliente)
        {
            return await database.Table<Direcciones>().Where(x => x.ClienteID == cliente).ToListAsync();
        }

        public async Task<int> DeleteDirecciones(Direcciones direcciones)
        {
            return await database.Table<Direcciones>().DeleteAsync(x => x.ID == direcciones.ID);
        }

        public async Task<int> DeleteDirecciones(int cliente)
        {
            return await database.Table<Direcciones>().DeleteAsync(x => x.ClienteID == cliente);
        }
    }
}
