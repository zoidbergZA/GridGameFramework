using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;
using UnityEngine;
using Prime31.ZestKit;

namespace Match3
{
    public class SwapAnimator : AAnimator
    {
        public override bool HandlePlayback(AAnimation swapAnim)
        {
            SwapAnimation anim = swapAnim as SwapAnimation;

            if (anim == null)
				return false;

            var fromFieldView = boardView.FieldViews[anim.from.x, anim.from.y];
            var fromGemView = fromFieldView.GemView;

            var toFieldView = boardView.FieldViews[anim.to.x, anim.to.y];
            var toGemView = toFieldView.GemView;

            var temp = toFieldView.GemView;
            toFieldView.SetGemView(fromGemView);
            fromFieldView.SetGemView(temp);

            fromGemView.RectTransform.ZKanchoredPositionTo(Vector2.zero, Duration).setEaseType(EaseType.CubicInOut).start();
            toGemView.RectTransform.ZKanchoredPositionTo(Vector2.zero, Duration).setEaseType(EaseType.CubicInOut).start();
        
            return true;
        }
    }
}