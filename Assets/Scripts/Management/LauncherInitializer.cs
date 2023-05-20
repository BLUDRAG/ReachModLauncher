using UnityEngine;

namespace ReachModLauncher
{
	public class LauncherInitializer : MonoBehaviour
	{
		public void Start()
		{
			GoogleDriveManagement.LoadData();
			bool saveLoaded = DataManagement.LoadData();
			if(!saveLoaded || string.IsNullOrEmpty(DataManagement.GetSaveData().SteamGameFolder)) FolderManagement.FindGameFolder();
			_ = ModManagement.DownloadModList();
		}
	}
}