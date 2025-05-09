using Microsoft.EntityFrameworkCore;
using OdontoAPIMinimal.Models;

namespace OdontoAPIMinimal.Context.Database
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
