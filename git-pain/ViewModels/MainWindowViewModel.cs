using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace git_pain.ViewModels
{
    
    public class MainWindowViewModel : ViewModelBase
    {

        public string debugRepoPath1 = @"/home/radaiko/Documents/Coding/";

        public RepoListViewModel List {get;}

        public MainWindowViewModel()
        {
            List = new RepoListViewModel(new string[]{debugRepoPath1});
        }
        
    }
}
