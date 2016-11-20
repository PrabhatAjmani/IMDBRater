using System;
using System.IO;
using System.Configuration;

namespace IMDBMovieRater
{
    public class ModifyDirectoryName
    {
        public string SourceDirectoryPath { get; set; }
        public string DestinationDirectoryPath { get; set; }
        public string TempPath { get; set; }
        public DirectoryInfo SourceDirectory { get; set; }
        public DirectoryInfo TempDirectory { get; set; }
        public DirectoryInfo DestinationDirectory { get; set; }
        private string tempDirectoryName = "TempDirectory";

        public ModifyDirectoryName()
        {
            SourceDirectoryPath = ConfigurationManager.AppSettings["SourceDirectoryPath"];
            DestinationDirectoryPath = ConfigurationManager.AppSettings["DestinationDirectoryPath"];
            TempPath = SourceDirectoryPath + "\\" + tempDirectoryName + "\\";

            SourceDirectory = new DirectoryInfo(SourceDirectoryPath);

            DestinationDirectory = CreateDestinationDirectory();
        }

        private DirectoryInfo CreateTempDirectory()
        {
            // create temp directory to move the folders
            var tempDirectory = new DirectoryInfo(TempPath);

            // if temp path doesn't exist, create one.
            if (!tempDirectory.Exists)
                tempDirectory.Create();

            return tempDirectory;
        }

        private DirectoryInfo CreateDestinationDirectory()
        {
            var destinationDirectory = new DirectoryInfo(DestinationDirectoryPath);

            // if destination path doesn't exists, create one.
            if (!destinationDirectory.Exists)
                destinationDirectory.Create();

            return destinationDirectory;
        }

        internal void Rename(DirectoryInfo directory, string movieRating)
        {
            TempDirectory = CreateTempDirectory();
            RenameDirectory(directory, movieRating, TempDirectory);
        }

        private void RenameDirectory(DirectoryInfo directory, string text, DirectoryInfo tempDirectory)
        {
            try
            {
                // ignore the temporary folder created above.
                if (directory.Name.Equals(tempDirectoryName) || directory.Name.Equals(DestinationDirectory.Name))
                    return;

                // set modified directory name based on the operation to perform (dependent upon theinput received)
                var modifiedDirectoryName = "-" + text + "- " + directory.Name;

                // create the temp path to move at.
                var newTemporaryPath = TempPath + modifiedDirectoryName;

                // move to temporary created folder.
                directory.MoveTo(newTemporaryPath);

                // update the destination path to move it back.
                var destinationPath = DestinationDirectoryPath + "\\" + modifiedDirectoryName;

                // create a new Directory object of moved folder (to it's new position).
                var tempDir = new DirectoryInfo(newTemporaryPath);

                // move it back to the destination folder from temp folder.
                tempDir.MoveTo(destinationPath);

                Console.WriteLine("\"" + directory.Name + "\"" + " renamed.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Folder is not renamed. " + ex.Message);
                tempDirectory.Delete();
            }
            finally
            {
                tempDirectory.Delete();
            }
        }
    }
}
