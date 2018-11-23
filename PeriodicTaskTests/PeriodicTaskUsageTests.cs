using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace PeriodicTaskTests
{
    [TestClass]
    public class PeriodicTaskUsageTests
    {
        [TestMethod]
        public void When_Executing_PeriodicTask_Then_DomainCode_Shall_Be_Executed()
        {
            var loggerMock = new Mock<ILogger>();

            var periodicTaskUsage = new PeriodicTaskForTest(loggerMock.Object);

            // Exécution de la 1ere itération
            Assert.IsTrue(periodicTaskUsage.ExecuteIteration().Wait(5000));

            // On peut effectuer des vérifications
            loggerMock.Verify(l => l.Info("Executing job..."), Times.Once);
            loggerMock.Verify(l => l.Info("Job executed"), Times.Once);

            // Exécution d'une 2e itération
            Assert.IsTrue(periodicTaskUsage.ExecuteIteration().Wait(5000));

            //etc...
        }
    }
}
