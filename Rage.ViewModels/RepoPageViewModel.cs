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
        public string CommitSummary { get; set; }
        public string CommitMessage { get; set; }

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

        private void StageFile(string fileName) {
            Repo.StagedFiles.Add(fileName);
            Repo.UnstagesFiles.Remove(fileName);
        }
        private void OnCommit(){
            // TODO: add files and commit
            // TODO: add option for auto push
            //gitHandler.Commit();
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

            // Get changed files
            Repo.UnstagesFiles = UpdateUnstagedFiles();


            // Get graph
            Repo.RepoGraphAsString = gitHandler.GetGraphAsString();

            
        }

        private ObservableCollection<string> UpdateUnstagedFiles(){
            return gitHandler.GetUnstagedFiles();
        }
    }
}