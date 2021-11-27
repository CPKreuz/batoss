namespace batoss.Options
{
    internal class Profile
    {
        public string Name { get; set; }

        public CompilerOptions[] Compilers { get; set; }

        public string[] Directories { get; set; }

        public (string key, string value)[] Arguments { get; set; }

        public LinkerOptions Linker { get; set; }
    }
}
