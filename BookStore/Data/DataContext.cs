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