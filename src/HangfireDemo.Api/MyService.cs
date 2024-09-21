namespace HangfireDemo.Api;

public class MyService (ILogger<IMyService> logger) : IMyService
{
    public void DoWork()
    {
        logger.LogInformation("Doing work...");
    }

    public async Task DoWorkAsync()
    {
        logger.LogInformation("Doing work asynchronously...");
        await Task.Delay(5000);
        logger.LogInformation("Work done.");
    }
}

public interface IMyService
{
    void DoWork();
    Task DoWorkAsync();
}