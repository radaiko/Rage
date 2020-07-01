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
    public class MainPageViewModel : ViewModelBase 
    {
        private int _selectedIndex;

        private Config config {get;}
        public ObservableCollection<RepoSearchFolder> RepoSearchFolders { get; }
        public ObservableCollection<Repo> OpenRepos { get; set; }
        public int SelectedIndex 
        {
            get => _selectedIndex;
            set {
                this.RaiseAndSetIfChanged(ref _selectedIndex, value);
                this.ChangeRepoView();
            } 
        }
        public RepoPageViewModel RepoPageViewModel {get; set;}

        public MainPageViewModel()
        {
            RepoSearchFolders = new ObservableCollection<RepoSearchFolder>();
            OpenRepos = new ObservableCollection<Repo>(); 

            this.config = new Config();
            Initialize();
        }

        #region Buttons

        public void OpenRepo(Repo repo){
            if (!OpenRepos.Contains(repo))
            { 
                OpenRepos.Add(repo);
                SaveOpenRepos();
                SelectedIndex = OpenRepos.Count-1;
            }
        }

        public void CloseRepo(Repo repo){
            OpenRepos.Remove(repo);
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
                        OpenRepos.Add(tempRepo);
                    }
                }

                
            }
            //

            // set Repo view
            ChangeRepoView();
        }
        
        private void SaveOpenRepos(){
            List<string> folderNames = new List<string>();
            foreach (var repo in OpenRepos)
            {
                folderNames.Add(repo.FolderName);
            }
            config.SetOpenRepos(folderNames);
        }
    
        private void ChangeRepoView(){
            if (SelectedIndex != -1)
            {
                RepoPageViewModel = null;
                var newRepo = OpenRepos[SelectedIndex];
                RepoPageViewModel = new RepoPageViewModel(newRepo);
            }
            
        }
    }
    
}