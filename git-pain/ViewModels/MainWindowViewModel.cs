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
        public MainViewModel MainViewItem { get; }

        public MainWindowViewModel()
        {
            MainViewItem = new MainViewModel();
            
        }
    }
}
