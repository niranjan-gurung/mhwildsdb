using FluentAssertions;
using mhwildsdb.DTOs.Armours.ArmourSet;
using mhwildsdb.Validators.ArmourValidators;

namespace mhwildsdb.Tests.Validators;

public class CreateArmourSetDtoValidatorTests
{
    private readonly CreateArmourSetDtoValidator _validator = new();
    private static ICollection<Guid> ArmourIds =>
        [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

    public static IList<object[]> InvalidNames =>
        [
            [""],
            ["Conga Set 1234"],
            [new string('A', 21)]
        ];

    [Fact]
    public async Task Validate_WithValidData_ShouldPass()
    {
        var dto = new CreateArmourSetDto(
            "Conga Set α",
            ArmourIds,
            Guid.NewGuid(),
            Guid.NewGuid());

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(InvalidNames))]
    public async Task Validate_WithInvalidNameField_ShouldFail(string name)
    {
        var dto = new CreateArmourSetDto(
            name,
            ArmourIds,
            Guid.NewGuid(),
            Guid.NewGuid());

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_WithDuplicateArmourPieceIds_ShouldFail()
    {
        var duplicateId = Guid.NewGuid();
        var dto = new CreateArmourSetDto(
            "Conga Set α",
            [duplicateId, duplicateId],
            Guid.NewGuid(),
            Guid.NewGuid());

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle(e => e.PropertyName == nameof(CreateArmourSetDto.ArmourPieceIds));
    }

    [Fact]
    public async Task Validate_WithEmptyArmourPieceIdsCollection_ShouldFail()
    {
        var dto = new CreateArmourSetDto(
            "Conga Set α",
            [],
            Guid.NewGuid(),
            Guid.NewGuid());
     
        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_WithEmptyGuidArmourPieceIds_ShouldFail()
    {
        var dto = new CreateArmourSetDto(
            "Conga Set α",
            [Guid.Empty],
            Guid.NewGuid(),
            Guid.NewGuid());

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task Validate_WithNullOptionalSkillIds_ShouldPass()
    {
        var dto = new CreateArmourSetDto(
            "Conga Set α",
            ArmourIds,
            null,
            null);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public async Task Validate_WithEmptySetBonusSkillId_ShouldFail()
    {
        var dto = new CreateArmourSetDto(
            "Conga Set α",
            ArmourIds,
            Guid.Empty,
            Guid.NewGuid());

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle(e => e.PropertyName == nameof(CreateArmourSetDto.SetBonusSkillId));
    }

    [Fact]
    public async Task Validate_WithEmptyGroupBonusSkillId_ShouldFail()
    {
        var dto = new CreateArmourSetDto(
            "Conga Set α",
            ArmourIds,
            Guid.NewGuid(),
            Guid.Empty);

        var result = await _validator.ValidateAsync(dto, TestContext.Current.CancellationToken);

        result.IsValid.Should().BeFalse();
        result.Errors.Should()
            .ContainSingle(e => e.PropertyName == nameof(CreateArmourSetDto.GroupBonusSkillId));
    }
}
