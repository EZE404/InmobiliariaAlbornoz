using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Models
{
    public class Inquilino
    {
        [Key]
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MinLength(8), MaxLength(16)]
        [Display(Name = "DNI")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MinLength(3)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime FechaN { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Dirección de Trabajo")]
        [MinLength(5)]
        public string DireccionTrabajo { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Teléfono")]
        [MinLength(8)]
        [DataType(DataType.PhoneNumber)]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [DataType(DataType.EmailAddress)]
        [MinLength(8)]
        public string Email { get; set; }


    }
}
