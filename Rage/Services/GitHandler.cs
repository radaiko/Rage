using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Rage.Models;
using Rage.Utils;
using Rage.Utils.Models;
using Rage.ViewModels;
using Rage.Views;
using Serilog;

namespace Rage.Services
{
    public class GitHandler{

        #region git parameters
        private string gitCommand = "/usr/bin/git";
        private string gitBranch = "branch";
        private string gitLogWithGraphic = "log --graph --abbrev-commit --decorate --date=relative --all";
        private string gitRestore = "restore";
        private string gitAdd = "add";
        private string gitCommit = "commit";
        private string gitCheckout = "checkout";
        private string gitShow = "show";
        private string gitStatus = "status";
        private string gitFetch = "fetch";  // Downloads all history from the remote tracking branches
        private string gitMerge = "merge";  // Combines remote tracking branches into current local branch
                                            // [branch-name] Combines the specified branch’s history into the current branch. This is usually done in pull requests, but is an important Git operation.
        private string gitPush = "push";    // Uploads all local branch commits to GitHub
        private string gitPull = "pull";    // git pull is a combination of git fetch and git merge
        private string gitDiff = "diff";
        private string gitReset = "reset";  // Undoes all commits after [commit], preserving changes locally
                                            // --hard [commit] Discards all history and changes back to the specified commit
                                            // -- unstage files
        private string gitLsFiles = "ls-files";




        #endregion

        #region parameters
            private string repoPath;
        #endregion


        #region public interface
        public Dictionary<string,ObservableCollection<Models.Branch>> GetBranches(){
            ObservableCollection<Branch> localBranches = new ObservableCollection<Branch>();
            ObservableCollection<Branch> remoteBranches = new ObservableCollection<Branch>();
            var branches = ListBranch();
            foreach (var branch in branches)
            {
                if (branch.IsRemote)
                {
                    remoteBranches.Add(branch);
                } else 
                {
                    localBranches.Add(branch);
                }
                
            }
            return new Dictionary<string,ObservableCollection<Branch>>() { {"local", localBranches}, {"remote", remoteBranches} };
        }

        public TransferModel GetGraphAsString(){
            return LogGraph();
        }

        public ObservableCollection<ChangedFile> GetUnstagedFiles(){
            ObservableCollection<ChangedFile> unstagedFiles = ListUnstagedFiles();

            return unstagedFiles;
        }

        public ObservableCollection<ChangedFile> GetStagedFiles(){
            ObservableCollection<ChangedFile> stagedFiles = ListStagedFiles();

            return stagedFiles;
        }

        public TransferModel GetDiffToLast(string filepath){
            return DiffFile(filepath);
        }

        public TransferModel StageFile(string filepath){
            return Stage(filepath);
        }
        public TransferModel UnstageFile(string filepath){
            return Unstage(filepath);
        }

        public void CommitChanges(List<string> filesToAdd, string commitSummary, string commitMessage)
        {
            // add files
            foreach (var filepath in filesToAdd)
            {
                Stage(filepath);     
            }
            Commit(commitSummary, commitMessage);
        }
        
        public TransferModel PushCommits()
        {
            return Push();
        }        



        #endregion



        #region constructor
        public GitHandler(string repoPath)
        {
            this.repoPath = repoPath;
        }

        #endregion

        #region private

        private ChangedFile ExtractFileStatus(string indicator, string filename){
            switch (indicator)
                {
                    case "M": // modified
                        return new ChangedFile(){
                            FileName = filename, 
                            FullPath = Path.Combine(repoPath, filename),
                            Status = Repo.FileStatus.M
                        };
                    case "D": // deleted
                        return new ChangedFile(){
                            FileName = filename, 
                            FullPath = Path.Combine(repoPath, filename), 
                            Status = Repo.FileStatus.D
                        };
                    case "A": // added
                        return new ChangedFile(){
                            FileName = filename, 
                            FullPath = Path.Combine(repoPath, filename), 
                            Status = Repo.FileStatus.A
                        };
                    case "?": // untracked
                        return new ChangedFile(){
                            FileName = filename, 
                            FullPath = Path.Combine(repoPath, filename), 
                            Status = Repo.FileStatus.U
                        };
                    default:
                        Log.Warning("Unknown git indicator found. Please report! Indicator: " + indicator);
                        return new ChangedFile();
                }

        }
        private ChangedFile AnalyzeFileStatus(bool isUnstage, string line){

            if (isUnstage)
            {
                // ---- untracked file ----
                if (line.StartsWith("?"))
                {
                    var filename = line.Split(" ", 2)[1];
                    return ExtractFileStatus("?", filename);
                } 
                // -----------------------
                // ---- unstaged file ----
                if (line.StartsWith(" "))
                {
                    line = line.Substring(1);
                    var indicator = line.Split(" ",2)[0];
                    var filename = line.Split(" ", 2)[1];
                    return ExtractFileStatus(indicator, filename);
                    
                }
            } else
            {
                // ---------------------
                // ---- staged file ----
                if (!line.StartsWith(" "))
                {
                    var indicator = line.Split(" ",2)[0];
                    var filename = line.Split(" ", 2)[1];
                    return ExtractFileStatus(indicator, filename);
                }
            }
            
            // ---- unknown ----
            Log.Error("Unknown file status");
            return new ChangedFile();
        }



