using System.Net;

namespace Application.Exceptions;

public sealed class NotFoundException: AppException
{
    public NotFoundException(string message) : base(message, HttpStatusCode.NotFound)
    { }
}