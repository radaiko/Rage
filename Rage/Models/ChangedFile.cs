using System.Collections.ObjectModel;

namespace Rage.Models
{
    public class ChangedFile{
        public string FullPath { get; set; }
        public string FileName { get; set; }
        public Repo.FileStatus Status { get; set; }
        public bool IsChecked { get; set; }

    }
    
}