namespace JungleOlympics.Api.Helpers;

public interface IAnimalLegs
{
    float Legs { get; }
}

public static class AnimalLegsExtensions
{
    public static int GetIntegerLegs(this IAnimalLegs animalLegs)
    {
        if (animalLegs.Legs % 1 > 0)
        {
            throw new ArgumentException("Legs must be a whole number.");
        }
        if ((int)animalLegs.Legs % 2 != 0)
        {
            throw new ArgumentException("Legs must be an even number.");
        }
        return (int)animalLegs.Legs;
    }
}
