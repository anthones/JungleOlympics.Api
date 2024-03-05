using System.ComponentModel.DataAnnotations;

namespace JungleOlympics.Api.Entities;

public class Animal
{
    public int Id { get; set; }

    [Required]
    [StringLength(20)]
    public required string Name { get; set; }

    [Required]
    [Range(2, 8)]
    public float Legs { get; set; }

    [Required]
    [StringLength(20)]
    public required string Sound { get; set; }
}
