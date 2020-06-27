using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Rage.Models;

namespace Rage.Services
{
    public class Config{

        // public interface
        public IEnumerable<string> GetSearchFolders() {
            if (configModel.SearchRepoPaths == null)
            {
                return new [] {@"/home/radaiko/src"};
            }
            return configModel.SearchRepoPaths;
        }

        public bool SetSearchFolders(IEnumerable<string> searchRepoPaths) {
            configModel.SearchRepoPaths = searchRepoPaths;
            return SaveConfigFile();
        }

        // parameters
        private ConfigModel configModel = new ConfigModel();
        private string configPath = "settings.config";


        // constructor
        public Config()
        {
            configModel = ReadConfigFile();
            
        }

        // internal
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
            catch (System.Exception)
            {
                // TODO: LOG
                return false;
            }


        }
    }
}