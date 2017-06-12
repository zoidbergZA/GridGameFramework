using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellDebugView : MonoBehaviour 
{
	public Image cellBackground;
	public Text cellText;

	public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }
}
