using System.IO;
using SimpleFileBrowser;
using UnityEngine.UI;

namespace ReachModLauncher
{
	public static class FolderManagement
	{
		private static Folder _steamGameFolder;
		private static Folder _customGameFolder;
		private static Toggle _playCustomToggle;
		
		private static readonly string _rootFolderGuess = Path.Combine("Program Files (x86)", "Steam", "steamapps", "common", "7 Days To Die");

		public static void Init(Folder steamGameFolder, Folder customGameFolder, Toggle playCustomToggle)
		{
			_steamGameFolder  = steamGameFolder;
			_customGameFolder = customGameFolder;
			_playCustomToggle = playCustomToggle;
		}

		public static void FindGameFolder()
		{
			DriveInfo[] drives = DriveInfo.GetDrives();

			foreach(DriveInfo drive in drives)
			{
				string searchFolder = Path.Combine(drive.RootDirectory.FullName, _rootFolderGuess);

				if(Directory.Exists(searchFolder))
				{
					DataManagement.GetSaveData().SteamGameFolder = searchFolder;
					SetLocalGameFolder(searchFolder);
					DataManagement.SaveData();

					return;
				}
			}
		}

		public static void SetLocalGameFolder(string folder)
		{
			_steamGameFolder.Text.text = folder;
		}
		
		public static void SetCustomGameFolder(string folder)
		{
			_customGameFolder.Text.text = folder;
		}
		
		public static void SetPlayCustom(bool playCustom)
		{
			_playCustomToggle.isOn = playCustom;
		}

		public static void SelectSteamFolder()
		{
			FileBrowser.ShowLoadDialog(success =>
			                           {
				                           DataManagement.GetSaveData().SteamGameFolder = success[0];
				                           _steamGameFolder.Text.text                   = success[0];
				                           DataManagement.SaveData();
			                           }, null, FileBrowser.PickMode.Folders, false, null, null,
			                           "Select Steam Game Folder");
		}

		public static void SelectCustomFolder(bool continuePlay = false, string continueGame = "")
		{
			FileBrowser.ShowLoadDialog(success =>
			                           {
				                           DataManagement.GetSaveData().CustomGameFolder = success[0];
				                           _customGameFolder.Text.text                   = success[0];
				                           DataManagement.SaveData();

				                           if(continuePlay)
				                           {
					                           GameManagement.PlayGame(continueGame);
				                           }
			                           },
			                           () =>
			                           {
			                           }, FileBrowser.PickMode.Folders, false, null, null, "Select Custom Game Folder");
		}
	}
}