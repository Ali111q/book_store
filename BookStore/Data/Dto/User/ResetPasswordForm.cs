namespace BookStore.Data.User;

public class ResetPasswordForm
{
    public string Token { get; set; }
    public string Identifier { get; set; }
    public string NewPassword { get; set; }
    
}