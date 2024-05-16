using Microsoft.Extensions.Logging;

namespace Manager_Security_BackEnd.Models.Generals
{
    public class General_Logger
    {
        public static void LogInformation(ILogger logger) => _informationSinParametros(logger, null);

        private static readonly Action<ILogger, Exception> _informationSinParametros = LoggerMessage.Define(LogLevel.Information,1,"Mensaje de log sin parámetros");
    }
}
