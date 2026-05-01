using System.Net;

namespace mhwildsdb.Exceptions
{
    public sealed class NotFoundException(string resource, object key) 
        : AppException($"{resource} with id {key} not found.", HttpStatusCode.NotFound)
    {
    }
}
