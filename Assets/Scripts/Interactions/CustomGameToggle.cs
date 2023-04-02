using UnityEngine;

namespace ReachModLauncher
{
	public class CustomGameToggle : MonoBehaviour
	{
		public void ToggleCustomGame(bool value)
		{
			DataManagement.GetSaveData().PlayCustom = value;
			DataManagement.SaveData();
		}
	}
}