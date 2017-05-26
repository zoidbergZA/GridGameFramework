using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public abstract class AAnimator : MonoBehaviour 
	{
		public BoardView boardView;

		[SerializeField]private float duration = 1f;

		public float Duration { get { return duration * (1f / boardView.animationController.animationSpeed); } }

		public abstract bool HandlePlayback(AAnimation animation);
	}	
}