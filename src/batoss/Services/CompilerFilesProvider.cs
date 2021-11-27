using batoss.Helpers;
using batoss.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace batoss.Services
{
    internal class CompilerFilesProvider
    {
        private readonly ProfileProvider _profiles;
        private readonly CompilerCommandFactory _commands;
        private readonly ILogger _logger;

        public CompilerFilesProvider(ProfileProvider profiles, CompilerCommandFactory commands, ILogger<CompilerFilesProvider> logger)
        {
            _profiles = profiles;
            _commands = commands;
            _logger = logger;
        }

        public IEnumerable<string> GetCompilationFiles()
        {
            _logger.LogDebug("Collecting files for compilation");

            foreach (string file in Directory.EnumerateFiles("./", "*.*", SearchOption.AllDirectories))
            {
                if (_commands.ForFile(file) == null ||
                    DirectoryHelpers.FilePartOf("/obj", file) || DirectoryHelpers.FilePartOf("/bin", file))
                {
                    _logger.LogDebug("Skipping {file} because there no compiler or in forbidden folder", file);
                    continue;
                }

                bool allowed = false;
                bool disallowed = false;

                foreach (var profile in _profiles.GetProfiles())
                {
                    bool isActive = profile.Name == _profiles.ActiveProfile;

                    if (profile.Directories is null)
                    {
                        continue;
                    }

                    foreach (string directory in profile.Directories)
                    {
                        if (DirectoryHelpers.FilePartOf(directory, file))
                        {
                            if (isActive)
                            {
                                _logger.LogDebug("{file} is allowed", file);
                                allowed = true;
                                break;
                            }
                            else
                            {
                                disallowed = true;
                            }
                        }
                    }

                    if (allowed)
                    {
                        break;
                    }
                }

                if (allowed || !disallowed)
                {
                    _logger.LogDebug("Providing {file} for compilation", file);
                    yield return file;
                }
                else
                {
                    _logger.LogDebug("Skipping {file} because the directory is not a part of the current profile", file);
                }
            }
        }
    }
}
