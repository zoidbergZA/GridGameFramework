using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class AnimationController : MonoBehaviour 
	{
		public float animationSpeed = 1f;

		private AAnimator[] animators;
		private float maxDuration = 0f;
		private List<AAnimation> animQueue = new List<AAnimation>();

		public bool Playing { get; private set; }
		
		void Awake()
		{
			animators = GetComponents<AAnimator>();
		}

		public void QueueAnimation(AAnimation animation)
		{
			animQueue.Add(animation);
		}

		public float PlayAnimations()
		{
			if (animQueue.Count > 0)
			{
				StartCoroutine(HandlePlayback());
				return maxDuration;
			}
			else
			{
				return 0;
			}
		}

		public void Reset()
		{
			maxDuration = 0;
			animQueue.Clear();
			Playing = false;
		}

		private IEnumerator HandlePlayback()
		{
			Playing = true;
			Debug.Log("play animations(" + animQueue.Count + "): " + Time.time);

			float duration = 0f;

			//trigger animators
			foreach (var anim in animQueue)
			{
				foreach (var animator in animators)
				{
					var success = animator.HandlePlayback(anim);

					if (success)
					{
						duration = animator.Duration;
						break;
					}
				}
			
				maxDuration = Mathf.Max(duration, maxDuration);
			}

			yield return new WaitForSeconds(maxDuration);

			Reset();
			Debug.Log("finished animations:" + Time.time);
		}
	}	
}