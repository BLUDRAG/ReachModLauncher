using System;
using Google.Apis.Drive.v3.Data;

namespace ReachModLauncher
{
    [Serializable]
    public sealed class POIData
    {
        public string User;
        public string Author;
        public File   File;
        public byte[] Preview;
    }
}
