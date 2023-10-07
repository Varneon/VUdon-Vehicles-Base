using System;
using System.IO;
using UnityEngine;

namespace Varneon.VUdon.VehiclesBase.Editor.Utilities
{
    public static class PathUtility
    {
        /// <summary>
        /// Converts full path to one relative to the project
        /// </summary>
        /// <param name="path">Full path pointing inside the project</param>
        /// <returns>Path relative to the project</returns>
        /// <exception cref="ArgumentException"/>
        public static string ConvertToRelativePath(string path)
        {
            // If string is null or empty, throw an exception
            if (string.IsNullOrEmpty(path)) { throw new ArgumentException("Invalid path!", nameof(path)); }

            // Get the 'Assets' directory
            string assetsDirectory = Application.dataPath;

            // Get the project's root directory (Trim 'Assets' from the end of the path)
            string projectDirectory = assetsDirectory.Substring(0, assetsDirectory.Length - 6);

            // Ensure that the path is the full path
            path = Path.GetFullPath(path);

            // Replace backslashes with forward slashes
            path = path.Replace('\\', '/');

            // If path doesn't point inside the project, throw an exception
            if (!path.StartsWith(projectDirectory)) { throw new ArgumentException("Path is not located in this project!", nameof(path)); }

            // Return a path relative to the project
            return path.Replace(projectDirectory, string.Empty);
        }
    }
}