        private void LogData(TransferModel transferModel){
            if (transferModel.Successful)
            {
                Log.Information(transferModel.Content);
            } else
            {
                Log.Error(transferModel.Content);
                var logWindow = new LogWindow{
                    DataContext = new LogWindowViewModel(),
                };
                logWindow.Show();
            }
        }

        #endregion


        #region Git Commands
        
        private Branch[] ListBranch(){
            //TODO: support all major platforms


            List<Branch> branches = new List<Branch>();
            
            // get local branches
            TransferModel transferModel = Utils.Tools.ExecutionProcess(gitCommand, gitBranch, repoPath);
            LogData(transferModel);
            var result = Utils.Tools.SplitStringToArray(transferModel.Content, "\n");


            foreach (var stringBranch in result)
            {
                if (!stringBranch.Contains("HEAD") && stringBranch != "")
                {
                    var cleanedString = stringBranch.Replace(" ", "");
                    Branch branch = new Branch(){Name = cleanedString};
                    branches.Add(branch);
                }
            }

            // get remote branches
            transferModel = Utils.Tools.ExecutionProcess(gitCommand, gitBranch + " -r", repoPath);
            LogData(transferModel);
            result = Utils.Tools.SplitStringToArray(transferModel.Content, "\n");

            foreach (var stringBranch in result)
            {
                if (!stringBranch.Contains("HEAD") && stringBranch != "")
                {
                    var cleanedString = stringBranch.Replace(" ", "");
                    var branchName = cleanedString.Split("/")[cleanedString.Split("/").Length-1];
                    var branchOrigin = cleanedString.Split("/")[0];
                    Branch branch = new Branch(){Name = branchName, IsRemote = true, Origin = branchOrigin};
                    branches.Add(branch);
                }
                
            }


            return branches.ToArray();
        }

        private TransferModel LogGraph(){
            TransferModel transferModel = Utils.Tools.ExecutionProcess(gitCommand, gitLogWithGraphic, repoPath);
            LogData(transferModel);
            return transferModel;
        }

        private ObservableCollection<ChangedFile> ListUnstagedFiles(){
            ObservableCollection<ChangedFile> changedFiles = new ObservableCollection<ChangedFile>();
            TransferModel files = Utils.Tools.ExecutionProcess(gitCommand, gitStatus + " --porcelain", repoPath);
            LogData(files);
            var lines = files.Content.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
            List<string> returnArr = new List<string>();
            foreach (var line in lines)
            {
                ChangedFile changedFile = AnalyzeFileStatus(isUnstage:true ,line);
                if (changedFile.FileName != null)
                {
                    
                changedFiles.Add(changedFile);
                }
            }
            return changedFiles;
        }

        private ObservableCollection<ChangedFile> ListStagedFiles(){
            ObservableCollection<ChangedFile> changedFiles = new ObservableCollection<ChangedFile>();
            var files = Utils.Tools.ExecutionProcess(gitCommand, gitStatus + " --porcelain", repoPath);
            LogData(files);
            var lines = files.Content.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();
            List<string> returnArr = new List<string>();
            foreach (var line in lines)
            {
                ChangedFile changedFile = AnalyzeFileStatus(isUnstage:false ,line);
                if (changedFile.FileName != null)
                {
                    changedFiles.Add(changedFile);
                }
            }
            return changedFiles;
        }
        private TransferModel DiffFile(string filepath){
            TransferModel transferModel = Utils.Tools.ExecutionProcess(gitCommand, gitDiff + " " + filepath, repoPath);
            LogData(transferModel);
            return transferModel;
        }

        private TransferModel Stage(string filepath){
            TransferModel transferModel = Utils.Tools.ExecutionProcess(gitCommand, gitAdd + " " + filepath, repoPath);
            LogData(transferModel);
            return transferModel;
        }
        
        private TransferModel Unstage(string filepath){
            TransferModel transferModel = Utils.Tools.ExecutionProcess(gitCommand, gitReset + " -- " + filepath, repoPath);
            LogData(transferModel);
            return transferModel;
        }

        private TransferModel Commit(string commitSummary, string commitMessage){
            TransferModel transferModel;
            if (commitMessage == "")
            {                
                transferModel = Utils.Tools.ExecutionProcess(gitCommand, gitCommit + " -m \"" + commitSummary + "\"", repoPath);
            } else
            {
                transferModel = Utils.Tools.ExecutionProcess(gitCommand, gitCommit + " -m \"" + commitSummary + "\" -m \"" + commitMessage + "\"", repoPath);
            }
            LogData(transferModel);
            return transferModel;
        }

        private TransferModel Push(){
            TransferModel transferModel = Utils.Tools.ExecutionProcess(gitCommand, gitPush, repoPath);
            LogData(transferModel);
            return transferModel;
        }
        #endregion
    }
}