using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace ReachModLauncher
{
	public static class DataManagement
	{
		private static SaveData      _saveData;
		private static SuperUserData _superUserData;

		private const string _saveFile      = "Data.json";
		private const string _superUserFile = "SuperUser.json";
		
		public static void Init()
		{
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

		public static string GetGameFolder()
		{
			return _saveData.PlayCustom ? _saveData.CustomGameFolder : _saveData.SteamGameFolder;
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

		public static void LoadSuperUserData()
		{
			string superUserFile = Path.Combine(Directory.GetParent(Application.dataPath).FullName, _superUserFile);
			if(!File.Exists(superUserFile)) return;

			_superUserData = JsonUtility.FromJson<SuperUserData>(File.ReadAllText(superUserFile));
			
			if(!GoogleDriveManagement.VerifySuperUser(_superUserData.VerificationKey))
			{
				_superUserData = null;
			}
		}

		public static bool IsSuperUser()
		{
			return _superUserData is not null;
		}
		
		public static SuperUserData GetSuperUserData()
		{
			return _superUserData;
		}
	}
}