using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using git_pain.ViewModels;

namespace git_pain.Views
{
    public class TopView : UserControl
    {
        public TopView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}