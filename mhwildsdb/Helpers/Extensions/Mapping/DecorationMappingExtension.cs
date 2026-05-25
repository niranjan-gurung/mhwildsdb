using mhwildsdb.DTOs.Decorations;
using mhwildsdb.Entities;

namespace mhwildsdb.Helpers.Extensions.Mapping;

public static class DecorationMappingExtension
{
    public static DecorationDto ToDto(this Decoration decoration) =>
        new(
            decoration.Id,
            decoration.Name,
            decoration.Description,
            decoration.Type,
            decoration.Rarity,
            decoration.Slot,
            decoration.Skills.Select(sr => sr.ToDetailDto()).ToList()
        );
}