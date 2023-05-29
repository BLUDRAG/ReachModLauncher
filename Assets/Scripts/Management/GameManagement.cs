using System.Diagnostics;
using System.IO;

namespace ReachModLauncher
{
	public static class GameManagement
	{
		public static void PlayGame(string version)
		{
			SaveData saveData = DataManagement.GetSaveData();
			
			if(saveData.PlayCustom && (string.IsNullOrEmpty(saveData.CustomGameFolder)
			                        || !Directory.Exists(saveData.CustomGameFolder)))
			{
				FolderManagement.SelectCustomFolder(true, version);
				return;
			}

			string gameFolder = DataManagement.GetGameFolder();
			string gameFile   = Path.Combine(gameFolder, version);

			if(!File.Exists(gameFile))
			{
				_ = DataManagement.CreateCopy(gameFile);

				return;
			}

			Process.Start(gameFile);
		}
	}
}