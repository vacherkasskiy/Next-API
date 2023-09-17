using FluentValidation;
using NextAPI.Requests.Messages;

namespace NextAPI.Validators.Message;

public class AddMessageRequestValidator : AbstractValidator<AddMessageRequest>
{
    public AddMessageRequestValidator()
    {
        RuleFor(x => x.ReceiverId).NotNull();
        RuleFor(x => x.ReceiverId).GreaterThan(0);
        
        RuleFor(x => x.Text).NotNull();
        RuleFor(x => x.Text).NotEmpty();
    }
}