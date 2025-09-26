using System.IO;

namespace MirraGames.SDK.Editor {

    internal static class PathExtensions {

        public static string NormalizePath(this string path) {
            path = path.Replace('/', Path.AltDirectorySeparatorChar);
            path = path.Replace('\\', Path.AltDirectorySeparatorChar);
            return path;
        }

    }

}