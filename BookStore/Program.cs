using System.Globalization;

using BiladAlsafari.Helpers;
using black_follow.Entity;
using BookStore.Extensions;
using BookStore.Resources;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLocalization(options => options.ResourcesPath = "Resource");
builder.Services.Configure<RequestLocalizationOptions>(op =>
{
    {
        var supportedCultures = new[]
        {
            new CultureInfo("en"),
            new CultureInfo("ar") // Add more as needed
        };

        op.DefaultRequestCulture = new RequestCulture("en");
        op.SupportedCultures = supportedCultures;
        op.SupportedUICultures = supportedCultures;
    }
});

builder.Services.AddValidationServices();
builder.Services.AddControllers().AddFluentValidation();
builder.Services.AddSingleton<IStringLocalizerFactory, ResourceManagerStringLocalizerFactory>();
builder.Services.AddSingleton<IStringLocalizer<SharedResource>, StringLocalizer<SharedResource>>();




builder.Services.AddOpenApi();
builder.Services.AddCustomScopes();

// Configure database context with PostgreSQL
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSingleton<MongoDbDataContext>(sp =>
{
    return new MongoDbDataContext("mongodb://localhost:27017", "ShopCartDB", sp);
});



// Configure Identity
builder.Services.AddIdentity<AppUser, ApplicationRole>(op =>
    {
        op.Password.RequireDigit = false;
        op.Password.RequiredLength = 8;
        op.Password.RequiredUniqueChars = 0;
        op.Password.RequireLowercase = false;
        op.Password.RequireNonAlphanumeric = false;
        op.Password.RequireUppercase = false;
    })
    
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();
app.UseMiddleware<CustomUnauthorizedMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseRequestLocalization(
    app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value
);
app.UseMiddleware<AuditMiddleware>();
// app.MapHub<SignalRNotificationHub>("/test");
app.UseMiddleware<ErrorHandlingMiddleware>();
app.Run();
