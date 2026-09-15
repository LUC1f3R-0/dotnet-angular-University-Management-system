using API.Models.Requests;
using API.Models.Responses;
using Application.Authentication.Login;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ILoginService _loginService;

    public AuthController(ILoginService loginService)
    {
        _loginService = loginService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request,CancellationToken cancellationToken)
    {
        var result = await _loginService.LoginAsync(request.Email, request.Password, HttpContext.Connection.RemoteIpAddress?.ToString(), Request.Headers.UserAgent.ToString(), cancellationToken);


        var loginResponse =
            new LoginResponse
            {
                AccessToken = result.AccessToken,
                AccessTokenExpiresAtUtc = result.AccessTokenExpiresAtUtc,
                RefreshToken = result.RefreshToken,
                RefreshTokenExpiresAtUtc = result.RefreshTokenExpiresAtUtc,
                UserUuid = result.UserUuid,
                Name = result.Name,
                Email = result.Email,
                Role = result.Role
            };
        return Ok(
            new ApiResponse<LoginResponse>
            {
                Message = "Login successful.",
                Data = loginResponse
            });
    }
}