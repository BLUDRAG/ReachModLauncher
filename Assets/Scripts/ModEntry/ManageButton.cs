using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ReachModLauncher
{
    public class ManageButton : MonoBehaviour
    {
        public ModDownloadInfo ModDownloadInfo;
        public GameObject      Manage;
        public TMP_Text        Text;
        public GameObject      Progress;
        public Transform       ProgressBar;
        public VersionDropdown VersionDropdown;

        public void OnClick()
        {
            InstalledMod installedMod = DataManagement.GetSaveData().InstalledMods.Find(x => x.Name == ModDownloadInfo.Name);

            if(installedMod != null && installedMod.Version == ModDownloadInfo.Version)
            {
                DeleteMod(installedMod);
                Text.text = "Download";
                VersionDropdown.IsUpdateAvailable();

                return;
            }

            Manage.SetActive(false);
            Progress.SetActive(true);

            DeleteMod(installedMod);
            _ = DownloadMod();
        }

        private void DeleteMod(InstalledMod installedMod)
        {
            SaveData saveData = DataManagement.GetSaveData();
            string gameFolder = !saveData.PlayCustom
                                    ? saveData.SteamGameFolder
                                    : saveData.CustomGameFolder;

            string modsFolder    = Path.Combine(gameFolder, "Mods");
            string sanitizedName = ModDownloadInfo.Name.Replace(" ", "");
            string zipFile       = Path.Combine(modsFolder, $"{sanitizedName}.zip");
            string folder        = Path.Combine(modsFolder, sanitizedName);

            saveData.InstalledMods.Remove(installedMod);
            DataManagement.SaveData();
            if(File.Exists(zipFile)) File.Delete(zipFile);
            if(Directory.Exists(folder)) Directory.Delete(folder, true);
        }

        private async Task DownloadMod()
        {
            SaveData saveData      = DataManagement.GetSaveData();
            byte[]   data          = await ModManagement.DownloadFile(ModDownloadInfo.Link, this);
            string   gameFolder    = !saveData.PlayCustom ? saveData.SteamGameFolder : saveData.CustomGameFolder;
            string   modsFolder    = Path.Combine(gameFolder, "Mods");
            string   sanitizedName = ModDownloadInfo.Name.Replace(" ", "");

            if(!Directory.Exists(modsFolder))
            {
                Directory.CreateDirectory(modsFolder);
            }

            string zipFile = Path.Combine(modsFolder, $"{sanitizedName}.zip");

            File.WriteAllBytes(zipFile, data);
            ZipFile.ExtractToDirectory(zipFile, modsFolder);
            string versionFile = Path.Combine(modsFolder, sanitizedName, "Version.txt");
            string version     = File.ReadAllText(versionFile);

            InstalledMod installedMod = saveData.InstalledMods.Find(x => x.Name == ModDownloadInfo.Name);

            if(installedMod is null)
            {
                installedMod = new InstalledMod
                               {
                                   Name = ModDownloadInfo.Name,
                                   Version = version
                               };

                saveData.InstalledMods.Add(installedMod);
            }
            else
            {
                installedMod.Version = version;
            }

            DataManagement.SaveData();
            VersionDropdown.IsUpdateAvailable();
        }
    }
}