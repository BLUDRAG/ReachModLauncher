using System;
using System.Collections.Generic;

namespace ReachModLauncher
{
    [Serializable]
    public sealed class SaveData
    {
        public string             SteamGameFolder;
        public string             CustomGameFolder;
        public bool               PlayCustom;
        public List<InstalledMod> InstalledMods = new List<InstalledMod>();
    }
}