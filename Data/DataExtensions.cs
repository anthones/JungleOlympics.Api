using JungleOlympics.Api.Entities;
using JungleOlympics.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JungleOlympics.Api.Data;

public static class DataExtensions
{
    public static async Task InitializeDbAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<JungleOlympicsContext>();
        await dbContext.Database.MigrateAsync();

        if (!dbContext.Animals.Any())
        {
            var animals = new List<Animal>
            {
                new()
                {
                    Id = 1,
                    Name = "Cat",
                    Legs = 4,
                    Sound = "Meooowww"
                },
                new()
                {
                    Id = 2,
                    Name = "Dog",
                    Legs = 4,
                    Sound = "Woof"
                }
            };
            dbContext.Animals.AddRange(animals);
            dbContext.SaveChanges();
        }
    }

    public static IServiceCollection AddRepositories(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connString = configuration.GetConnectionString("JungleOlympicsConnection");

        services
            .AddDbContext<JungleOlympicsContext>(options => options.UseSqlite(connString))
            .AddScoped<IAnimalsRepository, AnimalsRepository>();

        return services;
    }
}
