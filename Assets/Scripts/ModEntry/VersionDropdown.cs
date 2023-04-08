using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ReachModLauncher
{
	public class VersionDropdown : MonoBehaviour
	{
		public TMP_Dropdown                 Dropdown;
		public GameObject                   UpdateIcon;
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

            if(installedMod is null)
            {
	            ManageButton.Text.text = "Download";
	            UpdateIcon.SetActive(true);
            }
            else
            {
	            IsUpdateAvailable();
	            ManageButton.Text.text = installedMod.Version == Versions[index].Version ? "Delete" : "Update";
            }
        }

		public bool IsUpdateAvailable()
		{
			InstalledMod installedMod    = DataManagement.GetSaveData().InstalledMods.Find(x => x.Name == Mod.Name);
			int          latestVersion   = Versions.Count - 1;
			bool         modInstalled    = !(installedMod is null);
			
			bool onLatestVersion = modInstalled && installedMod.Version == Versions[latestVersion].Version;
			bool updateAvailable = !modInstalled || !onLatestVersion;
			
			UpdateIcon.SetActive(updateAvailable);
			return updateAvailable;
		}

		public void OnUpdateShortcutClicked()
		{
			int latestVersion = Versions.Count - 1;
			Dropdown.value = latestVersion;
			ManageButton.OnClick();
		}
	}
}