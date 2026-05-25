using mhwildsdb.DTOs.Decorations;

namespace mhwildsdb.Services.Interfaces;

public interface IDecorationService
{
    Task<DecorationDto> CreateDecorationAsync(CreateDecorationDto request);
    Task<ICollection<DecorationDto>> CreateDecorationRangeAsync(ICollection<CreateDecorationDto> requests);
    Task<DecorationDto> GetDecorationByIdAsync(Guid id);
    Task<IEnumerable<DecorationDto>> GetAllDecorationsAsync();
    Task UpdateDecorationAsync(Guid id, UpdateDecorationDto request);
    Task DeleteDecorationAsync(Guid id);
}