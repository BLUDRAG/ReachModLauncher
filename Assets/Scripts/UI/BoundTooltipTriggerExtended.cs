///Credit Martin Nerurkar // www.martin.nerurkar.de // www.sharkbombs.com
///Sourced from - http://www.sharkbombs.com/2015/02/10/tooltips-with-the-new-unity-ui-ugui/

using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
    [AddComponentMenu("UI/Extensions/Bound Tooltip/Bound Tooltip Trigger Extended")]
    public sealed class BoundTooltipTriggerExtended : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler,
                                                      IDeselectHandler
    {
        [TextAreaAttribute]
        public string text;

        public bool useMousePosition = true;

        public Vector2 offset;

        private RectTransform _rectTransform;

        private void Start()
        {
            _rectTransform = (RectTransform)transform;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(useMousePosition)
            {
                StartHover(GetRelativePosition(new Vector2(eventData.position.x, eventData.position.y)));
            }
            else
            {
                StartHover(GetRelativePosition(transform.position) + offset);
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            StartHover(transform.position);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopHover();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            StopHover();
        }

        void StartHover(Vector3 position)
        {
            BoundTooltipItemExtended.Instance.ShowTooltip(text, position);
        }

        void StopHover()
        {
            BoundTooltipItemExtended.Instance.HideTooltip();
        }

        private Vector2 GetRelativePosition(Vector2 screenPoint)
        {
            Vector2 relativePosition = transform.position;
            Rect    rect             = _rectTransform.rect;
            Vector2 screenHalves     = new Vector2(Screen.width / 2f, Screen.height / 2f);

            if(screenPoint.x < screenHalves.x && screenPoint.y > screenHalves.y) // Top Left
            {
                relativePosition.x = rect.xMax;
                relativePosition.y = rect.yMin;
            }
            else if(screenPoint.x > screenHalves.x && screenPoint.y > screenHalves.y) // Top Right
            {
                relativePosition.x = rect.xMin;
                relativePosition.y = rect.yMin;
            }
            else if(screenPoint.x < screenHalves.x && screenPoint.y < screenHalves.y) // Bottom Left
            {
                relativePosition.x = rect.xMax;
                relativePosition.y = rect.yMax;
            }
            else // Bottom Right
            {
                relativePosition.x = rect.xMin;
                relativePosition.y = rect.yMax;
            }

            return _rectTransform.TransformPoint(relativePosition);
        }
    }
}