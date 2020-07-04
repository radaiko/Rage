using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Rage.Models;
using Rage.Services;
using Rage.Utils.Models;
using ReactiveUI;
using Serilog;

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
            TransferModel transferModel = gitHandler.StageFile(changedFile.FullPath);
            if (transferModel.Successful)
            {
                ReadRepo();
            }
            //Repo.StagedFiles.Add(changedFile);
            //Repo.UnstagesFiles.Remove(changedFile);
        }

        private void UnStageFile(ChangedFile changedFile){
            TransferModel transferModel = gitHandler.UnstageFile(changedFile.FullPath);
            if (transferModel.Successful)
            {
                ReadRepo();
            }
            // Repo.UnstagesFiles.Add(changedFile);
            // gitHandler.UnstageFile(changedFile.FullPath);
            // Repo.StagedFiles.Remove(changedFile);

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
            TransferModel commitChanges = gitHandler.CommitChanges(filesToAdd, CommitSummary, CommitMessage);
            if (commitChanges.Successful)
            {
                TransferModel commit = gitHandler.PushCommits();
                if (commit.Successful)
                {
                    ReadRepo();
                    CleanCommit();
                } else
                {
                    Log.Error("Unable to push. Error: " + commit.Content);
                }
            } else
            {
                Log.Error("Unable to commit. Error: " + commitChanges.Content);
            }

        }

        private void SelectFile(string filename){
            MiddleSection = new DiffPageViewModel(gitHandler.GetDiffToLast(filename).Content); 
        }
        #endregion

        private void CleanCommit(){
            CommitSummary = "";
            CommitMessage = "";
        }

        private void ReadRepo(){
            Log.Information("Start scanning repos");
            
            // Get branches
            Dictionary<string,ObservableCollection<Models.Branch>> tempBranchDictionary = gitHandler.GetBranches();
            Repo.LocalBranches = tempBranchDictionary["local"];
            Repo.RemoteBranches = tempBranchDictionary["remote"];
            //

            // Get tags
            // TODO: read tags

            // Get changed files
            if (Repo.UnstagesFiles != null)
            {
                Repo.UnstagesFiles.Clear();
            } else
            {
                Repo.UnstagesFiles = new ObservableCollection<ChangedFile>();
            }
            foreach (var changedFile in UpdateUnstagedFiles())
            {
                Repo.UnstagesFiles.Add(changedFile);
            }

            // staged files
            if (Repo.StagedFiles != null)
            {
                Repo.StagedFiles.Clear();
            } else
            {
                Repo.StagedFiles = new ObservableCollection<ChangedFile>();
            }
            foreach (var changedFile in UpdateStagedFiles())
            {
                Repo.StagedFiles.Add(changedFile);
            }


            // Get graph
            Repo.RepoGraphAsString = gitHandler.GetGraphAsString().Content;
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