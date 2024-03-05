using JungleOlympics.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace JungleOlympics.Api.Data;

public class JungleOlympicsContext(DbContextOptions<JungleOlympicsContext> options)
    : DbContext(options)
{
    public DbSet<Animal> Animals => Set<Animal>();
}
