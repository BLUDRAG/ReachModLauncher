using UnityEngine;

namespace ReachModLauncher
{
	public class LauncherInitializer : MonoBehaviour
	{
		public void Start()
		{
			_ = GoogleDriveManagement.Init();
			DataManagement.Init();
			bool saveLoaded = DataManagement.LoadData();
			if(!saveLoaded || string.IsNullOrEmpty(DataManagement.GetSaveData().SteamGameFolder)) FolderManagement.FindGameFolder();
			_ = ModManagement.DownloadModList();
			_ = POIManagement.DownloadPOIList();
		}
	}
}