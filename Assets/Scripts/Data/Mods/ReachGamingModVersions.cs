using System;

namespace ReachModLauncher
{
    [Serializable]
    public sealed class ReachGamingModVersions
    {
        public string Version;
        public string Link;

        public override string ToString()
        {
            return Version;
        }
    }
}