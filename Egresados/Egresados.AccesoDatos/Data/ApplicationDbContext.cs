using Egresados.AccesoDatos;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Egresados.Models;

namespace Egresados.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        // Aqui se deben agregar cada uno de los modelos
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Articulo> Articulo { get; set; }
        public DbSet<Slider> Slider { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
    }
}