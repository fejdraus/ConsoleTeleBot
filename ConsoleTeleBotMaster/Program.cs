using System.Reflection;
using BlazorBootstrap;
using ConsoleTeleBotMaster.AutoMapperProfiles;
using ConsoleTeleBotMaster.Data;
using ElectronNET.API;
using Microsoft.EntityFrameworkCore;
using Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<ApplicationDbContext>(option =>
{
    option.UseSqlite(builder.Configuration.GetConnectionString("DbConnection"));
});
builder.Services.AddSingleton<IConfigService, ConfigService>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor(options => options.DetailedErrors = true);
builder.Services.AddElectron();
builder.WebHost.UseElectron(args);
builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped<DatabaseService>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(typeof(AppConfigProfile));
builder.Services.AddAutoMapper(typeof(AppConfigViewProfile));


if (HybridSupport.IsElectronActive)
{
    await Task.Run(async () =>
    {
        await ElectronUi.Window();
    });
}
builder.Services.AddHttpClient();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.Run();