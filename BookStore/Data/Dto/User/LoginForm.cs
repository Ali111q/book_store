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


    public LoginFormValidator()
    {

        RuleFor(x=>x.Identifier).NotNull().WithMessage("Identifier is required.s");
        RuleFor(x=>x.Password).NotNull().WithMessage("Password is required").MinimumLength(8).WithMessage("password must be 8 or more characters");
    }
    
}