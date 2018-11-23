using System;
using NLog;
using PeriodicTaskCore.PeriodicJob;

namespace PeriodicTaskCore
{
    public class PeriodicTaskConsumer
    {
        private readonly PeriodicTaskFactory factory;
        private readonly ILogger logger;

        private IDisposable periodicTask;

        public PeriodicTaskConsumer(ILogger logger, PeriodicTaskFactory factory)
        {
            this.factory = factory;
            this.logger = logger;
        } 

        public void LaunchPeriodicTask()
        {
            this.periodicTask = this.factory.CreatePeriodicTask(this.logger, 
                TimeSpan.FromMilliseconds(3000));
        }

        public void StopPeriodicTask()
        {
            this.periodicTask.Dispose();
        }
    }
}