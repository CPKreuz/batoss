namespace batoss.Options
{
    internal class ProfilesOptions
    {
        private Profile activeProfile;

        public string ActiveProfile { get; set; }

        public Profile[] Profiles { get; set; }

        public Profile GetActive()
        {
            if (activeProfile == null)
            {
                activeProfile = Profiles.First(x => x.Name == ActiveProfile);
            }

            return activeProfile;
        }
    }
}
