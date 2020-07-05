using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using Rage.Models;
using Rage.Services;
using ReactiveUI;
using Serilog;
using Serilog.Events;

namespace Rage.ViewModels
{

    public class SettingsWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentSettingsPageViewModel;
        private int _selectedIndex;


        public ObservableCollection<ViewModelBase> SettingsPageViewModels { get; set; }
        public ViewModelBase CurrentSettingsPageViewModel
        {
            get { return _currentSettingsPageViewModel; }
            set { this.RaiseAndSetIfChanged(ref _currentSettingsPageViewModel, value); }
        }
        public int SelectedIndex 
        {
            get => _selectedIndex;
            set {
                this.RaiseAndSetIfChanged(ref _selectedIndex, value);
                this.CurrentSettingsPageViewModel = SettingsPageViewModels[_selectedIndex];
            } 
        }

        private Config config {get;}
        public SettingsWindowViewModel()
        {
            this.config = new Config();
            Initialize();
           
        }

        #region Buttons

        private void SelectSettings(ViewModelBase viewModel){
            if (!SettingsPageViewModels.Contains(viewModel))
            SettingsPageViewModels.Add(viewModel);
 
            CurrentSettingsPageViewModel = SettingsPageViewModels.FirstOrDefault(vm => vm == viewModel);

        }

        #endregion
 

        


        private void Initialize(){  
            ConfigRepoSearchPathViewModel configRepoSearchPathViewModel = new ConfigRepoSearchPathViewModel("Repo Search Folders");
            

        }
    }
}
