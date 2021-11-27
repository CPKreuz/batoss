namespace batoss.Helpers
{
    internal static class DirectoryHelpers
    {
        public static void Clean(string directory)
        {
            DirectoryInfo info = new(directory);

            foreach (FileInfo file in info.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in info.GetDirectories())
            {
                dir.Delete(true);
            }
        }

        /// <summary>
        /// Checks if the <paramref name="file"/> is part the <paramref name="info"/>
        /// </summary>
        public static bool FilePartOf(this DirectoryInfo info, string file)
        {
            return Path.GetFullPath(file).Contains(info.FullName);
        }

        /// <summary>
        /// Checks if the <paramref name="file"/> is part the <paramref name="direcotry"/>
        /// </summary>
        public static bool FilePartOf(string directory, string file)
        {
            return Path.GetFullPath(file).StartsWith(Path.GetFullPath(directory));
        }
    }
}
