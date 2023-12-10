using System;
using System.Globalization;
using System.IO;

namespace IIS
{
    public class ProgramOptions
    {
        public string Folder { get; set; }
        public DateTimeOffset Since { get; set; }
        public int Minutes { get; set; }
        public int? Status { get; set; }

        public static ProgramOptions Create(string[] args)
        {
            if (args.Length == 3 || args.Length == 4)
            {
                var options = new ProgramOptions();
                options.Folder = args[0];

                if (!Directory.Exists(options.Folder))
                {
                    Console.WriteLine("Directory not found: " + options.Folder);
                    return null;
                }

                if (!DateTime.TryParseExact(args[1], "yyyy-MM-dd-HHmm", CultureInfo.CurrentCulture, DateTimeStyles.None, out DateTime since))
                {
                    Console.WriteLine("Use this format for Since: yyyy-MM-dd-HHmm");
                    return null;
                }
                options.Since = since;

                if (!int.TryParse(args[2], out int minutes))
                {
                    Console.WriteLine("Use an integer for Minutes");
                    return null;
                }
                options.Minutes = minutes;

                if (args.Length == 4)
                {
                    if (!int.TryParse(args[3], out int status))
                    {
                        Console.WriteLine("Use an integer for Status");
                        return null;
                    }
                    options.Status = status;
                }

                return options;
            }

            Console.WriteLine("Usage: LogFileParser Path Since Minutes [Status]");
            return null;
        }
    }
}
