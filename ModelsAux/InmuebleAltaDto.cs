using System.ComponentModel.DataAnnotations;

namespace InmobiliariaAlbornoz.Models
{
    public class InmuebleAltaDto
    {
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public string TipoNombre { get; set; } // Se recibe como string

        [Required(ErrorMessage = "Campo obligatorio")]
        public string UsoNombre { get; set; } // Se recibe como string

        [Required(ErrorMessage = "Campo obligatorio")]
        public int Ambientes { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal Precio { get; set; }
    }
}
