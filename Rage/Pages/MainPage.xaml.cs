using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Rage.Pages
{
    public class MainPage : UserControl
    {
        public MainPage()
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