using OdontoAPIMinimal.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OdontoAPIMinimal.Models
{
    [Table("table_usuario_usuario_odonto")]
    public class UsuarioModel
    {
        [Key]
        public int Id { get; set; }
        [Column("name_user")]
        [Required]
        [EmailAddress]
        [MaxLength(100)] // Define o comprimento máximo
        public required string Name { get; set; } = "Sem nome";

        [Column("email_user")]
        [Required]
        [EmailAddress]
        [MaxLength(255)] // Define o comprimento máximo
        public string Email { get; set; } = string.Empty;

        [Column("password_user")]
        [Required]
        [MinLength(6)]
        [MaxLength(128)] // Define o comprimento máximo
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Column("is_complete")]
        public bool IsComplete { get; set; }
        [Column("status_active_user")]
        public bool IsActive { get; set; } = true;
        [Column("role_user")]
        [MaxLength(50)] // Define o comprimento máximo
        public string Role { get; set; } = "User";

        [Column("avatar_user")]
        [MaxLength(2083)] // Defina o comprimento máximo de acordo com as melhores práticas para URLs
        public string Avatar { get; set; } = "https://images.hdqwalls.com/download/cute-pikachu-6o-3840x2160.jpg";
        public UsuarioModel() { }
        public UsuarioModel(UsuarioEntiti todo) => (Id, Name, IsComplete)
        = (todo.Id, todo.Name, todo.IsComplete);

    }
}
