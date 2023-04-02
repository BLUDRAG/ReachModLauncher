using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace ReachModLauncher
{
	public class ModLink : MonoBehaviour
	{
		public string                            Link;
		public Button                            Button;
		public TMP_Text                          Text;
		public List<BoundTooltipTriggerExtended> Tooltips = new List<BoundTooltipTriggerExtended>();

		public void OnLinkClicked()
		{
			Process.Start(Link);
		}
	}
}