using AutoMapper;
using black_follow.Entity;
using BookStore.Data.User;
using BookStore.Services;
using BookStore.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Services;

#region interface
public interface IUserService
{
    Task<(UserLoginDto? data, string? message)> Login(LoginForm form);
    Task<(UserLoginDto? data, string? message)> Register(RegisterForm form);
    Task<(bool? status, string? message)> RequestPasswordReset(ResetPasswordRequestForm form);
    Task<(bool? status, string? message)> ResetPassword(ResetPasswordForm form);
    Task<(bool? status, string? message)> ChangePassword(ChangePasswordForm form, Guid userId);
}
#endregion

public class UserService : IUserService
{
    #region private
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly DataContext _context;
    private readonly IEmailService _emailService;
    #endregion

    #region constructor
    public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IMapper mapper, ITokenService tokenService, DataContext context, IEmailService emailService)
    {
        _signInManager = signInManager;
        _mapper = mapper;
        _tokenService = tokenService;
        _context = context;
        _emailService = emailService;
    }
    #endregion

    #region login
    public async Task<(UserLoginDto? data, string? message)> Login(LoginForm form)
    {   
        var _user = await _context.Users.FirstOrDefaultAsync(u => form.Identifier == u.UserName || form.Identifier == u.Email);
        if (_user == null) return (null, "Username or Email is invalid");

        var _roles = await _signInManager.UserManager.GetRolesAsync(_user);
        var _signInResults = await _signInManager.CheckPasswordSignInAsync(_user, form.Password, false);
        if (!_signInResults.Succeeded) return (null, "Wrong Password");

        var _mappedUser = _mapper.Map<UserLoginDto>(_user);
        var _token = _tokenService.CreateToken(_user, _roles.First());
        _mappedUser.Token = _token;

        return (_mappedUser, null);
    }
    #endregion

    #region register
    public async Task<(UserLoginDto? data, string? message)> Register(RegisterForm form)
    {
        var _user = _mapper.Map<AppUser>(form);
        var _registerResults = await _signInManager.UserManager.CreateAsync(_user, form.Password);
        if (!_registerResults.Succeeded) return (null, _registerResults.Errors.ToString());

        await _signInManager.UserManager.AddToRoleAsync(_user, "User");
        var _mappedUser = _mapper.Map<UserLoginDto>(_user);

        return (_mappedUser, null);
    }
    #endregion

    #region password reset request
    public async Task<(bool? status, string? message)> RequestPasswordReset(ResetPasswordRequestForm form)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == form.Identifier || u.Email == form.Identifier);
        if (user == null) return (null, "Username or Email is invalid");

        var token = await _signInManager.UserManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"{Util.AppUrl}/reset-password?token={token}";
        // TODO: after change smtp caredetials uncomment this
        // _emailService.SendEmailAsync(user.Email, "Password Reset", resetLink);
        return (null, token);
    }
    #endregion

    #region reset password
    public async Task<(bool? status, string? message)> ResetPassword(ResetPasswordForm form)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == form.Identifier || u.Email == form.Identifier);
        if (user == null) return (null, "Username or Email is invalid");

        var result = await _signInManager.UserManager.ResetPasswordAsync(user, form.Token, form.NewPassword);
        if (!result.Succeeded) return (null, string.Join(", ", result.Errors.Select(e => e.Description)));

        return (true, null);
    }
    #endregion

    #region change password

    public async Task<(bool? status, string? message)> ChangePassword(ChangePasswordForm form, Guid userId)
    {
        var _user = await _context.Users.FindAsync(userId);
      var _changePasswordResault = await  _signInManager.UserManager.ChangePasswordAsync(_user,form.OldPassword, form.NewPassword);
      if (!_changePasswordResault.Succeeded) return (null, _changePasswordResault.Errors.First().Description);
      return (true, null);


    }
    

    #endregion
}
