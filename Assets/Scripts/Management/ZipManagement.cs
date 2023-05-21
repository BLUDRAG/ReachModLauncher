using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace ReachModLauncher
{
    public static class ZipManagement
    {
        public static Stream CreatePOIZipFile(params IList<string>[] filesList)
        {
            MemoryStream     stream  = new MemoryStream();
            using ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Create, true);

            foreach(IList<string> files in filesList)
            {
                AddFilesToArchive(archive, files);
            }

            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        private static void AddFilesToArchive(ZipArchive archive, IList<string> files)
        {
            foreach(string file in files)
            {
                if(string.IsNullOrEmpty(file)) continue;

                archive.CreateEntryFromFile(file, Path.GetFileName(file));
            }
        }
    }
}
