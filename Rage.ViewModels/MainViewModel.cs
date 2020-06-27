using System;
using System.Collections.ObjectModel;
using System.IO;
using Rage.Models;
using Rage.Services;

namespace Rage.ViewModels
{
    public class MainViewModel : ViewModelBase {
        private Config config {get;}

        public ObservableCollection<Repo> Repos { get; }
        public ObservableCollection<Repo> OpenRepos { get; set; }

        public MainViewModel()
        {
            Repos = new ObservableCollection<Repo>();
            OpenRepos = new ObservableCollection<Repo>(); 

            this.config = new Config();
            ScanRepos();

        }


        public void OpenRepo(Repo repo){
            if (!OpenRepos.Contains(repo))
            { 
                OpenRepos.Add(repo);
            }
        }


        #region repo scanning

        public void ScanRepos(){
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