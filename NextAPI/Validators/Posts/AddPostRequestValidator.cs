using FluentValidation;
using NextAPI.Requests.Posts;

namespace NextAPI.Validators.Posts;

public class AddPostRequestValidator : AbstractValidator<AddPostRequest>
{
    public AddPostRequestValidator()
    {
        RuleFor(x => x.ReceiverId).NotNull();
        RuleFor(x => x.ReceiverId).GreaterThan(0);
        
        RuleFor(x => x.Text).NotNull();
        RuleFor(x => x.Text).MinimumLength(1);
        RuleFor(x => x.Text).MaximumLength(800);
    }
}