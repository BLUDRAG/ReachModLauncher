namespace ReachModLauncher
{
	public sealed class CopyGameDialog : Dialog
	{
		public override void UpdateProgress(float percentage)
		{
			_progressBar.fillAmount = percentage;
			_progressText.text      = $"{percentage * 100f:0.00}%";
		}
	}
}