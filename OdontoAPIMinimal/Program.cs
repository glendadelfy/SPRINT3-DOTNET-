using IdempotentAPI.Cache.DistributedCache.Extensions.DependencyInjection;
using IdempotentAPI.Core;
using IdempotentAPI.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OdontoAPIMinimal.Context.Database;
using OdontoAPIMinimal.Middelewares.Endpoints;
using OdontoAPIMinimal.Models;
using OdontoAPIMinimal.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// desenvolvido por Glenda Delfy 21/05/2025
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
    // Configura o Swagger para usar o JWT
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Insira o token JWT no formato Bearer {seu token}"
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
    options.MapType<PacienteOdontoModel>(() => new OpenApiSchema
    {
        Type = "object",
        Properties = new Dictionary<string, OpenApiSchema>
        {
            { "Idade", new OpenApiSchema { Type = "number", Format = "float", Description = "Idade do paciente em anos" } },
            { "FrequenciaEscovacao", new OpenApiSchema { Type = "number", Format = "float", Description = "Número médio de escovações por dia" } },
            { "FrequenciaVisitasAno", new OpenApiSchema { Type = "number", Format = "float", Description = "Número de visitas ao dentista por ano" } },
            { "HistoricoCaries", new OpenApiSchema { Type = "boolean", Description = "Se o paciente já teve cáries" } },
            { "DoencaGengival", new OpenApiSchema { Type = "boolean", Description = "Indica se o paciente tem doença gengival" } },
            { "Fumante", new OpenApiSchema { Type = "boolean", Description = "Se o paciente é fumante" } },
            { "ConsomeAcucarFrequente", new OpenApiSchema { Type = "boolean", Description = "Indica se o paciente consome açúcar regularmente" } },
            { "RiscoAlto", new OpenApiSchema { Type = "boolean", Description = "Indica se o paciente está em alto risco odontológico" } }
        },
        Required = new HashSet<string> { "Idade", "FrequenciaEscovacao", "FrequenciaVisitasAno", "HistoricoCaries", "DoencaGengival", "Fumante", "ConsomeAcucarFrequente", "RiscoAlto" }
    });
});

builder.Services.AddIdempotentAPI();
builder.Services.AddIdempotentMinimalAPI(new IdempotencyOptions());
builder.Services.AddDistributedMemoryCache();
builder.Services.AddIdempotentAPIUsingDistributedCache();

// Adiciona a autenticação JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.MapPost("/gerar-token", (IConfiguration config) =>
{
    // Normalmente, você validaria as credenciais do usuário aqui
    // Para simplificar, vamos assumir que o usuário é válido

    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, "usuarioTeste"),
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
app.RegisterRiscoOdontoEndpoint();

app.RegisterUsuarioEndpoints();


app.Run();

//Code for unit test
public partial class Program { }
