

using System.ComponentModel.DataAnnotations;

namespace InmobiliariaAlbornoz.Models
{
    public class Inmueble
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MinLength(5)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public string Uso { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "N° Ambientes")]
        public int Ambientes { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal Precio { get; set; }

        public bool Disponible { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Propietario")]
        public int IdPropietario { get; set; }

        public Propietario Propietario { get; set; }

    }
}