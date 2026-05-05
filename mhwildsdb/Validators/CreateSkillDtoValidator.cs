using FluentValidation;
using mhwildsdb.DTOs.Skills.Skill;

namespace mhwildsdb.Validators;

public sealed class CreateSkillDtoValidator : AbstractValidator<CreateSkillDto>
{
    public CreateSkillDtoValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(20).WithMessage("{PropertyName} cannot exceed 20 characters.")
            .Must(BeValidName).WithMessage("{PropertyName} must contain only letters.");

        RuleFor(x => x.Type)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(6).WithMessage("{PropertyName} cannot exceed 6 characters.")
            .Must(BeValidName).WithMessage("{PropertyName} must contain only letters.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(200).WithMessage("{PropertyName} cannot exceed 200 characters.");

        RuleFor(x => x.Ranks)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} is required.")
            .NotEmpty().WithMessage("{PropertyName} must have at least one rank.")
            .Must(BeUnique).WithMessage("{PropertyName} must have unique levels.")
            .Must(BeSequential).WithMessage("{PropertyName} must be sequential starting from 1.");

        RuleForEach(x => x.Ranks)
            .SetValidator(new CreateSkillRankDtoValidator());
    }

    private static bool BeValidName(string? name)
    {
        return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter);
    }

    private static bool BeUnique(ICollection<CreateSkillRankDto> ranks)
    {
        return ranks.Select(r => r.Level).Distinct().Count() == ranks.Count;
    }

    private static bool BeSequential(ICollection<CreateSkillRankDto> ranks)
    {
        return ranks.Select(r => r.Level)
            .OrderBy(level => level)
            .SequenceEqual(Enumerable.Range(1, ranks.Count));
    }
}
