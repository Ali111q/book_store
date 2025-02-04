using black_follow.Entity;
using Microsoft.AspNetCore.Identity;

public class SeedExtension
{
    private readonly DataContext _context;

    public SeedExtension(DataContext context)
    {
        _context = context;
    }

    public  void Initialize()
    {


        // Check if no users exist in the database
        if (!_context.Users.Any())
        {
            // Define static roles with GUIDs
            var adminRole = new ApplicationRole
            {
                Id = Guid.NewGuid(), // Static GUID
                Name = "Admin",
                NormalizedName = "ADMIN"
            };

            var userRole = new ApplicationRole
            {
                Id = Guid.NewGuid(), // Static GUID

                Name = "User",
                NormalizedName = "USER"
            };

            // Seed roles if they don't exist
            if (!_context.Roles.Any(r => r.Name == adminRole.Name))
            {
                _context.Roles.Add(adminRole);
            }

            if (!_context.Roles.Any(r => r.Name == userRole.Name))
            {
                _context.Roles.Add(userRole);
            }

            // Define a user with a static GUID
            var user = new AppUser
            {
                Id = Guid.NewGuid(), // Static GUID

                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true
            };

            // Set a pre-hashed password (adjust it as needed)
            user.PasswordHash = "AQAAAAIAAYagAAAAEOGh5wElOCNPLMT13osY7X0sN7cR9J+1tLC24/+wDIwL2eECg2TuwDN/8qkgqq1Z7Q==";

            // Seed the user if they don't exist
            _context.Users.Add(user);

            // Seed the User-Role relationship if not already assigned
            if (!_context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == adminRole.Id))
            {
                _context.UserRoles.Add(new IdentityUserRole<Guid>
                {
                    UserId = user.Id,
                    RoleId = adminRole.Id
                });
            }

            _context.SaveChangesAsync();
        }
    }
}
