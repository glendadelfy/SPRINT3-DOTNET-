using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdontoAPIMinimal.Context.Database;
using OdontoAPIMinimal.Services;
using OdontoAPIMinimal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OdontoAPIMinimal.Middelewares.Endpoints
{
    public static class UsuarioEndpoint
    {
        public static void RegisterUsuarioEndpoints(this WebApplication app)
        {

            var usuarioItemsGroup = app.MapGroup("/usuarios");

            //app.MapGet("/dados-seguros", [Authorize] () =>
            //{
            //    return "Este é um dado protegido por JWT!";
            //});
            app.MapPost("/admin/login", (IConfiguration config, AdministradorModel usuario) =>
            {
                // Simulação de validação de usuário (substitua por validação real)
                if (usuario.Role != "admin" || usuario.Password!= "123456")
                    return Results.Unauthorized();

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
        new Claim(JwtRegisteredClaimNames.Sub, usuario.Role),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

                var token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(1),
                    signingCredentials: creds);

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Results.Ok(new { token = tokenString });
            });

            usuarioItemsGroup.MapGet("/cadastrados", GetAllUsuarios)
                .RequireAuthorization()
                .WithSummary("Lista todos os usuários cadastrados")
                .WithDescription("Acessa o banco de dados e retorna todos os usuários cadastrados.");

            usuarioItemsGroup.MapGet("/ativos", GetActiveUsuarios)
                 .RequireAuthorization()
                .WithSummary("Lista todos os usuários ativos")
                .WithDescription("Retorna apenas os usuários que estão marcados como ativos.");

            usuarioItemsGroup.MapGet("/buscar/{id}", GetUsuario)
                .WithSummary("Busca um usuário específico por ID")
                .WithDescription("Procura no banco de dados um usuário correspondente ao ID fornecido.");

            usuarioItemsGroup.MapPost("/criar", CreateUsuario)
                .WithSummary("Cria um novo usuário")
                .WithDescription("Adiciona um novo usuário ao banco de dados.");

            usuarioItemsGroup.MapPut("/atualizar/{id}", UpdateUsuario)
                .WithSummary("Atualiza um usuário existente")
                .WithDescription("Atualiza os dados de um usuário específico baseado no ID fornecido.");

            usuarioItemsGroup.MapDelete("/deletar/{id}", DeleteUsuario)
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
                .Where(u => u.IsActive)
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

        static async Task<IResult> CreateUsuario(UsuarioModel usuarioModel, AppDbContext db, [FromHeader(Name = "Idempotency-Key")] string idempotencyKey)
        {
            var idempotencyService = new IdempotencyService(db);

            // Verificar se a chave já foi usada
            if (await idempotencyService.IsDuplicateRequestAsync(idempotencyKey))
            {
                var storedResponse = await idempotencyService.GetStoredResponseAsync(idempotencyKey);
                return TypedResults.Ok(storedResponse); // Retornar resposta armazenada
            }
            Console.WriteLine($"Processando nova chave de idempotência: {idempotencyKey}");
            // Criar novo usuário
            var usuario = new UsuarioModel
            {
                Name = usuarioModel.Name,
                Email = usuarioModel.Email,
                Password = usuarioModel.Password,
                IsActive = usuarioModel.IsActive,
                IsComplete = usuarioModel.IsComplete,
                Role = usuarioModel.Role,
                Avatar = usuarioModel.Avatar
            };

            db.Usuarios.Add(usuario);
            await db.SaveChangesAsync();
            // Salvar a chave de idempotência e o resultado
            await idempotencyService.SaveIdempotencyKeyAsync(idempotencyKey, 201, usuario);
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
            usuario.IsActive = usuarioModel.IsActive;
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
