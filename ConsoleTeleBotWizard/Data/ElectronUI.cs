using ElectronNET.API;
using ElectronNET.API.Entities;

namespace ConsoleTeleBotWizard.Data;

public class ElectronUi
{
    public static async Task Window()
    {
        var browserWindowOptions = new BrowserWindowOptions
        {
            Width = 1000,
            Height = 850,
            Center = true,
            DarkTheme = true,
            MinHeight = 600,
            MinWidth = 800
        };
        var window = await Electron.WindowManager.CreateWindowAsync(browserWindowOptions);
        await window.WebContents.Session.ClearCacheAsync();
        window.RemoveMenu();
        window.OnClosed += () => {
             Electron.App.Quit();
        };
        var menu = new[]
        {
            new MenuItem
            {
                Label = "File", Submenu = new[]
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
                Label = "Edit", Submenu = new[]
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
                Label = "View", Submenu = new[]
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
                Label = "Help", Submenu = new[]
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
    }
}