using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using MinecraftLauncher.Helpers;
using ModuleLauncher.Re.Authenticators;
using ModuleLauncher.Re.Models.Authenticators;
using ModuleLauncher.Re.Utils.Extensions;
using System;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MinecraftLauncher.Control
{
    public sealed partial class LoginDialog : ContentDialog
    {
        public LoginDialog()
        {
            InitializeComponent();
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {

        }

        [Obsolete]
        private async void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            MojangAuthenticator Mojang = new(Username.Text, Password.Password);
            AuthenticateResult Result = await Mojang.Authenticate();
            if (await Result.Validate())
            {
                SettingsHelper.Set("Username", Username.Text);
                SettingsHelper.Set("Password", Password.Password);
            }
        }
    }
}
