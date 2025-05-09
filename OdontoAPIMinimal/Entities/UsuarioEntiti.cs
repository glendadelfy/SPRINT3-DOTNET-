namespace OdontoAPIMinimal.Entities
{
    public class UsuarioEntiti
    {
        public int Id { get; set; }
        public required string Name { get; set; } = "Sem nome";
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
        public bool IsActive { get; set; } = true;
        public string Role { get; set; } = "User";
        public string Avatar { get; set; } = "https://images.hdqwalls.com/download/cute-pikachu-6o-3840x2160.jpg";
    }
}
