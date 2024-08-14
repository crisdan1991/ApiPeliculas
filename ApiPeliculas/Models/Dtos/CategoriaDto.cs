using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models.Dtos
{
    public class CategoriaDto
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es requerido")]
        [MaxLength(50, ErrorMessage = "La longitud máxima del nombre es 50 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "La fecha de creación es requerida")]
        //[Display(Name = "Fecha de Creación")]  Se utiliza para Formularios
        public DateTime FechaCreacion { get; set; }
    }
}
