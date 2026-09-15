
using System.ComponentModel.DataAnnotations;

namespace API.Models.Requests;

public sealed class LoginRequest
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Email format is invalid")]
    [MaxLength(100, ErrorMessage = "Too much characters")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}