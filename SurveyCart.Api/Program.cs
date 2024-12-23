using SurveyCart.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDependencies();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
