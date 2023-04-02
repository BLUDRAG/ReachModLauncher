using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ReachModLauncher
{
	public class VersionDropdown : MonoBehaviour
	{
		public TMP_Dropdown                 Dropdown;
		public List<ReachGamingModVersions> Versions = new List<ReachGamingModVersions>();
		public ManageButton                 ManageButton;
		public ReachGamingMod               Mod;

		public void OnVersionChanged(int index)
        {
            InstalledMod installedMod = DataManagement.GetSaveData().InstalledMods.Find(x => x.Name == Mod.Name);

            ManageButton.Text.text = installedMod is null ? "Download" : "Delete";

            ManageButton.ModDownloadInfo = new ModDownloadInfo()
                                           {
	                                           Link    = Mod.Mods[index].Link,
	                                           Name    = Mod.Name,
	                                           Version = Versions[index].Version,
                                           };
        
            if (installedMod is null) ManageButton.Text.text = "Download";
            else
            {
	            if(installedMod.Version == Versions[index].Version) ManageButton.Text.text = "Delete";
	            else ManageButton.Text.text                                                = "Update";
            }
        }
	}
}