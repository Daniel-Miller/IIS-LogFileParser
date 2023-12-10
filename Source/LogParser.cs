using System;
using System.Collections.Generic;

namespace IIS
{
    public class LogParser
    {
        private int _patternNumber = 0;

        public List<LogEntry> Entries { get; set; } = new List<LogEntry>();

        public void Parse(List<LogFile> files)
        {
            foreach (var file in files)
            {
                var lines = System.IO.File.ReadAllLines(file.Path);
                for (var i = 0; i < lines.Length; i++)
                    Parse(file.Site, lines[i]);
            }
        }

        private void Parse(string site, string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            if (line.StartsWith("#"))
            {
                if (line.StartsWith("#Fields:"))
                {
                    if (line == "#Fields: date time s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs(User-Agent) cs(Referer) sc-status sc-substatus sc-win32-status time-taken")
                        _patternNumber = 1;
                    else if (line == "#Fields: date time s-ip cs-method cs-uri-stem cs-uri-query s-port cs-username c-ip cs(User-Agent) cs(Referer) cs-host sc-status sc-substatus sc-win32-status sc-bytes cs-bytes time-taken")
                        _patternNumber = 2;
                    else
                        throw new Exception("Unexpected Fields Pattern: " + line);
                }
                return;
            }

            var parts = line.Split(' ');

            var entry = new LogEntry
            {
                Site = site,
                Date = parts[0],
                Time = parts[1]
            };

            if (DateTime.TryParseExact(entry.Date + " " + entry.Time, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture, System.Globalization.DateTimeStyles.None, out DateTime d))
            {
                entry.When = ConvertFromUtcToLocal(d);
                entry.Date = entry.When.ToString("yyyy-MM-dd");
                entry.Time = entry.When.ToString("HH:mm:ss");

                if (_patternNumber == 1)
                {
                    entry.Method = parts[3];
                    if (!string.IsNullOrWhiteSpace(parts[5]) && parts[5] != "-")
                        entry.Url = $"{parts[4]}?{parts[5]}";
                    else
                        entry.Url = $"{parts[4]}";
                    entry.User = parts[7];
                    entry.IP = parts[8];
                    entry.Agent = parts[9];
                    entry.Referer = parts[10];
                    entry.Status = int.Parse(parts[11]);
                    entry.TimeTaken = int.Parse(parts[14]);
                    entry.IsParsed = true;
                }
                else if (_patternNumber == 2)
                {
                    entry.Method = parts[3];
                    if (!string.IsNullOrWhiteSpace(parts[5]) && parts[5] != "-")
                        entry.Url = $"{parts[11]}{parts[4]}?{parts[5]}";
                    else
                        entry.Url = $"{parts[11]}{parts[4]}";
                    entry.User = parts[7];
                    entry.IP = parts[8];
                    entry.Agent = parts[9];
                    entry.Referer = parts[10];
                    entry.Status = int.Parse(parts[12]);
                    entry.BytesSent = int.Parse(parts[15]);
                    entry.BytesReceived = int.Parse(parts[16]);
                    entry.TimeTaken = int.Parse(parts[17]);
                    entry.IsParsed = true;
                }
                else
                {
                    throw new Exception("Unexpected Line " + line);
                }

                if (entry.IsParsed)
                    Entries.Add(entry);
            }
        }

        private DateTimeOffset ConvertFromUtcToLocal(DateTime d)
        {
            if (d.Kind == DateTimeKind.Unspecified)
            {
                d = DateTime.SpecifyKind(d, DateTimeKind.Utc);
            }

            DateTime localDateTime = d.ToLocalTime();

            return localDateTime;
        }
    }
}
