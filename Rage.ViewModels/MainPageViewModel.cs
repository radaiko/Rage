using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Rage.Models;
using Rage.Services;

namespace Rage.ViewModels
{
    public class MainPageViewModel : ViewModelBase 
    {
        private Config config {get;}

        public ObservableCollection<Repo> Repos { get; }
        public ObservableCollection<Repo> OpenRepos { get; set; }
        public int SelectedIndex { get; set; }

        public MainPageViewModel()
        {
            Repos = new ObservableCollection<Repo>();
            OpenRepos = new ObservableCollection<Repo>(); 
            SelectedIndex = 1;

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
    
        private void Initialize(){
            ScanRepos();

            // reopen last opened repos
            var openFolderNames = config.GetOpenRepos();
            foreach (var openFolderName in openFolderNames)
            {
                var tempRepo = Repos.FirstOrDefault(p => p.FolderName == openFolderName);
                if (tempRepo != null)
                {
                    OpenRepos.Add(tempRepo);
                }
            }
            //
        }
        
        private void SaveOpenRepos(){
            List<string> folderNames = new List<string>();
            foreach (var repo in OpenRepos)
            {
                folderNames.Add(repo.FolderName);
            }
            config.SetOpenRepos(folderNames);
        }
    
    }
    
}