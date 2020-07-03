using System;
using System.Diagnostics;
using Rage.Utils.Models;

namespace Rage.Utils
{
    public static class Tools
    {

        public static TransferModel ExecutionProcess(string filename, string args, string workingDirectory){
            switch (System.Runtime.InteropServices.RuntimeInformation.OSDescription.ToString())
            {
                case string a when a.Contains("Windows"):
                    return RunningCommand(filename, args, workingDirectory);
                case string a when a.Contains("OSX"):
                    return RunningCommand(filename, args, workingDirectory);
                case string a when a.Contains("Linux"):
                    return RunningCommand(filename, args, workingDirectory);

                default:
                    return new TransferModel(){ Successful = false, Content = "Unknown OS" };;
            }
            

        }

        public static string[] SplitStringToArray(string inputString, string splitChar) {
            return inputString.Split(splitChar);
        }


        #region private

        private static TransferModel RunningCommand(string filename, string args, string workingDirectory){
            //var escapedArgs = args.Replace("\"", "\\\"");
            
            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = filename,
                    Arguments = args,
                    WorkingDirectory = workingDirectory,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (string.IsNullOrEmpty(error)) { return new TransferModel(){ Successful = true, Content = output }; }
            else { return new TransferModel(){ Successful = false, Content = error }; }
        }

        #endregion
    }
}
