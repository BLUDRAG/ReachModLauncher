using DG.Tweening;
using UnityEngine;

namespace ReachModLauncher
{
    public class PreviewAnimator : MonoBehaviour
    {
        [SerializeField] private Transform _animationTarget;
        [SerializeField] private Transform _targetLocation;
        [SerializeField] private float     _animationTime = 0.15f;
        [SerializeField] private Transform _originalLocation;

        public void Enlarge()
        {
            DOTween.Kill(this);

            _animationTarget.DOMove(_targetLocation.position, _animationTime)
                            .SetEase(Ease.OutQuad)
                            .SetId(this);

            _animationTarget.DOScale(_targetLocation.localScale, _animationTime)
                            .SetEase(Ease.OutQuad)
                            .SetId(this);
        }
        
        public void Shrink()
        {
            DOTween.Kill(this);

            _animationTarget.DOLocalMove(_originalLocation.localPosition, _animationTime)
                            .SetEase(Ease.OutQuad)
                            .SetId(this);

            _animationTarget.DOScale(_originalLocation.localScale, _animationTime)
                            .SetEase(Ease.OutQuad)
                            .SetId(this);
        }
    }
}
