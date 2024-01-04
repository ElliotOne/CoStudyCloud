using CoStudyCloud.Core.Repositories;
using CoStudyCloud.Infrastructure.CloudStorage;
using CoStudyCloud.Infrastructure.GoogleCalendar;
using CoStudyCloud.Persistence;
using CoStudyCloud.Persistence.Repositories;
using Microsoft.Extensions.Options;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.Configure<GoogleCalendarSettings>(builder.Configuration.GetSection(nameof(GoogleCalendarSettings)));
builder.Services.AddSingleton<IGoogleCalendarSettings>(s =>
    s.GetRequiredService<IOptions<GoogleCalendarSettings>>().Value);
builder.Services.AddScoped<IGoogleCalendarService, GoogleCalendarService>();
builder.Services.AddSingleton<ICloudStorage, GoogleCloudStorage>();
builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation();

//Add AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Application";
        options.DefaultSignInScheme = "External";
    })
    .AddCookie("Application")
    .AddCookie("External")
    .AddGoogle(options =>
    {
        options.ClientId = configuration.GetSection("GoogleOAuth:ClientId").Value!;
        options.ClientSecret = configuration.GetSection("GoogleOAuth:ClientSecret").Value!;
        options.SaveTokens = true;
        options.Scope.Add(Google.Apis.Calendar.v3.CalendarService.Scope.Calendar);
        options.AccessType = "offline";

        options.Scope.Add("profile");
        options.Events.OnCreatingTicket = (context) =>
        {
            var picture = context.User.GetProperty("picture").GetString()!;

            context.Identity?.AddClaim(new Claim("picture", picture));

            return Task.CompletedTask;
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithRedirects("/Home/Error/{0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//Ensure database is created
using var serviceScope = app.Services.GetService<IServiceScopeFactory>()?.CreateScope();
if (serviceScope != null)
{
    string? connectionString =
        serviceScope.ServiceProvider.GetRequiredService<IConfiguration>().GetConnectionString("SpannerConnection");

    if (!string.IsNullOrEmpty(connectionString))
    {
        await ApplicationDbInitializer.Initialize(connectionString);
    }
}

app.Run();
