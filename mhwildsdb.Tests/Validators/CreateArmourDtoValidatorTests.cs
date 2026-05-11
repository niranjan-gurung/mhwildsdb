using FluentAssertions;
using mhwildsdb.DTOs;
using mhwildsdb.DTOs.Armours;
using mhwildsdb.Validators.ArmourValidators;

namespace mhwildsdb.Tests.Validators;

public class CreateArmourDtoValidatorTests
{
    private readonly CreateArmourDtoValidator _validator = new();
    private readonly Guid _skillRankId = Guid.NewGuid();
    private readonly Guid _duplicateId = Guid.NewGuid();
    private readonly ResistancesDto _validResistances = new(-3, 1, -1, 1, 2);

    [Fact]
    public async Task Validate_WithValidData_ShouldPass()
    {
        var dto = new CreateArmourDto(
            "Conga Helm α", "head", "high", 
            5, 36,
            _validResistances,
            [1],
            [_skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        // assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithDuplicateSkillRankIds_ShouldFail()
    {
        var dto = new CreateArmourDto(
            "Conga Helm α", "head", "high",
            5, 36,
            _validResistances,
            [1, 2, 2],
            [_duplicateId, _duplicateId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .Contain(e => e.ErrorMessage == "Duplicate skill rank IDs are not allowed.");
    }

    [Fact]
    public async Task Validate_WithTooManySlots_ShouldFail()
    {
        var dto = new CreateArmourDto(
            "Conga Helm α", "head", "high",
            5, 36,
            _validResistances,
            [1, 2, 3, 4],
            [_skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .Contain(e => e.ErrorMessage == "Armour cannot have more than 3 slots.");
    }

    [Theory]
    [InlineData("", "head", "high")]                    // empty name
    [InlineData("Conga 123", "head", "high")]           // name field contains numbers
    [InlineData("Conga Helm α", "", "high")]            // empty piece 
    [InlineData("Conga Helm α", "head", "")]            // empty rank
    public async Task Validate_WithInvalidStringFields_ShouldFail(
        string name, string piece, string rank)
    {
        var dto = new CreateArmourDto(
            name, piece, rank,
            5, 36,
            _validResistances,
            [2, 3, 3],
            [_skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(0)]   // below minimum
    [InlineData(9)]   // above maximum
    public async Task Validate_WithInvalidRarity_ShouldFail(int rarity)
    {
        var dto = new CreateArmourDto(
            "Conga Helm α", "head", "high",
            rarity, 36,
            _validResistances,
            [3, 2],
            [_skillRankId]);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == "Rarity");
    }
}
