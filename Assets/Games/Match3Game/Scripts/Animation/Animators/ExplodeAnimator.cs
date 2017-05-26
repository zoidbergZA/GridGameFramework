using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using Prime31.ZestKit;

namespace Match3
{
	public class ExplodeAnimator : AAnimator
	{
		public Match3Game game;
		public TextAnimator textAnimator;

		public override bool HandlePlayback(AAnimation anim)
		{
			ExplodeAnimation explodeAnim = anim as ExplodeAnimation;

			if (explodeAnim == null)
				return false;

			for (int i = 0; i < explodeAnim.explodeCells.Length; i++)
			{
				Vec2 cell = explodeAnim.explodeCells[i];
				var gemView = boardView.FieldViews[cell.x, cell.y].GemView;

				gemView.RectTransform
					.ZKlocalScaleTo(Vector3.one * 2, Duration)
					.setCompletionHandler( x => { DestroyView(gemView); } )
					.start();
			
				int points = explodeAnim.points / explodeAnim.explodeCells.Length;
				var textAnim = new TextAnimation(cell, "+" + points, 0.95f);
				textAnimator.HandlePlayback(textAnim);
			}

			game.ScoreKeeper.AddScore(explodeAnim.points);

			return true;
		}

		private void DestroyView(GemView v)
		{
			Destroy(v.gameObject);
		}
	}
}
