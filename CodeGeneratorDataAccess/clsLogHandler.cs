using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace CodeGeneratorDataAccess
{
    public class clsLogHandler
    {
        public static void LogToEventViewer(string errorType, Exception ex)
        {
            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var sourceName = configuration.GetSection("ProjectName").Value;

            // Create the event source if it does not exist
            if (!EventLog.SourceExists(sourceName))
            {
                EventLog.CreateEventSource(sourceName, "Application");
            }

            string errorMessage = $"{errorType} in {ex.Source}\\n\\nException Message: {ex.Message}\\n\\nException Type: {ex.GetType().Name}\\n\\nStack Trace: {ex.StackTrace}\\n\\nException Location: {ex.TargetSite}";

            // Log an error event
            EventLog.WriteEntry(sourceName, errorMessage, EventLogEntryType.Error);
        }
    }
}
