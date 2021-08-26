

namespace InmobiliariaAlbornoz.Models
{
    public class Inmueble
    {
        public int Id { get; set; }
        public string Direccion { get; set; }
        public string Tipo { get; set; }
        public string Uso { get; set; }
        public int Ambientes { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }

        public int IdPropietario { get; set; }

        public Propietario Propietario { get; set; }

    }
}