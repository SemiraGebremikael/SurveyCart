using MapsterMapper;
using System.Reflection;

namespace SurveyCart.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
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
            return services;

        }
    }   
}
