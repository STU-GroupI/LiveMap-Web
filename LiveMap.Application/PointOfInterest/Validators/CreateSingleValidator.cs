using FluentValidation;
using LiveMap.Application.PointOfInterest.Requests;
using LiveMap.Domain.Models;

namespace LiveMap.Application.PointOfInterest.Validators;

public class CreateSingleValidator : AbstractValidator<CreateSingleRequest>
{
    public CreateSingleValidator()
    {
        RuleFor(request => request.Poi).SetValidator(new PointOfInterestValidator());

        RuleFor(request => request.Poi.StatusName)
            .Must(statusName => statusName == PointOfInterestStatus.ACTIVE)
            .WithMessage("Status must be active when POI is created.");
    }
}
