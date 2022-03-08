using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrionTekTestALG.Model
{
    public class Cliente
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public byte Sexo { get; set; }

        public Cliente()
        {
            FechaNacimiento = DateTime.Now;
        }
    }
}
