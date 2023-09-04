using FluentValidation;
using NextAPI.Requests.Auth;

namespace NextAPI.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Name).MinimumLength(4);
        RuleFor(x => x.Name).MaximumLength(20);
        
        RuleFor(x => x.Username).NotNull();
        RuleFor(x => x.Username).MinimumLength(4);
        RuleFor(x => x.Username).MaximumLength(20);
        
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();

        RuleFor(x => x.Password).NotNull();
        RuleFor(x => x.Password).MinimumLength(6);
    }
}