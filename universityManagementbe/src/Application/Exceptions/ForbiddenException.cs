using System.Net;

namespace Application.Exceptions;

public sealed class ForbiddenException : AppException
{
    public ForbiddenException(string message):base(message, HttpStatusCode.Forbidden)
    {}
}