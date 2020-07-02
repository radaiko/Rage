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
            gitHandler.StageFile(changedFile.FullPath);
            Repo.UnstagesFiles.Remove(changedFile);
        }

        private void UnStageFile(ChangedFile changedFile){
            Repo.UnstagesFiles.Add(changedFile);
            gitHandler.UnstageFile(changedFile.FullPath);
            Repo.StagedFiles.Remove(changedFile);

        }
        private void OnCommit(){
            List<string> filesToAdd = new List<string>();
            if (Repo.StagedFiles.Count == 0)
            {
                foreach (var changeFile in Repo.UnstagesFiles)
                {
                    filesToAdd.Add(changeFile.FullPath);
                }
            }
            gitHandler.CommitChanges(filesToAdd, CommitSummary, CommitMessage);
            gitHandler.PushCommits();

            // TODO: clean up interface.
            // TODO: reread repo.
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
            Repo.StagedFiles = UpdateStagedFiles();


            // Get graph
            Repo.RepoGraphAsString = gitHandler.GetGraphAsString();
            MiddleSection = new RepoGraphPageViewModel(Repo.RepoGraphAsString);

            
        }

        private ObservableCollection<ChangedFile> UpdateUnstagedFiles(){
            return gitHandler.GetUnstagedFiles();
        }
        private ObservableCollection<ChangedFile> UpdateStagedFiles(){
            return gitHandler.GetStagedFiles();
        }
    }
}