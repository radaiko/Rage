using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
        private string _commitSummary;
        private string _commitMessage;
        private bool _areAllChecked;

        public Repo Repo { get; set;}
        public string CommitSummary {
            get { return _commitSummary; }
            set { this.RaiseAndSetIfChanged(ref _commitSummary, value); }
        }
        public string CommitMessage {
            get { return _commitMessage; }
            set { this.RaiseAndSetIfChanged(ref _commitMessage, value); }
        }
        public ViewModelBase MiddleSection
        {
            get { return _middleSection; }
            set { this.RaiseAndSetIfChanged(ref _middleSection, value); }
        }
        public bool AreAllChecked
        {
            get { return _areAllChecked; }
            set { this.RaiseAndSetIfChanged(ref _areAllChecked, value);}
        }
        

        // -----------------------
        // ---- Constructor ----
        public RepoPageViewModel(Repo repo)
        {
            Repo = repo;
            gitHandler = new GitHandler(repo.FullPath);
            ReadRepo();
        }
        // -----------------------



        #region buttons
        private void SelectLocalBranch(){
            Debugger.Break();
        }

        private void OnCommit(){
            List<string> filesToAdd = new List<string>();
            if (Repo.ChangedFiles.Any(e => e.IsChecked == true ))
            {
                foreach (var changedFile in Repo.ChangedFiles)
                {
                    if (changedFile.IsChecked)
                    {
                        filesToAdd.Add(changedFile.FullPath);    
                    }
                }
            } else {
                filesToAdd = Repo.GetAllChangedFilesPaths();
            }
            TransferModel commitChanges = gitHandler.CommitChanges(filesToAdd, CommitSummary, CommitMessage);
            if (commitChanges.Successful)
            {
                CleanCommit();
                //ManualPush();
            } else
            {
                Log.Error("Unable to commit. Error: " + commitChanges.Content);
            }

        }

        public void ManualPush(){
            TransferModel push = gitHandler.PushCommits();
            if (push.Successful)
            {
                ReadRepo();
            } else
            {
                Log.Error("Unable to push. Error: " + push.Content);
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
            
            // -----------------------
            // ---- Get branches -----
            Dictionary<string,ObservableCollection<Models.Branch>> tempBranchDictionary = gitHandler.GetBranches();
            Repo.LocalBranches = tempBranchDictionary["local"];
            Repo.RemoteBranches = tempBranchDictionary["remote"];
            // -----------------------

            // -----------------------
            // ---- Get tags ---------
            // TODO: read tags
            // -----------------------
            
            // --------------------------
            // ---- Get changedFiles ----
            if (Repo.ChangedFiles != null)
            {
                Repo.ChangedFiles.Clear();
            }
            Repo.ChangedFiles = GetChangedFiles();
            // --------------------------

            // -----------------------
            // ---- Get graph --------
            // TODO: improve graph
            Repo.RepoGraphAsString = gitHandler.GetGraphAsString().Content;
            MiddleSection = new RepoGraphPageViewModel(Repo.RepoGraphAsString);
            // -----------------------
            
        }

        private ObservableCollection<ChangedFile> GetChangedFiles(){
            return gitHandler.GetChangedFiles();
        }
    }
}