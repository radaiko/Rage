using System;
using Rage.Views;

namespace Rage.ViewModels
{

    public class MenuPageViewModel : ViewModelBase
    {

        public MenuPageViewModel()
        {
            
        }

        private void OpenSettings(){
            // TODO: create settings dialog
        }
        private void OpenLog(){
            var window = new LogWindow{
                DataContext = new LogWindowViewModel(),
            };
            window.Show();
        }
        private void OpenCredits(){
            var window = new CreditsWindow{
                DataContext = new CreditsWindowViewModel(),
            };
            window.Show();
        }
        private void CloseApp(){
            Environment.Exit(0);
        }
    }
}