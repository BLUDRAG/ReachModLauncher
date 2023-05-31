using UnityEngine;

namespace ReachModLauncher
{
    public sealed class SearchBar : MonoBehaviour
    {
        public void Search(string text)
        {
            ModManagement.ApplySearchFilter(text);
        }
    }
}
