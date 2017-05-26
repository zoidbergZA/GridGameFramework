using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
	public class FloatingText : MonoBehaviour 
	{
		public Text text;

		public RectTransform RectTransform { get { return GetComponent<RectTransform>(); } }

		public void Init(string text)
		{
			this.text.text = text;
		}
	}
}