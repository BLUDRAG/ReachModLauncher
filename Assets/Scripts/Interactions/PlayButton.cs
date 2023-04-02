using UnityEngine;

namespace ReachModLauncher
{
	public class PlayButton : MonoBehaviour
	{
		[SerializeField] private string _game;
		
		public void PlayGame()
		{
			GameManagement.PlayGame(_game);
		}
	}
}