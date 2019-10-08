using FluentValidation;
using SVT.Platform.Controllers;

namespace SVT.Platform.Validators
{
    public class DeliveryRequestsValidator : AbstractValidator<DeliveryController.DeliveryRequests>
    {
        public DeliveryRequestsValidator()
        {
            RuleFor(x => x.Deliveries)
                .NotEmpty();
                
            RuleForEach(x => x.Deliveries).SetValidator(new DeliveryRequestValidator());
        }
    }

    public class DeliveryRequestValidator : AbstractValidator<DeliveryController.DeliveryRequest>
    {
        public DeliveryRequestValidator()
        {
            RuleFor(x => x.CartId)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.DeliveryType)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.DestinationArea)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.Location)
                .NotNull()
                .NotEmpty();
        }
    }
}