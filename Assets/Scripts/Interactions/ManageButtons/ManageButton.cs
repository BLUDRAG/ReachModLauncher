using TMPro;
using UnityEngine;

namespace ReachModLauncher
{
    public class ManageButton : MonoBehaviour
    {
        public GameObject Manage;
        public TMP_Text   Text;
        public GameObject Progress;
        public Transform  ProgressBar;

        public virtual void OnClick()
        {
        }

        public void UpdateManageState(ManageButtonStates state)
        {
            Manage.SetActive(true);
            Progress.SetActive(false);
            Text.text = state.ToString();
        }

        public void UpdateProgress(float percentage)
        {
            Manage.SetActive(false);
            Progress.SetActive(true);
            ProgressBar.localScale = new Vector3(percentage, 1f, 1f);
        }
    }
}