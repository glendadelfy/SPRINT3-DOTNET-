using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OdontoAPIMinimal.Infraestrutura.Database;
using OdontoAPIMinimal.Middelewares.Endpoints;
using OdontoMinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);
// desenvolvido por Glenda Delfy 01/04/2025
// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection"));
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Odonto Minimal API",
        Version = "v1",
        Description = "API para gerenciamento de usuarios e funcionalidades odontologicas.",
        Contact = new OpenApiContact
        {
            Name = "Glenda",
            Email = "glendadelfy20@gmail.com",
            Url = new Uri("https://github.com/glendadelfy/SPRINT3-DOTNET-") // Opcional: Adicione o link do seu reposit�rio/projeto.
        }
    });
    options.MapType<UsuarioModel>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            { "Id", new OpenApiSchema { Type = "integer", Description = "Identificador único do usuário" } },
            { "Name", new OpenApiSchema { Type = "string", Description = "Nome completo do usuário" } },
            { "Email", new OpenApiSchema { Type = "string", Description = "Endereço de email do usuário" } },
            { "Password", new OpenApiSchema { Type = "string", Description = "Senha do usuário (hash ou criptografada)" } },
            { "isActive", new OpenApiSchema { Type = "boolean", Description = "Indicador se o usuário está ativo ou inativo" } },
            { "IsComplete", new OpenApiSchema { Type = "boolean", Description = "Indicador se o perfil do usuário está completo" } },
            { "Role", new OpenApiSchema { Type = "string", Description = "Função ou cargo do usuário na aplicação" } },
            { "Avatar", new OpenApiSchema { Type = "string", Description = "URL para o avatar ou imagem do usuário" } }
        }
    });

});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
#region ExceptionHandling

if (!app.Environment.IsDevelopment())
{

    // Configura��o para lidar com p�ginas de status HTTP (ex.: 404, 500)
    app.UseStatusCodePages(statusCodeHandlerApp =>
    {
        statusCodeHandlerApp.Run(async httpContext =>
        {
            var pds = httpContext.RequestServices.GetService<IProblemDetailsService>();
            if (pds == null || !await pds.TryWriteAsync(new() { HttpContext = httpContext }))
            {
                // Comportamento padr�o no fallback
                await httpContext.Response.WriteAsync("Fallback: An error occurred.");
            }
        });
    });
}

#endregion
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.RegisterUsuarioEndpoints();

app.Run();
