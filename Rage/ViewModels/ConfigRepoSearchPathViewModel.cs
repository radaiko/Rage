using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Rage.Models;
using Rage.Services;
using Rage.Utils.Models;
using ReactiveUI;
using Serilog;

namespace Rage.ViewModels
{

    public class ConfigRepoSearchPathViewModel : ViewModelBase
    {
        public ConfigRepoSearchPathViewModel(string name)
        {
            this.Name = name;
        }
    }
}