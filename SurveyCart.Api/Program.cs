
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddIdentityApiEndpoints<User>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDependencies(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapIdentityApi<User>();

app.MapControllers();

app.Run();
