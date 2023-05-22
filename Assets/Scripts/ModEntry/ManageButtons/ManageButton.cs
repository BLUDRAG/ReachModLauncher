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

        public void UpdateManageState(string text)
        {
            Manage.SetActive(true);
            Progress.SetActive(false);
            Text.text = text;
        }

        public void UpdateProgress(float percentage)
        {
            ProgressBar.localScale = new Vector3(percentage, 1f, 1f);
        }
    }
}