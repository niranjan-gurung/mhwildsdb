using FluentValidation;
using mhwildsdb.DTOs.Talismans;
using mhwildsdb.DTOs.Talismans.CharmRank;
using mhwildsdb.Helpers;

namespace mhwildsdb.Validators.CharmValidators;

public sealed class CreateCharmDtoValidator : AbstractValidator<CreateCharmDto>
{
    public CreateCharmDtoValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(20).WithMessage("{PropertyName} cannot exceed 20 characters.")
            .Must(ValidationHelpers.BeValidName)
                .WithMessage("{PropertyName} must contain only letters.");

        RuleFor(x => x.Ranks)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("{PropertyName} is required.")
            .NotEmpty().WithMessage("{PropertyName} must have at least one rank.")
            .Must(ranks => ValidationHelpers.BeUnique(ranks, r => r.Level))
                .WithMessage("{PropertyName} must have unique levels.")
            .Must(ranks => ValidationHelpers.BeSequential(ranks, r => r.Level))
                .WithMessage("{PropertyName} must be sequential starting from 1.");

        RuleForEach(x => x.Ranks)
            .SetValidator(new CreateCharmRankDtoValidator());
    }
}
