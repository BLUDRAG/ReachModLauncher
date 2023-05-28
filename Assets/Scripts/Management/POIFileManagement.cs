using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReachModLauncher
{
	public static class POIFileManagement
	{
		private static readonly (string type, string file, bool found)[] _requiredPOIFiles =
			new (string, string, bool)[]
			{
				(".nim", null, false), (".ins", null, false), (".tts", null, false), (".xml", null, false),
			};

		private static readonly (string type, string file, bool found)[] _optionalPOIFiles =
			new (string, string, bool)[]
			{
				(".mesh", null, false), (".jpg", null, false),
			};

		private static string _previewFile;

		public static void CapturePOIFiles(string directory)
		{
			ResetPOIFiles();
			string[] files = Directory.GetFiles(directory);

			foreach(string file in files)
			{
				UpdatePOIList(_requiredPOIFiles, file);
				UpdatePOIList(_optionalPOIFiles, file);
			}
			
			_previewFile = _optionalPOIFiles.FirstOrDefault(file => file.type == ".jpg").file;
		}

		public static List<string> GetMissingPOIFiles()
		{
			List<string> missingFiles = new List<string>();

			foreach((string type, string file, bool found) data in _requiredPOIFiles)
			{
				if(!data.found)
				{
					missingFiles.Add(data.type);
				}
			}

			return missingFiles;
		}

		public static List<string> GetPOIFiles()
		{
			List<string> poiFiles = new List<string>();

			poiFiles.AddRange(_requiredPOIFiles.Select(data => data.file).ToList());
			poiFiles.AddRange(_optionalPOIFiles.Select(data => data.file).ToList());

			return poiFiles;
		}
		
		public static string GetPreviewFile()
		{
			return _previewFile;
		}

		private static void ResetPOIFiles()
		{
			for(int i = 0; i < _requiredPOIFiles.Length; i++)
			{
				(string type, string file, bool found) requiredPOIFile = _requiredPOIFiles[i];
				requiredPOIFile.file  = null;
				requiredPOIFile.found = false;
				_requiredPOIFiles[i]  = requiredPOIFile;
			}

			for(int i = 0; i < _optionalPOIFiles.Length; i++)
			{
				(string type, string file, bool found) optionalPOIFile = _optionalPOIFiles[i];
				optionalPOIFile.file  = null;
				optionalPOIFile.found = false;
				_optionalPOIFiles[i]  = optionalPOIFile;
			}
		}

		private static void UpdatePOIList((string type, string file, bool found)[] list, string file)
		{
			string extension = Path.GetExtension(file);

			for(int i = 0; i < list.Length; i++)
			{
				if(list[i].type != extension) continue;

				list[i].file  = file;
				list[i].found = true;
			}
		}
	}
}
