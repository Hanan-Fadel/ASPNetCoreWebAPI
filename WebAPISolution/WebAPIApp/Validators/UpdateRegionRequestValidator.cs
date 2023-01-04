using FluentValidation;

namespace WebAPIApp.Validators
{
    public class UpdateRegionRequestValidator : AbstractValidator<Models.DTO.UpdateRegionRequest>
    {
        public UpdateRegionRequestValidator()
        {
            //define rules for the all properties
            RuleFor(x => x.Code).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Area).GreaterThan(0);
            RuleFor(x => x.Population).GreaterThanOrEqualTo(0);
        }
    }
}
