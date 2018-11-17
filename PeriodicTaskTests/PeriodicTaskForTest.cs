using System;
using System.Threading.Tasks;
using PeriodicTaskCore;
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
            this.CreateAndLaunchTimer();

            await this.Timer;
        }
    }
}