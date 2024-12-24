
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SurveyCart.Api.Peristence;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): IdentityDbContext<User>(options)
{
    public DbSet<Poll>Polls { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);

    }
}

