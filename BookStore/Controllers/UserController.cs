using BookStore.Data.User;
using BookStore.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

public class UserController:BaseController

{
    private   readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
  /// <summary>
  /// Open For Admin Example.
  /// </summary>
  /// <remarks>
  /// **Test Admin:**
  /// 
  /// ```json
  /// {
  ///   "identifier": "admin",
  ///   "password": "12345678"
  /// }
  /// ```
  /// ***For User testing go to register***
  /// </remarks>
  /// <param name="user">User object</param>
  /// <returns>Created user</returns>
  /// <response code="201">User created successfully</response>
  /// <response code="400">Invalid request</response>
    [HttpPost("login")]
    
    public async Task<ActionResult>Login([FromBody]LoginForm form)=> Ok(await _userService.Login(form));
    [HttpPost("register")]
    public async Task<ActionResult>Register([FromBody]RegisterForm form)=> Ok(await _userService.Register(form));

    [HttpPost("request-reset-password")]
    public async Task<ActionResult> RequestResetPassword([FromBody] ResetPasswordRequestForm form) =>
        Ok(await _userService.RequestPasswordReset(form));
    [HttpPost("request-reset")]
    public async Task<ActionResult> RequestReset([FromBody] ResetPasswordForm form) =>
        Ok(await _userService.ResetPassword(form));

}