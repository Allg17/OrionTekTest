using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace OrionTekTestALG.Model
{
    public class Direcciones
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Alias { get; set; }
        public int ClienteID { get; set; }
        public string Municipio { get; set; }
        public string Provincia { get; set; }
        public string Referencia { get; set; }
        public string Direccion { get; set; }
        public int NumResidencia { get; set; }
        public bool Predeterminada { get; set; }
    }
}
