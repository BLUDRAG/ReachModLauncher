using System.Diagnostics;
using UnityEngine;

namespace ReachModLauncher
{
	public sealed class ExternalLink : MonoBehaviour
	{
		[SerializeField] private string _url;
		
		public void OpenLink()
		{
			Process.Start(_url);
		}
	}
}