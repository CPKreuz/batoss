using batoss.Models;
using batoss.Options;
using Microsoft.Extensions.Options;

namespace batoss.Services
{
    internal class CompilerCommandFactory
    {
        private readonly WslOptions _wslOptions;
        private readonly Profile _activeProfile;

        private readonly Dictionary<string, Command> _commands;

        public CompilerCommandFactory(IOptions<WslOptions> projectOptions, IOptions<ProfilesOptions> profiles)
        {
            _wslOptions = projectOptions.Value;
            _activeProfile = profiles.Value.Profiles.First(x => x.Name == profiles.Value.ActiveProfile);

            _commands = new();
            GenerateCommands();
        }

        public Command ForFile(string path)
        {
            string extension = Path.GetExtension(path);
            string filename = Path.GetFileNameWithoutExtension(path);

            if (extension == null)
            {
                _commands.GetValueOrDefault(filename);
            }

            return _commands.GetValueOrDefault($"{filename}{extension}",
                       _commands.GetValueOrDefault($"*{extension}"));
        }

        private void GenerateCommands()
        {
            _commands.Clear();

            foreach (var option in _activeProfile.Compilers)
            {
                Command command = new(option.File, option.Arguments, option.UseWsl, option.UseWsl ? _wslOptions : null, _activeProfile.Arguments);

                foreach (var pattern in option.FilePattern.Split('/'))
                {
                    _commands.Add(pattern, command);
                }
            }
        }
    }
}
