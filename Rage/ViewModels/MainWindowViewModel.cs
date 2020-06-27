using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Text;
using Rage.Models;
using Rage.Services;
using ReactiveUI;

namespace Rage.ViewModels
{
    
    public class MainWindowViewModel : ViewModelBase
    {
        public MenuViewModel MenuViewModel { get;}
        public MainViewModel MainViewModel { get; }
        public BottomViewModel BottomViewModel {get; set;}

        public MainWindowViewModel()
        {
            MenuViewModel = new MenuViewModel();
            MainViewModel = new MainViewModel();
            BottomViewModel = new BottomViewModel();
            
        }
    }
}
