using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using git_pain.ViewModels;
using ReactiveUI;

namespace git_pain.ViewModels
{

    public class MenuViewModel : ViewModelBase
    {

        public MenuViewModel()
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