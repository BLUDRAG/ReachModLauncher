using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Upload;
using SimpleFileBrowser;
using TMPro;
using UnityEngine;

namespace ReachModLauncher
{
    public sealed class UploadPOIDialog : Dialog
    {
        [SerializeField] private GameObject     _progressBarParent;
        [SerializeField] private GameObject     _uploadButton;
        [SerializeField] private TMP_Text       _authorText;
        [SerializeField] private TMP_InputField _poiFolder;

        private        long  _currentFileBytes;
        private static float _currentUploadProgress;

        private void Update()
        {
            if(_currentFileBytes > 0)
            {
                UpdateProgress(_currentUploadProgress);
            }
        }
        
        public override Task Show()
        {
            _authorText.text = $"Author : <b>{SteamManagement.GetSteamUser()}</b>";
            _poiFolder.text  = "";
            _progressBarParent.SetActive(false);
            _uploadButton.SetActive(true);
            return base.Show();
        }
        
        public void ShowOnClick()
        {
            _ = Show();
        }
        
        public override async Task Hide()
        {
            await base.Hide();
            _progressBarParent.SetActive(false);
        }

        public void HideOnClick()
        {
            if(_currentFileBytes > 0)
            {
                return;
            }

            _ = Hide();
        }

        public override void UpdateProgress(float percentage)
        {
            _progressBar.transform.localScale = new Vector3(percentage, 1f, 1f);
            _progressText.text                = $"{percentage * 100f:0.00}%";
        }

        public void SelectPOIFolder()
        {
            FileBrowser.ShowLoadDialog(OnPOIFolderSelected,
                                       () =>
                                       {
                                       }, FileBrowser.PickMode.Folders, false, null, null, "Select POI Folder");
        }

        public void UploadPOI()
        {
            _ = UploadPOIInternal();
        }

        private async Task UploadPOIInternal()
        {
            _uploadButton.SetActive(false);
            _progressBarParent.SetActive(true);
            
            string user = SteamManagement.GetSteamUser();
            (bool exists, string folderId) = await GoogleDriveManagement.FolderExists(user);

            if(!exists)
            {
                folderId = await GoogleDriveManagement.CreateFolder(user);
            }

            List<string> poiFiles  = POIManagement.GetPOIFiles();
            string       firstFile = Path.GetFileName(poiFiles[0]);
            string       filename  = $"{firstFile.Substring(0, firstFile.IndexOf('.'))}.zip";
            (exists, _) = await GoogleDriveManagement.FileExists(folderId, filename);

            if(!exists)
            {
                Stream stream = ZipManagement.CreatePOIZipFile(poiFiles);
                _currentFileBytes = stream.Length;

                await GoogleDriveManagement.UploadFile(folderId, filename, stream, OnUploadProgressUpdated);

                UpdateProgress(1f);

                string previewFile = POIManagement.GetPreviewFile();

                if(!string.IsNullOrEmpty(previewFile))
                {
                    FileStream previewStream = File.OpenRead(previewFile);
                    _currentFileBytes = previewStream.Length;
                    await GoogleDriveManagement.UploadFile(folderId, Path.GetFileName(previewFile),
                                                           previewStream, OnUploadProgressUpdated);
                    UpdateProgress(1f);
                }
            }

            await POIManagement.DownloadPOIList();
            _currentFileBytes = 0;
            _uploadButton.SetActive(true);
            _progressBarParent.SetActive(false);
        }

        private void OnPOIFolderSelected(string[] folders)
        {
            string folder = folders[0];
            if(!ValidateSelectedFolder(folder)) return;
            _poiFolder.text = Path.GetFileName(folder);
        }

        private bool ValidateSelectedFolder(string folder)
        {
            POIManagement.CapturePOIFiles(folder);
            List<string> missingFiles = POIManagement.GetMissingPOIFiles();

            if(missingFiles.Count <= 0) return true;

            Debug.Log($"Missing required files : {missingFiles.Aggregate((current, next) => $"{current}, {next}")}");
            return false;
        }

        private void OnUploadProgressUpdated(IUploadProgress progress)
        {
            _currentUploadProgress = (float)progress.BytesSent / _currentFileBytes;
        }
    }
}