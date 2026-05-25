namespace mhwildsdb.DTOs.Decorations;

public sealed record CreateDecorationDto(
    string Name,
    string Description,
    string Type,
    int Rarity,
    int Slot,
    ICollection<Guid> Skills);
