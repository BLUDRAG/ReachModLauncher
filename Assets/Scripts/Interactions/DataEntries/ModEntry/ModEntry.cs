using UnityEngine;

namespace ReachModLauncher
{
	public sealed class ModEntry : MonoBehaviour
	{
		public ReachGamingMod  Mod;
		public ModLink         ModLink;
		public VersionDropdown VersionDropdown;
		public ModManageButton ManageButton;
	}
}