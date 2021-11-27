using batoss.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace batoss.Services
{
    internal class ProfileProvider
    {
        private readonly ProfilesOptions _profiles;

        public string ActiveProfile { get; set; }

        public ProfileProvider(IOptions<ProfilesOptions> profiles)
        {
            _profiles = profiles.Value;
            ActiveProfile = _profiles.ActiveProfile;
        }

        public Profile GetCurrentProfile()
        {
            return _profiles.Profiles.First(x => x.Name == ActiveProfile);
        }

        public IEnumerable<Profile> GetProfiles()
        {
            return _profiles.Profiles;
        }
    }
}
