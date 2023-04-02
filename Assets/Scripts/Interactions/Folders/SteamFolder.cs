namespace ReachModLauncher
{
	public class SteamFolder : Folder
	{
		public override void Select()
		{
			FolderManagement.SelectSteamFolder();
		}
	}
}