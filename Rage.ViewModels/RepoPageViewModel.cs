using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Rage.Models;
using Rage.Services;

namespace Rage.ViewModels
{

    public class RepoPageViewModel : ViewModelBase
    {
        private GitHandler gitHandler;
        public Repo Repo { get; set;}

        public RepoPageViewModel(Repo repo)
        {
            Repo = repo;
            gitHandler = new GitHandler(repo.FullPath);

            ReadRepo();
        }

        #region buttons
        private void SelectLocalBranch(){
            Debugger.Break();
        }
        #endregion


        private void ReadRepo(){
            // Get branches
            Dictionary<string,ObservableCollection<Models.Branch>> tempBranchDictionary = gitHandler.GetBranches();
            Repo.LocalBranches = tempBranchDictionary["local"];
            Repo.RemoteBranches = tempBranchDictionary["remote"];
            //

            // Get tags
            // TODO: read tags
            //

            // get graph
            Repo.RepoGraphAsString = gitHandler.GetGraphAsString();

            
        }
    }
}