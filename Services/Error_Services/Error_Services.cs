

using Manager_Security_BackEnd.Interfaces;

namespace Manager_Security_BackEnd.Services.Error_Services
{
    
    public class ErrorServices : Exception
    {
        public int StatusCode { get; }
        public int ErrorCode { get; }

        public ErrorServices(string message, int statusCode, int errorCode)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }

    public class Error : IError
    {
        public ErrorServices GetBadRequestException(string message, int errorCode)
        {
            return new ErrorServices(message, StatusCodes.Status400BadRequest, errorCode);
        }

        public ErrorServices GetNotFoundException(string message, int errorCode)
        {
            return new ErrorServices(message, StatusCodes.Status404NotFound, errorCode);
        }
        public ErrorServices GetConflictException(string message, int errorCode)
        {
            return new ErrorServices(message, StatusCodes.Status409Conflict, errorCode);
        }
        public ErrorServices GetInternalServerException(string message, int errorCode)
        {
            return new ErrorServices(message, StatusCodes.Status500InternalServerError, errorCode);
        }
    }
}
