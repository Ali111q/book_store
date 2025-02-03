using AutoMapper;
using black_follow.Entity;
using BookStore.Data.User;
using BookStore.Resources;
using BookStore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

public interface IUserService
{
    Task<(UserLoginDto? data, string? message)> Login(LoginForm form);
    Task<(UserLoginDto? data, string? message)> Register(RegisterForm form);
    Task<string?> RequestPasswordReset(string emailOrUsername);  // For sending reset token
    Task<string?> ResetPassword(string token, string newPassword);  // For resetting password
}

public class UserService : IUserService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IStringLocalizer<SharedResource> _localizer;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly DataContext _context;
    private readonly IEmailService _emailService;  // Assume you have an email service for sending emails
    
    public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IStringLocalizer<SharedResource> localizer, IMapper mapper, ITokenService tokenService, DataContext context, IEmailService emailService)
    {
        _signInManager = signInManager;
        _localizer = localizer;
        _mapper = mapper;
        _tokenService = tokenService;
        _context = context;
        _emailService = emailService;
    }

    public async Task<(UserLoginDto? data, string? message)> Login(LoginForm form)
    {   
        var _user = await  _context.Users.FirstOrDefaultAsync(u=> form.Identifier == u.UserName || form.Identifier == u.Email);
        if (_user == null) return (null, _localizer["emailNotFound"]);
        var _roles = await _signInManager.UserManager.GetRolesAsync(_user);
        var _signInResults = await _signInManager.CheckPasswordSignInAsync(_user, form.Password, false);
        if (!_signInResults.Succeeded) return (null, _localizer["wrongPasword"]);
        
        var _mappedUser = _mapper.Map<UserLoginDto>(_user);
        var _token = _tokenService.CreateToken(_user, _roles.First());
        _mappedUser.Token = _token;
        
        return (_mappedUser, null);
    }

    public async Task<(UserLoginDto? data, string? message)> Register(RegisterForm form)
    {
        var _user = _mapper.Map<AppUser>(form);
        var _registerResults = await _signInManager.UserManager.CreateAsync(_user, form.Password);
        if (!_registerResults.Succeeded) return (null, _registerResults.Errors.ToString());
        
        await _signInManager.UserManager.AddToRoleAsync(_user, "User");
        var _mappedUser = _mapper.Map<UserLoginDto>(_user);
        
        return (_mappedUser, null);
    }

    // Password Reset Request: Generate and send reset token to user's email
    public async Task<string?> RequestPasswordReset(string emailOrUsername)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == emailOrUsername || u.Email == emailOrUsername);
        if (user == null) return _localizer["emailNotFound"];
        
        var token = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
        
        // Send the reset token to the user via email
        // TODO: change application link
        var resetLink = $"https://yourapp.com/reset-password?token={token}";
        await _emailService.SendEmailAsync(user.Email, "Password Reset Request", $"Click the link to reset your password: {resetLink}");

        return null; // No error, token sent successfully
    }

    // Reset Password: Validate token and reset user's password
    public async Task<string?> ResetPassword(string token, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == "user@example.com"); // You can replace this line with user lookup logic
        if (user == null) return _localizer["emailNotFound"];

        var result = await _signInManager.UserManager.ResetPasswordAsync(user, token, newPassword);
        
        if (!result.Succeeded) return string.Join(", ", result.Errors.Select(e => e.Description));

        return null;  // Password reset successful
    }
}
