using Microsoft.AspNetCore.Mvc;
using Saay.Infrastructure.DTOs.AuthDTOs;
using Saay.Infrastructure.DTOs.UserDTOs;
using Saay.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Saay.API.Controllers
{
    [Route("api/saay/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly IUserTokenService _userTokenService;

        public AuthController(IUserService userService, IPasswordHasher passwordHasher,
            ITokenService tokenService, IConfiguration configuration, IUserTokenService usertoken)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _configuration = configuration;
            _userTokenService = usertoken;
        }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            LoginUserDto loginUserDto = await _userService.FindUserByEmailAsync(request.Email);

            if (loginUserDto == null)
                return Unauthorized("Invalid credentials");

            if (!_passwordHasher.VerifyPassword(request.Password, loginUserDto.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = _tokenService.GenerateJwtToken(loginUserDto, _configuration);

            if (token is null)
                return StatusCode(500, "JWT key missing from Key Vault");

            string refreshToken = _tokenService.GenerateRefreshToken();

            bool? loginResult = await _userTokenService.LoginAsync(loginUserDto.UserId, refreshToken, DateTime.UtcNow.AddDays(7));

            if (loginResult is null)
                return Unauthorized("Invalid credentials");

            if (loginResult == false)
                return StatusCode(500, "An error occurred while logging in");

            return Ok(new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken
            });
        }
    }
}
