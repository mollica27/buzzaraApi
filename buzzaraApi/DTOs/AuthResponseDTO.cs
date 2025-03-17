namespace buzzaraApi.DTOs

{
    public class AuthResponseDTO
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
        public required object UserData { get; set; }
    }
};
