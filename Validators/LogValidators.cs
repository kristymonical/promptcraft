using FluentValidation;
using SVT.Platform.Controllers;

namespace SVT.Platform.Validators
{
    public class LogRequestValidator : AbstractValidator<LogController.LogRequest<LogController.WebAppLogRequest>>
    {
        public LogRequestValidator()
        {
            RuleFor(x => x.Action)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.Data)
                .NotNull()
                .NotEmpty();
            RuleFor(x => x.Data.Method)
                .NotNull().When(x => x.Data != null)
                .NotEmpty().When(x => x.Data != null);
            RuleFor(x => x.Data.Route)
                .NotNull().When(x => x.Data != null)
                .NotEmpty().When(x => x.Data != null);
            RuleFor(x => x.Data.StatusCode)
                .NotNull().When(x => x.Data != null)
                .NotEmpty().When(x => x.Data != null);
            RuleFor(x => x.Data.Success)
                .Must(x => x == true || x == false)
                .WithMessage("Data.Success must be a boolean")
                .When(x => x.Data != null);
        }
    }
}