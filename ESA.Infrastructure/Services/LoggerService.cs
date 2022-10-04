using GV.DomainModel.SharedKernel.Interfaces;
using Microsoft.Extensions.Logging;

namespace ESA.Infrastructure.Services
{
    public class LoggerService<T> : IAppLogger<T>
    {
        private readonly ILogger<T> logger;

        public LoggerService(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<T>();
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            logger.LogError(exception, message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            logger.LogWarning(message, args);
        }
    }
}
