using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Rage.ViewModels;
using Rage.Views;

namespace Rage.Pages
{
    public class ConfigRepoSearchPath : UserControl
    {
        public ConfigRepoSearchPath()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}