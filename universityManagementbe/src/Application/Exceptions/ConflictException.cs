using System.Net;

namespace Application.Exceptions;

public sealed class ConflictException : AppException
{
    public ConflictException(string message) : base(message, HttpStatusCode.Conflict)
    { }
}