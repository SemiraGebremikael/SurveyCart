using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDependencies(builder.Configuration);

builder.Services.AddOutputCache(options =>
{
options.AddPolicy("polls", x => 
      x.Cache()
       .Expire(TimeSpan.FromSeconds(120))
       .Tag("availableQuestions")
       );
});

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
//app.UseCors();
app.UseAuthorization();
app.UseOutputCache();

app.MapControllers();
//app.UseExceptionHandler();

app.Run();
