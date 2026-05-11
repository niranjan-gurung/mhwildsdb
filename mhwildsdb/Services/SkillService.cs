using mhwildsdb.DTOs.Skills.Skill;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Exceptions;
using mhwildsdb.Extensions.Mapping;
using mhwildsdb.Persistance;
using Microsoft.EntityFrameworkCore;

namespace mhwildsdb.Services;

public class SkillService(
    MhwildsDbContext _context,
    ILogger<SkillService> _logger) : ISkillService
{
    public async Task<SkillDto> CreateSkillAsync(CreateSkillDto request)
    {
        var exists = await _context.Skills.AnyAsync(s => s.Name == request.Name);
        if (exists)
            throw new ConflictException($"Skill '{request.Name}' already exists.");

        // convert request.Ranks from dto to entity
        var ranks = request.Ranks
            .Select(r => SkillRank.Create(r.Level, r.Description))
            .ToList();

        var skill = Skill.Create(request.Name, request.Type, request.Description, ranks);

        await _context.Skills.AddAsync(skill);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new skill with {SkillId}", skill.Id);

        return skill.ToDto();
    }

    public async Task<ICollection<SkillDto>> CreateSkillRangeAsync(ICollection<CreateSkillDto> requests)
    {
        // check for duplicates against existing db entries
        var names = requests.Select(r => r.Name).ToList();
        var existingNames = await _context.Skills
            .Where(s => names.Contains(s.Name))
            .Select(s => s.Name)
            .ToListAsync();

        if (existingNames.Count != 0)
            throw new ConflictException($"Skills already exist: {string.Join(", ", existingNames)}");

        var skills = requests
            .Select(s => Skill.Create(
                s.Name,
                s.Type,
                s.Description,
                s.Ranks.Select(r => SkillRank.Create(r.Level, r.Description)).ToList()
            ))
            .ToList();

        await _context.Skills.AddRangeAsync(skills);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created {Count} new skills", skills.Count);

        return skills.Select(s => s.ToDto()).ToList();
    }

    public async Task<IEnumerable<SkillDto>> GetAllSkillsAsync()
    {
        return await _context.Skills
            .AsNoTracking()
            .Include(s => s.Ranks)
            .Select(s => s.ToDto()).ToListAsync();
    }

    public async Task<SkillDto> GetSkillByIdAsync(Guid id)
    {
        var skill = await _context.Skills
            .AsNoTracking()
            .Include(s => s.Ranks)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (skill is null)
            throw new NotFoundException("Skill", id);

        return skill.ToDto();
    }

    public async Task UpdateSkillAsync(Guid id, UpdateSkillDto request)
    {
        var skill = await _context.Skills.FindAsync(id);

        if (skill is null)
            throw new NotFoundException("Skill", id);

        skill.Update(request.Name, request.Type, request.Description);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated skill with {SkillId}", id);
    }

    public async Task DeleteSkillAsync(Guid id)
    {
        var skill = await _context.Skills.FindAsync(id);

        if (skill is null)
            throw new NotFoundException("Skill", id);

        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted skill with {SkillId}", id);
    }
}
