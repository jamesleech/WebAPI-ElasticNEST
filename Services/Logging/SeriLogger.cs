using Serilog;

namespace CountDownAPI.Services
{
    public class SeriLogger : ILog
    {
        private ILogger _logger;
        
        public SeriLogger()
        {
            _logger = new LoggerConfiguration()
                .WriteTo.ColoredConsole()
                .CreateLogger();
                
                _logger.Debug("Logger started");
        }
        
        public void Log(string text, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "")
        {
            _logger.Information($"{memberName} - {text}");
        }
    }
}