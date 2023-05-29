using System.IO;
using System.IO.Compression;

namespace ReachModLauncher
{
    public class POIManageButton : ManageButton
    {
        public POIData Data;
        
        public override void OnClick()
        {
            DownloadPOI();
        }

        private async void DownloadPOI()
        {
            byte[] data       = await GoogleDriveManagement.DownloadFile(Data.File, UpdateProgress);
            string poiFolder  = POIManagement.GetPOIDirectory(Data);

            if(!Directory.Exists(poiFolder))
            {
                Directory.CreateDirectory(poiFolder);
            }

            using ZipArchive archive = new ZipArchive(new MemoryStream(data), ZipArchiveMode.Read);
            archive.ExtractToDirectory(poiFolder);
            UpdateManageState(ManageButtonStates.Delete);
        }
    }
}
