using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Rage.Views
{
    public class BottomView : UserControl
    {
        public BottomView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}