using System.Collections.Generic;

namespace Rage.Models
{
    public class ConfigModel {
        public IEnumerable<string> SearchRepoPaths { get; set; }
        public IEnumerable<string> OpenRepos {get; set;}
    }
}