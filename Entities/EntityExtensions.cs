namespace JungleOlympics.Api.Entities;

public static class EntityExtensions
{
    public static AnimalDto AsDto(this Animal animal)
    {
        return new AnimalDto(animal.Id, animal.Name, animal.Legs, animal.Sound);
    }
}
