using TMPro;
using UnityEngine;

namespace ReachModLauncher
{
	public abstract class Folder : MonoBehaviour
	{
		public TMP_InputField Text;

		public abstract void Select();
	}
}