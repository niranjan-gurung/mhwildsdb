using System.Net;

namespace mhwildsdb.Exceptions
{
    public sealed class ConflictException(string message) 
        : AppException(message, HttpStatusCode.Conflict)
    {
    }
}
