using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Upload;
using SimpleFileBrowser;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReachModLauncher
{
    public sealed class UploadPOIDialog : Dialog
    {
        [SerializeField] private GameObject     _progressBarParent;
        [SerializeField] private Button         _uploadButton;
        [SerializeField] private TMP_Text       _uploadButtonText;
        [SerializeField] private TMP_Text       _authorText;
        [SerializeField] private TMP_Text       _errorText;
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
            string steamUser = SteamManagement.GetSteamUser();
            _authorText.text = $"Author : <b>{steamUser}</b>";
            _poiFolder.text  = "";
            _errorText.text  = "";
            _progressBarParent.SetActive(false);
            _uploadButton.gameObject.SetActive(true);
            CheckUploadLimit(steamUser);

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
            _uploadButton.gameObject.SetActive(false);
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

            POIManagement.UpdateUploadCount();
            await POIManagement.DownloadPOIList();
            _currentFileBytes = 0;
            CheckUploadLimit(user);
            _uploadButton.gameObject.SetActive(true);
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

            string aggregatedFiles = missingFiles.Aggregate((current, next) => $"{current}, <b>{next}</b>");
            _errorText.text = $"<u>Missing File{(missingFiles.Count > 1 ? "s" : "")}</u> : {aggregatedFiles}";
            return false;
        }

        private void OnUploadProgressUpdated(IUploadProgress progress)
        {
            _currentUploadProgress = (float)progress.BytesSent / _currentFileBytes;
        }

        private void CheckUploadLimit(string steamUser)
        {
            if(GoogleDriveManagement.CanUpload(steamUser)) return;

            _uploadButton.interactable = false;
            _uploadButtonText.text     = "Limit Reached";
        }
    }
}