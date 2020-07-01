using System.Collections.ObjectModel;

namespace Rage.Models
{
    public class RepoSearchFolder{
        public string Name { get; }
        public ObservableCollection<Repo> Repos { get; set; }

        public RepoSearchFolder(string name)
        {
            this.Name = name;
            Repos = new ObservableCollection<Repo>();
            
        }
    }
    
}