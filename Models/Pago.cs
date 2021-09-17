using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaAlbornoz.Models
{
    public class Pago
    {
        [Key]
        [Display (Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El contrato es necesario"), Display (Name ="Contrato")]
        public int IdContrato { get; set; }

        [Required(ErrorMessage = "La fecha de pago es requerida")]
        [Display(Name ="Fecha de pago")]
        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La fecha correspidiente al pago es requerida")]
        [Display(Name = "Fecha Correpondiente")]
        [DataType(DataType.Date)]
        public DateTime FechaCorrespondiente { get; set; }

        [Required(ErrorMessage ="El monto es requerido")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage ="El tipo de pago es requerido"), Display(Name ="Tipo de pago")]
        public string Tipo { get; set; }

        public Contrato Contrato { get; set; }
    }
}
