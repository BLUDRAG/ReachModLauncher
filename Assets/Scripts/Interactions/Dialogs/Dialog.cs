using System.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ReachModLauncher
{
    public abstract class Dialog : MonoBehaviour
    {
        [SerializeField] protected Image    _progressBar;
        [SerializeField] protected TMP_Text _progressText;
        [SerializeField] private   Image    _inputBlocker;

        public virtual async Task Show()
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

        public virtual async Task Hide()
        {
            bool complete = false;
            
            _inputBlocker.DOFade(0f, 0.5f)
                         .OnComplete(() =>
                                     {
                                         gameObject.SetActive(false);
                                         complete = true;
                                     });
            
            while(!complete)
            {
                await Task.Yield();
            }
        }

        public abstract void UpdateProgress(float percentage);
    }
}