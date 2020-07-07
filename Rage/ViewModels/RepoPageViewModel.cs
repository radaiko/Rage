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
        // -----------------------
        // ---- Parameters ----
        #region 
        private GitHandler gitHandler;
        #endregion
        // -----------------------

        // -----------------------
        // ---- Properties ----
        #region 
        public Repo Repo { get; set;}
        private string _commitSummary;
        public string CommitSummary {
            get { return _commitSummary; }
            set { this.RaiseAndSetIfChanged(ref _commitSummary, value); }
        }
        private ViewModelBase _middleSection;
        private string _commitMessage;
        public string CommitMessage {
            get { return _commitMessage; }
            set { this.RaiseAndSetIfChanged(ref _commitMessage, value); }
        }
        public ViewModelBase MiddleSection
        {
            get { return _middleSection; }
            set { this.RaiseAndSetIfChanged(ref _middleSection, value); }
        }
        private bool _areAllChecked;
        public bool AreAllChecked
        {
            get { return _areAllChecked; }
            set { this.RaiseAndSetIfChanged(ref _areAllChecked, value);}
        }
        private bool _isAutoPushChecked;
        public bool IsAutoPushChecked
        {
            get { return _isAutoPushChecked; }
            set { this.RaiseAndSetIfChanged(ref _isAutoPushChecked, value);  
            if(value) { this.CommitButtonText = "Commit and Push";} else { this.CommitButtonText = "Commit"; }}
        }
        private string _commitButtonText = "Commit";
        public string CommitButtonText
        {
            get { return _commitButtonText; }
            set { this.RaiseAndSetIfChanged(ref _commitButtonText, value); }
        }
        
        #endregion
        // -----------------------
        

        // -----------------------
        // ---- Constructor ----
        #region 
        public RepoPageViewModel(Repo repo)
        {
            Repo = repo;
            gitHandler = new GitHandler(repo.FullPath);
            ReadRepo();
        }
        #endregion
        // -----------------------


        // -----------------------
        // ---- Buttons ----
        #region 
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
                ReadRepo();
                if(IsAutoPushChecked){
                    ManualPush();
                }
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
        // -----------------------


        // -----------------------
        // ---- Methods ----
        #region 
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
        #endregion
        // -----------------------
    }
}