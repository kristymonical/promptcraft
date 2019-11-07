using FluentValidation;
using SVT.Platform.Controllers;

namespace SVT.Platform.Validators
{
    public class ByLocationDeliveryTypeValidator : AbstractValidator<AreaController.ByLocationDeliveryType>
    {
        public ByLocationDeliveryTypeValidator()
        {
            RuleFor(x => x.LocationName)
                .NotNull()
                .NotEmpty();
        }
    }
}