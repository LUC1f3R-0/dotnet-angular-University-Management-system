using API.Models.Requests;
using API.Models.Responses;
using Application.Authentication.Login;
using Application.Authentication.Logout;
using Application.Authentication.Refresh;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController : ControllerBase
{
    private readonly ILoginService _loginService;
    private readonly IRefreshService _refreshService;
    private readonly ILogoutService _logoutService;
    private readonly IWebHostEnvironment _environment;

    public AuthController(ILoginService loginService, IRefreshService refreshService, ILogoutService logoutService, IWebHostEnvironment environment)
    {
        _loginService = loginService;
        _refreshService = refreshService;
        _logoutService = logoutService;
        _environment = environment;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request,CancellationToken cancellationToken)
    {
        var result = await _loginService.LoginAsync(request.Email, request.Password, HttpContext.Connection.RemoteIpAddress?.ToString(), Request.Headers.UserAgent.ToString(), cancellationToken);

        SetAccessCookie(result.AccessToken, result.AccessTokenExpiresAtUtc);
        SetRefreshCookie(result.RefreshToken, result.RefreshTokenExpiresAtUtc);

        var response =
            new LoginResponse
            {
                UserUuid = result.UserUuid,
                Name = result.Name,
                Email = result.Email,
                Role = result.Role
            };

        return Ok(
            new ApiResponse<LoginResponse>
            {
                Message = "Login successful.",
                Data = response
            });
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<object>>> Refresh(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refresh_token"];
        var result = await _refreshService.RefreshAsync(refreshToken ?? string.Empty, cancellationToken);

        SetAccessCookie(result.AccessToken, result.AccessTokenExpiresAtUtc);
        SetRefreshCookie(result.RefreshToken, result.RefreshTokenExpiresAtUtc);

        return Ok(
            new ApiResponse<object>
            {
                Message = "Authentication refreshed successfully.",
                Data = null
            });
    }

    [AllowAnonymous]
    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<object>>> Logout(CancellationToken cancellationToken)
    {
        var refreshToken = Request.Cookies["refresh_token"];

        await _logoutService.LogoutAsync(refreshToken, cancellationToken);
        
        DeleteAuthenticationCookies();

        return Ok(
            new ApiResponse<object>
            {
                Message = "Logout successful.",
                Data = null
            });
    }

    private void SetAccessCookie(string token, DateTimeOffset expiresAtUtc)
    {
        Response.Cookies.Append("access_token", token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = !_environment.IsDevelopment(),
                SameSite = SameSiteMode.Lax,
                Expires = expiresAtUtc,
                Path = "/api",
                IsEssential = true
            });
    }

    private void SetRefreshCookie(string token, DateTimeOffset expiresAtUtc)
    {
        Response.Cookies.Append("refresh_token", token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = !_environment.IsDevelopment(),
                SameSite = SameSiteMode.Lax,
                Expires = expiresAtUtc,
                Path = "/api/auth",
                IsEssential = true
            });
    }

    private void DeleteAuthenticationCookies()
    {
        Response.Cookies.Delete("access_token",
            new CookieOptions
            {
                Path = "/api"
            });

        Response.Cookies.Delete("refresh_token",
            new CookieOptions
            {
                Path = "/api/auth"
            });
    }
}
