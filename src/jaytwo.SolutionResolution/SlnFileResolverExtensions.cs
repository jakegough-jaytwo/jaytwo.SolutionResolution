namespace jaytwo.SolutionResolution
{
    public static class SlnFileResolverExtensions
    {
        public static SlnFileResolver WithBasePath(this SlnFileResolver resolver, string basePath)
        {
            resolver.BasePath = basePath;
            return resolver;
        }

        public static SlnFileResolver WithSlnPattern(this SlnFileResolver resolver, string slnPattern)
        {
            resolver.SlnPattern = slnPattern;
            return resolver;
        }
    }
}
