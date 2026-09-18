using System.Security.Claims;
using API.Models.Responses;
using Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/account")]
[Authorize]
public sealed class AccountController : ControllerBase
{
    [HttpGet("me")]
    public ActionResult<ApiResponse<CurrentUserResponse>> Me()
    {
        var userId = User.FindFirstValue("sub");
        var sessionId = User.FindFirstValue("sid");
        var role = User.FindFirstValue("role");

        if (!Guid.TryParse(userId, out var userUuid) ||!Guid.TryParse(sessionId, out var sessionUuid))
        {
            throw new UnauthorizedException("Invalid authentication token.");
        }

        return Ok(
            new ApiResponse<CurrentUserResponse>
            {
                Data = new CurrentUserResponse
                {
                    UserUuid = userUuid,
                    SessionUuid = sessionUuid,
                    Role = role ?? string.Empty
                }
            });
    }
}
