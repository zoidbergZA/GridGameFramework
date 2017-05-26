using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Match3
{
	public class LayerSelector : MonoBehaviour 
	{
		public Text nameText;
		public Toggle toggle;

		private LayerDebugView layerView;

		public bool IsDisabled { get { return layerView.gameObject.activeSelf; } }

		public void Init(LayerDebugView layerDebugView, bool startDisabled = true)
		{
			layerView = layerDebugView;

			name = layerView.name;
			nameText.text = name;

			toggle.onValueChanged.AddListener(onToggle);

			if (startDisabled)
			{
				toggle.isOn = false;
				layerView.gameObject.SetActive(false);
			}
		}

		private void onToggle(bool activated)
		{
			layerView.gameObject.SetActive(activated);
		}
	}
}