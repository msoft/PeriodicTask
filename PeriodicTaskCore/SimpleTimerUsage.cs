using System;
using NLog;
using System.Threading;

namespace PeriodicTaskCore
{
    public class SimpleTimerUsage : IDisposable
    {
        private readonly ILogger logger;
        private Timer timer;

        public SimpleTimerUsage(ILogger logger, int periodicity)
        {
            this.logger = logger;
            this.timer = new Timer(ExecuteJob, null, 0, periodicity);
        }

        #region IDisposable member

        public void Dispose()
        {
            if (this.timer != null)
            {
                this.timer.Dispose();
                this.timer = null;
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        private void ExecuteJob(object stateInfo)
        {
            this.logger.Info("Executing job...");
            Thread.Sleep(100);
            this.logger.Info("Job executed");
        }
    }
}