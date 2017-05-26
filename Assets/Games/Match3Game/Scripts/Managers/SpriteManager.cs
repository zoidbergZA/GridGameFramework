using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
	public class SpriteManager : MonoBehaviour 
	{
		public GemSprites gemSprites;

		public Sprite GetGemSprite(GemColor gemColor)
		{
			Sprite sprite = null;

			switch (gemColor)
			{
				case GemColor.Blue :
					sprite = gemSprites.blue;
					break;
				case GemColor.Lila :
					sprite = gemSprites.lila;
					break;
				case GemColor.Orange :
					sprite = gemSprites.orange;
					break;
				case GemColor.Pink :
					sprite = gemSprites.pink;
					break;
				case GemColor.Yellow :
					sprite = gemSprites.yellow;
					break;
			}

			return sprite;
		}
	}	
}