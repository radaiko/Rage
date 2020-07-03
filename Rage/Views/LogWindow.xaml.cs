using System;
using System.Collections.ObjectModel;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Rage.Services;
using Serilog.Events;

namespace Rage.Views
{
    public class LogWindow : Window
    {
        public LogWindow()
        {
            InitializeComponent();

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }


    }
}