using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace git_pain.Views
{
    public class MenuView : UserControl
    {
        public MenuView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}