using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ReachModLauncher
{
	public class Molotov : MonoBehaviour, IPointerDownHandler
	{
		public Transform   MolotovRoot;
		public Image       MolotovImage;
		public GameObject  FireAnimation;
		public AudioSource AudioSource;
		public AudioClip   ThrowSound;
		public AudioClip   DropSound;
		public AudioClip   FireSound;
		public float       AnimationTime;
		public float       DropTime;
		public float       RotateAmount;

		private bool _active;

		public void OnPointerDown(PointerEventData eventData)
		{
			if(_active) return;
			StartCoroutine(Animate(eventData));
		}

		private IEnumerator Animate(PointerEventData eventData)
		{
			_active                                 = true;
			MolotovRoot.position                    = eventData.position;
			MolotovImage.transform.localScale       = Vector3.one;
			MolotovImage.transform.localEulerAngles = Vector3.zero;
			MolotovImage.enabled                    = true;

			AudioSource.clip   = ThrowSound;
			AudioSource.volume = 1f;
			AudioSource.Play();
			float delay = DropTime - DropSound.length + 0.2f;

			DOVirtual.DelayedCall(delay, () =>
			                             {
				                             AudioSource.clip = DropSound;
				                             AudioSource.Play();
			                             });

			MolotovImage.transform.DOLocalRotate(new Vector3(0f, 0f, RotateAmount), DropTime, RotateMode.FastBeyond360);
			yield return MolotovImage.transform.DOScale(Vector3.zero, DropTime)
			                         .WaitForCompletion();

			MolotovImage.enabled = false;

			yield return new WaitForSeconds(0.03f);
			
			FireAnimation.SetActive(true);
			AudioSource.clip = FireSound;
			AudioSource.Play();

			yield return new WaitForSeconds(AnimationTime);

			FireAnimation.SetActive(false);
			AudioSource.DOFade(0f, 0.1f);
			_active = false;
		}
	}
}