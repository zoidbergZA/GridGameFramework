using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;
using Prime31.ZestKit;

namespace Match3
{
	public class TextAnimator : AAnimator
	{
		public FloatingText floatingTextPrefab;

		public override bool HandlePlayback(AAnimation animation)
		{
			TextAnimation anim = (TextAnimation)animation;

			if (anim == null)
				return false;

			var floatingText = Instantiate(floatingTextPrefab);
			floatingText.Init(anim.text);

			var halfSize = boardView.fieldSize;
			var offset = floatingText.RectTransform.sizeDelta * 0.5f;
			var pos = (Vector2)anim.cell * boardView.fieldSize + new Vector2(halfSize, halfSize) - offset;

			floatingText.transform.SetParent(boardView.boardPanel);
			floatingText.transform.localScale = Vector3.one;
			floatingText.RectTransform.anchoredPosition = pos;

			floatingText.RectTransform
					.ZKanchoredPositionTo(pos + Vector2.up * boardView.fieldSize, anim.duration)
					.setCompletionHandler( x => { Destroy(floatingText.gameObject); } )
					.setEaseType(EaseType.CircOut)
					.start();
		
			return true;
		}
	}
}