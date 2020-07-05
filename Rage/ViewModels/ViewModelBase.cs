using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace Rage.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public string Name { get; set; }
    }
}
