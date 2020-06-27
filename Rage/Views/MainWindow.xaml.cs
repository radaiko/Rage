using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Rage.Services;
using Rage.ViewModels;

namespace Rage.Views
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}