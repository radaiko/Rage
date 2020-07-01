using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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
    }
}