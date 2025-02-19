namespace buzzaraApi.DTOs
{
    public class AuthResponseDTO
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
