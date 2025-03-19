using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OdontoAPIMinimal.Infraestrutura.Database;
using OdontoAPIMinimal.Middelewares.Endpoints;

var builder = WebApplication.CreateBuilder(args);
// desenvolvido por Glenda Delfy 19/03/2025
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
        Description = "API para gerenciamento de usu�rios e funcionalidades odontol�gicas.",
        Contact = new OpenApiContact
        {
            Name = "Glenda",
            Email = "glendadelfy20@gmail.com",
            Url = new Uri("https://github.com/glendadelfy/SPRINT3-DOTNET-") // Opcional: Adicione o link do seu reposit�rio/projeto.
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
