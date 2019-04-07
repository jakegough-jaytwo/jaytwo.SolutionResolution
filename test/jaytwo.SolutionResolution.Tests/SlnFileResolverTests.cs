using System;
using System.IO;
using Xunit;

namespace jaytwo.SolutionResolution.Tests
{
    public class SlnFileResolverTests
    {
        [Theory]
        [InlineData("TestData/one/a/b", "foo.txt")]
        [InlineData("TestData/one/a/b", "./foo.txt")]
        [InlineData("TestData/one/a/b", @".\foo.txt")]
        [InlineData("TestData/one/a", "foo.txt")]
        [InlineData("TestData/one/a/b", "b/bar.txt")]
        [InlineData("TestData/one/a/b", "./b/bar.txt")]
        [InlineData("TestData/one/a", "b/bar.txt")]
        [InlineData("TestData/one/a/b", "../../two/baz.txt")]
        public void ResolvePathRelativeToSln_works(string basePath, string relativePath)
        {
            // arrange
            var resolver = new SlnFileResolver().WithBasePath(basePath);

            // act
            var fullPath = resolver.ResolvePathRelativeToSln(relativePath);

            // assert
            Assert.NotNull(fullPath);
            Assert.NotEmpty(fullPath);
            Assert.True(File.Exists(fullPath), $"File does not exist: {fullPath}");
        }

        [Fact]
        public void ResolvePathRelativeToSln_works_with_different_sln_pattern()
        {
            // arrange
            var slnPattern = "one.sln";
            var basePath = "TestData/one/a/b";
            var relativePath = "a/foo.txt";
            var resolver = new SlnFileResolver().WithBasePath(basePath).WithSlnPattern(slnPattern);

            // act
            var fullPath = resolver.ResolvePathRelativeToSln(relativePath);

            // assert
            Assert.NotNull(fullPath);
            Assert.NotEmpty(fullPath);
            Assert.True(File.Exists(fullPath), $"File does not exist: {fullPath}");
        }

        [Fact]
        public void ResolvePathRelativeToSln_works_defaults()
        {
            // arrange
            var relativePath = "README.md";
            var resolver = new SlnFileResolver();

            // act
            var fullPath = resolver.ResolvePathRelativeToSln(relativePath);

            // assert
            Assert.NotNull(fullPath);
            Assert.NotEmpty(fullPath);
            Assert.True(File.Exists(fullPath), $"File does not exist: {fullPath}");
        }

        [Fact]
        public void ResolveSln_throws_exception_if_no_sln_found()
        {
            // arrange
            var rootDirectory = new DirectoryInfo(Directory.GetCurrentDirectory()).Root.FullName; // better not be a .sln file in the root
            var resolver = new SlnFileResolver(rootDirectory, null);

            // act & assert
            var exception = Assert.Throws<InvalidOperationException>(() => resolver.ResolveSln());

            Assert.Contains("Could not find solution file", exception.Message);
        }
    }
}
