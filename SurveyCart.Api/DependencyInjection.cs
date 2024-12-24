
namespace SurveyCart.Api;
public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration Configuration)
    {
       services.AddControllers();
       services.AddEndpointsApiExplorer();
       services.AddSwaggerGen();
       services.AddTransient<IPollService, PollService>();
       services.AddFluentValidationAutoValidation()
               .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        var mappingConfiguration = TypeAdapterConfig.GlobalSettings;
        mappingConfiguration.Scan(Assembly.GetExecutingAssembly());
        services.AddSingleton<IMapper>(implementationInstance: new Mapper(mappingConfiguration));

        var connectionString = Configuration.GetConnectionString("DefaultConnection")??
            throw new InvalidOperationException("connection String 'DefaultConnection' not found");
        services.AddDbContext<ApplicationDbContext>(options =>
         options.UseSqlServer(connectionString));





        return services;

    }
}   
