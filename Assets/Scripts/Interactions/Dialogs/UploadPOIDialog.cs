using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ReachModLauncher
{
    public sealed class UploadPOIDialog : Dialog
    {
        [SerializeField] private GameObject _progressBarParent;
        [SerializeField] private TMP_Text _authorText;
        
        public override Task Show()
        {
            _authorText.text = $"Author : <b>{SteamManagement.GetSteamUser()}</b>";
            _progressBarParent.SetActive(false);
            return base.Show();
        }
        
        public void ShowOnClick()
        {
            _ = Show();
        }
        
        public override async Task Hide()
        {
            await base.Hide();
            _progressBarParent.SetActive(false);
        }

        public void HideOnClick()
        {
            _ = Hide();
        }

        public override void UpdateProgress(float percentage)
        {
            _progressBar.transform.localScale = new Vector3(percentage, 1f, 1f);
            _progressText.text                = $"{percentage * 100f:0.00}%";
        }
    }
}