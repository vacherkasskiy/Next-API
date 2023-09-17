using FluentValidation;
using NextAPI.Requests.Users;

namespace NextAPI.Validators.Users;

public class EditProfileRequestValidator : AbstractValidator<EditProfileRequest>
{
    public EditProfileRequestValidator()
    {
        RuleFor(x => x.Name).NotNull();
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Name).MinimumLength(4);
        RuleFor(x => x.Name).MaximumLength(20);
        
        RuleFor(x => x.Username).NotNull();
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Username).MinimumLength(4);
        RuleFor(x => x.Username).MaximumLength(20);
        
        RuleFor(x => x.Email).NotNull();
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Email).NotEmpty();

        RuleFor(x => x.City).MaximumLength(20);
    }
}