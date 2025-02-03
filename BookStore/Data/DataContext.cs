
using black_follow.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class DataContext : IdentityDbContext<AppUser, ApplicationRole, Guid>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

  
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ApplicationRole>().HasData([
            new ApplicationRole()
            {
                Id = new Guid("11111111-1111-1111-1111-111111111111"),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new ApplicationRole()
            {
                Id = new Guid("22222222-2222-2222-2222-222222222222"),
                Name = "User",
                NormalizedName = "USER"
            }
        ]);
        base.OnModelCreating(builder);

       
        
    }



  
}
