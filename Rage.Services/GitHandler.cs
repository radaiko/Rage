using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Rage.Models;
using Rage.Utils;

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
        private string gitFetch = "fetch";  // Downloads all history from the remote tracking branches
        private string gitMerge = "merge";  // Combines remote tracking branches into current local branch
                                            // [branch-name] Combines the specified branch’s history into the current branch. This is usually done in pull requests, but is an important Git operation.
        private string gitPush = "push";    // Uploads all local branch commits to GitHub
        private string gitPull = "pull";    // git pull is a combination of git fetch and git merge
        private string gitDiff = "diff";
        private string gitReset = "reset";  // Undoes all commits after [commit], preserving changes locally
                                            // --hard [commit] Discards all history and changes back to the specified commit

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

        public string GetGraphAsString(){
            return LogGraph();
        }

        public ObservableCollection<string> GetUnstagedFiles(){
            ObservableCollection<string> unstagedFiles = new ObservableCollection<string>();
            var changedFiles = ListChangedFiles();
            var newFiles = ListNewFile();
            foreach (var changedFile in changedFiles)
            {
                unstagedFiles.Add(changedFile);
            }
            foreach (var newFile in newFiles)
            {
                unstagedFiles.Add(newFile);
            }
            

            return unstagedFiles;
        }

        #endregion



        #region constructor
        public GitHandler(string repoPath)
        {
            this.repoPath = repoPath;
        }

        #endregion

        #region private
        private void ReadRepo(){

        }





        #endregion


        #region Git Commands
        
        private Branch[] ListBranch(){
            //TODO: support all major platforms


            List<Branch> branches = new List<Branch>();
            
            // get local branches
            var result = Utils.Utils.SplitStringToArray(Utils.Utils.ExecutionProcess(gitCommand, gitBranch, repoPath), "\n");


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
            result = Utils.Utils.SplitStringToArray(Utils.Utils.ExecutionProcess(gitCommand, gitBranch + " -r", repoPath), "\n");

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

        private string LogGraph(){
            return Utils.Utils.ExecutionProcess(gitCommand, gitLogWithGraphic, repoPath);
        }

        private string[] ListChangedFiles(){
            var returnValue = Utils.Utils.ExecutionProcess(gitCommand, gitLsFiles + " -m --exclude-standard", repoPath);
            return returnValue.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();;
        }

        private string[] ListNewFile(){
            var returnValue = Utils.Utils.ExecutionProcess(gitCommand, gitLsFiles + " -o  --exclude-standard", repoPath);
            return returnValue.Split("\n").Where(x => !string.IsNullOrEmpty(x)).ToArray();;
        }

        #endregion
    }
}