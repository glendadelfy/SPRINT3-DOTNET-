using Microsoft.EntityFrameworkCore;
using OdontoMinimalAPI.Models;
using System.Collections.Generic;

namespace OdontoAPIMinimal.Infraestrutura.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<UsuarioModel> Usuarios { get; set; }
    }
}
