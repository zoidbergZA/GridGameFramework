using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
	public class LayerSelector : MonoBehaviour 
	{
		public Button selectorButton;
		public Text buttonText;
		// public Toggle toggle;

		// public bool IsDisabled { get { return layerView.gameObject.activeSelf; } }

		public void Init(string name)
		{
			// layerView = layerDebugView;

			// name = layerView.name;
			buttonText.text = name;

			// toggle.onValueChanged.AddListener(onToggle);

			// if (startDisabled)
			// {
			// 	toggle.isOn = false;
			// 	layerView.gameObject.SetActive(false);
			// }
		}

		// private void onToggle(bool activated)
		// {
		// 	// layerView.gameObject.SetActive(activated);
		// }
	}
}