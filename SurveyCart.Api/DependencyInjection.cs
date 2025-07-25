
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
namespace SurveyCart.Api;
public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration Configuration)
    {
       services.AddControllers();
        //services.AddCors(options =>
        //                 options.AddDefaultPolicy(
        //                     builder => builder .AllowAnyMethod()
        //                                         .AllowAnyHeader()
        //                                         .WithOrigins(Configuration.GetSection("AllowedOrgins").Get<string[]>()!)

        //                 ));
       services.AddEndpointsApiExplorer();
       services.AddSwaggerGen();
       services.AddTransient<IPollService, PollService>();
       services.AddScoped<IAuthService, AuthService>();
       services.AddScoped<IQuestionService, QuestionService>();
       //services.AddExceptionHandler<GlobalExceptionHandler>();
       //services.AddProblemDetails();
       services.AddHybridCache();
       services.AddAuthConfig(Configuration);

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

    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration Configuration)
    {
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

        services.AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateDataAnnotations()
                .ValidateOnStart();

        var jwtSetting = Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
          .AddJwtBearer(o =>
          {
              o.SaveToken = true;
              o.TokenValidationParameters = new TokenValidationParameters
              {
                  ValidateIssuerSigningKey = true,
                  ValidateIssuer = true,
                  ValidateAudience = true,
                  ValidateLifetime = true,
                  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:key"]!)),
                  ValidIssuer =Configuration["Jwt:Issuer"],
                  ValidAudience =Configuration["Jwt:Audience"],
                  
              };

          });

        services.Configure<IdentityOptions>(options =>
        {
            // Default Password settings.
            options.Password.RequiredLength = 6;
            options.SignIn.RequireConfirmedEmail = true;
            options.User.RequireUniqueEmail = true;


        });


        return services;

    }
}

