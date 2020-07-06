using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Rage.Models;
using Serilog;

namespace Rage.Services
{
    public class Config{

        #region Properties
        private ConfigModel _configModel;
        private ConfigModel ConfigModel
        {
            get { 
                _configModel = ReadConfigFile();
                return _configModel; 
                }
            set { 
                _configModel = value;
                SaveConfigFile();
                }
        }
        
        #endregion

        
        #region public interface
        // * ---- GET ----
        // ! always set whole configModel to this.ConfigModel (autoSave)
        public IEnumerable<string> GetSearchFolders() {
            return ConfigModel.SearchRepoPaths;
        }
        public IEnumerable<string> GetOpenRepos() {
            if (ConfigModel.OpenRepos == null)
            {
                return Enumerable.Empty<string>();
            }
            return ConfigModel.OpenRepos;
        }
        // * ------------------------------------------------------
        
        // * ---- SET -----
        // ! always set whole configModel to this.ConfigModel (autoSave)
        public void SetSearchFolders(IEnumerable<string> searchRepoPaths) {
            ConfigModel configModel = _configModel;
            configModel.SearchRepoPaths = searchRepoPaths;
            this.ConfigModel = configModel;
        }

        public void SetOpenRepos(IEnumerable<string> openRepos){
            ConfigModel configModel = _configModel;
            configModel.OpenRepos = openRepos;
            this.ConfigModel = configModel;
        }
        // * ------------------------------------------------------

        #endregion


        #region parameters
        private string configPath = "settings.json";
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
                File.WriteAllText(configPath, JsonSerializer.Serialize(this._configModel));
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