using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;
using Prime31.ZestKit;

namespace Match3
{
    public class SpawnAnimator : AAnimator
    {
        public override bool HandlePlayback(AAnimation spawnAnim)
        {
            SpawnAnimation anim = spawnAnim as SpawnAnimation;

            if (anim == null)
				return false;

            GemView gemView = boardView.CreateGemView(anim.position);

            gemView.RectTransform.localScale = Vector3.zero;
            gemView.RectTransform.ZKlocalScaleTo(Vector3.one, Duration).start();
        
            return true;
        }
    }
}