using BookStore.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace BookStore.Data.User;

public class LoginForm
{
    public string Identifier { get; set; }
    public string Password { get; set; }
    
}

public class LoginFormValidator : AbstractValidator<LoginForm>
{
    private readonly IStringLocalizer<SharedResource> _localizer;

    public LoginFormValidator(IStringLocalizer<SharedResource> localizer)
    {
        _localizer = localizer;
        RuleFor(x=>x.Identifier).NotNull().WithMessage(_localizer["identifierRequired"]);
        RuleFor(x=>x.Password).NotNull().WithMessage(_localizer["passwordRequired"]).MinimumLength(8).WithMessage(_localizer["passwordMinLength"]);
    }
    
}