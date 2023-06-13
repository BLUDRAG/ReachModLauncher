using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace ReachModLauncher
{
	public static class GameManagement
	{
		private static CopyGameDialog _dialog;

		public static void Init(CopyGameDialog dialog)
		{
			_dialog = dialog;
		}
		
		public static async void PlayGame(string version)
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
				await CreateCopy();
			}

			Process.Start(gameFile);
		}

		public static async Task CreateCopy()
		{
			SaveData saveData = DataManagement.GetSaveData();
			string   local    = saveData.SteamGameFolder;
			string   copy     = saveData.CustomGameFolder;
			string[] files    = Directory.GetFiles(local, "*", SearchOption.AllDirectories);

			await _dialog.Show();

			if(!Directory.Exists(copy))
			{
				Directory.CreateDirectory(copy);
			}

			for(int i = 0, totalFiles = files.Length; i < totalFiles; i++)
			{
				_dialog.UpdateProgress(i / (float)totalFiles);
				string file       = files[i];
				string targetFile = file.Replace(local, copy);
				Directory.CreateDirectory(Path.GetDirectoryName(targetFile));

				await using FileStream sourceStream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read,
				                                                     bufferSize: 4096, useAsync: true);

				await using FileStream destinationStream = new FileStream(targetFile, FileMode.CreateNew,
				                                                          FileAccess.Write, FileShare.None, bufferSize: 4096,
				                                                          useAsync: true);

				await sourceStream.CopyToAsync(destinationStream);
			}

			_ = _dialog.Hide();
		}
	}
}