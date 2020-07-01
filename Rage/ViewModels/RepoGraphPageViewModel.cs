using System;

namespace Rage.ViewModels
{

    public class RepoGraphPageViewModel : ViewModelBase
    {
        public string Graph { get; set; }

        public RepoGraphPageViewModel(string graph)
        {
            this.Graph = graph;
        }
    }
}