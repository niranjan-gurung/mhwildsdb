using mhwildsdb.DTOs;
using mhwildsdb.Entities;

namespace mhwildsdb.Helpers.Extensions.Mapping;

public static class ResistancesMappingExtension
{
    public static Resistances ToDomain(this ResistancesDto dto) =>
        new(dto.Fire, dto.Water, dto.Ice, dto.Thunder, dto.Dragon);

    public static ResistancesDto ToDto(this Resistances resistances) =>
        new(resistances.Fire, resistances.Water, resistances.Ice, 
            resistances.Thunder, resistances.Dragon);
}
