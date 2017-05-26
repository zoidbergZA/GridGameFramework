using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class BoardView : MonoBehaviour 
	{
		public float fieldSize = 100f;
		public AnimationController animationController;
		public RectTransform boardPanel;
		public FieldView protoFieldView;
		public GemView gemViewPrefab;

		private Vec2 boardSize;
		private BoardLayer<Field> fieldsLayer;
		private FieldView[,] fieldViews;

		public FieldView[,] FieldViews { get { return fieldViews; } }

		public void InitView(BoardLayer<Field> fieldsLayer)
		{
			this.fieldsLayer = fieldsLayer;
			boardSize = fieldsLayer.GetDimensions();

			InitFieldViews();
			InitGemViews();
		}

		public GemView CreateGemView(Vec2 position)
		{
			FieldView fieldView = fieldViews[position.x, position.y];
			GemView gemView = Instantiate(gemViewPrefab);
						
			fieldView.SetGemView(gemView);
			gemView.RectTransform.localScale = Vector3.one;
			gemView.RectTransform.anchoredPosition = Vector2.zero;

			gemView.Gem = fieldsLayer.cells[position.x,position.y].Gem;
			gemView.RefreshView();

			return gemView;
		}

		private void InitFieldViews()
		{
			fieldViews = new FieldView[boardSize.x, boardSize.y];

			for (int i = 0; i < fieldViews.GetLength(0); i++)
			{
				for (int j = 0; j < fieldViews.GetLength(1); j++)
				{
					FieldView fieldView = Instantiate(protoFieldView);
					fieldView.transform.SetParent(protoFieldView.transform.parent);
					fieldView.transform.localScale = Vector3.one;
					fieldView.name = "FieldView [" + i + ", " + j + "]";
					fieldView.InitView(fieldSize, fieldsLayer.cells[i,j]);
					fieldView.SetPosition(new Vec2(i,j));

					fieldViews[i,j] = fieldView;
				}	
			}

			protoFieldView.gameObject.SetActive(false);
		}

		private void InitGemViews()
		{
			for (int i = 0; i < fieldViews.GetLength(0); i++)
			{
				for (int j = 0; j < fieldViews.GetLength(1); j++)
				{				
					if (fieldsLayer.cells[i, j].Gem != null)
					{
						CreateGemView(new Vec2(i,j));					
					}
				}	
			}
		}
	}
}