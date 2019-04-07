using System;
using System.IO;

namespace jaytwo.SolutionResolution
{
    public interface ISlnFileResolver
    {
        FileInfo ResolveSln();
        DirectoryInfo ResolveSlnDirectory();
        string ResolvePathRelativeToSln(string path);
    }
}
