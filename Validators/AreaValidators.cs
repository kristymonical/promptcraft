using FluentValidation;
using SVT.Platform.Controllers;

namespace SVT.Platform.Validators
{
    public class ByLocationNameValidator : AbstractValidator<AreaController.ByLocationName>
    {
        public ByLocationNameValidator()
        {
            RuleFor(x => x.LocationName)
                .NotNull()
                .NotEmpty();
        }
    }
}