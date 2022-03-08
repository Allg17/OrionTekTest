using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrionTekTestALG.Model.DataBase
{
    public class ClientesDataBase
    {
        public readonly SQLiteAsyncConnection database;
        public ClientesDataBase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Cliente>().Wait();
            database.ExecuteScalarAsync<string>("PRAGMA auto_vacuum = FULL");
        }

        public Task<int> Save(Cliente cliente)
        {
            if (cliente.Id > 0)
            {
                return database.UpdateAsync(cliente);
            }
            else
            {
                return database.InsertAsync(cliente);
            }
        }

        public async Task<List<Cliente>> GetClientes()
        {
            return await database.Table<Cliente>().ToListAsync();
        }

        public async Task<int> DeleteCliente(Cliente cliente)
        {
            var res = await database.Table<Cliente>().DeleteAsync(x => x.Id == cliente.Id);
            if (res > 0)
            {
                await App.DireccionesDb.DeleteDirecciones(cliente.Id);
            }
            return res;
        }

    }
}
