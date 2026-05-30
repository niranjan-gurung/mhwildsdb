using FluentValidation;
using mhwildsdb.DTOs.Weapons;
using mhwildsdb.Entities.Weapons;
using mhwildsdb.Helpers;

namespace mhwildsdb.Validators.WeaponValidators;

public sealed class CreateWeaponDtoValidator : AbstractValidator<CreateWeaponDto>
{
    public CreateWeaponDtoValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(80).WithMessage("{PropertyName} cannot exceed 80 characters.")
            .Must(ValidationHelpers.BeValidName)
                .WithMessage("{PropertyName} must contain only letters, spaces, or apostrophes.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("{PropertyName} cannot exceed 500 characters.")
            .When(x => x.Description is not null);

        RuleFor(x => x.WeaponType)
            .IsInEnum().WithMessage("{PropertyName} is not valid.");

        RuleFor(x => x.Defense)
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} cannot be negative.");

        RuleFor(x => x.Rarity)
            .InclusiveBetween(1, 12).WithMessage("{PropertyName} must be between 1 and 12.");

        RuleForEach(x => x.Slots)
            .InclusiveBetween(1, 3).WithMessage("Slot value must be between 1 and 3.")
            .When(x => x.Slots is not null);

        RuleFor(x => x.Affinity)
            .InclusiveBetween(-100, 100).WithMessage("{PropertyName} must be between -100 and 100.");

        RuleFor(x => x.Damage)
            .NotNull().WithMessage("{PropertyName} is required.")
            .SetValidator(new DamageDtoValidator());

        RuleForEach(x => x.Specials)
            .SetValidator(new WeaponSpecialDtoValidator())
            .When(x => x.Specials is not null);

        RuleFor(x => x.Skills)
            .Must(ids => ids is null || ValidationHelpers.BeUnique(ids, id => id))
            .WithMessage("{PropertyName} must not contain duplicate skill rank IDs.");

        RuleForEach(x => x.Skills)
            .NotEqual(Guid.Empty).WithMessage("Skill rank ID cannot be an empty GUID.")
            .When(x => x.Skills is not null);

        RuleFor(x => x.Sharpness)
            .NotNull().WithMessage("{PropertyName} is required for melee weapons.")
            .When(x => IsMelee(x.WeaponType));

        RuleFor(x => x.Sharpness)
            .Null().WithMessage("{PropertyName} is only valid for melee weapons.")
            .When(x => !IsMelee(x.WeaponType));

        RuleFor(x => x.Sharpness!)
            .SetValidator(new SharpnessDtoValidator())
            .When(x => x.Sharpness is not null);

        RuleFor(x => x.Phial)
            .NotNull().WithMessage("{PropertyName} is required for switch axe and charge blade.")
            .When(x => IsPhialWeapon(x.WeaponType));

        RuleFor(x => x.Phial)
            .Null().WithMessage("{PropertyName} is only valid for switch axe and charge blade.")
            .When(x => !IsPhialWeapon(x.WeaponType));

        RuleFor(x => x.Phial!)
            .SetValidator(new PhialDtoValidator())
            .When(x => x.Phial is not null);

        RuleFor(x => x.Shell)
            .NotNull().WithMessage("{PropertyName} is required for gunlance.")
            .When(x => x.WeaponType == WeaponType.Gunlance);

        RuleFor(x => x.Shell)
            .Null().WithMessage("{PropertyName} is only valid for gunlance.")
            .When(x => x.WeaponType != WeaponType.Gunlance);

        RuleFor(x => x.Shell!)
            .SetValidator(new ShellDtoValidator())
            .When(x => x.Shell is not null);

        RuleFor(x => x.KinsectLevel)
            .NotNull().WithMessage("{PropertyName} is required for insect glaive.")
            .GreaterThanOrEqualTo(0).WithMessage("{PropertyName} cannot be negative.")
            .When(x => x.WeaponType == WeaponType.InsectGlaive);

        RuleFor(x => x.KinsectLevel)
            .Null().WithMessage("{PropertyName} is only valid for insect glaive.")
            .When(x => x.WeaponType != WeaponType.InsectGlaive);

        RuleFor(x => x.Ammo)
            .NotEmpty().WithMessage("{PropertyName} is required for bowguns.")
            .When(x => IsBowgun(x.WeaponType));

        RuleFor(x => x.Ammo)
            .Must(ammo => ammo is null || ammo.Count == 0)
            .WithMessage("{PropertyName} is only valid for bowguns.")
            .When(x => !IsBowgun(x.WeaponType));

        RuleForEach(x => x.Ammo)
            .SetValidator(new AmmoDtoValidator())
            .When(x => x.Ammo is not null);

        RuleFor(x => x.SpecialAmmo)
            .NotEmpty().WithMessage("{PropertyName} is required for light bowgun.")
            .MaximumLength(50).WithMessage("{PropertyName} cannot exceed 50 characters.")
            .When(x => x.WeaponType == WeaponType.LightBowgun);

        RuleFor(x => x.SpecialAmmo)
            .Null().WithMessage("{PropertyName} is only valid for light bowgun.")
            .When(x => x.WeaponType != WeaponType.LightBowgun);

        RuleFor(x => x.Coatings)
            .Must(coatings => coatings is null || coatings.Count == 0)
            .WithMessage("{PropertyName} is only valid for bow.")
            .When(x => x.WeaponType != WeaponType.Bow);

        RuleForEach(x => x.Coatings)
            .IsInEnum().WithMessage("Coating type is not valid.")
            .When(x => x.Coatings is not null);
    }

    private static bool IsMelee(WeaponType weaponType) => weaponType is
        WeaponType.Greatsword or
        WeaponType.Longsword or
        WeaponType.SwordAndShield or
        WeaponType.DualBlades or
        WeaponType.Hammer or
        WeaponType.HuntingHorn or
        WeaponType.SwitchAxe or
        WeaponType.ChargeBlade or
        WeaponType.Lance or
        WeaponType.Gunlance or
        WeaponType.InsectGlaive;

    private static bool IsPhialWeapon(WeaponType weaponType) =>
        weaponType is WeaponType.SwitchAxe or WeaponType.ChargeBlade;

    private static bool IsBowgun(WeaponType weaponType) =>
        weaponType is WeaponType.LightBowgun or WeaponType.HeavyBowgun;
}

public sealed class CreateWeaponRangeDtoValidator : AbstractValidator<ICollection<CreateWeaponDto>>
{
    public CreateWeaponRangeDtoValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Weapons must contain at least one weapon.")
            .Must(weapons => ValidationHelpers.BeUnique(weapons, weapon => weapon.Name))
                .WithMessage("Weapons must not contain duplicate names.");

        RuleForEach(x => x).SetValidator(new CreateWeaponDtoValidator());
    }
}
