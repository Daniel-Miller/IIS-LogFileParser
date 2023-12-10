using System;

namespace IIS
{
    public class LogEntry
    {
        public DateTimeOffset When { get; set; }
        
        public bool IsParsed { get; set; }

        public string Site { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public string Method { get; set; }
        public string Url { get; set; }
        public string User { get; set; }
        public string IP { get; set; }
        public int Status { get; set; }
        public int TimeTaken { get; set; }
        public int BytesSent { get; set; }
        public int BytesReceived { get; set; }
        public string Agent { get; set; }
        public string Referer { get; set; }

        public override string ToString()
        {
            return string.Format("{0},{1},{2},{3},\"{4}\",{5},{6},{7},{8},{9},{10},{11},{12}", Site, Date, Time, Method, Url, User, IP, Status, TimeTaken, BytesSent, BytesReceived, Agent, Referer);
        }
    }
}
