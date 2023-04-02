using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ReachModLauncher
{
	public static class DataManagement
	{
		private static SaveData       _saveData;
		private static CopyGameDialog _dialog;

		private const string _saveFile = "Data.json";
		
		public static void Init(CopyGameDialog dialog)
		{
			_dialog = dialog;
			LoadData();
		}

		public static SaveData GetSaveData()
		{
			if(_saveData is null)
			{
				_saveData = new SaveData();
			}

			return _saveData;
		}

		public static bool LoadData()
		{
			string saveFile = Path.Combine(Directory.GetCurrentDirectory(), _saveFile);

			if(!File.Exists(saveFile)) return false;

			string json = File.ReadAllText(saveFile);
			_saveData = JsonConvert.DeserializeObject<SaveData>(json);
			FolderManagement.SetLocalGameFolder(_saveData.SteamGameFolder);
			FolderManagement.SetCustomGameFolder(_saveData.CustomGameFolder);
			FolderManagement.SetPlayCustom(_saveData.PlayCustom);

			return true;
		}

		public static void SaveData()
		{
			string saveFile = Path.Combine(Directory.GetCurrentDirectory(), _saveFile);
			string json     = JsonConvert.SerializeObject(_saveData);
			File.WriteAllText(saveFile, json);
		}

		public static async Task CreateCopy(string gameFile)
		{
			string   local = _saveData.SteamGameFolder;
			string   copy  = _saveData.CustomGameFolder;
			string[] files = Directory.GetFiles(local, "*", SearchOption.AllDirectories);

			_dialog.ProgressBar.fillAmount = 0f;
			_dialog.gameObject.SetActive(true);

			if(!Directory.Exists(copy))
			{
				Directory.CreateDirectory(copy);
			}

			for(int i = 0; i < files.Length; i++)
			{
				_dialog.ProgressBar.fillAmount = i / (float)files.Length;
				string file       = files[i];
				string targetFile = file.Replace(local, copy);
				Directory.CreateDirectory(Path.GetDirectoryName(targetFile));
				File.Copy(file, targetFile);
				if(i % 100 == 0) await Task.Delay(1);
			}

			_dialog.gameObject.SetActive(false);
			Process.Start(gameFile);
		}
	}
}