using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Rage.Pages
{
    public class MenuPage : UserControl
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}