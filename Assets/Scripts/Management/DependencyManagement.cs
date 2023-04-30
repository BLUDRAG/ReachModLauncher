using UnityEngine;
using UnityEngine.UI;

namespace ReachModLauncher
{
	public class DependencyManagement : MonoBehaviour
	{
		[SerializeField] private ModEntry       _modEntryTemplate;
		[SerializeField] private CopyGameDialog _copyGameDialog;
		[SerializeField] private Folder         _steamGameFolder;
		[SerializeField] private Folder         _customGameFolder;
		[SerializeField] private Toggle         _playCustomToggle;
		
		private void Awake()
		{
			InjectManagementDependencies();
		}

		private void InjectManagementDependencies()
		{
			ModManagement.Init(_modEntryTemplate);
			FolderManagement.Init(_steamGameFolder, _customGameFolder, _playCustomToggle);
			DataManagement.Init(_copyGameDialog);
		}
	}
}