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
		public float moveDuration = 0.3f;

		private List<AAnimation> animQueue = new List<AAnimation>();
		private float animationTime;

		public void QueueAnimation(AAnimation anim)
		{
			animQueue.Add(anim);
			animationTime = moveDuration;
		}

		public float PlayAnimations()
		{
			for (int i = 0; i < animQueue.Count; i++)
			{
				if (animQueue[i] is MoveAnimation)
				{
					MoveAnimation move = (MoveAnimation)animQueue[i];
					PlayMoveAnimation(move);
				}
				else if (animQueue[i] is MergeAnimation)
				{
					MergeAnimation merge = (MergeAnimation)animQueue[i];
					PlayMergeAnimation(merge);
				}
			}

			animQueue.Clear();
			animationTime = 0;
			return moveDuration;
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
			fromView.RectTransform
                .ZKanchoredPositionTo(boardView.GetBoardPosition(anim.to), moveDuration)
                .setEaseType(EaseType.Linear)
                .start();

			fromView.SetRank(anim.rank);
			boardView.DestroyTileView(toView);
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
