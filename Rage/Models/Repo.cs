using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Rage.Models
{
    public class Repo{
        public string FullPath { get; set; }
        public string FolderName { get; set; }
        public bool isUpToDate { get; set; }
        public ObservableCollection<Branch> LocalBranches { get; set; }
        public string ActiveBranchName { get; set; }
        public ObservableCollection<Branch> RemoteBranches { get; set; }
        public ObservableCollection<Tag> Tags { get; set; }
        public string RepoGraphAsString { get; set; }
        public ObservableCollection<ChangedFile> ChangedFiles { get; set; } = new ObservableCollection<ChangedFile>();


        public enum FileStatus
        {
            D, // deleted
            M, // modified
            U, // untracked
            A, // added

        }

        public List<string> GetAllChangedFilesPaths(){
            List<string> returnValue = new List<string>();
            foreach (var changedFile in this.ChangedFiles)
            {
                returnValue.Add(changedFile.FullPath);
            }
            return returnValue;
        }
    }
    
}