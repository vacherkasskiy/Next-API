﻿using FluentValidation;
using NextAPI.Requests.Users;

namespace NextAPI.Validators.Users;

public class SetStatusRequestValidator : AbstractValidator<SetStatusRequest>
{
    public SetStatusRequestValidator()
    {
        RuleFor(x => x.UserId).NotNull();
        RuleFor(x => x.UserId).GreaterThan(0);
    }
}