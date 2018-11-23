using System;

namespace PeriodicTaskCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = NLog.LogManager.GetCurrentClassLogger();

            //var timerUsage = new SimpleTimerUsage(logger, 3000);
            var periodicTaskUsage = new PeriodicTaskConsumer(logger, 
                new PeriodicJob.PeriodicTaskFactory());
            periodicTaskUsage.LaunchPeriodicTask();
            // var periodicTaskUsage = new PeriodicTaskUsage(logger, 
            //     TimeSpan.FromMilliseconds(3000));

            Console.ReadLine();

            periodicTaskUsage.StopPeriodicTask();
        }
    }
}
