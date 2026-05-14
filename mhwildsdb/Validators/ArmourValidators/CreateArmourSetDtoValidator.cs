using FluentValidation;
using mhwildsdb.DTOs.Armours.ArmourSet;
using mhwildsdb.Helpers;

namespace mhwildsdb.Validators.ArmourValidators;

public class CreateArmourSetDtoValidator : AbstractValidator<CreateArmourSetDto>
{
    public CreateArmourSetDtoValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(20).WithMessage("{PropertyName} cannot exceed 20 characters.")
            .Must(ValidationHelpers.BeValidName)
                .WithMessage("{PropertyName} must contain only letters and spaces.");

        RuleFor(x => x.ArmourPieceIds)
            .NotEmpty().WithMessage("Armour piece IDs are required.")
            .Must(ids => ValidationHelpers.BeUnique(ids, id => id))
                .WithMessage("Duplicate armour piece IDs are not allowed.");

        RuleForEach(x => x.ArmourPieceIds)
            .NotEmpty().WithMessage("Armour piece ID cannot be empty.");

        RuleFor(x => x.SetBonusSkillId)
            .NotEqual(Guid.Empty)
                .WithMessage("{PropertyName} cannot be an empty GUID.")
            .When(x => x.SetBonusSkillId.HasValue);

        RuleFor(x => x.GroupBonusSkillId)
            .NotEqual(Guid.Empty)
                .WithMessage("{PropertyName} cannot be an empty GUID.")
            .When(x => x.GroupBonusSkillId.HasValue);
    }
}
