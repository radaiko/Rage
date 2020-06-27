using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Rage.ViewModels;

namespace Rage.Views
{
    public class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void SwitchExpanderStatus(object sender, RoutedEventArgs e){
            Expander expander = this.FindControl<Expander>("OverviewExpander");
            if (expander.IsExpanded)
            {
                expander.IsExpanded = false;
            } else
            {
                expander.IsExpanded = true;
            }

        }
    }
}