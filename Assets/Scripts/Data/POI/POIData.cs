using System;
using Google.Apis.Drive.v3.Data;

namespace ReachModLauncher
{
    [Serializable]
    public class POIData
    {
        public string User;
        public File   File;
        public byte[] Preview;
    }
}
