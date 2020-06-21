using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using git_pain.Models;

namespace git_pain.ViewModels
{
    public class RepoListViewModel : ViewModelBase
    {
        public ObservableCollection<Repo> Repos { get; }
        public IEnumerable<string> repoSearchPaths {get; set;}


        public RepoListViewModel(IEnumerable<string> paths)
        {
            Repos = new ObservableCollection<Repo>();
            repoSearchPaths = paths;
            
        }

        private void ScanRepos(){
            foreach (var path in repoSearchPaths)
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
    }
}