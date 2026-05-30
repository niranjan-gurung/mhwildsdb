using FluentAssertions;
using mhwildsdb.DTOs.Weapons;
using mhwildsdb.Entities.Skills;
using mhwildsdb.Entities.Weapons;
using mhwildsdb.Exceptions;
using mhwildsdb.Persistance;
using mhwildsdb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace mhwildsdb.Tests.Services;

public class WeaponServiceTests
{
    private readonly MhwildsDbContext _context;
    private readonly WeaponService _service;
    private Guid _skillRankId;

    private static DamageDto Damage => new(100, 480);
    private static SharpnessDto Sharpness => new(20, 30, 40, 50, 30, 10, 0);

    private CreateWeaponDto ValidGreatsword =>
        new(
            "Hope Blade",
            "A reliable greatsword.",
            WeaponType.Greatsword,
            0,
            3,
            [1],
            5,
            Damage,
            [new(null, WeaponSpecialType.Element, ElementType.Water, null, new DamageDto(20, 200), false)],
            [_skillRankId],
            Sharpness,
            null,
            null,
            null,
            null,
            null,
            null);

    private static CreateWeaponDto ValidLightBowgun =>
        new(
            "Hope Rifle",
            null,
            WeaponType.LightBowgun,
            0,
            3,
            [],
            0,
            Damage,
            [],
            [],
            null,
            null,
            null,
            null,
            [new("Normal", 1, 5, true)],
            "Wyvernblast",
            null);

    public WeaponServiceTests()
    {
        var options = new DbContextOptionsBuilder<MhwildsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new MhwildsDbContext(options);
        _service = new WeaponService(_context, NSubstitute.Substitute.For<ILogger<WeaponService>>());

        SeedDatabase();
    }

    private void SeedDatabase()
    {
        var skill = Skill.Create("Attack Boost", "weapon",
            [SkillRank.Create(1, "Attack +3")], "Increases attack power.");

        _context.Skills.Add(skill);
        _context.SaveChanges();

        _skillRankId = _context.SkillRanks.First().Id;
    }

    [Fact]
    public async Task CreateWeaponAsync_WithMeleeWeapon_ShouldReturnWeaponDto()
    {
        var result = await _service.CreateWeaponAsync(ValidGreatsword);

        result.Should().NotBeNull();
        result.Name.Should().Be(ValidGreatsword.Name);
        result.WeaponType.Should().Be(WeaponType.Greatsword);
        result.Sharpness.Should().NotBeNull();
        result.Specials.Should().ContainSingle(s => s.Element == ElementType.Water);
        result.Skills.Should().ContainSingle(s => s.Id == _skillRankId);
    }

    [Fact]
    public async Task CreateWeaponAsync_WithRangedWeapon_ShouldReturnWeaponDto()
    {
        var result = await _service.CreateWeaponAsync(ValidLightBowgun);

        result.WeaponType.Should().Be(WeaponType.LightBowgun);
        result.Ammo.Should().ContainSingle(a => a.Rapid == true);
        result.SpecialAmmo.Should().Be("Wyvernblast");
        result.Sharpness.Should().BeNull();
    }

    [Fact]
    public async Task CreateWeaponAsync_WithDuplicateName_ShouldThrowConflictException()
    {
        await _service.CreateWeaponAsync(ValidGreatsword);

        var act = async () => await _service.CreateWeaponAsync(ValidGreatsword);

        await act.Should().ThrowAsync<ConflictException>();
    }

    [Fact]
    public async Task CreateWeaponAsync_WithNonExistentSkillRankId_ShouldThrowNotFoundException()
    {
        var invalid = ValidGreatsword with { Skills = [Guid.NewGuid()] };

        var act = async () => await _service.CreateWeaponAsync(invalid);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task CreateWeaponRangeAsync_WithValidData_ShouldReturnWeaponDtoList()
    {
        var result = await _service.CreateWeaponRangeAsync([ValidGreatsword, ValidLightBowgun]);

        result.Should().HaveCount(2);
        result.Select(w => w.Name).Should().BeEquivalentTo(["Hope Blade", "Hope Rifle"]);
    }

    [Fact]
    public async Task GetWeaponByIdAsync_WithNonExistentId_ShouldThrowNotFoundException()
    {
        var act = async () => await _service.GetWeaponByIdAsync(Guid.NewGuid());

        await act.Should().ThrowAsync<NotFoundException>();
    }
}
