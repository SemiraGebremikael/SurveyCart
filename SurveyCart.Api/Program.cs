using Hangfire;
using Hangfire.Dashboard;
using HangfireBasicAuthenticationFilter;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);
builder.Host.UseSerilog((context, configuration) =>

    configuration.ReadFrom.Configuration(context.Configuration)
);
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseHangfireDashboard(pathMatch: "/job", options: new DashboardOptions
{
    //Authorization = 
    //[
    //    new HangfireCustomBasicAuthenticationFilter 
    //    {
    //        User = app.Configuration.GetValue<string>(key: "HangFireSettings: Username"),
    //        Pass = app.Configuration.GetValue<string>( key: "HangFireSettings: Password")
    //    }
    //],
    DashboardTitle="Survey Cart Dashboard",
});
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using var scope = scopeFactory.CreateScope();
var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
RecurringJob.AddOrUpdate("SendNewPollNotification", () => notificationService.SendNewPollNotification(null), Cron.Daily);

//app.UseCors();
app.UseAuthorization();
app.MapControllers();
//app.UseExceptionHandler();

app.Run();
