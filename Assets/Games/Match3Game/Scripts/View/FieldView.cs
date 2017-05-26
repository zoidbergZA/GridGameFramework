using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class FieldView : MonoBehaviour 
	{
		public GemView GemView { get; private set; }

		private float fieldSize;

		public RectTransform RectTransform { get; private set; }
		public Field Field { get; set; }

		private void Awake()
		{
			RectTransform = GetComponent<RectTransform>();
		}

		public void InitView(float fieldSize, Field field)
		{
			this.fieldSize = fieldSize;
			Field = field;
		}

		public void SetPosition(Vec2 gridPosition)
		{
			RectTransform.anchoredPosition = new Vector2(gridPosition.x, gridPosition.y) * fieldSize;
		}

		public void SetGemView(GemView view)
		{
			GemView = view;

			if (view != null)
				view.transform.SetParent(this.transform);
		}
	}
}