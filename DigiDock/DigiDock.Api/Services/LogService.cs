using Serilog;
using System.Threading.Tasks;

namespace DigiDock.Api.Services
{
    public class LogService
    {
        public Task LogAsync(string logLevel, string message)
        {
            switch (logLevel.ToLower())
            {
                case "information":
                    Log.Information(message);
                    break;
                case "warning":
                    Log.Warning(message);
                    break;
                case "error":
                    Log.Error(message);
                    break;
                default:
                    Log.Information(message);
                    break;
            }

            return Task.CompletedTask;
        }
    }
}