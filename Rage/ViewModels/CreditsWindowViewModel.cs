using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reactive.Linq;
using System.Timers;
using Rage.Models;
using Rage.Services;
using Serilog;
using Serilog.Events;

namespace Rage.ViewModels
{

    public class CreditsWindowViewModel : ViewModelBase
    {
        private string nugetUrl = @"https://www.nuget.org/packages/";
        private string packageFile = @"package.txt";
        
        public ObservableCollection<Credit> Credits { get; set; } = new ObservableCollection<Credit>();

        public CreditsWindowViewModel()
        {
            var lines = File.ReadAllLines(packageFile);
            Log.Debug("Read Package File");
            foreach (var line in lines)
            {
                if (line.StartsWith("   > "))
                {
                    var newLine = line.Substring(5);
                    var name = newLine.Split(" ",2)[0];
                    var path = nugetUrl + name;
                    var version = new Version(); // TODO: extract version
                    var license = ExtractLicenseUrlFromNugetSite(path);
                    Credit credit = new Credit(){
                        Name = name,
                        Path = path,
                        Version = version,
                        License = license
                    };                 
                    Credits.Add(credit);
                }

            }

        }

        private string ExtractLicenseUrlFromNugetSite(string url) {
            var nugetUrlContent = (new WebClient()).DownloadString(url);
            var licenseUrl = nugetUrlContent.Split("ms-Icon ms-Icon--Certificate")[1].Split("<a href=\"")[1].Split("\"")[0];  
            var licenseContent = (new WebClient()).DownloadString(licenseUrl);
            return licenseContent;

        }
    }
}