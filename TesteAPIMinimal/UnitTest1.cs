using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;
using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;
using OdontoAPIMinimal.Models;

namespace TesteApiMinimal
{
    public class UsuarioEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UsuarioEndpointTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllUsuarios_ReturnsStatus200()
        {
            var response = await _client.GetAsync("/usuarios/cadastrados");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetActiveUsuarios_ReturnsStatus200()
        {
            var response = await _client.GetAsync("/usuarios/ativos");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUsuario_ReturnsStatus200_WhenUserExists()
        {
            int usuarioId = 1; // Substitua por um ID existente
            var response = await _client.GetAsync($"/usuarios/buscar/{usuarioId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateUsuario_ReturnsStatus201()
        {
            var novoUsuario = new UsuarioModel
            {
                Name = "Teste",
                Email = "teste@email.com",
                Password = "Senha123",
                IsActive = true,
                IsComplete = true,
                Role = "User",
                Avatar = "avatar-url"
            };

            var response = await _client.PostAsJsonAsync("/usuarios/criar", novoUsuario);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task UpdateUsuario_ReturnsStatus204_WhenUserExists()
        {
            int usuarioId = 1; // Substitua por um ID existente
            var usuarioAtualizado = new UsuarioModel
            {
                Name = "Nome Atualizado",
                Email = "atualizado@email.com",
                Password = "NovaSenha123",
                IsActive = true,
                IsComplete = true,
                Role = "Admin",
                Avatar = "novo-avatar-url"
            };

            var response = await _client.PutAsJsonAsync($"/usuarios/atualizar/{usuarioId}", usuarioAtualizado);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUsuario_ReturnsStatus204_WhenUserExists()
        {
            int usuarioId = 1; // Substitua por um ID existente
            var response = await _client.DeleteAsync($"/usuarios/deletar/{usuarioId}");
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }

}