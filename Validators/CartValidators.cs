using FluentValidation;
using SVT.Platform.Controllers;

namespace SVT.Platform.Validators
{
    public class MoveCartRequestValidator : AbstractValidator<CartController.MoveCartRequest>
    {
        public MoveCartRequestValidator()
        {
            RuleFor(x => x.CartId)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.LocationName)
                .NotNull()
                .NotEmpty();
        }
    }

    public class CleanInfoRequestValidator : AbstractValidator<CartController.CleanInfoRequest>
    {
        public CleanInfoRequestValidator()
        {
            RuleFor(x => x.CartId)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.MalLocationName)
                .NotNull()
                .NotEmpty();
        }
    }
}