using batoss.Helpers;
using batoss.Models;
using Microsoft.Extensions.Logging;

namespace batoss.Services
{
    internal class CompilerService
    {
        private readonly CompilerFilesProvider _filesProvider;
        private readonly CompilerCommandFactory _commandFactory;
        private readonly ILogger _logger;

        public CompilerService(CompilerFilesProvider filesProvider, CompilerCommandFactory commandFactory, ILogger<CompilerService> logger)
        {
            _filesProvider = filesProvider;
            _commandFactory = commandFactory;
            _logger = logger;
        }

        public bool Compile()
        {
            _logger.LogInformation(" -<|>- Starting Compiler -<|>- ");

            foreach (string file in _filesProvider.GetCompilationFiles())
            {
                Command command = _commandFactory.ForFile(file);
                string input = PathHelpers.ToCorrectPath(file, command.UseWsl);
                string output = Path.ChangeExtension(ToCorrectPath(out string hostPath), ".o");

                _logger.LogInformation("Compiling file {file} to {target}", input, output);

                int code = command.Execute(_logger, ("in", input), ("out", output));

                if (code != 0)
                {
                    _logger.LogError("{name} returned {code}", command.File, code);
                    return false;
                }

                string ToCorrectPath(out string hostPath)
                {
                    hostPath = ToObjFile(file);
                    return PathHelpers.ToCorrectPath(hostPath, command.UseWsl);
                }
            }

            return true;
        }

        private static string ToObjFile(string file)
        {
            string relativePath = Path.GetRelativePath("./", file);
            relativePath = relativePath.Replace('\\', '_');

            return Path.Combine("obj", "_comp", relativePath);
        }
    }
}
