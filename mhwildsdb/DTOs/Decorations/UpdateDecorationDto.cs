namespace mhwildsdb.DTOs.Decorations;

public sealed record UpdateDecorationDto(
    string Name,
    string Description,
    string Type,
    int Rarity,
    int Slot);