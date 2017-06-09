using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridGame;

public class TileView : MonoBehaviour 
{
	public Image containerImage;
	public Text ValueText;

	public Vec2 BoardPosition { get; set; }
	public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }

	public void SetValue(int value)
	{
		ValueText.text = Mathf.Pow(2, value).ToString();
	}
}
