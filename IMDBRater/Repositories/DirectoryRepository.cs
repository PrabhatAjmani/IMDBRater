using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;

namespace IMDBMovieRater
{
    /// <summary>
    /// Reporsitory for all Directory related actions like retrival of directory names , renaming directory names. 
    /// </summary>
    public class DirectoryRepository
    {
        private string sourceDirectoryPath;

        public DirectoryRepository()
        {
            sourceDirectoryPath = ConfigurationManager.AppSettings["SourceDirectoryPath"];
        }

        /// <summary>
        /// Get the list of all folders name present in the source directory path.
        /// </summary>
        /// <returns></returns>
        internal List<DirectoryInfo> GetDirectoryList()
        {
            
            if (!Directory.Exists(sourceDirectoryPath))
                throw new DirectoryNotFoundException("Folder not found.");

            var parentDirectoryInfo = new DirectoryInfo(sourceDirectoryPath);

            var directoryList = parentDirectoryInfo.EnumerateDirectories();

            return (directoryList.Any()) ? directoryList.ToList() : null;
        }
    }
}
