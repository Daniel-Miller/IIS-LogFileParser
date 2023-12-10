namespace IIS
{
    public class LogFile
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string Site { get; set; }

        public LogFile(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileName(path);

            var directory = System.IO.Path.GetDirectoryName(path);
            Site = directory.Substring(1 + directory.LastIndexOf('\\'));
        }
    }
}
