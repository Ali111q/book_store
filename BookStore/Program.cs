using System.Globalization;

using BiladAlsafari.Helpers;
using black_follow.Entity;
using BookStore.Extensions;
using BookStore.Utils;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

#region Configure Services

builder.Services.AddValidationServices();
builder.Services.AddControllers().AddFluentValidation();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder
            .AllowAnyHeader()
            .WithOrigins("*")
            .AllowAnyMethod());
});

// builder.Services.AddOpenApi();
builder.Services.AddCustomScopes(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSignalR();


#endregion

var app = builder.Build();

#region Configure Middleware

// if (app.Environment.IsDevelopment())
// {
    // app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseMiddleware<CustomUnauthorizedMiddleware>();
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();

#endregion

#region Configure Endpoints

app.MapHub<SignalRNotificationHub>("/signalR").RequireCors("AllowAllOrigins");
app.MapControllers();

#endregion

#region  Middleware

app.UseMiddleware<AuditMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

#endregion

app.Run();