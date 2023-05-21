using System.Collections.Generic;
using UnityEngine;

namespace ReachModLauncher
{
    public class DropdownObjectToggle : MonoBehaviour
    {
        public List<GameObject> Objects = new List<GameObject>();

        public void Toggle(int index)
        {
            for(int i = 0, condition = Objects.Count; i < condition; i++)
            {
                Objects[i].SetActive(i == index);
            }
        }
    }
}
