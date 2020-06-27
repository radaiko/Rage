using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace git_pain.Views
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