using BookStore.Data.User;

using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers;

public class UserController:BaseController

{
    private   readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    [HttpPost("login")]
    public async Task<ActionResult>Login([FromBody]LoginForm form, [FromHeader(Name = "Accept-Language")]string language)=> Ok(await _userService.Login(form));
    [HttpPost("register")]
    public async Task<ActionResult>Login([FromBody]RegisterForm form, [FromHeader(Name = "Accept-Language")]string language)=> Ok(await _userService.Register(form));
}