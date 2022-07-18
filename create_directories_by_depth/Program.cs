using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace create_directories_by_depth
{
    class Program
    {
        static void Main(string[] args)
        {
            // The Copy-From source is mocked in the output directory.
            var source = AppDomain.CurrentDomain.BaseDirectory;

            // The Copy-To destination is mocked in local app data for this app
            var destination = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "create_directories_by_depth"
            );

            // Making sure the list of directories (whatever the source) is sorted by depth
            var directoryLevels = 
                Directory
                .GetDirectories(source, String.Empty, SearchOption.AllDirectories)
                .Select(path=>path.Replace(source, String.Empty))
                .GroupBy(path=>path.Split(Path.DirectorySeparatorChar).Length)
                .OrderBy(group=>group.Key);

            // Ensure that the top-level folder exists.
            Directory.CreateDirectory(destination);

            foreach (var directoryLevel in directoryLevels)
            {
                var shortPaths = directoryLevel.ToArray();
                foreach (
                    var folder 
                    in shortPaths.Select(shortPath=>Path.Combine(destination, shortPath)))
                {
                    var parse = folder.Split(Path.DirectorySeparatorChar).ToList();
                    parse.Remove(parse.Last());
                    var parent = Path.Combine(parse.ToArray());
                    // MAKE THE CALL
                    CreateDirectory(folder, parent);
                }
            }
            foreach (var directory in Directory.GetDirectories(destination, String.Empty, SearchOption.AllDirectories))
            {
                Console.WriteLine(directory);
            }
            Process.Start("explorer.exe", destination);
        }

        // No access to source code
        // Creates a directory if it finds its parent
        private static void CreateDirectory(string folder, string parent)
        {
            if (Directory.Exists(parent))
            {
                Directory.CreateDirectory(folder);
            }
            else Debug.Assert(false, "Expecting to find existing parent!");
        }
    }
}
