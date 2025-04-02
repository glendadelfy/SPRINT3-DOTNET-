//using Microsoft.EntityFrameworkCore;
//using Moq;
//using OdontoAPIMinimal.Infraestrutura.Database;
//using OdontoAPIMinimal.Middelewares.Endpoints;
//using OdontoMinimalAPI.Models;
//using Xunit;

//namespace OdontoAPIMinimal.Testes
//{
//    public class UsuarioEnpointTests
//    {
//        private readonly Mock<AppDbContext> _mockDbContext;

//        public UsuarioEnpointTests()
//        {
//            _mockDbContext = new Mock<AppDbContext>();
//        }

//        [Fact] // Define o método como um teste
//        public async Task GetAllUsuarios_ShouldReturnListOfUsuarios()
//        {
//            // Arrange: Configurando o contexto falso
//            var mockUsuarios = new List<UsuarioModel>
//            {
//                new UsuarioModel { Id = 1, Name = "Glenda", Email = "glenda@example.com" },
//                new UsuarioModel { Id = 2, Name = "John", Email = "john@example.com" }
//            };

//            var mockDbSet = new Mock<DbSet<UsuarioModel>>();
//            mockDbSet.Setup(static m => m.ToListAsync()).ReturnsAsync(mockUsuarios);

//            _mockDbContext.Setup(db => db.Usuarios).Returns(mockDbSet.Object);

//            // Act: Chamando o método GetAllUsuarios
//            var result = await UsuarioEndpoint.GetAllUsuarios(_mockDbContext.Object);

//            // Assert: Validando o resultado
//            result.Should().NotBeNull(); // Verifica se o resultado não é nulo
//            result.Should().HaveCount(2); // Verifica se há 2 itens na lista
//        }

//    }
//}
