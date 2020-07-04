using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Timers;
using Rage.Services;
using Serilog;
using Serilog.Events;

namespace Rage.ViewModels
{

    public class LogWindowViewModel : ViewModelBase
    {
        private Timer _timer;
        public ObservableCollection<LogEvent> LogItems { get; set;} = new ObservableCollection<LogEvent>(LogModel.LogItems.Reverse());

        public LogWindowViewModel()
        {
            Log.Information("Log Window opened.");
            //TODO: filter 
        }
    }
}