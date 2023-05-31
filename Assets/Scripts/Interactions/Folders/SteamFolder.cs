namespace ReachModLauncher
{
	public sealed class SteamFolder : Folder
	{
		public override void Select()
		{
			FolderManagement.SelectSteamFolder();
		}
	}
}