using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Models
{
    public class Inquilino
    {
        public int Id { get; set; }
        public string Dni { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaN { get; set; }
        public string DireccionTrabajo { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }

        // Garante
        public string DniGarante { get; set; }
        public string NombreGarante { get; set; }
        public string TelefonoGarante { get; set; }
        public string EmailGarante { get; set; }
    }
}
