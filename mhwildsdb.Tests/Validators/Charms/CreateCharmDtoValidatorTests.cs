using FluentAssertions;
using mhwildsdb.DTOs.Talismans;
using mhwildsdb.DTOs.Talismans.CharmRank;
using mhwildsdb.Validators.CharmValidators;

namespace mhwildsdb.Tests.Validators.Charms;

public class CreateCharmDtoValidatorTests
{
    private readonly CreateCharmDtoValidator _validator = new();
    private readonly Guid _skillRankId = Guid.NewGuid();

    private ICollection<CreateCharmRankDto> ValidRanks =>
    [
        new("Attack Charm I", "Attack +4", 1, 3, [_skillRankId]),
        new("Attack Charm II", "Attack +8", 2, 5, [_skillRankId]),
        new("Attack Charm III", "Attack +12", 3, 7, [_skillRankId])
    ];

    [Fact]
    public async Task Validate_WithValidData_ShouldPass()
    {
        var dto = new CreateCharmDto("Attack Charm", ValidRanks);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("")]                        // empty name
    [InlineData("Attack123")]               // name with numbers
    [InlineData("AAAAABBBBBCCCCCDDDDDE")]   // exceeds 20 chars
    public async Task Validate_WithInvalidName_ShouldFail(string name)
    {
        var dto = new CreateCharmDto(name, ValidRanks);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateCharmDto.Name));
    }

    [Fact]
    public async Task Validate_WithEmptyRanks_ShouldFail()
    {
        var dto = new CreateCharmDto("Attack Charm", []);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateCharmDto.Ranks));
    }

    [Fact]
    public async Task Validate_WithDuplicateRankLevels_ShouldFail()
    {
        ICollection<CreateCharmRankDto> duplicateLevelRanks =
        [
            new("Attack Charm I", "Attack +4", 1, 3, [_skillRankId]),
            new("Attack Charm II", "Attack +8", 1, 5, [_skillRankId])
        ];

        var dto = new CreateCharmDto("Attack Charm", duplicateLevelRanks);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateCharmDto.Ranks));
    }

    [Fact]
    public async Task Validate_WithNonSequentialRankLevels_ShouldFail()
    {
        ICollection<CreateCharmRankDto> nonSequentialRanks =
        [
            new("Attack Charm I", "Attack +4", 1, 3, [_skillRankId]),
            new("Attack Charm III", "Attack +12", 3, 7, [_skillRankId])  // skips level 2
        ];

        var dto = new CreateCharmDto("Attack Charm", nonSequentialRanks);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.PropertyName == nameof(CreateCharmDto.Ranks));
    }
}
