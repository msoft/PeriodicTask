using System;
using System.Threading.Tasks;
using PeriodicTaskCore.PeriodicJob;
using NLog;

namespace PeriodicTaskTests
{
    internal class PeriodicTaskForTest : PeriodicTaskUsage
    {
        public PeriodicTaskForTest(ILogger logger) :
            base(logger, null)
        {

        }

        public async Task ExecuteIteration()
        {
            this.LaunchJob();

            await this.Timer;
        }
    }
}