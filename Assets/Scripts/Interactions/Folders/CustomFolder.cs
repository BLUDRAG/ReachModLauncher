namespace ReachModLauncher
{
	public sealed class CustomFolder : Folder
	{
		public override void Select()
		{
			FolderManagement.SelectCustomFolder();
		}
	}
}