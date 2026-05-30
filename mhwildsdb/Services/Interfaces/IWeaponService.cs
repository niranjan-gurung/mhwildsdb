using mhwildsdb.DTOs.Weapons;

namespace mhwildsdb.Services.Interfaces;

public interface IWeaponService
{
    Task<WeaponDto> CreateWeaponAsync(CreateWeaponDto request);
    Task<ICollection<WeaponDto>> CreateWeaponRangeAsync(ICollection<CreateWeaponDto> requests);
    Task<IEnumerable<WeaponDto>> GetAllWeaponsAsync();
    Task<WeaponDto> GetWeaponByIdAsync(Guid id);
}
