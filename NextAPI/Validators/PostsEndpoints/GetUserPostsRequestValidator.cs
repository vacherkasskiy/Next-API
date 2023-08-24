using FluentValidation;
using NextAPI.Requests.Posts;

namespace NextAPI.Validators.PostsEndpoints;

public class GetUserPostsRequestValidator : AbstractValidator<GetUserPostsRequest>
{
    public GetUserPostsRequestValidator()
    {
        RuleFor(x => x.UserId).NotNull();
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}