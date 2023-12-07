using CoStudyCloud.Infrastructure.CloudStorage;
using CoStudyCloud.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<ICloudStorage, GoogleCloudStorage>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

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
