using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Models
{
    public class Contrato
    {
        [Display(Name  = "Índice")]
        public int Id { get; set; }

        [Display(Name = "Inmueble"), Required]
        public int IdInmueble { get; set; }

        [Display(Name = "Inquilino"), Required]
        public int IdInquilino { get; set; }

        [Display(Name = "Fecha de Inicio"), Required]
        public DateTime Desde { get; set; }

        [Display(Name = "Fecha de Vencimiento"), Required]
        public DateTime Hasta { get; set; }

        [Display(Name = "Válido")]
        public bool Valido { get; set; }

        //Relaciones
        public Inmueble Inmueble { get; set; }
        public Inquilino Inquilino { get; set; }

        // Garante
        [Display(Name = "Dni del Garante"), Required]
        public string DniGarante { get; set; }
        [Display(Name = "Nombre del Garante"), Required]
        public string NombreGarante { get; set; }
        [Display(Name = "Teléfono del Garante"), Required]
        public string TelefonoGarante { get; set; }
        [Display(Name = "Email del Garante")]
        public string EmailGarante { get; set; }
    }
}
