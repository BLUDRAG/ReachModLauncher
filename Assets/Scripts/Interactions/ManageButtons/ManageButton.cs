using TMPro;
using UnityEngine;

namespace ReachModLauncher
{
    public abstract class ManageButton : MonoBehaviour
    {
        public GameObject Manage;
        public TMP_Text   StateText;
        public GameObject Progress;
        public Transform  ProgressBar;
        public TMP_Text   ProgressText;

        public abstract void OnClick();

        public void UpdateManageState(ManageButtonStates state)
        {
            Manage.SetActive(true);
            Progress.SetActive(false);
            StateText.text = state.ToString();
        }

        public void UpdateProgress(float percentage)
        {
            Manage.SetActive(false);
            Progress.SetActive(true);
            ProgressBar.localScale = new Vector3(percentage, 1f, 1f);
            ProgressText.text = $"{(int)(percentage * 100f)}%";
        }
    }
}