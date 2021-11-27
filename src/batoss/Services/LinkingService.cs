using batoss.Helpers;
using batoss.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace batoss.Services
{
    internal class LinkingService
    {
        private readonly ProfileProvider _profileProvider;
        private readonly WslOptions _wslOptions;
        private readonly ILogger _logger;
        private readonly CommandFactory _commandFactory;

        public LinkingService(ProfileProvider profileProvider, CommandFactory factory, IOptions<WslOptions> wslOptions, ILogger<LinkingService> logger)
        {
            _profileProvider = profileProvider;
            _wslOptions = wslOptions.Value;
            _logger = logger;
            _commandFactory = factory;
        }

        public bool Link()
        {
            _logger.LogInformation(" -<|>-  Starting Linker  -<|>- ");

            Profile profile = _profileProvider.GetCurrentProfile();
            LinkerOptions options = profile.Linker;

            var command = _commandFactory.Generate(options.File, options.Arguments, options.UseWsl);

            string inputFiles = PathHelpers.ToCorrectPath(Path.Combine("obj", "_comp", "**"), options.UseWsl);
            string outputFiles = PathHelpers.ToCorrectPath(Path.Combine("bin", $"{profile.Name}.bin"), options.UseWsl);
            string linkerFile = PathHelpers.ToCorrectPath(profile.Linker.LinkerFile, options.UseWsl);

            command.Execute(_logger,
                ("linker", linkerFile),
                ("in", inputFiles), 
                ("out", outputFiles));

            return true;
        }
    }
}
