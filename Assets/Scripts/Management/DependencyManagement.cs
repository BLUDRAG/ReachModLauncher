using UnityEngine;
using UnityEngine.UI;

namespace ReachModLauncher
{
	public class DependencyManagement : MonoBehaviour
	{
		[SerializeField] private ModEntry       _modEntryTemplate;
		[SerializeField] private POIEntry       _poiEntryTemplate;
		[SerializeField] private CopyGameDialog _copyGameDialog;
		[SerializeField] private Folder         _steamGameFolder;
		[SerializeField] private Folder         _customGameFolder;
		[SerializeField] private Toggle         _playCustomToggle;
		[SerializeField] private TextAsset      _poiModInfo;
		
		private void Awake()
		{
			InjectManagementDependencies();
		}

		private void InjectManagementDependencies()
		{
			ModManagement.Init(_modEntryTemplate);
			POIManagement.Init(_poiEntryTemplate, _poiModInfo.text);
			FolderManagement.Init(_steamGameFolder, _customGameFolder, _playCustomToggle);
			GameManagement.Init(_copyGameDialog);
			DataManagement.Init();
		}
	}
}