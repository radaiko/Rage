using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Rage.Models;
using Serilog;

namespace Rage.Services
{
    public class Config{

        #region public interface
        public IEnumerable<string> GetSearchFolders() {
            if (configModel.SearchRepoPaths == null)
            {
                return new [] {@"/home/radaiko/src/private", @"/home/radaiko/src/trumpf", @"/Users/radaiko/Documents/GitHub/Rage"};
            }
            return configModel.SearchRepoPaths;
        }

        public bool SetSearchFolders(IEnumerable<string> searchRepoPaths) {
            configModel.SearchRepoPaths = searchRepoPaths;
            return SaveConfigFile();
        }

        public bool SetOpenRepos(IEnumerable<string> openRepos){
            configModel.OpenRepos = openRepos;
            return SaveConfigFile();
        }

        public IEnumerable<string> GetOpenRepos() {
            if (configModel.OpenRepos == null)
            {
                return Enumerable.Empty<string>();
            }
            return configModel.OpenRepos;
        }
        #endregion


        #region parameters
        private ConfigModel configModel = new ConfigModel();
        private string configPath = "settings.json";
        #endregion

        #region constructor
        public Config()
        {
            configModel = ReadConfigFile();
            
        }
        #endregion

        #region private
        private ConfigModel ReadConfigFile(){
            if (!File.Exists(configPath))
            {
                return new ConfigModel();
            } else {
                var configString = File.ReadAllText(configPath);
                return JsonSerializer.Deserialize<ConfigModel>(configString);
            }
        }

        private bool SaveConfigFile(){
            try
            {
                File.WriteAllText(configPath, JsonSerializer.Serialize(configModel));
                return true;
            }
            catch (System.Exception e)
            {
                Log.Error("Error while saving config: " + e);
                return false;
            }


        }
        #endregion
    }
}