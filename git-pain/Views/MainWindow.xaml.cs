using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using git_pain.Services;
using git_pain.ViewModels;

namespace git_pain.Views
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