using FluentValidation;
using mhwildsdb.DTOs.Decorations;
using mhwildsdb.Helpers;

namespace mhwildsdb.Validators.DecorationValidators;

public sealed class CreateDecorationDtoValidator : AbstractValidator<CreateDecorationDto>
{
    public CreateDecorationDtoValidator()
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

        RuleFor(x => x.Type)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(10).WithMessage("{PropertyName} cannot exceed 10 characters.")
            .Must(ValidationHelpers.BeValidName)
                .WithMessage("{PropertyName} must contain only letters.");

        RuleFor(x => x.Rarity)
            .InclusiveBetween(1, 12).WithMessage("{PropertyName} must be between 1 and 12.");

        RuleFor(x => x.Slot)
            .InclusiveBetween(1, 3).WithMessage("{PropertyName} must be between 1 and 3.");

        RuleFor(x => x.Skills)
            .NotEmpty().WithMessage("{PropertyName} must contain at least one skill rank.")
            .Must(ids => ValidationHelpers.BeUnique(ids, id => id))
                .WithMessage("{PropertyName} must not contain duplicate skill rank IDs.");

        RuleForEach(x => x.Skills)
            .NotEqual(Guid.Empty).WithMessage("Skill rank ID cannot be an empty GUID.");
    }
}