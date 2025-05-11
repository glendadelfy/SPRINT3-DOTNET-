using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using OdontoAPIMinimal.Models;

namespace OdontoApiMinimalTeste
{
    public class TesteEndpoints : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public TesteEndpoints(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllUsuarios_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/usuarios/cadastrados");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetActiveUsuarios_ReturnsOk()
        {
            // Act
            var response = await _client.GetAsync("/usuarios/ativos");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetUsuarioById_ReturnsOkOrNotFound()
        {
            // Act
            var response = await _client.GetAsync("/usuarios/buscar/1"); // ID fict�cio

            // Assert
            Assert.Contains(response.StatusCode, new[] { HttpStatusCode.OK, HttpStatusCode.NotFound });
        }
        [Fact]
        public async Task UpdateUsuario_ReturnsOkOrNotFound()
        {
            // Arrange
            var updatedUser = new UsuarioModel
            {
                Id = 1, // ID fict�cio
                Name = "Usu�rio Atualizado",
                Email = "atualizado@exemplo.com",
                Password = "novasenha123"
            };

            // Act
            var response = await _client.PutAsJsonAsync("/usuarios/atualizar/1", updatedUser);

            // Assert
            Assert.Contains(response.StatusCode, new[] { HttpStatusCode.OK, HttpStatusCode.NotFound });
        }
        [Fact]
        public async Task DeleteUsuario_ReturnsNoContentOrNotFound()
        {
            // Act
            var response = await _client.DeleteAsync("/usuarios/deletar/1"); // ID fict�cio

            // Assert
            Assert.Contains(response.StatusCode, new[] { HttpStatusCode.NoContent, HttpStatusCode.NotFound });
        }
        [Fact]
        public async Task CreateUsuario_ReturnsBadRequest_WhenDataIsInvalid()
        {
            var invalidUser = new UsuarioModel
            {
                Name = "", // Nome inválido
                Email = "email_invalido", // Email mal formatado
                Password = "123" // Senha muito curta
            };

            var response = await _client.PostAsJsonAsync("/usuarios/criar", invalidUser);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
        [Fact]
        public async Task CreateUsuario_ReturnsCreated()
        {
            var newUser = new UsuarioModel
            {
                Name = "Novo Usuário",
                Email = "novo@exemplo.com",
                Password = "senha123",
                IsActive = true,
                IsComplete = true,
                Role = "User",
                Avatar = ""
            };
            var response = await _client.PostAsJsonAsync("/usuarios/criar", newUser);
            var responseBody = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Resposta da API: {responseBody}");
        }
        [Fact]
        public async Task CreateUsuario_WithIdempotencyKey_ReturnsSameResponse()
        {
            var newUser = new UsuarioModel
            {
                Name = "Usuário Teste",
                Email = "teste@exemplo.com",
                Password = "senha123",
                IsActive = true,
                IsComplete = true,
                Role = "User",
                Avatar = "https://exemplo.com/avatar.jpg"
            };

            var idempotencyKey = Guid.NewGuid().ToString();

            var request1 = new HttpRequestMessage(HttpMethod.Post, "/usuarios/criar")
            {
                Content = JsonContent.Create(newUser)
            };
            request1.Headers.Add("Idempotency-Key", idempotencyKey);

            var response1 = await _client.SendAsync(request1);
            var responseBody1 = await response1.Content.ReadAsStringAsync();

            var request2 = new HttpRequestMessage(HttpMethod.Post, "/usuarios/criar")
            {
                Content = JsonContent.Create(newUser)
            };
            request2.Headers.Add("Idempotency-Key", idempotencyKey);

            var response2 = await _client.SendAsync(request2);
            var responseBody2 = await response2.Content.ReadAsStringAsync();

            Console.WriteLine($"Resposta 1: {responseBody1}");
            Console.WriteLine($"Resposta 2: {responseBody2}");

            Assert.Equal(responseBody1, responseBody2); // As respostas devem ser idênticas
        }


        [Fact]
        public async Task GetUsuarioById_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var response = await _client.GetAsync("/usuarios/buscar/99999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task DeleteUsuario_ReturnsNotFound_WhenUserDoesNotExist()
        {
            var response = await _client.DeleteAsync("/usuarios/deletar/99999");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        
    }
}
