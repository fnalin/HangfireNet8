using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using HangfireDemo.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHangfire(config =>
{
    config
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        //.UseInMemoryStorage();
        .UseSqlServerStorage("Data Source=localhost;Initial Catalog=HangFireDb;User ID=sa;Password=MyStrongPass123;Encrypt=False;");
});
builder.Services.AddHangfireServer(sp => sp.SchedulePollingInterval = TimeSpan.FromSeconds(1));

builder.Services.AddTransient<IMyService, MyService>();

var app = builder.Build();

app.MapGet("/", (IBackgroundJobClient jobBg, IRecurringJobManager jobRec) =>
{
    // jobBg.Enqueue(() => Console.WriteLine("Hello from Hangfire - queue!"));
    // jobBg.Schedule(() => Console.WriteLine("Hello from Hangfire - schedule!"), TimeSpan.FromSeconds(5));
    //
    // jobRec.AddOrUpdate("jobId", () => 
    //     Console.WriteLine("Hello from Hangfire - recurring!"), Cron.Minutely);
    //
    //jobBg.Enqueue<IMyService>(x => x.DoWorkAsync());
    // jobBg.Schedule<IMyService>(x=>x.DoWork(), TimeSpan.FromSeconds(5));
    
    jobRec.AddOrUpdate<IMyService>("jobId2", x=>x.DoWorkAsync(), "* * * * * *");

    
    
    return "Hello World!";
});

app.UseHangfireDashboard("/hangfire",new DashboardOptions
{
    Authorization = new[] { new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
    {
        RequireSsl = false,
        SslRedirect = false,
        LoginCaseSensitive = true,
        Users = new []
        {
            new BasicAuthAuthorizationUser
            {
                Login = "admin",
                PasswordClear =  "test"
            } 
        }

    }) }
});

app.Run();