using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Download;
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
			bool hasFiles = files.Count > 0;

			return (hasFiles, hasFiles ? files[0].Id : null);
		}

		public static async Task<(bool exists, string id)> FolderExists(string folder)
		{
			return await FileExists(_repoDirectory, folder);
		}

		public static async Task<string> CreateFolder(string folder)
		{
			File metaData = new File
			                {
				                Name = folder,
				                Parents = new[]
				                          {
					                          _repoDirectory
				                          },
				                MimeType = "application/vnd.google-apps.folder"
			                };

			FilesResource.CreateRequest request = _service.Files.Create(metaData);
			request.Fields = "*";
			File result = await request.ExecuteAsync();

			return result.Id;
		}

		public static async Task<List<File>> GetFiles(string folder)
		{
			FilesResource.ListRequest fileRequest = _service.Files.List();
			fileRequest.Fields = "*";
			fileRequest.Q      = $"'me' in owners and '{folder}' in parents";
			IList<File> files = (await fileRequest.ExecuteAsync()).Files;

			return files.ToList();
		}

		public static async Task<List<string>> GetUsers()
		{
			return (await GetFiles(_repoDirectory)).Select(file => file.Id).ToList();
		}

		public static async Task UploadFile(string folder, string filename, Stream stream)
		{
			File metaData = new File
			                {
				                Name = filename,
				                Parents = new[]
				                          {
					                          folder
				                          },
				                MimeType = "application/octet-stream"
			                };

			FilesResource.CreateMediaUpload request = _service.Files.Create(metaData, stream, metaData.MimeType);

			try
			{
				await request.UploadAsync();
			}
			finally
			{
				await stream.DisposeAsync();
			}
		}
		
		public static async Task<byte[]> DownloadFile(File file, Action<long> progressCallback)
		{
			FilesResource.GetRequest request = _service.Files.Get(file.Id);
			request.Fields = "*";
			long                    totalBytes = file.Size ?? long.MaxValue;
			MemoryStream            stream     = new MemoryStream();
			Task<IDownloadProgress> progress   = request.DownloadAsync(stream);
			
			while(!progress.IsCompleted)
			{
				progressCallback?.Invoke(progress.Result.BytesDownloaded / totalBytes);
				await Task.Yield();
			}
			
			return stream.ToArray();
		}
	}
}
