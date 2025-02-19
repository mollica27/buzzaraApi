using buzzaraApi.DTOs;
using buzzaraApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace buzzaraApi.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.Login(loginDTO);
            if (response == null)
                return Unauthorized(new { message = "Email ou senha inválidos." });
            return Ok(response);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            var response = await _authService.RefreshToken(refreshTokenDTO);
            if (response == null)
                return Unauthorized(new { message = "Refresh token inválido ou expirado." });
            return Ok(response);
        }

        [HttpPost("recuperar-senha")]
        public async Task<IActionResult> RecuperarSenha([FromBody] RecuperarSenhaDTO recuperarSenhaDTO)
        {
            var sucesso = await _authService.EnviarRecuperacaoSenha(recuperarSenhaDTO);
            if (!sucesso)
                return NotFound(new { message = "Usuário não encontrado." });
            return Ok(new { message = "Um e-mail de recuperação foi enviado." });
        }

        [HttpPost("redefinir-senha")]
        public async Task<IActionResult> RedefinirSenha([FromBody] RedefinirSenhaDTO redefinirSenhaDTO)
        {
            var sucesso = await _authService.RedefinirSenha(redefinirSenhaDTO);
            if (!sucesso)
                return BadRequest(new { message = "Token inválido ou expirado." });
            return Ok(new { message = "Senha redefinida com sucesso!" });
        }
    }
}
