using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Text;
using git_pain.Models;
using git_pain.Services;
using ReactiveUI;

namespace git_pain.ViewModels
{
    
    public class MainWindowViewModel : ViewModelBase
    {
        public MenuViewModel MenuViewModel { get;}
        public TopViewModel TopViewModel { get; }
        public MainViewModel MainViewModel { get; }
        public BottomViewModel BottomViewModel {get; set;}

        public MainWindowViewModel()
        {
            MenuViewModel = new MenuViewModel();
            TopViewModel = new TopViewModel();
            MainViewModel = new MainViewModel();
            BottomViewModel = new BottomViewModel();
            
        }
    }
}
