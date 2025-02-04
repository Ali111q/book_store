using Microsoft.AspNetCore.Identity;

namespace black_follow.Entity;

public class AppUser: IdentityUser<Guid>
{
    public List<Order> Orders { get; set; }
}