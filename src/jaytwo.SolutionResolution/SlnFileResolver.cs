using System;
using System.IO;
using System.Linq;

namespace jaytwo.SolutionResolution
{
    public class SlnFileResolver : ISlnFileResolver
    {
        public SlnFileResolver()
            : this(null, null)
        {
        }

        public SlnFileResolver(string basePath, string slnPattern)
        {
            BasePath = basePath ?? Directory.GetCurrentDirectory();
            SlnPattern = slnPattern ?? "*.sln";
        }

        public string BasePath { get; set; }

        public string SlnPattern { get; set; }

        public FileInfo ResolveSln()
        {
            var directoryCursor = new DirectoryInfo(BasePath);

            do
            {
                var slnFiles = directoryCursor.GetFiles(SlnPattern, SearchOption.TopDirectoryOnly);

                if (slnFiles.Any())
                {
                    return slnFiles.First();
                }
            }
            while ((directoryCursor = directoryCursor.Parent) != null);

            throw new InvalidOperationException("Could not find solution file!");
        }

        public DirectoryInfo ResolveSlnDirectory() => ResolveSln().Directory;

        public string ResolvePathRelativeToSln(string path)
        {
            if (Path.DirectorySeparatorChar != '/')
            {
                path = path.Replace('/', Path.DirectorySeparatorChar);
            }

            if (Path.DirectorySeparatorChar != '\\')
            {
                path = path.Replace('\\', Path.DirectorySeparatorChar);
            }

            var slnDirectory = ResolveSlnDirectory();
            var resultPath = Path.Combine(slnDirectory.FullName, path);
            var result = Path.GetFullPath(resultPath); // normalize './', '../', etc.
            return result;
        }
    }
}
