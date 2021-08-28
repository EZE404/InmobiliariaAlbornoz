

using System.ComponentModel.DataAnnotations;

namespace InmobiliariaAlbornoz.Models
{
    public class Inmueble
    {
        [Display(Name = "C�digo")]
        public int Id { get; set; }
        [Display(Name = "Direcci�n"), Required]
        public string Direccion { get; set; }
        [Required]
        public string Tipo { get; set; }
        [Required]
        public string Uso { get; set; }
        [Display(Name = "Cantidad de Ambientes"), Required]
        public int Ambientes { get; set; }
        [Required]
        public decimal Precio { get; set; }
        [Required]
        public bool Disponible { get; set; }
        [Display(Name = "Propietario"), Required]
        public int IdPropietario { get; set; }

        public Propietario Propietario { get; set; }

    }
}