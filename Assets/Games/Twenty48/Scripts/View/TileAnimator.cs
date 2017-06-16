using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using Prime31.ZestKit;

namespace Twenty48
{
	public class TileAnimator : MonoBehaviour 
	{
		public BoardView boardView;
		public float moveDuration = 1f;

		private List<AAnimation> animQueue = new List<AAnimation>();
		private float animationTime;

		public bool IsPlaying { get; private set; }

		public void QueueAnimation(AAnimation anim)
		{
			animQueue.Add(anim);
			animationTime = moveDuration;
		}

		public IEnumerator PlayAnimations()
		{
			if (animQueue.Count == 0)
				yield return null;
			else
			{
				IsPlaying = true;

				for (int i = 0; i < animQueue.Count; i++)
				{
					if (animQueue[i] is MergeAnimation)
					{
						MergeAnimation merge = (MergeAnimation)animQueue[i];
						PlayMergeAnimation(merge);
					}

					else if (animQueue[i] is MoveAnimation)
					{
						MoveAnimation move = (MoveAnimation)animQueue[i];
						PlayMoveAnimation(move);
					}					
				}

				yield return new WaitForSeconds(animationTime);

				animQueue.Clear();
				animationTime = 0;
				IsPlaying = false;
			}
		}

		private void PlayMoveAnimation(MoveAnimation anim)
		{
			var tileView = boardView.GetTileView(anim.from);

			tileView.BoardPosition = anim.to;
			tileView.RectTransform
                .ZKanchoredPositionTo(boardView.GetBoardPosition(anim.to), moveDuration)
                .setEaseType(EaseType.Linear)
                .start();
		}

		private void PlayMergeAnimation(MergeAnimation anim)
		{
			var fromView = boardView.GetTileView(anim.from);
			var toView = boardView.GetTileView(anim.to);

			fromView.BoardPosition = anim.to;
			toView.BoardPosition = Vec2.invalid;
			fromView.RectTransform
                .ZKanchoredPositionTo(boardView.GetBoardPosition(anim.to), moveDuration)
                .setEaseType(EaseType.Linear)
                .start();

			fromView.SetRank(anim.rank);

			//bug: null ref error if no delay on Destroy() call
			boardView.DestroyTileView(toView, moveDuration * 0.5f);
		}
	}

	public abstract class AAnimation
	{

	}

	public class MoveAnimation : AAnimation
	{
		public Vec2 from;
		public Vec2 to;

		public MoveAnimation(Vec2 from, Vec2 to)
		{
			this.from = from;
			this.to = to;
		}
	}

	public class MergeAnimation : AAnimation
	{
		public Vec2 from;
		public Vec2 to;
		public int rank;

		public MergeAnimation(Vec2 from, Vec2 to, int rank)
		{
			this.from = from;
			this.to = to;
			this.rank = rank;
		}
	}
}
