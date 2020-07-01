using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Rage.Models;
using Rage.Services;
using ReactiveUI;

namespace Rage.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase _currentRepoPageViewModel;
        private int _selectedIndex;


        public MenuPageViewModel MenuPageViewModel { get;}
        public BottomPageViewModel BottomPageViewModel {get; set;}

        public ObservableCollection<ViewModelBase> RepoPageViewModels { get; set; }
        public ViewModelBase CurrentRepoPageViewModel
        {
            get { return _currentRepoPageViewModel; }
            set { this.RaiseAndSetIfChanged(ref _currentRepoPageViewModel, value); }
        }
        public int SelectedIndex 
        {
            get => _selectedIndex;
            set {
                this.RaiseAndSetIfChanged(ref _selectedIndex, value);
                this.CurrentRepoPageViewModel = RepoPageViewModels[_selectedIndex];
            } 
        }

        private Config config {get;}
        public ObservableCollection<RepoSearchFolder> RepoSearchFolders { get; }

        public MainWindowViewModel()
        {
            MenuPageViewModel = new MenuPageViewModel();
            BottomPageViewModel = new BottomPageViewModel();
            RepoSearchFolders = new ObservableCollection<RepoSearchFolder>();
            RepoPageViewModels = new ObservableCollection<ViewModelBase>();

            this.config = new Config();
            Initialize();
            
        }

        #region Buttons

        private void SelectRepo(ViewModelBase viewModel){
            if (!RepoPageViewModels.Contains(viewModel))
            RepoPageViewModels.Add(viewModel);
 
            CurrentRepoPageViewModel = RepoPageViewModels.FirstOrDefault(vm => vm == viewModel);

        }

        public void OpenRepo(Repo repo){
            //PageViewModels.Add(new HomeViewModel());

            if (!RepoPageViewModels.Contains(new RepoPageViewModel(repo)))
            { 
                RepoPageViewModels.Add(new RepoPageViewModel(repo));
                SaveOpenRepos();
                // SelectedIndex = OpenRepos.Count-1;
            }
            if (RepoPageViewModels.Count == 1)
            {
                CurrentRepoPageViewModel = RepoPageViewModels[0];
            }
        }

        public void CloseRepo(ViewModelBase viewModel){
            RepoPageViewModels.Remove(RepoPageViewModels.FirstOrDefault(vm => vm == viewModel));
            SaveOpenRepos();
        }

        #endregion
 

        #region repo scanning

        public void ScanRepos(){
            if(config.GetSearchFolders() == null){
                return;
            }
            foreach (var path in config.GetSearchFolders())
            {
                RepoSearchFolder repoSearchFolder = new RepoSearchFolder(Path.GetFileName(path));
                SearchDirectoryForRepo(ref repoSearchFolder, path);
                RepoSearchFolders.Add(repoSearchFolder);
            }
        }

        private void SearchDirectoryForRepo(ref RepoSearchFolder repoSearchFolder ,string folderPath){
            
            var folders = Directory.GetDirectories(folderPath);

            // check if main folder has git folder
            if (Array.Exists(folders, element => element == Path.Combine(folderPath,".git")))
            {
                Repo repo = new Repo();
                repo.FullPath = folderPath;
                repo.FolderName = Path.GetFileName(folderPath);
                repo.isUpToDate = true;
                if (!CheckifRepoAlreadyExists(repo))
                {
                    repoSearchFolder.Repos.Add(repo);
                }
            }
            foreach (var folder in folders)
            {
                SearchDirectoryForRepo(ref repoSearchFolder, folder);
            }
        }

        private bool CheckifRepoAlreadyExists(Repo newRepo){
            foreach (var repoSearchFolder in RepoSearchFolders)
            {
                foreach (var repo in repoSearchFolder.Repos)
                {
                    if (repo.FullPath == newRepo.FullPath)
                    {
                        return true;
                    }
                }
    
            }
            return false;
        }
        #endregion


        private void Initialize(){
            ScanRepos();

            // reopen last opened repos
            var openFolderNames = config.GetOpenRepos();
            foreach (var openFolderName in openFolderNames)
            {
                foreach (var repoSearchFolder in RepoSearchFolders)
                {
                    var tempRepo = repoSearchFolder.Repos.FirstOrDefault(p => p.FolderName == openFolderName);
                    if (tempRepo != null)
                    {
                        RepoPageViewModels.Add(new RepoPageViewModel(tempRepo));
                    }
                }

                
            }
            

            // set Repo view
            if (RepoPageViewModels.Count >= 1)
            {
                CurrentRepoPageViewModel = RepoPageViewModels[0];
            }
        }
        
        private void SaveOpenRepos(){
            List<string> folderNames = new List<string>();
            foreach (RepoPageViewModel repo in RepoPageViewModels)
            {
                folderNames.Add(repo.Repo.FolderName);
            }
            config.SetOpenRepos(folderNames);
        }
    
    }
}
