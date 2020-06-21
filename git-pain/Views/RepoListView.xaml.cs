using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace git_pain.Views
{
    public class RepoListView : UserControl
    {
        public RepoListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}