using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridGame;

public class TileView : MonoBehaviour 
{
	public Image containerImage;
	public Text ValueText;
	public Color[] rankColors;

	public Vec2 BoardPosition { get; set; }
	public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }

	public void SetRank(int rank)
	{
		int rankIndex = rank-1;
		ValueText.text = Mathf.Pow(2, rank).ToString();

		if (rankIndex < rankColors.Length)
		{
			containerImage.color = rankColors[rankIndex];
		}
		else
		{
			var index = rankIndex % rankColors.Length;
			containerImage.color = rankColors[index];
		}
	}
}
