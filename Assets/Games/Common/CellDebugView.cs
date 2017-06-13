using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellDebugView : MonoBehaviour 
{
	public Image cellBackground;
	public Text cellText;

	public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }

	public void Refresh(Color color, string text = null, float opacity = 1f)
	{
		color.a = opacity;
		cellBackground.color = color;

		if (text != null)
			cellText.text = text;
		else
			cellText.text = "";
	}
}
