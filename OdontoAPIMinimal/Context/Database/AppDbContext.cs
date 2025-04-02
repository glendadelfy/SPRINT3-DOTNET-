using Microsoft.EntityFrameworkCore;
using OdontoAPIMinimal.Models;
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
        public DbSet<IdempotencyKey> IdempotencyKeys { get; set; } // Novo DbSet
    }
}
