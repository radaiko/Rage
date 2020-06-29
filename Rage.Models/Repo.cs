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
        public ObservableCollection<string> UnstagesFiles { get; set; }
        public ObservableCollection<string> StagedFiles { get; set; } = new ObservableCollection<string>();

    }
    
}