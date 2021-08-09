using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace OneDrivePhotoOrganizer
{
    internal class Program
    {
        private static int Main(string[] arguments)
        {
            var folderPath = string.Empty;

            if (arguments.Length == 0) return -1;
            if (arguments.Any(
                argument => !StartupArgumentsParser.ProcessDirectoryPathArgument(argument, out folderPath)))
            {
                Console.WriteLine("Wrong arguments, The application only accepts:" +
                                  $"{Environment.NewLine}{StartupArgumentsParser.DirectoryPath}{Environment.NewLine}");
                return -1;
            }

            if (!Directory.Exists(folderPath)) return -1;

            var photoOrganizer = new PhotoOrganizer();
            photoOrganizer.AddDatePattern(new DatePattern(@"\d{4}_\d{2}_\d{2}", "yyyy_MM_dd"));
            photoOrganizer.AddDatePattern(new DatePattern(@"\d{4}-\d{2}-\d{2}", "yyyy-MM-dd"));
            photoOrganizer.AddDatePattern(new DatePattern(@"\d{8}", "yyyyMMdd"));
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var processedFilesCount = photoOrganizer.OrganizeFiles(folderPath);
            stopwatch.Stop();
            Console.WriteLine($"Done, {processedFilesCount} files in {stopwatch.ElapsedMilliseconds} ms");
            Console.ReadLine();

            return 0;
        }
    }
}