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

        private void OpenLogWindow(object sender, RoutedEventArgs e){
            var logWindow = new LogWindow{
                DataContext = new LogWindowViewModel(),
            };
            logWindow.Show();
        }
    }
}