using FluentValidation;
using mhwildsdb.DTOs.Skills;

namespace mhwildsdb.Validators.SkillValidators
{
    public class CreateSkillRangeDtoValidator : AbstractValidator<ICollection<CreateSkillDto>>
    {
        public CreateSkillRangeDtoValidator() 
        {
            RuleFor(x => x)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Skill range cannot be empty.");

            RuleForEach(x => x)
                .SetValidator(new CreateSkillDtoValidator());
        }
    }
}
