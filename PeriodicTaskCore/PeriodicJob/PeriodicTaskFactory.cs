using System;
using NLog;

namespace PeriodicTaskCore.PeriodicJob
{
    public class PeriodicTaskFactory
    {
        public IDisposable CreatePeriodicTask(ILogger logger, TimeSpan periodicity)
        {
            return new PeriodicTaskUsage(logger, periodicity);
        }
    }
}