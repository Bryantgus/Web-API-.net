namespace CRUDconSQL.Models
{
    public class LoginDto
    {
        public string Usuario { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
    }

    public class RegisterDto
    {
        public string Usuario { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
    }

    public class LoginResponseDto
    {
        public string Token { get; set; } = null!;
        public string Usuario { get; set; } = null!;
        public int UserId { get; set; }
        public DateTime Expiry { get; set; }
    }
}