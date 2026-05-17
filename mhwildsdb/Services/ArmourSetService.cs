using mhwildsdb.DTOs.Armours;
using mhwildsdb.DTOs.Armours.ArmourSet;
using mhwildsdb.Entities.Armours;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Exceptions;
using mhwildsdb.Helpers.Extensions.Mapping;
using mhwildsdb.Persistance;
using Microsoft.EntityFrameworkCore;

namespace mhwildsdb.Services;

public class ArmourSetService(
    MhwildsDbContext _context,
    ILogger<ArmourSetService> _logger) : IArmourSetService
{
    public async Task<ArmourSetDto> CreateArmourSetAsync(CreateArmourSetDto request)
    {
        var exists = await _context.ArmourSets.AnyAsync(a => a.Name == request.Name);
        if (exists)
            throw new ConflictException($"Armour set '{request.Name}' already exists.");

        // null values accepted by default
        // if skill id not found, throw not found exception
        Skill? setBonusSkill = null;
        if (request.SetBonusSkillId.HasValue)
        {
            setBonusSkill = await _context.Skills.FindAsync(request.SetBonusSkillId.Value)
                ?? throw new NotFoundException("Skill", request.SetBonusSkillId.Value);
        }

        Skill? groupSkill = null;
        if (request.GroupBonusSkillId.HasValue)
        {
            groupSkill = await _context.Skills.FindAsync(request.GroupBonusSkillId.Value)
                ?? throw new NotFoundException("Skill", request.GroupBonusSkillId.Value);
        }

        ICollection<Armour> armourPieces = [];

        // link appropriate armour pieces to current set
        if (request.ArmourPieceIds.Count > 0)
            armourPieces = await GetArmourPiecesAsync(request.ArmourPieceIds);

        var armourSet = ArmourSet.Create(
            request.Name,
            armourPieces,
            setBonusSkill,
            groupSkill);

        _context.ArmourSets.Add(armourSet);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Created new armour set with {ArmourSetId}", armourSet.Id);

        // reload all navigation properties to return a complete dto
        return await GetArmourSetQuery()
            .Where(a => a.Id == armourSet.Id)
            .Select(a => a.ToDto())
            .FirstAsync();
    }

    public async Task<ICollection<ArmourDto>> CreateArmourRangeAsync(ICollection<CreateArmourSetDto> requests)
    {
        // TODO
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<ArmourSetDto>> GetAllArmourSetsAsync()
    {
        return await GetArmourSetQuery()
            .Select(a => a.ToDto())
            .ToListAsync();
    }

    public async Task<ArmourSetDto> GetArmourSetByIdAsync(Guid id)
    {
        var armourSet = await GetArmourSetQuery()
            .FirstOrDefaultAsync(a => a.Id == id);

        if (armourSet is null)
            throw new NotFoundException("ArmourSet", id);

        return armourSet.ToDto();
    }

    public async Task UpdateArmourSetAsync(Guid id, UpdateArmourSetDto request)
    {
        var armourSet = await _context.ArmourSets.FindAsync(id);

        if (armourSet is null)
            throw new NotFoundException("ArmourSet", id);

        armourSet.Update(request.Name);

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated armour set with {ArmourSetId}", id);
    }

    public async Task DeleteArmourSetAsync(Guid id)
    {
        var armourSet = await _context.ArmourSets.FindAsync(id);

        if (armourSet is null)
            throw new NotFoundException("ArmourSet", id);

        _context.ArmourSets.Remove(armourSet);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted armour set with {ArmourSetId}", id);
    }

    private async Task<ICollection<Armour>> GetArmourPiecesAsync(ICollection<Guid> armourPieceIds)
    {
        var armours = await _context.Armours
            .Where(a => armourPieceIds.Contains(a.Id))
            .Include(a => a.SkillRanks)
                .ThenInclude(asr => asr.Skill)
            .Include(a => a.ArmourSet)
            .ToListAsync();

        var missingIds = armourPieceIds
            .Except(armours.Select(a => a.Id))
            .ToList();

        if (missingIds.Count != 0)
            throw new NotFoundException("Armour pieces", string.Join(", ", missingIds));

        return armours;
    }

    private IQueryable<ArmourSet> GetArmourSetQuery()
    {
        return _context.ArmourSets
            .AsNoTracking()
            .Include(a => a.Pieces)
                .ThenInclude(p => p.SkillRanks)
                    .ThenInclude(sr => sr.Skill)
            .Include(a => a.SetBonusSkill)
                .ThenInclude(s => s!.Ranks)
            .Include(a => a.GroupBonusSkill)
                .ThenInclude(s => s!.Ranks);
    }
}
