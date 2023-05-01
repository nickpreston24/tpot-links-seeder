using System;
using System.IO;
using System.Linq;

namespace CodeMechanic.Extensions
{
    public static class DirectoryExtensions
    {
        public static DirectoryInfo AsDirectory(this string path) => new DirectoryInfo(path);

        public static string GoUp(this DirectoryInfo dir, int numDirs = 1) =>
         dir.Exists
              ? Path.GetFullPath(Path.Combine(dir.FullName, string.Join(string.Empty, Enumerable.Repeat(@"../", numDirs))))
              : dir.FullName;


        public static string GoUp(this string folderPath, int numDirs = 1) =>
            Directory.Exists(folderPath)
                ? Path.GetFullPath(Path.Combine(folderPath, string.Join(string.Empty, Enumerable.Repeat(@"../", numDirs))))
                : folderPath;


        public static bool IsDirectory(this string path)
            => string.IsNullOrWhiteSpace(Path.GetFileName(path)) || Directory.Exists(path);


        /// <summary>
        /// Handles Directory creation whether a file or dir has been given.
        /// </summary>
        public static string CreateDirectory(this string fullpath)
        {
            bool exists = File.Exists(fullpath) || Directory.Exists(fullpath);

            string possible_dir = exists && fullpath.IsDirectory() ? fullpath : Path.GetDirectoryName(fullpath);

            if (!Directory.Exists(possible_dir))
                Directory.CreateDirectory(possible_dir);

            return fullpath;
        }

        public static string GetRelativePath(this string sourcePath, string targetPath)
        {
            if (!Path.IsPathRooted(sourcePath)) throw new ArgumentException("Path must be absolute", "sourcePath");
            if (!Path.IsPathRooted(targetPath)) throw new ArgumentException("Path must be absolute", "targetPath");

            string[] sourceParts = sourcePath.Split(Path.DirectorySeparatorChar);
            string[] targetParts = targetPath.Split(Path.DirectorySeparatorChar);

            int position;
            for (position = 0; position < Math.Min(sourceParts.Length, targetParts.Length); position++)
            {
                if (!string.Equals(sourceParts[position], targetParts[position], StringComparison.CurrentCultureIgnoreCase))
                {
                    break;
                }
            }

            if (position == 0)
                throw new ApplicationException($"Files must be on the same volume.  " +
                    $"Please update your {nameof(sourcePath)} and ${nameof(targetPath)}");

            string relativePath = new string('.', sourceParts.Length - position)
                .Replace(".", ".." + Path.DirectorySeparatorChar);

            if (position <= targetParts.Length)
            {
                relativePath += string.Join(Path.DirectorySeparatorChar.ToString(), targetParts.Skip(position).ToArray());
            }

            return string.IsNullOrWhiteSpace(relativePath) ? "." : relativePath;
        }

    }
}
