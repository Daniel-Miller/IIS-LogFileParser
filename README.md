# Microsoft IIS Log File Parser

This is a simple console application that parses a directory containing Microsoft IIS log files. 

## Usage

`LogFileParser Folder Since Minutes [Status]`

* Folder = Fully qualified path to the directory on your file system where log files are stored.
* Since = Starting date and time (in your local time zone) for the log entries you want to extract from all log files.
* Minutes = Duration for the period you want to extract.
* Status (optional) = HTTP status code to filter the output.

**PowerShell Example:**

`> .\LogFileParser.exe 2023-12-08-1750 45 500`
