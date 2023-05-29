using System.Diagnostics;
using TMPro;
using UnityEngine;

namespace ReachModLauncher
{
    public class TextLink : MonoBehaviour
    {
        public string   Link;
        public TMP_Text Text;
        
        public void OnLinkClicked()
        {
            Process.Start(Link);
        }
    }
}
