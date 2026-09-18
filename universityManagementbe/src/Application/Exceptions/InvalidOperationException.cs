using System.Net;
using Application.Exceptions;

namespace Application.Exceptions;

public sealed class InvalidRequestOperationException : AppException
{
    public InvalidRequestOperationException(string message) : base(message, HttpStatusCode.BadRequest)
    { }
}