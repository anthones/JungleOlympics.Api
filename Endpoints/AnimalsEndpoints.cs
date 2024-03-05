using JungleOlympics.Api.Entities;
using JungleOlympics.Api.Helpers;
using JungleOlympics.Api.Repositories;

namespace JungleOlympics.Api.Endpoints;

public static class AnimalsEndpoints
{
    static readonly string GetAnimalEndpointName = "GetAnimal";

    public static RouteGroupBuilder MapAnimalsEndpoints(this IEndpointRouteBuilder routes)
    {
        var animalGroup = routes.MapGroup("/api/animal").WithParameterValidation();
        var animalTeamsGroup = routes.MapGroup("/api/animalTeams").WithParameterValidation();

        animalGroup.MapGet(
            "/",
            async (IAnimalsRepository repository) =>
                (await repository.GetAllAsync()).Select(animal => animal.AsDto())
        );

        animalGroup
            .MapGet(
                "/{id}",
                async (IAnimalsRepository repository, int id) =>
                {
                    Animal? animal = await repository.GetAsync(id);

                    return animal is not null ? Results.Ok(animal.AsDto()) : Results.NotFound();
                }
            )
            .WithName(GetAnimalEndpointName);

        animalTeamsGroup.MapGet(
            "/",
            async (IAnimalsRepository repository) =>
            {
                var totalLegs = (await repository.GetAllAsync()).Sum(a => a.Legs);
                var completedTeams = totalLegs / Constants.AnimalLegsCoeficient;

                return Results.Ok(Math.Floor(completedTeams));
            }
        );

        animalGroup.MapPost(
            "/",
            async (IAnimalsRepository repository, CreateAnimalDto animalDto) =>
            {
                int integerLegs;

                try
                {
                    integerLegs = animalDto.GetIntegerLegs();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }

                if (!repository.CanAnimalBeAdded(integerLegs))
                {
                    return Results.BadRequest(
                        "Adding this animal would violate team leg count constraints."
                    );
                }

                Animal animal =
                    new()
                    {
                        Name = animalDto.Name,
                        Legs = integerLegs,
                        Sound = animalDto.Sound
                    };

                await repository.CreateAsync(animal);

                return Results.CreatedAtRoute(
                    GetAnimalEndpointName,
                    new { id = animal.Id },
                    animalDto
                );
            }
        );

        animalGroup.MapPut(
            "/{id}",
            async (IAnimalsRepository repository, int id, UpdateAnimalDto updatedAnimalDto) =>
            {
                Animal? existingAnimal = await repository.GetAsync(id);

                int integerLegs;

                if (existingAnimal is null)
                {
                    return Results.NotFound();
                }

                try
                {
                    integerLegs = updatedAnimalDto.GetIntegerLegs();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(ex.Message);
                }

                bool isLastAnimal = repository.IsLastAnimal(id);
                bool legsChangeRequested = existingAnimal.Legs != integerLegs;

                if (isLastAnimal && legsChangeRequested)
                {
                    if (!repository.CanAnimalBeUpdated(id, integerLegs))
                    {
                        return Results.BadRequest(
                            "Changing the number of legs would violate team leg count constraints."
                        );
                    }
                    else
                    {
                        existingAnimal.Legs = integerLegs;
                    }
                }
                else if (!isLastAnimal && legsChangeRequested)
                {
                    return Results.BadRequest(
                        "Only the last entered animal can have its number of legs changed."
                    );
                }

                if (!legsChangeRequested || (isLastAnimal && legsChangeRequested))
                {
                    existingAnimal.Name = updatedAnimalDto.Name ?? existingAnimal.Name;
                    existingAnimal.Sound = updatedAnimalDto.Sound ?? existingAnimal.Sound;

                    await repository.UpdateAsync(existingAnimal);
                }

                return Results.NoContent();
            }
        );

        animalGroup.MapDelete(
            "/{id}",
            async (IAnimalsRepository repository, int id) =>
            {
                Animal? animal = await repository.GetAsync(id);

                if (repository.IsLastAnimal(id))
                {
                    if (animal is not null)
                        await repository.DeleteAsync(id);
                }
                else
                {
                    return Results.BadRequest("Only the last entered animal can be deleted.");
                }

                return Results.NoContent();
            }
        );

        return animalGroup;
    }
}
