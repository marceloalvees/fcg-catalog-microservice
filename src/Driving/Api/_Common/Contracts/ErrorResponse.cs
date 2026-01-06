using Domain._Common.Exceptions;
using System.Net;
using System.Text.Json.Serialization;

namespace Api._Common.Contracts
{
    public class ErrorResponse
    {
        public ErrorResponse(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ErrorResponse(FCGDuplicateException duplicateException)
        {
            StatusCode = HttpStatusCode.Conflict;
            Message = duplicateException.Message;
            Entity = duplicateException.Entity;
            Key = duplicateException.Key;
        }

        public ErrorResponse(FCGNotFoundException notFoundException)
        {
            StatusCode = HttpStatusCode.NotFound;
            Message = notFoundException.Message;
            Key = notFoundException.Key;
            Entity = notFoundException.Entity;
        }

        public ErrorResponse() { }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        public string Message { get; set; }

        public Guid? Key { get; set; }

        public string? Field { get; set; }

        public string? Entity { get; set; }

        public IReadOnlyCollection<ErrorResponse>? Errors { get; set; }
    }
}
