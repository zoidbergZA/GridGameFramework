using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
	public class LayerViewer : MonoBehaviour 
	{
		public LayerSelector protoLayerSelector;

		private LayerDebugView[] debugLayers;
		private LayerSelector[] layerSelectors;

		void OnEnable()
		{
			BoardDebugView.LayerDebuggersInitialized += OnLayerDebuggersInitialized;
		}

		void OnDisable()
		{
			BoardDebugView.LayerDebuggersInitialized -= OnLayerDebuggersInitialized;
		}

		private void Init()
		{
			layerSelectors = new LayerSelector[debugLayers.Length];

			for (int i = 0; i < debugLayers.Length; i++)
			{
				layerSelectors[i] = Instantiate(protoLayerSelector);
				layerSelectors[i].transform.SetParent(protoLayerSelector.transform.parent);
				layerSelectors[i].transform.localScale = Vector3.one;
				layerSelectors[i].Init(debugLayers[i]);
			}

			protoLayerSelector.gameObject.SetActive(false);
		}

		private void OnLayerDebuggersInitialized(LayerDebugView[] debugLayers)
		{
			this.debugLayers = debugLayers;
			Init();
		}
	}
}