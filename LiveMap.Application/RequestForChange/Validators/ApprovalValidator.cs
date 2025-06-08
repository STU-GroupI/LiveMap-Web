using FluentValidation;
using LiveMap.Application.PointOfInterest.Validators;
using LiveMap.Application.RequestForChange.Requests;
using LiveMap.Domain.Models;

namespace LiveMap.Application.RequestForChange.Validators;

public class ApprovalValidator : AbstractValidator<ApprovalRequest>
{
    public ApprovalValidator()
    {
        RuleFor(request => request.Poi).SetValidator(new PointOfInterestValidator());
    }
}
