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
    public async Task<SkillDto> CreateSkillAsync(CreateSkillDto command)
    {
        var exists = await _context.Skills.AnyAsync(s => s.Name == command.Name);
        if (exists)
            throw new ConflictException($"Skill '{command.Name}' already exists.");

        // convert command.Ranks from dto to entity
        var ranks = command.Ranks
            .Select(r => SkillRank.Create(r.Level, r.Description))
            .ToList();

        var skill = Skill.Create(command.Name, command.Type, command.Description, ranks);

        await _context.Skills.AddAsync(skill);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new skill with {ID}", skill.Id);

        return skill.ToDto();
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

    public async Task UpdateSkillAsync(Guid id, UpdateSkillDto command)
    {
        var skill = await _context.Skills.FindAsync(id);

        if (skill is null)
            throw new NotFoundException("Skill", id);

        skill.Update(command.Name, command.Type, command.Description);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated skill with {id}", id);
    }

    public async Task DeleteSkillAsync(Guid id)
    {
        var skill = await _context.Skills.FindAsync(id);

        if (skill is null)
            throw new NotFoundException("Skill", id);

        _context.Skills.Remove(skill);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted skill with {id}", id);
    }
}
