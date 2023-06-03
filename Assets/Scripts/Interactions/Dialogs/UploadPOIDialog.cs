using System.Threading.Tasks;
using UnityEngine;

namespace ReachModLauncher
{
    public sealed class UploadPOIDialog : Dialog
    {
        [SerializeField] private GameObject _progressBarParent;
        
        public override Task Show()
        {
            _progressBarParent.SetActive(false);
            return base.Show();
        }
        
        public override async Task Hide()
        {
            await base.Hide();
            _progressBarParent.SetActive(false);
        }

        public override void UpdateProgress(float percentage)
        {
            _progressBar.transform.localScale = new Vector3(percentage, 1f, 1f);
            _progressText.text                = $"{percentage * 100f:0.00}%";
        }
    }
}