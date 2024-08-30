using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiPeliculas.Models
{
    public class CrearPeliculaDto
    {

        [Required]
        [MaxLength(50, ErrorMessage = "El nombre no debe tener más de 50 caracteres")]
        public string Nombre { get; set; }

        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public string RutaImagen { get; set; }
        public enum CrearTipoClasificacion { Siete, Trece, Dieciseis, Dieciocho }
        public CrearTipoClasificacion Clasificacion { get; set; }

        // Relacion con la tabla categoria
        public int categoriaId { get; set; }
    }
}
