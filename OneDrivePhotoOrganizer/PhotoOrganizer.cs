using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace OneDrivePhotoOrganizer
{
    public class PhotoOrganizer
    {
        private readonly Regex _colonRegexew = new(":", RegexOptions.Compiled);
        private readonly List<DatePattern> _datePatterns;

        public PhotoOrganizer()
        {
            _datePatterns = new List<DatePattern>();
        }

        private DateTime? GetDateTakenFromImage(string path, bool ignoreExceptions = true)
        {
            try
            {
                using var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                using var imageFile = Image.FromStream(fileStream, false, false);
               
                if (imageFile.PropertyItems.All(item => item.Id != 36867)) return null;
                var propItem = imageFile.GetPropertyItem(36867);
                if (propItem is { Value: { } })
                {
                    var dateTaken = _colonRegexew.Replace(Encoding.UTF8.GetString(propItem.Value), "-", 2);
                    return DateTime.Parse(dateTaken);
                }
            }
            catch (Exception ex)
            {
                if (ignoreExceptions)
                    Console.WriteLine(ex);
                else
                    throw;
            }

            return null;
        }

        private DateTime? GetDateTakenFromImageName(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);

            foreach (var datePattern in _datePatterns)
            {
                var regex = new Regex(datePattern.RegexFormat);
                foreach (Match m in regex.Matches(fileName))
                    if (DateTime.TryParseExact(m.Value, datePattern.ParseFormat, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var dateTaken))
                        return dateTaken;
            }

            return null;
        }

        private string GetNewDirectoryPath(string currentDirectoryPath, DateTime dateTaken)
        {
            return $@"{currentDirectoryPath}\{dateTaken.Year}\{dateTaken.Month:D2}\";
        }

        private void CreateDirectories(string directoryPath)
        {
            if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        }

        public int OrganizeFiles(string folderPath, bool ignoreExceptions = true)
        {
            var processedFilesCount = 0;
            foreach (var filePath in Directory.GetFiles(folderPath))
                try
                {
                    var dateTaken = GetDateTakenFromImage(filePath) ?? GetDateTakenFromImageName(filePath);
                    if (dateTaken is null) throw new ArgumentException("Cannot extract date from file.");

                    var newDirectoryPath = GetNewDirectoryPath(folderPath, dateTaken.Value);
                    CreateDirectories(newDirectoryPath);
                    File.Move(filePath, newDirectoryPath + Path.GetFileName(filePath));
                    processedFilesCount++;
                }
                catch (Exception ex)
                {
                    if (ignoreExceptions)
                        Console.WriteLine(ex);
                    else
                        throw;
                }

            return processedFilesCount;
        }

        public void AddDatePattern(DatePattern datePattern)
        {
            _datePatterns.Add(datePattern);
        }
    }
}