using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GridGame;

namespace Match3
{
	public class LayerSelector : MonoBehaviour 
	{
		public Button selectorButton;
		public Text buttonText;

		private LayerViewer layerViewer;
		private IGenericLayer layer;

		public void Init(LayerViewer viewer, IGenericLayer layer)
		{
			layerViewer = viewer;
			this.layer = layer;

			buttonText.text = layer.GetLayerName();

			selectorButton.onClick.AddListener(OnClick);
		}

		private void OnClick()
		{
			layerViewer.RefreshView(layer);
		}
	}
}