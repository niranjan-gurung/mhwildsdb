using FluentValidation;
using mhwildsdb.DTOs.Weapons;
using mhwildsdb.Entities.Weapons;

namespace mhwildsdb.Validators.WeaponValidators;

public sealed class DamageDtoValidator : AbstractValidator<DamageDto>
{
    public DamageDtoValidator()
    {
        RuleFor(x => x.Raw)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} cannot be negative.");

        RuleFor(x => x.Display)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} cannot be negative.");
    }
}

public sealed class WeaponSpecialDtoValidator : AbstractValidator<WeaponSpecialDto>
{
    public WeaponSpecialDtoValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("{PropertyName} is not valid.");

        RuleFor(x => x.Damage)
            .NotNull().WithMessage("{PropertyName} is required.")
            .SetValidator(new DamageDtoValidator());

        RuleFor(x => x.Element)
            .NotNull().WithMessage("{PropertyName} is required for element specials.")
            .IsInEnum().WithMessage("{PropertyName} is not valid.")
            .When(x => x.Type == WeaponSpecialType.Element);

        RuleFor(x => x.Status)
            .Null().WithMessage("{PropertyName} is not valid for element specials.")
            .When(x => x.Type == WeaponSpecialType.Element);

        RuleFor(x => x.Status)
            .NotNull().WithMessage("{PropertyName} is required for status specials.")
            .IsInEnum().WithMessage("{PropertyName} is not valid.")
            .When(x => x.Type == WeaponSpecialType.Status);

        RuleFor(x => x.Element)
            .Null().WithMessage("{PropertyName} is not valid for status specials.")
            .When(x => x.Type == WeaponSpecialType.Status);
    }
}

public sealed class SharpnessDtoValidator : AbstractValidator<SharpnessDto>
{
    public SharpnessDtoValidator()
    {
        RuleFor(x => x.Red).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Orange).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Yellow).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Green).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Blue).GreaterThanOrEqualTo(0);
        RuleFor(x => x.White).GreaterThanOrEqualTo(0);
        RuleFor(x => x.Purple).GreaterThanOrEqualTo(0);
    }
}

public sealed class PhialDtoValidator : AbstractValidator<PhialDto>
{
    public PhialDtoValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("{PropertyName} is not valid.");

        RuleFor(x => x.Damage!)
            .SetValidator(new DamageDtoValidator())
            .When(x => x.Damage is not null);
    }
}

public sealed class ShellDtoValidator : AbstractValidator<ShellDto>
{
    public ShellDtoValidator()
    {
        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("{PropertyName} is not valid.");

        RuleFor(x => x.Power)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} cannot be negative.");
    }
}

public sealed class AmmoDtoValidator : AbstractValidator<AmmoDto>
{
    public AmmoDtoValidator()
    {
        RuleFor(x => x.Type)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(30).WithMessage("{PropertyName} cannot exceed 30 characters.");

        RuleFor(x => x.Level)
            .InclusiveBetween(1, 3).WithMessage("{PropertyName} must be between 1 and 3.");

        RuleFor(x => x.Capacity)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} cannot be negative.");
    }
}
