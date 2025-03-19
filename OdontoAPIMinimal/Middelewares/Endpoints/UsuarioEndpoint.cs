using Microsoft.EntityFrameworkCore;
using OdontoAPIMinimal.Infraestrutura.Database;
using OdontoMinimalAPI.Models;

namespace OdontoAPIMinimal.Middelewares.Endpoints
{
    public static class UsuarioEndpoint
    {
        public static void RegisterUsuarioEndpoints(this WebApplication app)
        {
            var usuarioItemsGroup = app.MapGroup("/usuarios");

            usuarioItemsGroup.MapGet("/", GetAllUsuarios)
                .WithSummary("Lista todos os usuários cadastrados")
                .WithDescription("Acessa o banco de dados e retorna todos os usuários cadastrados.");

            usuarioItemsGroup.MapGet("/ativos", GetActiveUsuarios)
                .WithSummary("Lista todos os usuários ativos")
                .WithDescription("Retorna apenas os usuários que estão marcados como ativos.");

            usuarioItemsGroup.MapGet("/{id}", GetUsuario)
                .WithSummary("Busca um usuário específico por ID")
                .WithDescription("Procura no banco de dados um usuário correspondente ao ID fornecido.");

            usuarioItemsGroup.MapPost("/", CreateUsuario)
                .WithSummary("Cria um novo usuário")
                .WithDescription("Adiciona um novo usuário ao banco de dados.");

            usuarioItemsGroup.MapPut("/{id}", UpdateUsuario)
                .WithSummary("Atualiza um usuário existente")
                .WithDescription("Atualiza os dados de um usuário específico baseado no ID fornecido.");

            usuarioItemsGroup.MapDelete("/{id}", DeleteUsuario)
                .WithSummary("Deleta um usuário")
                .WithDescription("Remove um usuário específico do banco de dados.");
        }

        #region Métodos de Endpoints
        static async Task<IResult> GetAllUsuarios(AppDbContext db)
        {
            var usuarios = await db.Usuarios.ToListAsync();
            return TypedResults.Ok(usuarios);
        }

        static async Task<IResult> GetActiveUsuarios(AppDbContext db)
        {
            var usuariosAtivos = await db.Usuarios
                .Where(u => u.isActive)
                .ToListAsync();
            return TypedResults.Ok(usuariosAtivos);
        }

        static async Task<IResult> GetUsuario(int id, AppDbContext db)
        {
            var usuario = await db.Usuarios.FindAsync(id);
            return usuario is not null
                ? TypedResults.Ok(usuario)
                : TypedResults.NotFound();
        }

        static async Task<IResult> CreateUsuario(UsuarioModel usuarioModel, AppDbContext db)
        {
            var usuario = new UsuarioModel
            {
                Name = usuarioModel.Name,
                Email = usuarioModel.Email,
                Password = usuarioModel.Password,
                isActive = usuarioModel.isActive,
                IsComplete = usuarioModel.IsComplete,
                Role = usuarioModel.Role,
                Avatar = usuarioModel.Avatar
            };

            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/usuarios/{usuario.Id}", usuario);
        }

        static async Task<IResult> UpdateUsuario(int id, UsuarioModel usuarioModel, AppDbContext db)
        {
            var usuario = await db.Usuarios.FindAsync(id);

            if (usuario is null)
                return TypedResults.NotFound();

            usuario.Name = usuarioModel.Name;
            usuario.Email = usuarioModel.Email;
            usuario.Password = usuarioModel.Password;
            usuario.isActive = usuarioModel.isActive;
            usuario.IsComplete = usuarioModel.IsComplete;
            usuario.Role = usuarioModel.Role;
            usuario.Avatar = usuarioModel.Avatar;

            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        static async Task<IResult> DeleteUsuario(int id, AppDbContext db)
        {
            var usuario = await db.Usuarios.FindAsync(id);

            if (usuario is null)
                return TypedResults.NotFound();

            db.Usuarios.Remove(usuario);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }
        #endregion
    }
}
