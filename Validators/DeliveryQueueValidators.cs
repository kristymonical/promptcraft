using FluentValidation;
using SVT.Platform.Controllers;

namespace SVT.Platform.Validators
{
    public class PriorityRouteValidator : AbstractValidator<DeliveryQueueController.PriorityRoute>
    {
        public PriorityRouteValidator()
        {
            RuleFor(x => x.DeliveryId)
                .NotNull()
                .NotEmpty();
        }
    }

    public class PriorityQueryValidator : AbstractValidator<DeliveryQueueController.PriorityQuery>
    {
        public PriorityQueryValidator()
        {
            RuleFor(x => x.NewChildDeliveryId)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.NewParentDeliveryId)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.PoolId)
                .NotNull()
                .NotEmpty();
        }
    }
    
    public class ByPoolRequestValidator : AbstractValidator<DeliveryQueueController.ByPoolRequest>
    {
        public ByPoolRequestValidator()
        {
            RuleFor(x => x.PoolId)
                .NotNull()
                .NotEmpty();
        }
    }
}