using batoss.Options;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace batoss.Models
{
    internal record Command(string File, string Arguments, bool UseWsl, WslOptions WslOptions, params (string, string)[] profileArguments)
    {
        public int Execute(ILogger logger, params (string key, string value)[] parameters)
        {
            string arguments = Arguments;

            foreach (var (key, value) in parameters)
            {
                arguments = Regex.Replace(arguments, $"{{{key}}}", value, RegexOptions.IgnoreCase);
            }

            if (profileArguments is not null)
            {
                foreach (var (key, value) in profileArguments)
                {
                    arguments = Regex.Replace(arguments, $"{{{key}}}", value, RegexOptions.IgnoreCase);
                }
            }

            string file;
            string toExecute;

            if (UseWsl)
            {
                string directory = WslOptions.Directory != null ? $"--cd \"{WslOptions.Directory}\" " : string.Empty;
                string distro = WslOptions.Distro != null ? $"--distribution \"{WslOptions.Distro}\" " : string.Empty;
                string user = WslOptions.User != null ? $"--user \"{WslOptions.User}\" " : string.Empty;

                file = "wsl";
                toExecute = $"{directory}{distro}{user}{File} {arguments}";
            }
            else
            {
                file = File;
                toExecute = arguments;
            }

            logger?.LogTrace("Executing: {file} {arguments}", file, toExecute);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Process process = Process.Start(file, toExecute);
            process.WaitForExit();
            int exitCode = process.ExitCode;

            process.Dispose();

            Console.ResetColor();
            return exitCode;
        }
    }
}
