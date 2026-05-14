using FluentValidation;
using mhwildsdb.DTOs.Armours;
using mhwildsdb.Helpers;

namespace mhwildsdb.Validators.ArmourValidators;

public class CreateArmourDtoValidator : AbstractValidator<CreateArmourDto>
{
    public CreateArmourDtoValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(20).WithMessage("{PropertyName} cannot exceed 20 characters.")
            .Must(ValidationHelpers.BeValidName)
                .WithMessage("{PropertyName} must contain only letters.");

        RuleFor(x => x.Piece)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(5).WithMessage("{PropertyName} cannot exceed 5 characters.")
            .Must(ValidationHelpers.BeValidName)
                .WithMessage("{PropertyName} must contain only letters.");

        // accomodates for upcoming 'master' rank
        RuleFor(x => x.Rank)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(6).WithMessage("{PropertyName} cannot exceed 6 characters.")
            .Must(ValidationHelpers.BeValidName)
                .WithMessage("{PropertyName} must contain only letters.");

        RuleFor(x => x.Rarity)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .InclusiveBetween(1, 8).WithMessage("{PropertyName} must be between 1 and 8.");

        RuleFor(x => x.Defense)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Resistances)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.");

        RuleFor(x => x.Slots)
            .Must(slots => slots.Count <= 3)
            .WithMessage("Armour cannot have more than 3 slots.");

        RuleForEach(x => x.Slots)
            .Cascade(CascadeMode.Stop)
            .InclusiveBetween(1, 3)
            .WithMessage("Each slot must be 1, 2, or 3.");

        RuleFor(x => x.SkillRankIds)
            .NotEmpty()
                .WithMessage("Skill rank can't be empty. All armours contains atleast one skill rank.")
            .Must(ids => ValidationHelpers.BeUnique(ids, id => id))
                .WithMessage("Duplicate skill rank IDs are not allowed.");

        RuleForEach(x => x.SkillRankIds)
            .NotEmpty().WithMessage("Skill rank ID cannot be empty.");
    }
}
