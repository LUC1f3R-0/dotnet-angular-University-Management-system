using Domain.Entities;

namespace Application.Authentication.Abstractions;

public interface IPasswordService
{
    bool VerifyPassword(User user, string password);
}