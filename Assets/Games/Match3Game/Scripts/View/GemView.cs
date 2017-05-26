using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
	[RequireComponent(typeof(Image))]
	public class GemView : MonoBehaviour 
	{
		public RectTransform RectTransform { get; private set; }
		public Gem Gem { get; set; }

		private Image image;

		void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
			image = GetComponent<Image>();		
		}

		public void RefreshView()
		{
			image.sprite = GameManager.Instance.spriteManager.GetGemSprite(Gem.color);
		}
	}
}