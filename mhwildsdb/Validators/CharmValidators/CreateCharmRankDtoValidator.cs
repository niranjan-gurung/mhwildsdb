using FluentValidation;
using mhwildsdb.DTOs.Talismans.CharmRank;
using mhwildsdb.Helpers;

namespace mhwildsdb.Validators.CharmValidators;

public sealed class CreateCharmRankDtoValidator : AbstractValidator<CreateCharmRankDto>
{
    public CreateCharmRankDtoValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(20).WithMessage("{PropertyName} cannot exceed 20 characters.")
            .Must(ValidationHelpers.BeValidName)
                .WithMessage("{PropertyName} must contain only letters.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(200).WithMessage("{PropertyName} cannot exceed 200 characters.");

        RuleFor(x => x.Level)
            .Cascade(CascadeMode.Stop)
            .GreaterThan(0).WithMessage("{PropertyName} must be greater than 0.")
            .LessThanOrEqualTo(3).WithMessage("{PropertyName} must be less than or equal to 3.");

        RuleFor(x => x.Rarity)
            .InclusiveBetween(1, 12).WithMessage("{PropertyName} must be between 1 and 12.");

        RuleFor(x => x.Skills)
            .NotNull().WithMessage("{PropertyName} is required.")
            .Must(ids => ValidationHelpers.BeUnique(ids, id => id))
                .WithMessage("{PropertyName} must not contain duplicate skill rank IDs.");

        RuleForEach(x => x.Skills)
            .NotEqual(Guid.Empty).WithMessage("Skill rank ID cannot be an empty GUID.");
    }
}
