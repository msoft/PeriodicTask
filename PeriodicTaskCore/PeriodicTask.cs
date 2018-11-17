using System;
using System.Threading.Tasks;
using System.Threading;
using NLog;

namespace PeriodicTaskCore
{
	public abstract class PeriodicTask : IDisposable
	{
		#region Fields

		private TimeSpan? periodicity;
		private Task timer;
		private readonly object timerCreationLock = new object();
		private readonly CancellationTokenSource cancellationTokenSource;

		#endregion

		protected PeriodicTask(ILogger logger, TimeSpan? periodicity)
		{
			this.Logger = logger;
			this.periodicity = periodicity;
			this.cancellationTokenSource = new CancellationTokenSource();
		}

		protected bool IsDisposing { get; private set; }

		public void Dispose()
		{
			this.IsDisposing = false;

			if (this.IsTimerRunning() && !this.cancellationTokenSource.Token.IsCancellationRequested)
			{
				this.OnPeriodicTaskStopping();
				this.cancellationTokenSource.Cancel();

				bool completed = false;
				try
				{
					completed = this.timer.Wait(TimeSpan.FromSeconds(5));
				}
				catch (AggregateException occuredException)
				{

					occuredException.Handle(ex => 
					{
						if (ex is TaskCanceledException || ex is OperationCanceledException)
						{
							completed = true;
							return true;
						}

						return false;
					});
				}
			}

			this.Disposing();

			GC.SuppressFinalize(this);
		}

		#region Abstract members

		protected abstract void ExecutePeriodicTask(CancellationToken cancellationToken);

		#endregion

		protected ILogger Logger { get; }

		protected Task Timer => this.timer;

		protected virtual void Disposing()
		{

		}

		protected void CreateAndLaunchTimer()
		{
			var cancellationToken = this.cancellationTokenSource.Token;

			lock(this.timerCreationLock)
			{
				if (!this.IsTimerRunning())
				{
					this.timer = new Task(() => 
					{
						this.ExecuteJobOnce(cancellationToken);
						this.ExecuteJobPeriodically(cancellationToken);
					}, cancellationToken, TaskCreationOptions.LongRunning);

					this.timer.ContinueWith(t => 
					{
						this.OnPeriodicTaskFaulted(t.Exception);
					}, TaskContinuationOptions.OnlyOnFaulted);

					this.timer.ContinueWith(t =>
					{
						this.OnPeriodicTaskCanceled();
					}, TaskContinuationOptions.OnlyOnCanceled);

					this.timer.ContinueWith(t =>
					{
						this.OnPeriodiTaskCompleted();
					}, TaskContinuationOptions.OnlyOnRanToCompletion);

					this.timer.Start();
				}
			}
		}

		protected virtual void OnPeriodicTaskFaulted(AggregateException exception)
		{
			this.Logger.Error("Periodic task raised an exception: {0}", exception);
		}

		protected virtual void OnPeriodicTaskCanceled()
		{
			this.Logger.Info("Periodic task has been canceled.");
		}

		protected virtual void OnPeriodiTaskCompleted()
		{
			this.Logger.Info("Periodic task ended");
		}

		protected virtual void OnPeriodicTaskStopping()
		{
			this.Logger.Info("Stopping the periodic task");
		}

		private void ExecuteJobPeriodically(CancellationToken cancellationToken)
		{
			if (!this.periodicity.HasValue) return;

			while (true)
			{
				Task.Delay(this.periodicity.Value, cancellationToken).Wait(cancellationToken);
				cancellationToken.ThrowIfCancellationRequested();

				this.ExecuteJobOnce(cancellationToken);
			}
		}

		private void ExecuteJobOnce(CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested) return;

			this.ExecutePeriodicTask(cancellationToken);
		}

		private bool IsTimerRunning()
		{
			return !(this.timer == null || this.timer.IsFaulted || this.timer.IsCompleted);
		}
	}
}
