using ApiPeliculas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPeliculas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet es una colección de entidades que se pueden consultar, agregar, modificar y eliminar.
        public DbSet<Categoria> Categoria { get; set; }
    }
}
