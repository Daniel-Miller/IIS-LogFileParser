using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IIS
{
    public class Program
    {
        static ProgramOptions _options;

        static void Main(string[] args)
        {
            _options = ProgramOptions.Create(args);
            if (_options == null)
                return;

            var parser = new LogParser();
            parser.Parse(GetLogFiles());

            ExtractSubset(parser.Entries);
        }

        private static void ExtractSubset(List<LogEntry> entries)
        {
            var since = _options.Since;
            var until = since.AddMinutes(_options.Minutes);

            var list = entries
                .Where(x => since <= x.When && x.When <= until);

            if (_options.Status.HasValue)
                list = list.Where(x => x.Status == _options.Status.Value);

            list = list.OrderBy(x => x.When).ToList();

            Console.WriteLine("Site,Date,Time,Method,Url,User,IP,Status,TimeTaken,BytesSent,BytesReceived,Agent,Referer");
            foreach (var item in list)
                Console.WriteLine(item.ToString());
        }

        private static List<LogFile> GetLogFiles()
        {
            var files = new List<LogFile>();
            var logs = Directory.GetFiles(_options.Folder, "*.log", SearchOption.AllDirectories);
            foreach (var log in logs)
                files.Add(new LogFile(log));
            return files;
        }
    }
}
