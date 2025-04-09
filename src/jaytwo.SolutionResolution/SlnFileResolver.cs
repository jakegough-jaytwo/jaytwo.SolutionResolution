using System;
using System.IO;
using System.Linq;

namespace jaytwo.SolutionResolution;

public class SlnFileResolver : ISlnFileResolver
{
    private const string DefaultSlnPattern = "*.sln";

    private static Lazy<SlnFileResolver> _lazyDefault = new Lazy<SlnFileResolver>(() => new SlnFileResolver());

    public SlnFileResolver()
        : this(null, null)
    {
    }

    public SlnFileResolver(string? basePath, string? slnPattern = DefaultSlnPattern)
    {
        BasePath = basePath ?? Directory.GetCurrentDirectory();
        SlnPattern = slnPattern ?? DefaultSlnPattern;
    }

    public static SlnFileResolver Default => _lazyDefault.Value;

    public string BasePath { get; }

    public string SlnPattern { get; }

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

    public DirectoryInfo ResolveSlnDirectory() => ResolveSln().Directory ?? throw new Exception("Solution file is not in a direcotry!");

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
