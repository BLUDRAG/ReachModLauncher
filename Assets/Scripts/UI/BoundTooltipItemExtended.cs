///Credit Martin Nerurkar // www.martin.nerurkar.de // www.sharkbombs.com
///Sourced from - http://www.sharkbombs.com/2015/02/10/tooltips-with-the-new-unity-ui-ugui/

using TMPro;

namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("UI/Extensions/Bound Tooltip/Bound Tooltip Item Extended")]
    public sealed class BoundTooltipItemExtended : MonoBehaviour
    {
        public bool IsActive
        {
            get
            {
                return gameObject.activeSelf;
            }
        }

        public TMP_Text  TooltipText;
        public Vector3   ToolTipOffset;
        public Transform TopLeft;
        public Transform TopRight;
        public Transform BottomLeft;
        public Transform BottomRight;

        void Awake()
        {
            instance = this;
            if(!TooltipText) TooltipText = GetComponentInChildren<TMP_Text>();
            HideTooltip();
        }

        public void ShowTooltip(string text, Vector3 pos)
        {
            if(TooltipText.text != text)
                TooltipText.text = text;

            Vector2   screenPoint  = pos;
            Vector2   screenHalves = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Transform anchor;
            
            if(screenPoint.x < screenHalves.x && screenPoint.y > screenHalves.y) // Top Left
            {
                anchor = TopLeft;
            }
            else if(screenPoint.x > screenHalves.x && screenPoint.y > screenHalves.y) // Top Right
            {
                anchor = TopRight;
            }
            else if(screenPoint.x < screenHalves.x && screenPoint.y < screenHalves.y) // Bottom Left
            {
                anchor = BottomLeft;
            }
            else // Bottom Right
            {
                anchor = BottomRight;
            }
            
            transform.position = pos + ToolTipOffset - transform.InverseTransformPoint(anchor.position);

            gameObject.SetActive(true);
        }

        public void HideTooltip()
        {
            gameObject.SetActive(false);
        }

        // Standard Singleton Access
        private static BoundTooltipItemExtended instance;

        public static BoundTooltipItemExtended Instance
        {
            get
            {
                if(instance == null)
                    instance = FindObjectOfType<BoundTooltipItemExtended>();

                return instance;
            }
        }
    }
}