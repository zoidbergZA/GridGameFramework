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

		private List<MoveAnimation> animQueue = new List<MoveAnimation>();
		private float animationTime;

		public void QueueAnimation(MoveAnimation anim)
		{
			animQueue.Add(anim);
			animationTime = moveDuration;
		}

		public float PlayAnimations()
		{
			for (int i = 0; i < animQueue.Count; i++)
			{
				PlayMoveAnimation(animQueue[i]);
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
	}

	public struct MoveAnimation
	{
		public Vec2 from;
		public Vec2 to;

		public MoveAnimation(Vec2 from, Vec2 to)
		{
			this.from = from;
			this.to = to;
		}
	}
}
