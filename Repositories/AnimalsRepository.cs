using JungleOlympics.Api.Data;
using JungleOlympics.Api.Entities;
using JungleOlympics.Api.Helpers;
using Microsoft.EntityFrameworkCore;

namespace JungleOlympics.Api.Repositories;

public class AnimalsRepository(JungleOlympicsContext context) : IAnimalsRepository
{
    private readonly JungleOlympicsContext dbContext = context;

    public async Task<IEnumerable<Animal>> GetAllAsync()
    {
        return await dbContext.Animals.AsNoTracking().ToListAsync();
    }

    public async Task<Animal?> GetAsync(int id)
    {
        return await dbContext.Animals.FindAsync(id);
    }

    public async Task CreateAsync(Animal animal)
    {
        dbContext.Animals.Add(animal);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Animal updatedAnimal)
    {
        dbContext.Animals.Update(updatedAnimal);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        await dbContext.Animals.Where(animal => animal.Id == id).ExecuteDeleteAsync();
    }

    public bool CanAnimalBeAdded(int legs)
    {
        var totalLegs = dbContext.Animals.Sum(a => a.Legs);
        var currentTeamLegs = totalLegs % Constants.AnimalLegsCoeficient;
        var fitsCurrentTeam = currentTeamLegs + legs <= Constants.AnimalLegsCoeficient;

        return fitsCurrentTeam;
    }

    public bool CanAnimalBeUpdated(int animalId, int newLegs)
    {
        var animal = dbContext.Animals.FirstOrDefault(a => a.Id == animalId);
        if (animal == null)
            return false;

        var legDifference = newLegs - animal.Legs;

        var newTotalLegs = dbContext.Animals.Sum(a => a.Legs) + legDifference;

        var teamsAreValid =
            newTotalLegs % Constants.AnimalLegsCoeficient <= Constants.AnimalLegsCoeficient
            && (newTotalLegs / Constants.AnimalLegsCoeficient) == dbContext.Animals.Count() / 2;

        return teamsAreValid;
    }

    public bool IsLastAnimal(int animalId)
    {
        var lastAnimal = dbContext.Animals.OrderByDescending(a => a.Id).FirstOrDefault();
        return lastAnimal != null && lastAnimal.Id == animalId;
    }
}
