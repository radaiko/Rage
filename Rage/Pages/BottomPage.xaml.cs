using System.Diagnostics;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Rage.ViewModels;
using Rage.Views;

namespace Rage.Pages
{
    public class BottomPage : UserControl
    {
        public BottomPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OpenSettings(object sender, RoutedEventArgs e){
            var window = new SettingsWindow{
                DataContext = new SettingsWindowViewModel(),
            };
            window.Show();
        }

        private void OpenLogWindow(object sender, RoutedEventArgs e){
            var window = new LogWindow{
                DataContext = new LogWindowViewModel(),
            };
            window.Show();
        }
        private void OpenCreditsWindow(object sender, RoutedEventArgs e){
            var window = new CreditsWindow{
                DataContext = new CreditsWindowViewModel(),
            };
            window.Show();
        }

        private void OpenGithubPage(object sender, RoutedEventArgs e){
            Utils.Tools.OpenUrl("https://github.com/radaiko/Rage");
        }
    }
}