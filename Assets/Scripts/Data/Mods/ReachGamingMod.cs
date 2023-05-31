using System;
using System.Collections.Generic;

namespace ReachModLauncher
{
    [Serializable]
    public sealed class ReachGamingMod
    {
        public string                       Name;
        public string                       Description;
        public List<ReachGamingModVersions> Mods = new List<ReachGamingModVersions>();
    }
}