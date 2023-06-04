using System.IO;

namespace ReachModLauncher
{
	public static class SteamManagement
	{
		private static string _user;

		private const string _steamConfigFolder = "config";
		private const string _steamUsersFile    = "loginusers.vdf";
		private const string _steamAccountField = "AccountName";
		private const string _steamRecentField  = "MostRecent";
		private const string _anonymousUser     = "Anonymous";

		public static string GetSteamUser()
		{
			if(DataManagement.IsSuperUser())
			{
				return DataManagement.GetSuperUserData().User;
			}

			if(!string.IsNullOrEmpty(_user))
			{
				return _user;
			}

			string steamFolder = DataManagement.GetSaveData().SteamGameFolder;

			if(string.IsNullOrEmpty(steamFolder))
			{
				_user = _anonymousUser;
				return _user;
			}

			for(int i = 0; i < 3; i++)
			{
				steamFolder = Directory.GetParent(steamFolder).FullName;
			}

			steamFolder = Path.Combine(steamFolder, _steamConfigFolder);
			string userCache = Path.Combine(steamFolder, _steamUsersFile);
			_user = !File.Exists(userCache) ? _anonymousUser : GetLastSteamUser(userCache);
			
			return _user;
		}

		private static string GetLastSteamUser(string userCache)
		{
			(string name, bool loggedIn) user = new("", false);

			using StreamReader reader = new StreamReader(userCache);

			while(!reader.EndOfStream)
			{
				string line = reader.ReadLine();

				if(line is null)
				{
					break;
				}

				user.name     = GetSteamDataValue(line, _steamAccountField) ?? user.name;
				user.loggedIn = GetSteamDataValue(line, _steamRecentField) == "1";

				if(user.loggedIn)
				{
					break;
				}
			}

			return user.loggedIn ? user.name : _anonymousUser;
		}

		private static string GetSteamDataValue(string line, string data)
		{
			return line.Contains(data) ? line.Split('"')[3] : null;
		}
	}
}
