using System.ComponentModel.DataAnnotations;
using JungleOlympics.Api.Helpers;

namespace JungleOlympics.Api.Entities;

public record AnimalDto(int Id, string? Name, float? Legs, string? Sound);

public record CreateAnimalDto(
    [Required] [StringLength(20)] string Name,
    [Required] [Range(2, 8)] float Legs,
    [Required] [StringLength(20)] string Sound
) : IAnimalLegs;

public record UpdateAnimalDto(
    [StringLength(20)] string Name,
    [Range(2, 8)] float Legs,
    [StringLength(20)] string Sound
) : IAnimalLegs;
