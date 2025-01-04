using Authscape.Avalonia.Views;
using AuthScape.Client;
using AuthScape.Client.Models;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using System;
using System.Diagnostics;
using System.Security;

namespace Authscape.Avalonia.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        public string Greeting { get; } = "Welcome to Avalonia!";

        public void loginCommand()
        {
            var apiService = new APIService();
            apiService.Authenticate(async (response) =>
            {
                if (response.state == LoginState.Success)
                {

                    //await SecureStorage.SetAsync("access_token", response.access_token);
                    //await SecureStorage.SetAsync("refresh_token", response.refresh_token);

                    //await Navigator.NavigateViewModelAsync<MainModel>(this, qualifier: Qualifiers.ClearBackStack);

                    var box = MessageBoxManager
                      .GetMessageBoxStandard("Caption", response.access_token,
                          ButtonEnum.YesNo);

                    var result = await box.ShowAsync();

                }
                else
                {
                    var box = MessageBoxManager
                      .GetMessageBoxStandard("Caption", "Are you sure you would like to delete appender_replace_page_1?",
                          ButtonEnum.YesNo);

                    var result = await box.ShowAsync();
                }

            }, async (authorizationRequest) =>
            {
                var topLevel = TopLevel.GetTopLevel(Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null);
                if (topLevel != null)
                {
                    var launcher = topLevel.Launcher;
                    Uri uri = new Uri(authorizationRequest);

                    await launcher.LaunchUriAsync(uri);
                }
            });
        }
    }
}
