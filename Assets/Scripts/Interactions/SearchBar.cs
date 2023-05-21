using UnityEngine;

namespace ReachModLauncher
{
    public class SearchBar : MonoBehaviour
    {
        public void Search(string text)
        {
            ModManagement.ApplySearchFilter(text);
        }
    }
}
