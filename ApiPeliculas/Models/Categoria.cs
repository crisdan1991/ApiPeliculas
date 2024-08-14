﻿using System.ComponentModel.DataAnnotations;

namespace ApiPeliculas.Models
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        //[Display(Name = "Fecha de Creación")]  Se utiliza para Formularios
        public DateTime FechaCreacion { get; set; }
        
    }
}
