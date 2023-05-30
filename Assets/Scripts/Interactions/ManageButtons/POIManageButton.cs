using System.IO;
using System.IO.Compression;

namespace ReachModLauncher
{
    public class POIManageButton : ManageButton
    {
        public POIData Data;

        private bool   _downloading      = false;
        private float  _downloadProgress = 0f;
        
        private void Update()
        {
            if(_downloading)
            {
                UpdateProgress(_downloadProgress);
            }
        }

        public override void OnClick()
        {
            DownloadPOI();
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
        
        private void UpdateDownloadProgress(float percentage)
        {
            _downloadProgress = percentage;
        }
    }
}
