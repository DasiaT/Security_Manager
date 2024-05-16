using Manager_Security_BackEnd.Services.Error_Services;

namespace Manager_Security_BackEnd.Interfaces
{
    public interface IError
    {
        ErrorServices GetBadRequestException(string message, int errorCode);
        ErrorServices GetNotFoundException(string message, int errorCode);
        ErrorServices GetConflictException(string message, int errorCode);
        ErrorServices GetInternalServerException(string message, int errorCode);
    }
}
