namespace mhwildsdb.DTOs.Armours;

public sealed record UpdateArmourDto(
    string Name,
    string Piece,
    string Rank,
    int Rarity,
    int Defense,
    ResistancesDto Resistances);
