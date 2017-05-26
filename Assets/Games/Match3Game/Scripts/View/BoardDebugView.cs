using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;

namespace Match3
{
	public class BoardDebugView : MonoBehaviour 
	{
		public delegate void evtHandler(LayerDebugView[] debugLayers);
		public static event evtHandler LayerDebuggersInitialized;

		public LayerDebugView protoLayerView;
		public float cellSize = 100f;
		
		private Vec2 boardSize;

		//layers
		private BoardLayer<Field> fieldsLayer;
		private BoardLayer<int> matchesLayer;
		private BoardLayer<int> candidatesLayer;
		private LayerDebugView[] debugLayers;
		private BoardLayer<TrickleState> trickleLayer;
		private BoardAlert[] lastTickAlerts = new BoardAlert[0];
		
		public LayerDebugView[] DebuggerLayers { get { return debugLayers; } }
		public int AlertIndex { get; private set; }

		public void InitView(BoardLayer<Field> fieldsLayer, BoardLayer<int> matchesLayer, BoardLayer<int> candidatesLayer, BoardLayer<TrickleState> trickleLayer)
		{
			boardSize = fieldsLayer.GetDimensions();
			this.fieldsLayer = fieldsLayer;
			this.matchesLayer = matchesLayer;
			this.trickleLayer = trickleLayer;
			this.candidatesLayer = candidatesLayer;

			InitLayerDebuggers();	
		}

		public void ClearLayerDebuggers()
		{
			for (int i = 0; i < debugLayers.Length; i++)
			{
				debugLayers[i].Clear();
			}
		}

		public void RefreshBoardAlerts(BoardAlert[] alerts)
		{
			AlertIndex = 0;
			lastTickAlerts = alerts;

			RefreshLayerDebuggers();
		}

		public void ShowNextAlert()
		{
			if (AlertIndex >= lastTickAlerts.Length)
				return;

			Debug.Log("show alert (" + (AlertIndex + 1) + "/" + lastTickAlerts.Length + ")");
			
			AlertIndex++;
		}

		private void InitLayerDebuggers()
		{
			//debugger layers
			LayerDebugView fieldLayerView = Instantiate(protoLayerView);
			LayerDebugView matchesLayerView = Instantiate(protoLayerView);
			LayerDebugView trickleLayerView = Instantiate(protoLayerView);
			LayerDebugView candidateLayerView = Instantiate(protoLayerView);

			debugLayers = new LayerDebugView[]
			{
				InitLayerView("Fields Layer", fieldLayerView, boardSize, new FieldsDebbugger(fieldLayerView, fieldsLayer)),
				InitLayerView("Matches Layer", matchesLayerView, boardSize, new MatchesDebugger(matchesLayerView, matchesLayer)),
				InitLayerView("Trickle Layer", trickleLayerView, boardSize, new TrickleDebugger(trickleLayerView, trickleLayer)),
				InitLayerView("Candidates Layer", candidateLayerView, boardSize, new CandidatesDebugger(candidateLayerView, candidatesLayer))
			};

			ClearLayerDebuggers();

			Destroy(protoLayerView.gameObject);

			if (LayerDebuggersInitialized != null)
			{
				LayerDebuggersInitialized(debugLayers);
			}
		}

		private void RefreshLayerDebuggers()
		{
			ClearLayerDebuggers();

			for (int i = 0; i < debugLayers.Length; i++)
			{
				debugLayers[i].Refresh();
			}
		}

		private LayerDebugView InitLayerView(string layerName, LayerDebugView view, Vec2 dimensions, LayerDebugger debugger)
		{
			view.transform.SetParent(protoLayerView.transform.parent);
			view.name = layerName;
			view.Init(layerName, dimensions, cellSize, debugger);

			return view;
		}
	}
}