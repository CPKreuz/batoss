namespace batoss.Helpers
{
    internal static class PathHelpers
    {
        public static string GetAppDataPath(params string[] pathes)
        {
            string[] paths = new string[pathes.Length + 2];
            paths[0] = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            paths[1] = "batoss";

            for (int i = 0; i < pathes.Length; i++)
            {
                paths[i + 2] = pathes[i];
            }

            return Path.Combine(paths);
        }

        public static string ToCorrectPath(string path, bool useWsl)
        {
            if (useWsl && OperatingSystem.IsWindows())
            {
                return ToWslPath(path);
            }

            return path;
        }

        private static string ToWslPath(string path)
        {
            string fullPath = Path.GetFullPath(path);
            string drive = Path.GetPathRoot(fullPath);
            char driveLetter = drive[0];

            return $"/mnt/{char.ToLower(driveLetter)}/{fullPath[drive.Length..].Replace('\\', '/')}";
        }
    }
}
