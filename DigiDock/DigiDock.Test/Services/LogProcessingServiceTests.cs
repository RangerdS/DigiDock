using DigiDock.Api.Services;
using Moq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiDock.Test.Services
{
    // fill here
    /*
    public class LogProcessingServiceTests
    {
        private readonly Mock<LogQueueService> mockLogQueueService;
        private readonly LogProcessingService logProcessingService;

        public LogProcessingServiceTests()
        {
            mockLogQueueService = new Mock<LogQueueService>();
            logProcessingService = new LogProcessingService(mockLogQueueService.Object);
        }

        [Fact]
        public void ProcessLogMessages_ShouldLogMessages_WhenMessagesAreAvailable()
        {
            // Arrange
            var logMessage = "Test log message";
            mockLogQueueService.Setup(m => m.TryReceiveLog(out logMessage)).Returns(true);

            // Act
            logProcessingService.ProcessLogMessages();

            // Assert
            mockLogQueueService.Verify(m => m.TryReceiveLog(out logMessage), Times.Once);
            Log.Information(logMessage);
        }

        [Fact]
        public void ProcessLogMessages_ShouldNotLogMessages_WhenNoMessagesAreAvailable()
        {
            // Arrange
            string logMessage;
            mockLogQueueService.Setup(m => m.TryReceiveLog(out logMessage)).Returns(false);

            // Act
            logProcessingService.ProcessLogMessages();

            // Assert
            mockLogQueueService.Verify(m => m.TryReceiveLog(out logMessage), Times.Once);
        }
    }*/
}
