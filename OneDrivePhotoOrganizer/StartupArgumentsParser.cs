using System.IO;

namespace OneDrivePhotoOrganizer
{
    public static class StartupArgumentsParser
    {
        #region Constants

        /// <summary>
        ///     Command line argument to run the Directory Path.
        /// </summary>
        public const string DirectoryPath = "-path:";

        #endregion


        /// <summary>
        ///     Check if program argument is CmdArgRunConfig and process it.
        /// </summary>
        /// <param name="argument">The passed argument value.</param>
        /// <param name="directoryPath">True if app runs in Config mode, else False.</param>
        /// <returns>True if argument processed otherwise False.</returns>
        public static bool ProcessDirectoryPathArgument(string argument, out string directoryPath)
        {
            var isProcessed = false;
            directoryPath = string.Empty;
            if (argument.Length > DirectoryPath.Length)
            {
                directoryPath = argument.Remove(0, DirectoryPath.Length);
                if (!string.IsNullOrEmpty(directoryPath) && Directory.Exists(directoryPath)) isProcessed = true;
            }

            return isProcessed;
        }
    }
}