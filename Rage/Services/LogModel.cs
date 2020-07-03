using System.Collections.ObjectModel;
using Serilog.Events;

namespace Rage.Services
{
    public static class LogModel{

        public static ObservableCollection<LogEvent> LogItems { get; set; } = new ObservableCollection<LogEvent>();
    }
}