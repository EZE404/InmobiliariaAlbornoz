using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Models
{
    public class Contrato
    {
        [Key]
        [Display(Name  = "Índice")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Inmueble")]
        public int IdInmueble { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Inquilino")]
        public int IdInquilino { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Fecha de Inicio")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Desde { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Fecha de Vencimiento")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = false)]
        public DateTime Hasta { get; set; }

        [Display(Name = "Válido")]
        public bool Valido { get; set; }

        [NotMapped]
        public string ValidoNombre => Valido ? "Sí" : "No";

        //Relaciones
        [ForeignKey(nameof(IdInmueble))]
        public Inmueble Inmueble { get; set; }
        [ForeignKey(nameof(IdInquilino))]
        public Inquilino Inquilino { get; set; }

        // Garante
        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Dni del Garante")]
        [MinLength(8), MaxLength(16)]
        public string DniGarante { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Nombre del Garante")]
        [MinLength(10)]
        public string NombreGarante { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Teléfono del Garante")]
        [DataType(DataType.PhoneNumber)]
        [MinLength(8), MaxLength(16)]
        public string TelefonoGarante { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Email del Garante")]
        [DataType(DataType.EmailAddress)]
        [MinLength(5)]
        public string EmailGarante { get; set; }
    }
}
