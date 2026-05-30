using mhwildsdb.DTOs.Weapons;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Entities.Weapons;
using mhwildsdb.Exceptions;
using mhwildsdb.Helpers.Extensions.Mapping;
using mhwildsdb.Persistance;
using mhwildsdb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mhwildsdb.Services;

public class WeaponService(
    MhwildsDbContext _context,
    ILogger<WeaponService> _logger) : IWeaponService
{
    public async Task<WeaponDto> CreateWeaponAsync(CreateWeaponDto request)
    {
        var exists = await _context.Weapons.AnyAsync(w => w.Name == request.Name);
        if (exists)
            throw new ConflictException($"Weapon '{request.Name}' already exists.");

        var skillRanks = await GetSkillRanksAsync(request.Skills ?? []);
        var weapon = CreateWeapon(request, skillRanks);

        _context.Weapons.Add(weapon);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new weapon with {WeaponId}", weapon.Id);

        return await GetWeaponQuery()
            .Where(w => w.Id == weapon.Id)
            .Select(w => w.ToDto())
            .FirstAsync();
    }

    public async Task<ICollection<WeaponDto>> CreateWeaponRangeAsync(ICollection<CreateWeaponDto> requests)
    {
        var names = requests.Select(r => r.Name).ToList();
        var duplicateRequestNames = names
            .GroupBy(name => name)
            .Where(group => group.Count() > 1)
            .Select(group => group.Key)
            .ToList();

        if (duplicateRequestNames.Count != 0)
            throw new ConflictException($"Duplicate weapon names in request: {string.Join(", ", duplicateRequestNames)}");

        var existingNames = await _context.Weapons
            .Where(w => names.Contains(w.Name))
            .Select(w => w.Name)
            .ToListAsync();

        if (existingNames.Count != 0)
            throw new ConflictException($"Weapons already exist: {string.Join(", ", existingNames)}");

        var allSkillRankIds = requests
            .SelectMany(r => r.Skills ?? [])
            .Distinct()
            .ToList();

        var allSkillRanks = await GetSkillRanksAsync(allSkillRankIds);
        var skillRankLookup = allSkillRanks.ToDictionary(sr => sr.Id);

        var weapons = requests.Select(r =>
        {
            var skillRanks = (r.Skills ?? [])
                .Where(id => skillRankLookup.ContainsKey(id))
                .Select(id => skillRankLookup[id])
                .ToList();

            return CreateWeapon(r, skillRanks);
        }).ToList();

        _context.Weapons.AddRange(weapons);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created {Count} new weapons", weapons.Count);

        var weaponIds = weapons.Select(w => w.Id).ToList();

        return await GetWeaponQuery()
            .Where(w => weaponIds.Contains(w.Id))
            .Select(w => w.ToDto())
            .ToListAsync();
    }

    public async Task<IEnumerable<WeaponDto>> GetAllWeaponsAsync()
    {
        return await GetWeaponQuery()
            .Select(w => w.ToDto())
            .ToListAsync();
    }

    public async Task<WeaponDto> GetWeaponByIdAsync(Guid id)
    {
        var weapon = await GetWeaponQuery()
            .FirstOrDefaultAsync(w => w.Id == id);

        if (weapon is null)
            throw new NotFoundException("Weapon", id);

        return weapon.ToDto();
    }

    private static Weapon CreateWeapon(CreateWeaponDto request, ICollection<SkillRank> skillRanks)
    {
        var core = new WeaponCore(
            request.Name,
            request.Description,
            request.Defense,
            request.Rarity,
            request.Slots ?? [],
            request.Affinity,
            request.Damage.ToDomain(),
            request.Specials?.Select(s => s.ToDomain()).ToList() ?? [],
            skillRanks);

        return request.WeaponType switch
        {
            WeaponType.Greatsword => Greatsword.Create(core, request.Sharpness!.ToDomain()),
            WeaponType.Longsword => Longsword.Create(core, request.Sharpness!.ToDomain()),
            WeaponType.SwordAndShield => SwordAndShield.Create(core, request.Sharpness!.ToDomain()),
            WeaponType.DualBlades => DualBlades.Create(core, request.Sharpness!.ToDomain()),
            WeaponType.Hammer => Hammer.Create(core, request.Sharpness!.ToDomain()),
            WeaponType.HuntingHorn => HuntingHorn.Create(core, request.Sharpness!.ToDomain()),
            WeaponType.SwitchAxe => SwitchAxe.Create(core, request.Sharpness!.ToDomain(), request.Phial!.ToDomain()),
            WeaponType.ChargeBlade => ChargeBlade.Create(core, request.Sharpness!.ToDomain(), request.Phial!.ToDomain()),
            WeaponType.Lance => Lance.Create(core, request.Sharpness!.ToDomain()),
            WeaponType.Gunlance => Gunlance.Create(core, request.Sharpness!.ToDomain(), request.Shell!.ToDomain()),
            WeaponType.InsectGlaive => InsectGlaive.Create(
                core,
                request.Sharpness!.ToDomain(),
                request.KinsectLevel!.Value),
            WeaponType.LightBowgun => LightBowgun.Create(
                core,
                request.Ammo!.Select(a => a.ToDomain()).ToList(),
                request.SpecialAmmo!),
            WeaponType.HeavyBowgun => HeavyBowgun.Create(
                core,
                request.Ammo!.Select(a => a.ToDomain()).ToList()),
            WeaponType.Bow => Bow.Create(core, request.Coatings ?? []),
            _ => throw new BadRequestException($"Weapon type '{request.WeaponType}' is not supported.")
        };
    }

    private async Task<List<SkillRank>> GetSkillRanksAsync(ICollection<Guid> skillRankIds)
    {
        if (skillRankIds.Count == 0)
            return [];

        var skillRanks = await _context.SkillRanks
            .Include(sr => sr.Skill)
            .Where(sr => skillRankIds.Contains(sr.Id))
            .ToListAsync();

        var missingIds = skillRankIds
            .Except(skillRanks.Select(sr => sr.Id))
            .ToList();

        if (missingIds.Count != 0)
            throw new NotFoundException("SkillRanks", string.Join(", ", missingIds));

        return skillRanks;
    }

    private IQueryable<Weapon> GetWeaponQuery()
    {
        return _context.Weapons
            .AsNoTracking()
            .Include(w => w.SkillRanks)
                .ThenInclude(sr => sr.Skill);
    }
}
