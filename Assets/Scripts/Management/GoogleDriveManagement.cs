using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using UnityEngine;
using File = Google.Apis.Drive.v3.Data.File;

namespace ReachModLauncher
{
    public static class GoogleDriveManagement
    {
	    private static DriveService _service;
        private static string       _repoDirectory;
        private const  string       _dataLocation = "Data/GoogleDriveRepoData";
        
        public static void LoadData()
        {
            GoogleDriveRepoData data = Resources.Load<GoogleDriveRepoData>(_dataLocation);
            _repoDirectory = data.RepoDirectory;
            PrepareService(data.UserData);
        }

        private static void PrepareService(string user)
        {
	        GoogleCredential credentials = GoogleCredential.FromJson(user)
	                                                       .CreateScoped(DriveService.Scope.Drive);

	        _service = new DriveService(new Google.Apis.Services.BaseClientService.Initializer
	                                    {
		                                    HttpClientInitializer = credentials
	                                    });
        }

        public static async Task<(bool exists, string id)> FileExists(string folder, string file)
        {
        	FilesResource.ListRequest fileRequest = _service.Files.List();
        	fileRequest.Fields = "*";
        	fileRequest.Q      = $"'me' in owners and '{folder}' in parents and name = '{file}'";
        	IList<File> files = (await fileRequest.ExecuteAsync()).Files;

        	return (files.Count > 0, files.Count > 0 ? files[0].Id : null);
        }

        public static async Task<(bool exists, string id)> FolderExists(string folder)
        {
        	return await FileExists(_repoDirectory, folder);
        }
        
        public static async Task<string> CreateFolder(string folder)
        {
        	File metaData = new File
        	                {
        		                Name     = folder,
        		                Parents  = new[] { _repoDirectory },
        		                MimeType = "application/vnd.google-apps.folder"
        	                };

        	FilesResource.CreateRequest request = _service.Files.Create(metaData);
        	request.Fields = "*";
        	File result = await request.ExecuteAsync();

        	return result.Id;
        }
        
        public static async Task<IList<File>> GetFiles(string folder)
        {
        	FilesResource.ListRequest fileRequest = _service.Files.List();
        	fileRequest.Q = $"'me' in owners and '{folder}' in parents";
        	IList<File> files = (await fileRequest.ExecuteAsync()).Files;

        	return files;
        }
        
        public static async Task UploadFile(string folder, string file)
        {
        	File metaData = new File
        	                {
        		                Name     = Path.GetFileName(file),
        		                Parents  = new[] { folder },
        		                MimeType = "application/octet-stream"
        	                };

        	await using FileStream stream = new FileStream(file, FileMode.Open);

        	FilesResource.CreateMediaUpload request = _service.Files.Create(metaData, stream, metaData.MimeType);
        	await request.UploadAsync();
        }
    }
}
