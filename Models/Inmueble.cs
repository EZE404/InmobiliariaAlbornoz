

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace InmobiliariaAlbornoz.Models
{
    public enum enTipos
    {
        Local = 1,
        Depósito = 2,
        Casa = 3,
        Depto = 4,
        Otros = 5
    }

    public enum enUsos
    {
        Comercial = 1,
        Residencial = 2
    }

    public class Inmueble
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        [MinLength(5)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public int Tipo { get; set; }

        [Display(Name ="Tipo")]
        public string TipoNombre => Tipo > 0 ? ((enTipos)Tipo).ToString() : "";

        [Required(ErrorMessage = "Campo obligatorio")]
        public int Uso { get; set; }

        [Display(Name = "Uso")]
        public string UsoNombre => Uso > 0 ? ((enUsos)Uso).ToString() : "";

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Num. Ambientes")]
        public int Ambientes { get; set; }

        [Required(ErrorMessage = "Campo obligatorio")]
        public decimal Precio { get; set; }

        public bool Disponible { get; set; }
        public string DisponibleNombre => Disponible ? "Sí" : "No";

        [Required(ErrorMessage = "Campo obligatorio")]
        [Display(Name = "Propietario")]
        public int IdPropietario { get; set; }

        [ForeignKey("IdPropietario")]
        public Propietario Propietario { get; set; }

        public string ImageUrl { get; set; }

        public static IDictionary<int, string> ObtenerTipos()
        {
            SortedDictionary<int, string> tipos = new SortedDictionary<int, string>();
            Type tipoEnumTipo = typeof(enTipos);
            foreach (var valor in Enum.GetValues(tipoEnumTipo))
            {
                tipos.Add((int)valor, Enum.GetName(tipoEnumTipo, valor));
            }
            return tipos;
        }

        public static IDictionary<int, string> ObtenerUsos()
        {
            SortedDictionary<int, string> usos = new SortedDictionary<int, string>();
            Type tipoEnumUso = typeof(enUsos);
            foreach (var valor in Enum.GetValues(tipoEnumUso))
            {
                usos.Add((int)valor, Enum.GetName(tipoEnumUso, valor));
            }
            return usos;
        }
    }
}