using OdontoAPIMinimal;
using OdontoMinimalAPI.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OdontoMinimalAPI.Models
{
    [Table("table_usuario_usuario_odonto")]
    public class UsuarioModel
    {
        [Key]
        public int Id { get; set; }
        [Column("name_user")]
        public required string Name { get; set; } = "Sem nome";
        [Column("email_user")]
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Column("password_user")]
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        [Column("status_active_user")]
        public bool IsComplete { get; set; }
        [Column("is_complete")]
        public bool isActive { get; set; } = true;
        [Column("role_user")]
        public string Role { get; set; } = "User";
        [Column("avatar_user")]
        public string Avatar { get; set; } = "https://images.hdqwalls.com/download/cute-pikachu-6o-3840x2160.jpg";

        public UsuarioModel() { }
        public UsuarioModel(UsuarioEntiti todo) => (Id, Name, IsComplete)
        = (todo.Id, todo.Name, todo.IsComplete);

    }
}
