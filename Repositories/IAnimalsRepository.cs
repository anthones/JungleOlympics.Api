using JungleOlympics.Api.Entities;

namespace JungleOlympics.Api.Repositories;

public interface IAnimalsRepository
{
    Task CreateAsync(Animal animal);
    Task DeleteAsync(int id);
    Task<Animal?> GetAsync(int id);
    Task<IEnumerable<Animal>> GetAllAsync();
    Task UpdateAsync(Animal updatedAnimal);
    bool CanAnimalBeAdded(int legs);
    bool CanAnimalBeUpdated(int animalId, int newLegs);
    bool IsLastAnimal(int animalId);
}
