using mhwildsdb.DTOs.Decorations;
using mhwildsdb.Entities;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Exceptions;
using mhwildsdb.Helpers.Extensions.Mapping;
using mhwildsdb.Persistance;
using mhwildsdb.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace mhwildsdb.Services;

public class DecorationService(
    MhwildsDbContext _context,
    ILogger<DecorationService> _logger) : IDecorationService
{
    public async Task<DecorationDto> CreateDecorationAsync(CreateDecorationDto request)
    {
        var exists = await _context.Decorations.AnyAsync(d => d.Name == request.Name);
        if (exists)
            throw new ConflictException($"Decoration '{request.Name}' already exists.");

        var skillRanks = await GetSkillRanksAsync(request.Skills);

        var decoration = Decoration.Create(
            request.Name,
            request.Description,
            request.Type,
            request.Rarity,
            request.Slot,
            skillRanks);

        _context.Decorations.Add(decoration);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new decoration with {DecorationId}", decoration.Id);

        return await GetDecorationQuery()
            .Where(d => d.Id == decoration.Id)
            .Select(d => d.ToDto())
            .FirstAsync();
    }

    public async Task<ICollection<DecorationDto>> CreateDecorationRangeAsync(ICollection<CreateDecorationDto> requests)
    {
        var names = requests.Select(r => r.Name).ToList();
        var existingNames = await _context.Decorations
            .Where(d => names.Contains(d.Name))
            .Select(d => d.Name)
            .ToListAsync();

        if (existingNames.Count != 0)
            throw new ConflictException($"Decorations already exist: {string.Join(", ", existingNames)}");

        var allSkillRankIds = requests
            .SelectMany(r => r.Skills)
            .Distinct()
            .ToList();

        var allSkillRanks = await GetSkillRanksAsync(allSkillRankIds);
        var skillRankLookup = allSkillRanks.ToDictionary(sr => sr.Id);

        var decorations = requests.Select(r =>
        {
            var skillRanks = r.Skills
                .Where(id => skillRankLookup.ContainsKey(id))
                .Select(id => skillRankLookup[id])
                .ToList();

            return Decoration.Create(
                r.Name,
                r.Description,
                r.Type,
                r.Rarity,
                r.Slot,
                skillRanks);
        }).ToList();

        _context.Decorations.AddRange(decorations);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created {Count} new decorations", decorations.Count);

        var decorationIds = decorations.Select(d => d.Id).ToList();

        return await GetDecorationQuery()
            .Where(d => decorationIds.Contains(d.Id))
            .Select(d => d.ToDto())
            .ToListAsync();
    }

    public async Task<IEnumerable<DecorationDto>> GetAllDecorationsAsync()
    {
        return await GetDecorationQuery()
            .Select(d => d.ToDto())
            .ToListAsync();
    }

    public async Task<DecorationDto> GetDecorationByIdAsync(Guid id)
    {
        var decoration = await GetDecorationQuery()
            .FirstOrDefaultAsync(d => d.Id == id);

        if (decoration is null)
            throw new NotFoundException("Decoration", id);

        return decoration.ToDto();
    }

    public async Task UpdateDecorationAsync(Guid id, UpdateDecorationDto request)
    {
        var decoration = await _context.Decorations.FindAsync(id);

        if (decoration is null)
            throw new NotFoundException("Decoration", id);

        decoration.Update(request.Name, request.Description, request.Type, request.Rarity, request.Slot);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated decoration with {DecorationId}", id);
    }

    public async Task DeleteDecorationAsync(Guid id)
    {
        var decoration = await _context.Decorations.FindAsync(id);

        if (decoration is null)
            throw new NotFoundException("Decoration", id);

        _context.Decorations.Remove(decoration);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted decoration with {DecorationId}", id);
    }

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

    private IQueryable<Decoration> GetDecorationQuery()
    {
        return _context.Decorations
            .AsNoTracking()
            .Include(d => d.Skills)
                .ThenInclude(sr => sr.Skill);
    }
}