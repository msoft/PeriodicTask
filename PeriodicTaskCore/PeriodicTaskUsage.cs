using System;
using System.Threading.Tasks;
using System.Threading;
using NLog;

namespace PeriodicTaskCore
{
    public class PeriodicTaskUsage : PeriodicTask
    {
        public PeriodicTaskUsage(ILogger logger, TimeSpan? periodicity) :
        base(logger, periodicity)
        {
            // Lancement de la tâche périodique
           this.CreateAndLaunchTimer(); 
        }

        protected override void ExecutePeriodicTask(CancellationToken cancellationToken)
        {
            this.Logger.Info("Executing job...");
            Thread.Sleep(100);
            this.Logger.Info("Job executed");
        }
    }
}