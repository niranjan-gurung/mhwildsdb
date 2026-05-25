using mhwildsdb.DTOs.Armours;
using mhwildsdb.Entities.Armours;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Exceptions;
using mhwildsdb.Helpers.Extensions.Mapping;
using mhwildsdb.Persistance;
using mhwildsdb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mhwildsdb.Services;

public class ArmourService(
    MhwildsDbContext _context,
    ILogger<ArmourService> _logger) : IArmourService
{
    public async Task<ArmourDto> CreateArmourAsync(CreateArmourDto request)
    {
        var exists = await _context.Armours.AnyAsync(a => a.Name == request.Name);
        if (exists)
            throw new ConflictException($"Armour '{request.Name}' already exists.");

        List<SkillRank> skillRanks = [];

        // handle skill assignment
        if (request.SkillRankIds.Count > 0)
            skillRanks = await GetSkillRanksAsync(request.SkillRankIds);

        var armour = Armour.Create(
            request.Name, 
            request.Piece, 
            request.Rank, 
            request.Rarity, 
            request.Defense, 
            request.Resistances.ToDomain(),
            request.Slots,
            skillRanks);

        await _context.Armours.AddAsync(armour);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new armour with {ArmourId}", armour.Id);

        return await GetArmourQuery()
            .Where(a => a.Id == armour.Id)
            .Select(a => a.ToDto())
            .FirstAsync();
    }

    public async Task<ICollection<ArmourDto>> CreateArmourRangeAsync(ICollection<CreateArmourDto> requests)
    {
        // check for duplicates against existing db entries
        var names = requests.Select(r => r.Name).ToList();
        var existingNames = await _context.Armours
            .Where(a => names.Contains(a.Name))
            .Select(a => a.Name)
            .ToListAsync();

        if (existingNames.Count != 0)
            throw new ConflictException($"Armours already exist: {string.Join(", ", existingNames)}");

        // grab existing skill rank ids for assignment
        var allSkillRankIds = requests
            .SelectMany(r => r.SkillRankIds)
            .Distinct()
            .ToList();

        var allSkillRanks = await GetSkillRanksAsync(allSkillRankIds);

        // lookup table grouped by id
        var skillRankLookup = allSkillRanks.ToDictionary(sr => sr.Id);

        var armours = requests
            .Select(a =>
            {
                var skillRanks = a.SkillRankIds
                    .Where(id => skillRankLookup.ContainsKey(id))
                    .Select(id => skillRankLookup[id])
                    .ToList();

                return Armour.Create(
                    a.Name,
                    a.Piece,
                    a.Rank,
                    a.Rarity,
                    a.Defense,
                    a.Resistances.ToDomain(),
                    a.Slots,
                    skillRanks);
            })
            .ToList();

        await _context.Armours.AddRangeAsync(armours);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created {Count} new armours", armours.Count);

        var armourIds = armours.Select(a => a.Id).ToList();

        return await GetArmourQuery()
            .Where(a => armourIds.Contains(a.Id))
            .Select(a => a.ToDto())
            .ToListAsync();
    }

    public async Task<IEnumerable<ArmourDto>> GetAllArmoursAsync()
    {
        return await GetArmourQuery()
            .Select(a => a.ToDto())
            .ToListAsync();
    }

    public async Task<ArmourDto> GetArmourByIdAsync(Guid id)
    {
        var armour = await GetArmourQuery()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (armour is null)
            throw new NotFoundException("Armour", id);

        return armour.ToDto();
    }

    public async Task UpdateArmourAsync(Guid id, UpdateArmourDto request)
    {
        var armour = await _context.Armours.FindAsync(id);

        if (armour is null)
            throw new NotFoundException("Armour", id);

        armour.Update(
            request.Name, 
            request.Piece, 
            request.Rank, 
            request.Rarity, 
            request.Defense, 
            request.Resistances.ToDomain());

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated armour with {ArmourId}", id);
    }

    public async Task DeleteArmourAsync(Guid id)
    {
        var armour = await _context.Armours.FindAsync(id);

        if (armour is null)
            throw new NotFoundException("Armour", id);

        _context.Armours.Remove(armour);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted armour with {ArmourId}", id);
    }

    // TODO: need to move into skills repository (once repository layer is implemented)
    private async Task<List<SkillRank>> GetSkillRanksAsync(ICollection<Guid> skillRankIds)
    {
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

    private IQueryable<Armour> GetArmourQuery()
    {
        return _context.Armours
            .AsNoTracking()
            .Include(a => a.SkillRanks)
                .ThenInclude(sr => sr.Skill)
            .Include(a => a.ArmourSet);
    }
}
