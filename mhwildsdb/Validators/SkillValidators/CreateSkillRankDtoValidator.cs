using FluentValidation;
using mhwildsdb.DTOs.Skills.SkillRank;

namespace mhwildsdb.Validators.SkillValidators;

public sealed class CreateSkillRankDtoValidator : AbstractValidator<CreateSkillRankDto>
{
    public CreateSkillRankDtoValidator()
    {
        RuleFor(x => x.Level)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
            .LessThanOrEqualTo(5).WithMessage("{PropertyName} must be less than or equal to 5.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(200).WithMessage("{PropertyName} cannot exceed 200 characters.");
    }
}
