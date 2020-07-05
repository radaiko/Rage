using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using Rage.Services;
using ReactiveUI;

namespace Rage.ViewModels
{

    public class ConfigRepoSearchPathViewModel : ViewModelBase
    {
        #region Parameters
        private Config config = new Config();
        #endregion
        
        #region Properties
        public ObservableCollection<string> SearchFolders { get;  set; } = new ObservableCollection<string>();
        #endregion

        public ConfigRepoSearchPathViewModel(string name)
        {
            this.Name = name;
            var existingFolder = config.GetSearchFolders();
            foreach (var folder in existingFolder)
            {
                SearchFolders.Add(folder);
            }
        }
        
        #region Buttons
        public void AddFolder(){
            SearchFolders.Add("");
            Save();
        }

        public async System.Threading.Tasks.Task SelectFolderAsync(string path){
            var indexOf = SearchFolders.IndexOf(path);

            var dlg = new OpenFolderDialog(){
                Title = "Select Folder",
                Directory = path
            };

            var result = await dlg.ShowAsync(new Window());
            if (!string.IsNullOrWhiteSpace(result))
            {
                SearchFolders[indexOf] = result;
            }
            Save();
        }
        public void DeleteFolder(string path){
            SearchFolders.Remove(path);
            Save();
        }
        #endregion

        private void Save(){
            config.SetSearchFolders(SearchFolders);
        }
    }
}