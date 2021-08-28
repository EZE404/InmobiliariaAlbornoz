using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Models
{
    public class Inquilino
    {
        [Display(Name = "Código")]
        public int Id { get; set; }
        [Required]
        public string Dni { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Display(Name = "Fecha de Nacimiento"), Required]
        public DateTime FechaN { get; set; }
        [Display(Name ="Dirección de Trabajo"), Required]
        public string DireccionTrabajo { get; set; }
        [Display(Name = "Teléfono"), Required]
        public string Telefono { get; set; }
        [Required]
        public string Email { get; set; }


    }
}
