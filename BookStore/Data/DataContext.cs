using black_follow.Entity;
using BookStore.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class DataContext : IdentityDbContext<AppUser, ApplicationRole, Guid>
{
    public DbSet<Book> Books { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<Ads> Ads { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Author>().HasMany(au => au.Books).WithOne(b => b.Author).HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Genre>().HasMany(au => au.Books).WithOne(b => b.Genre).HasForeignKey(b => b.GenreId)
            .OnDelete(DeleteBehavior.NoAction);
        builder.Entity<Order>().HasMany(o => o.Items).WithOne(oi => oi.Order).HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<AppUser>().HasMany(u => u.Orders).WithOne(o => o.User).HasForeignKey(o=>o.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Book>().HasMany(b => b.OrderItems).WithOne(oi => oi.Book).HasForeignKey(oi => oi.BookId)
            .OnDelete(DeleteBehavior.Restrict);
        
        base.OnModelCreating(builder);

        // var adminRole = new ApplicationRole
        // {
        //     Id = new Guid("11111111-1111-1111-1111-111111111111"), // Static GUID
        //     Name = "Admin",
        //     NormalizedName = "ADMIN"
        // };
        //
        // var userRole = new ApplicationRole
        // {
        //     Id = new Guid("22222222-2222-2222-2222-222222222222"), // Static GUID
        //     Name = "User",
        //     NormalizedName = "USER"
        // };
        //
        // builder.Entity<ApplicationRole>().HasData(adminRole, userRole);
        //
        // // Seed User with static GUID
        //
        // var user = new AppUser
        // {
        //     Id = new Guid("22222222-2222-2222-2222-222222222223"), // Static GUID
        //     UserName = "admin",
        //     NormalizedUserName = "ADMIN",
        //     Email = "admin@admin.com",
        //     NormalizedEmail = "ADMIN@ADMIN.COM",
        //     EmailConfirmed = true
        // };
        //
        // user.PasswordHash = "AQAAAAIAAYagAAAAEOGh5wElOCNPLMT13osY7X0sN7cR9J+1tLC24/+wDIwL2eECg2TuwDN/8qkgqq1Z7Q==";
        // builder.Entity<AppUser>().HasData(user);
        //
        // // Seed User-Role Relationship
        // builder.Entity<IdentityUserRole<Guid>>().HasData(
        //     new IdentityUserRole<Guid>
        //     {
        //         UserId = user.Id,  // Static user ID
        //         RoleId = adminRole.Id // Static role ID
        //     }
        // );
    }
}