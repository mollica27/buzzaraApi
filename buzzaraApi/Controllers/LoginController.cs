using buzzaraApi.DTOs;
using buzzaraApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace buzzaraApi.Controllers
{
    [Route("api/login")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            try
            {
                var token = await _loginService.Login(loginDTO);

                if (token == null)
                    return Unauthorized(new { message = "Email ou senha inválidos." });

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
