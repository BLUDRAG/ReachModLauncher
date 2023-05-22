using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TMPro;
using Object = UnityEngine.Object;

namespace ReachModLauncher
{
	public static class ModManagement
	{
		private static ModEntry _modEntryTemplate;
		private static List<ReachGamingMod> _mods;
		private static List<ModEntry> _modEntries = new List<ModEntry>();
		private static readonly Dictionary<WebClient, ModManageButton> _progressPairs = new Dictionary<WebClient, ModManageButton>();
		private const string _modListLink = "https://raw.githubusercontent.com/BLUDRAG/ReachGamingModsVault/release/ModList.json";
		
		public static void Init(ModEntry modEntryTemplate)
		{
			_modEntryTemplate = modEntryTemplate;
		}

		public static async Task DownloadModList()
		{
			string modList = Encoding.ASCII.GetString(await DownloadFile(_modListLink));
			_mods = JsonConvert.DeserializeObject<List<ReachGamingMod>>(modList);
			_mods.Sort((first, second) => string.Compare(first.Name, second.Name, StringComparison.Ordinal));

			foreach(ReachGamingMod mod in _mods)
			{
				_modEntries.Add(AddModEntry(mod));
			}
		}

		public static async Task<byte[]> DownloadFile(string url, ModManageButton manageButton = null)
		{
			using WebClient client = new WebClient();

			if(manageButton != null)
			{
				manageButton.UpdateProgress(0f);
				_progressPairs[client]         =  manageButton;
				client.DownloadProgressChanged += OnDownloadProgressChanged;
			}

			using MemoryStream stream = new MemoryStream(await client.DownloadDataTaskAsync(new Uri(url)));

			if(manageButton == null)
			{
				return stream.ToArray();
			}

			manageButton.UpdateManageState("Delete");

			return stream.ToArray();
		}

		public static void ApplySearchFilter(string filter)
		{
			foreach(ModEntry entry in _modEntries)
			{
				entry.gameObject.SetActive(entry.Mod.Name.Contains(filter, StringComparison.OrdinalIgnoreCase));
			}
		}

		private static void OnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			WebClient client = (WebClient)sender;
			_progressPairs[client].UpdateProgress(e.ProgressPercentage / 100f);
		}

		private static ModEntry AddModEntry(ReachGamingMod mod)
		{
			ModEntry entry = Object.Instantiate(_modEntryTemplate, _modEntryTemplate.transform.parent);
			entry.gameObject.SetActive(true);

			entry.Mod               = mod;
			entry.ModLink.Text.text = mod.Name;
			entry.ModLink.Tooltips.ForEach(tooltip => tooltip.text = mod.Description);
			entry.VersionDropdown.Dropdown.ClearOptions();

			foreach(ReachGamingModVersions version in mod.Mods)
			{
				entry.VersionDropdown.Versions.Add(version);
				entry.VersionDropdown.Dropdown.options.Add(new TMP_Dropdown.OptionData(version.Version));
			}

			InstalledMod installedMod = null;

			SaveData saveData      = DataManagement.GetSaveData();
			string   gameFolder    = !saveData.PlayCustom ? saveData.SteamGameFolder : saveData.CustomGameFolder;
			string   modsFolder    = Path.Combine(gameFolder, "Mods");
			string   sanitizedName = mod.Name.Replace(" ", "");
			string   versionFile   = Path.Combine(modsFolder, sanitizedName, "Version.txt");

			if(File.Exists(versionFile))
			{
				string currentVersion = File.ReadAllText(versionFile);
				installedMod = saveData.InstalledMods.Find(x => x.Name == mod.Name);

				if(installedMod is null)
				{
					installedMod = new InstalledMod()
					               {
						               Name    = mod.Name,
						               Version = currentVersion,
					               };

					saveData.InstalledMods.Add(installedMod);
				}
				else
				{
					installedMod.Version = currentVersion;
				}

				saveData.InstalledMods.Add(installedMod);
			}

			if(installedMod is null)
			{
				installedMod = DataManagement.GetSaveData().InstalledMods.Find(x => x.Name == mod.Name);
			}

			int modIndex = installedMod is null
				               ? mod.Mods.Count - 1
				               : mod.Mods.FindIndex(_mod => _mod.Version == installedMod.Version);

			entry.VersionDropdown.Dropdown.SetValueWithoutNotify(modIndex);
			entry.VersionDropdown.Mod = mod;
			entry.ModLink.Link        = mod.Mods[modIndex].Link;

			entry.ManageButton.UpdateManageState(installedMod is null ? "Download" : "Delete");

			entry.ManageButton.ModDownloadInfo = new ModDownloadInfo()
			                                     {
				                                     Link    = mod.Mods[modIndex].Link,
				                                     Name    = mod.Name,
				                                     Version = (entry.VersionDropdown.Versions[modIndex]).Version,
			                                     };

			entry.VersionDropdown.IsUpdateAvailable();
			return entry;
		}
	}
}