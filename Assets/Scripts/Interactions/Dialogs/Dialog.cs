using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReachModLauncher
{
    public abstract class Dialog : MonoBehaviour
    {
        [SerializeField] private Image    _progressBar;
        [SerializeField] private TMP_Text _progressText;
        [SerializeField] private Image    _inputBlocker;

        public async Task Show()
        {
            UpdateProgress(0f);
            gameObject.SetActive(true);
            bool complete = false;
            _inputBlocker.DOFade(0.3f, 0.5f).OnComplete(() => complete = true);

            while(!complete)
            {
                await Task.Yield();
            }
        }

        public void Hide()
        {
            _inputBlocker.DOFade(0f, 0.5f).OnComplete(() => gameObject.SetActive(false));
        }

        public void UpdateProgress(float percentage)
        {
            _progressBar.fillAmount = percentage;
            _progressText.text      = $"{percentage * 100f:0.00}%";
        }
    }
}