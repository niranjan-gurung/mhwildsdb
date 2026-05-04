using FluentValidation;
using mhwildsdb.DTOs;

namespace mhwildsdb.Validators;

public class CreateSkillDtoValidator : AbstractValidator<CreateSkillDto>
{
    public CreateSkillDtoValidator()
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(20).WithMessage("{PropertyName} cannot exceed 20 characters.")
            .Must(IsValidName).WithMessage("{PropertyName} must contain only letters.");

        RuleFor(x => x.Type)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(6).WithMessage("{PropertyName} cannot exceed 6 characters.")
            .Must(IsValidName).WithMessage("{PropertyName} must contain only letters.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("{PropertyName} is required.")
            .MaximumLength(200).WithMessage("{PropertyName} cannot exceed 200 characters.");
    }

    private static bool IsValidName(string? name)
    {
        return !string.IsNullOrWhiteSpace(name) && name.All(char.IsLetter);
    }
}
