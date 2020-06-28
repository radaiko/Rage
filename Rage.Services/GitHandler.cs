using System.Collections.Generic;
using System.Collections.ObjectModel;
using Rage.Models;
using Rage.Utils;

namespace Rage.Services
{
    public class GitHandler{

        #region public interface
        public Dictionary<string,ObservableCollection<Models.Branch>> GetBranches(){
            ObservableCollection<Models.Branch> localBranches = new ObservableCollection<Models.Branch>();
            ObservableCollection<Models.Branch> remoteBranches = new ObservableCollection<Models.Branch>();
            var branches = Branch();
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
            return new Dictionary<string,ObservableCollection<Models.Branch>>() { {"local", localBranches}, {"remote", remoteBranches} };
        }

        public string GetGraphAsString(){
            return LogGraph();
        }

        #endregion


        #region parameters
            private string repoPath;
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
        
        private Branch[] Branch(){
            //TODO: support all major platforms


            List<Branch> branches = new List<Branch>();
            
            // get local branches
            var result = Utils.Utils.SplitStringToArray(Utils.Utils.ExecutionProcess("/usr/bin/git", "branch", repoPath), "\n");


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
            result = Utils.Utils.SplitStringToArray(Utils.Utils.ExecutionProcess("/usr/bin/git", "branch -r", repoPath), "\n");

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
            return Utils.Utils.ExecutionProcess("/usr/bin/git", "log --graph --abbrev-commit --decorate --date=relative --all", repoPath);
        }

        #endregion
    }
}