using mhwildsdb.DTOs.Charms;
using mhwildsdb.DTOs.Charms.CharmRank;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Exceptions;
using mhwildsdb.Persistance;
using Microsoft.EntityFrameworkCore;
using mhwildsdb.Helpers.Extensions.Mapping;
using mhwildsdb.Entities.Charms;

namespace mhwildsdb.Services;

public class CharmService(
    MhwildsDbContext _context,
    ILogger<CharmService> _logger) : ICharmService
{
    public async Task<CharmDto> CreateCharmAsync(CreateCharmDto request)
    {
        var exists = await _context.Charms.AnyAsync(c => c.Name == request.Name);
        if (exists)
            throw new ConflictException($"Charm '{request.Name}' already exists.");

        var ranks = await BuildCharmRanksAsync(request.Ranks);
        var charm = Charm.Create(request.Name, ranks);

        _context.Charms.Add(charm);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new charm with {CharmId}", charm.Id);

        return await GetCharmQuery()
            .Where(c => c.Id == charm.Id)
            .Select(c => c.ToDto())
            .FirstAsync();
    }

    public async Task<ICollection<CharmDto>> CreateCharmRangeAsync(ICollection<CreateCharmDto> requests)
    {
        var names = requests.Select(r => r.Name).ToList();
        var existingNames = await _context.Charms
            .Where(c => names.Contains(c.Name))
            .Select(c => c.Name)
            .ToListAsync();

        if (existingNames.Count != 0)
            throw new ConflictException($"Charms already exist: {string.Join(", ", existingNames)}");

        var charms = new List<Charm>();
        foreach (var request in requests)
        {
            var ranks = await BuildCharmRanksAsync(request.Ranks);
            charms.Add(Charm.Create(request.Name, ranks));
        }

        _context.Charms.AddRange(charms);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created {Count} new charms", charms.Count);

        var charmIds = charms.Select(c => c.Id).ToList();

        return await GetCharmQuery()
            .Where(c => charmIds.Contains(c.Id))
            .Select(c => c.ToDto())
            .ToListAsync();
    }

    public async Task<IEnumerable<CharmDto>> GetAllCharmsAsync()
    {
        return await GetCharmQuery()
            .Select(c => c.ToDto())
            .ToListAsync();
    }

    public async Task<CharmDto> GetCharmByIdAsync(Guid id)
    {
        var charm = await GetCharmQuery()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (charm is null)
            throw new NotFoundException("Charm", id);

        return charm.ToDto();
    }

    public async Task UpdateCharmAsync(Guid id, UpdateCharmDto request)
    {
        var charm = await _context.Charms.FindAsync(id);

        if (charm is null)
            throw new NotFoundException("Charm", id);

        charm.Update(request.Name);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Updated charm with {CharmId}", id);
    }

    public async Task DeleteCharmAsync(Guid id)
    {
        var charm = await _context.Charms.FindAsync(id);

        if (charm is null)
            throw new NotFoundException("Charm", id);

        _context.Charms.Remove(charm);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted charm with {CharmId}", id);
    }

    private async Task<List<CharmRank>> BuildCharmRanksAsync(ICollection<CreateCharmRankDto> rankDtos)
    {
        // collect all distinct skill rank ids across all ranks in one query
        var allSkillRankIds = rankDtos
            .SelectMany(r => r.Skills)
            .Distinct()
            .ToList();

        var skillRankLookup = new Dictionary<Guid, SkillRank>();

        if (allSkillRankIds.Count > 0)
        {
            var skillRanks = await _context.SkillRanks
                .Include(sr => sr.Skill)
                .Where(sr => allSkillRankIds.Contains(sr.Id))
                .ToListAsync();

            var missingIds = allSkillRankIds.Except(skillRanks.Select(sr => sr.Id)).ToList();
            if (missingIds.Count != 0)
                throw new NotFoundException("SkillRanks", string.Join(", ", missingIds));

            skillRankLookup = skillRanks.ToDictionary(sr => sr.Id);
        }

        return rankDtos.Select(r =>
        {
            var skills = r.Skills
                .Where(id => skillRankLookup.ContainsKey(id))
                .Select(id => skillRankLookup[id])
                .ToList();

            return CharmRank.Create(r.Name, r.Description, r.Level, r.Rarity, skills);
        }).ToList();
    }

    private IQueryable<Charm> GetCharmQuery()
    {
        return _context.Charms
            .AsNoTracking()
            .Include(c => c.Ranks)
                .ThenInclude(cr => cr.Skills)
                    .ThenInclude(sr => sr.Skill);
    }
}
