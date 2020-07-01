using System;

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
        private void CloseApp(){
            Environment.Exit(0);
        }
    }
}