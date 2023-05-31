using UnityEngine;
using UnityEngine.UI;

namespace ReachModLauncher
{
	public sealed class CustomGameToggle : MonoBehaviour
	{
		public Toggle Toggle;
		
		public void ToggleCustomGame(bool value)
		{
			SaveData saveData = DataManagement.GetSaveData();
			saveData.PlayCustom = value;
			DataManagement.SaveData();

			if(value && string.IsNullOrEmpty(saveData.CustomGameFolder))
			{
				FolderManagement.SelectCustomFolder(onCancel: () => Toggle.isOn = false);
			}
		}
	}
}