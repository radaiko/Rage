using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using git_pain.Models;
using git_pain.Services;
using ReactiveUI;

namespace git_pain.ViewModels
{
    public class MainViewModel : ViewModelBase {
        private Config config {get;}


        public IReadOnlyList<MenuItemViewModel> MenuItems { get; set; }
        public ObservableCollection<Repo> Repos { get; }
        public ReactiveCommand<Unit, Unit> ConfigCommand { get; }

        public MainViewModel()
        {
            Repos = new ObservableCollection<Repo>();

            this.config = new Config();
            ConfigCommand = ReactiveCommand.Create(OpenConfig);
        }

        public void OpenConfig()
        {
            System.Diagnostics.Debug.WriteLine("OpenConfig");
        }


        #region repo scanning

        private void ScanRepos(){
            if(config.GetSearchFolders() == null){
                return;
            }
            foreach (var path in config.GetSearchFolders())
            {
                SearchDirectoryForRepo(path);
            }
        }

        private void SearchDirectoryForRepo(string folderPath){
            
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
                    Repos.Add(repo);
                }
            }
            foreach (var folder in folders)
            {
                SearchDirectoryForRepo(folder);
            }
        }

        private bool CheckifRepoAlreadyExists(Repo newRepo){
            foreach (var repo in Repos)
            {
                if (repo.FullPath == newRepo.FullPath)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
    
}