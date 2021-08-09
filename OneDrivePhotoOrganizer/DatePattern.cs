namespace OneDrivePhotoOrganizer
{
    public readonly struct DatePattern
    {
        public string RegexFormat { get; }
        public string ParseFormat { get; }

        public DatePattern(string regexFormat, string parseFormat)
        {
            RegexFormat = regexFormat;
            ParseFormat = parseFormat;
        }
    }
}