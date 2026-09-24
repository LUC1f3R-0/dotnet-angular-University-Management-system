using System.Net;

namespace Application.Exceptions;

public sealed class ValidationException : AppException
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public ValidationException(string message): base(message, HttpStatusCode.BadRequest)
    {
        Errors = new Dictionary<string, string[]>();
    }

    public ValidationException(IReadOnlyDictionary<string, string[]> errors) : base("One or more validation errors occurred.", HttpStatusCode.BadRequest)
    {
        Errors = errors;
    }
}
