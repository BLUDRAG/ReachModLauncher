using System.Diagnostics;
using UnityEngine;

namespace ReachModLauncher
{
	public class ExternalLink : MonoBehaviour
	{
		[SerializeField] private string _url;
		
		public void OpenLink()
		{
			Process.Start(_url);
		}
	}
}