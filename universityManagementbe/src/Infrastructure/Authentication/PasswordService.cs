using Application.Authentication.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Authentication;

public sealed class PasswordService : IPasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordService(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public bool VerifyPassword(User user,string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result != PasswordVerificationResult.Failed;
    }
}