using batoss.Models;
using batoss.Options;
using Microsoft.Extensions.Options;

namespace batoss.Services
{
    internal class CommandFactory
    {
        private readonly WslOptions _wslOptions;

        public CommandFactory(IOptions<WslOptions> options)
        {
            _wslOptions = options.Value;
        }

        public Command Generate(string file, string arguments, bool useWsl)
        {
            return new(file, arguments, useWsl, _wslOptions);
        }
    }
}
