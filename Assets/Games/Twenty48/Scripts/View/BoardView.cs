using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Twenty48
{
	public class BoardView : MonoBehaviour 
	{
		public RectTransform protoFieldView;
		public TileView protoTileView;
		
		private RectTransform boardTransform;
		private Vector2 fieldSize;
		private Vec2 boardDimmensions;

		public List<TileView> TileViews { get; private set; }

		void Awake()
		{
			TileViews = new List<TileView>();
			boardTransform = GetComponent<RectTransform>();
			fieldSize = protoFieldView.sizeDelta;
		}

		public void Init(Vec2 size)
		{
			boardDimmensions = size;

			boardTransform.sizeDelta = new Vector2(fieldSize.x * size.x, fieldSize.y * size.y);

			for (int i = 0; i < size.x; i++)
			{
				for (int j = 0; j < size.y; j++)
				{
					var fieldView = Instantiate(protoFieldView) as RectTransform;
					fieldView.SetParent(protoFieldView.transform.parent);
					fieldView.localScale = Vector3.one;
					fieldView.anchoredPosition = new Vector2(i * fieldSize.x, j * fieldSize.y);
					fieldView.name = "FieldView [" + i + ", " + j + "]";
				}
			}

			protoFieldView.gameObject.SetActive(false);
			protoTileView.gameObject.SetActive(false);
		}

		public TileView CreateTileView(Vec2 position, int value = 1)
		{
			var tileView = Instantiate(protoTileView) as TileView;

			tileView.RectTransform.SetParent(protoTileView.transform.parent);
			tileView.RectTransform.localScale = Vector3.one;
			tileView.RectTransform.anchoredPosition = new Vector2(position.x * fieldSize.x, position.y * fieldSize.y);
			tileView.name = "TileView [" + position.x + ", " + position.y + "]";
			tileView.BoardPosition = position;
			tileView.SetValue(value);

			TileViews.Add(tileView);
			tileView.gameObject.SetActive(true);

			return tileView;
		}
	}
}