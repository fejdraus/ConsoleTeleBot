using System.Linq;
using System.Threading.Tasks;
using BlazorBootstrap;
using ConsoleTeleBotMaster.Data;
using ElectronNET.API;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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


if (HybridSupport.IsElectronActive)
{
    await Task.Run(async () => {
        
        var window = await Electron.WindowManager.CreateWindowAsync();
        await window.WebContents.Session.ClearCacheAsync();
        window.RemoveMenu();
        window.OnClosed += () => {
             Electron.App.Quit();
        };
        window.SetMinimumSize(800, 600);
        window.SetSize(1000, 700);
        var menu = new MenuItem[]
        {
            new MenuItem
            {
                Label = "File", Submenu = new MenuItem[]
                {
                    new MenuItem
                    {
                        Label = "Exit",
                        Click = () =>
                            Electron.WindowManager.BrowserWindows.First().Close()
                    }
                }
            },
            new MenuItem
            {
                Label = "Edit", Submenu = new MenuItem[]
                {
                    new MenuItem { Label = "Undo", Accelerator = "CmdOrCtrl+Z", Role = MenuRole.undo },
                    new MenuItem { Label = "Redo", Accelerator = "Shift+CmdOrCtrl+Z", Role = MenuRole.redo },
                    new MenuItem { Type = MenuType.separator },
                    new MenuItem { Label = "Cut", Accelerator = "CmdOrCtrl+X", Role = MenuRole.cut },
                    new MenuItem { Label = "Copy", Accelerator = "CmdOrCtrl+C", Role = MenuRole.copy },
                    new MenuItem { Label = "Paste", Accelerator = "CmdOrCtrl+V", Role = MenuRole.paste }
                }
            },
            new MenuItem
            {
                Label = "View", Submenu = new MenuItem[]
                {
                    new MenuItem
                    {
                        Label = "Reload",
                        Accelerator = "CmdOrCtrl+R",
                        Click = () =>
                        {
                            Electron.WindowManager.BrowserWindows.ToList().ForEach(browserWindow =>
                            {
                                if (browserWindow.Id != 1)
                                {
                                    browserWindow.Close();
                                }
                                else
                                {
                                    browserWindow.Reload();
                                }
                            });
                        }
                    }
                }
            },
            new MenuItem
            {
                Label = "Help", Submenu = new MenuItem[]
                {
                    new MenuItem
                    {
                        Label = "About",
                        Click = () =>
                        {
                            var options = new MessageBoxOptions("Author: Oleksandr Tyra\n" +
                                                                "Email: a.tyra@banzait.com\n" +
                                                                "Version: 1.0.0\n" +
                                                                "Product name: ConsoleTeleBot\n" +
                                                                "Copyright: Copyright © 2023. Oleksandr Tyra, All Rights Reserved.\n" +
                                                                "License: AGPL-3.0")
                                {
                                    Type = MessageBoxType.info,
                                    Title = "About"
                                };
                            Task.Run(() => Electron.Dialog.ShowMessageBoxAsync(options));
                        }
                    }
                }
            }
        };
        Electron.Menu.SetApplicationMenu(menu);
        //window.WebContents.OpenDevTools();
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