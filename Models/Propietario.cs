using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Models
{
    public class Propietario
    {
        public int Id { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaN { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
    }
}
