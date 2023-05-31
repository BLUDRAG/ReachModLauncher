using System.IO;
using System.IO.Compression;

namespace ReachModLauncher
{
    public sealed class POIManageButton : ManageButton
    {
        public POIData Data;

        private bool  _downloading      = false;
        private float _downloadProgress = 0f;

        private void Update()
        {
            if(_downloading)
            {
                UpdateProgress(_downloadProgress);
            }
        }

        public override void OnClick()
        {
            string poiFolder = POIManagement.GetPOIDirectory(Data, false);
            
            if(Directory.Exists(poiFolder))
            {
                DeletePOI();
            }
            else
            {
                DownloadPOI();
            }
        }

        public void Init(POIData data)
        {
            Data = data;
            string poiFolder = POIManagement.GetPOIDirectory(Data, false);
            
            UpdateManageState(Directory.Exists(poiFolder) ? ManageButtonStates.Delete : ManageButtonStates.Download);
        }

        private async void DownloadPOI()
        {
            _downloadProgress = 0f;
            _downloading      = true;
            byte[] data = await GoogleDriveManagement.DownloadFile(Data.File, UpdateDownloadProgress);
            _downloading = false;

            string poiFolder  = POIManagement.GetPOIDirectory(Data);
            using ZipArchive archive = new ZipArchive(new MemoryStream(data), ZipArchiveMode.Read);
            archive.ExtractToDirectory(poiFolder);
            UpdateManageState(ManageButtonStates.Delete);
        }

        private void DeletePOI()
        {
            string poiFolder = POIManagement.GetPOIDirectory(Data);
            Directory.Delete(poiFolder, true);
            UpdateManageState(ManageButtonStates.Download);
        }

        private void UpdateDownloadProgress(float percentage)
        {
            _downloadProgress = percentage;
        }
    }
}
