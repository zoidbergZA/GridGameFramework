using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;
using Prime31.ZestKit;

namespace Match3
{
    public class TrickleAnimator : AAnimator
    {
        public override bool HandlePlayback(AAnimation trickleAnim)
        {
            TrickleAnimation anim = trickleAnim as TrickleAnimation;

            if (anim == null)
				return false;

            var fromFieldView = boardView.FieldViews[anim.from.x, anim.from.y];
            var fromGemView = fromFieldView.GemView;

            var toFieldView = boardView.FieldViews[anim.to.x, anim.to.y];

            fromFieldView.SetGemView(null);
            toFieldView.SetGemView(fromGemView);

            toFieldView.GemView.RectTransform
                .ZKanchoredPositionTo(Vector2.zero, Duration)
                .setEaseType(EaseType.Linear)
                .start();

            return true;
        }
    }
}