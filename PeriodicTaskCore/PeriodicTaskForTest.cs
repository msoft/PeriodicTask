// internal class PeriodicTaskForTest : UsingPeriodicTask
// {
// 	public PeriodicTaskForTest(ILogger logger, Action<CancellationToken> dequeueBondDataRequests) :
// 		base(logger, null, dequeueBondDataRequests)
// 	{

// 	}

// 	public async Task Dequeue()
// 	{
// 		this.CreateAndLaunchTimerIfNeeded();

// 		await this.Timer;
// 	}
// }