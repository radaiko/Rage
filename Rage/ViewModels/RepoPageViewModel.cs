using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Rage.Models;
using Rage.Services;
using ReactiveUI;

namespace Rage.ViewModels
{

    public class RepoPageViewModel : ViewModelBase
    {
        private GitHandler gitHandler;
        private ViewModelBase _middleSection;

        public Repo Repo { get; set;}
        public string CommitSummary { get; set; }
        public string CommitMessage { get; set; }
        public ViewModelBase MiddleSection
        {
            get { return _middleSection; }
            set { this.RaiseAndSetIfChanged(ref _middleSection, value); }
        }

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

        private void StageFile(ChangedFile changedFile) {
            Repo.StagedFiles.Add(changedFile);
            Repo.UnstagesFiles.Remove(changedFile);
        }
        private void OnCommit(){
            // TODO: add files and commit
            // TODO: add option for auto push
            //gitHandler.Commit();
        }

        private void SelectFile(string filename){
            MiddleSection = new DiffPageViewModel(gitHandler.GetDiffToLast(filename)); 
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
            MiddleSection = new RepoGraphPageViewModel(Repo.RepoGraphAsString);

            
        }

        private ObservableCollection<ChangedFile> UpdateUnstagedFiles(){
            return gitHandler.GetUnstagedFiles();
        }
    }
}