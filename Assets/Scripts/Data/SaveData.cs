using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace ReachModLauncher
{
    [Serializable]
    public class SaveData
    {
        [FormerlySerializedAs("LocalGameFolder")] public string             SteamGameFolder;
        public                                           string             CustomGameFolder;
        public                                           bool               PlayCustom;
        public                                           List<InstalledMod> InstalledMods = new List<InstalledMod>();
    }
}