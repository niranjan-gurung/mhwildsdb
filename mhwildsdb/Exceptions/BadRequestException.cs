using System.Net;

namespace mhwildsdb.Exceptions
{
    public sealed class BadRequestException(string message) 
        : AppException(message, HttpStatusCode.BadRequest)
    {
    }
}
