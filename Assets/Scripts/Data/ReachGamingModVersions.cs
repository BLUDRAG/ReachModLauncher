using System;

namespace ReachModLauncher
{
    [Serializable]
    public class ReachGamingModVersions
    {
        public string Version;
        public string Link;

        public override string ToString()
        {
            return Version;
        }
    }
}