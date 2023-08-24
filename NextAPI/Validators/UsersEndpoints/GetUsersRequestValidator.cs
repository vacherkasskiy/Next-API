using FluentValidation;
using NextAPI.Requests.Users;

namespace NextAPI.Validators.UsersEndpoints;

public class GetUsersRequestValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersRequestValidator()
    {
        RuleFor(x => x.Limit).NotNull();
        RuleFor(x => x.Limit).GreaterThan(0);
        RuleFor(x => x.Skip).NotNull();
        RuleFor(x => x.Skip).GreaterThan(-1);
    }
}