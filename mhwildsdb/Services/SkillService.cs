using mhwildsdb.DTOs;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Exceptions;
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

        var skill = Skill.Create(command.Name, command.Type, command.Description);

        await _context.Skills.AddAsync(skill);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new skill with {ID}", skill.Id);

        return new SkillDto(
            skill.Id, 
            skill.Name, 
            skill.Type, 
            skill.Description
        );
    }

    public async Task<IEnumerable<SkillDto>> GetAllSkillsAsync()
    {
        return await _context.Skills
            .AsNoTracking()
            .Select(s => new SkillDto(
                s.Id,
                s.Name,
                s.Type,
                s.Description
            ))
            .ToListAsync();
    }

    public async Task<SkillDto> GetSkillByIdAsync(Guid id)
    {
        var skill = await _context.Skills
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id);

        if (skill is null)
            throw new NotFoundException("Skill", id);

        return new SkillDto(
            skill.Id,
            skill.Name,
            skill.Type,
            skill.Description
        );
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
