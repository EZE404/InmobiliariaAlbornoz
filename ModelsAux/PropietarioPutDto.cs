using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace InmobiliariaAlbornoz.ModelsAux
{
    public class PropietarioPutDto
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "DNI")]
        public string Dni { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MinLength(3)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MinLength(3)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Nacimiento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime FechaN { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MinLength(5)]
        [Display(Name = "Dirección")]
        [Column("Domicilio")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MinLength(10, ErrorMessage = "Debe insertar al menos 10 dígitos"), MaxLength(20)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [DataType(DataType.EmailAddress)]
        [MinLength(5)]
        public string Email { get; set; }
    }
}
