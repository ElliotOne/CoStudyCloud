using CoStudyCloud.Infrastructure.CloudStorage;
using CoStudyCloud.Persistence;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddSingleton<ICloudStorage, GoogleCloudStorage>();
builder.Services
    .AddControllersWithViews()
    .AddRazorRuntimeCompilation();

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
