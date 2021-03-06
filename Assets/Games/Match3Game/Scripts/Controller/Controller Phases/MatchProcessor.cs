﻿using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Match3
{
	public class MatchProcessor : ControllerPhase
	{
		public List<MatchGroup> matchGroups = new List<MatchGroup>();
		public BoardLayer<Field> fieldLayer;
		public BoardLayer<int> matchesLayer;

		private Matcher[] matchers;
		private int matcherIndex;
		private Board board;

		public int MatchCount { get { return matchGroups.Count; } }
		public Matcher[] Matchers { get { return matchers; } }

		public MatchProcessor(Board board, int fieldsLayerId, int matchesLayerId, int candidatesLayerId)
		{
			this.board = board;
			fieldLayer = board.GetLayer<Field>(fieldsLayerId);
			matchesLayer = board.GetLayer<int>(matchesLayerId);

			var candidatesLayer = board.GetLayer<int>(candidatesLayerId);

			matchers = new Matcher[] 
			{
				new Matcher(InitBlockShapes(), fieldLayer, matchesLayer, candidatesLayer), 	//block matcher
				new Matcher(InitLineShapes(), fieldLayer, matchesLayer, candidatesLayer) 		//line matcher
			};
		}

		public override void Reset()
		{
			base.Reset();

			matcherIndex = 0;
			matchGroups.Clear();

			//clear layer and reset matchers
			for (int i = 0; i < board.Size.x; i++)
			{
				for (int j = 0; j < board.Size.y; j++)
				{
					matchesLayer.cells[i,j] = 0;
				}
			}
		}

		public override BoardAlert[] Tick()
		{
			if (matcherIndex >= matchers.Length)
			{
				State = PhaseState.Done;
				return null;
			}

			var results = matchers[matcherIndex].FindMatches();

			if (results.Count > 0)
			{
				matchGroups.AddRange(results);
			}

			matcherIndex++;
			return null;
		}

		public void FindAllMatches()
		{
			Reset();
			Start();

			while (State != PhaseState.Done)
			{
				Tick();
			}
		}

		private Stencil[] InitLineShapes()
		{
			Stencil line3 = new Stencil(
			new StencilFilter[,] 
			{
				{ StencilFilter.Ignored, StencilFilter.Ignored,  StencilFilter.Option,    StencilFilter.Ignored },
				{ StencilFilter.Fixed, 	 StencilFilter.Fixed,    StencilFilter.Free,      StencilFilter.Option },
				{ StencilFilter.Ignored, StencilFilter.Ignored,  StencilFilter.Option,    StencilFilter.Ignored }
			});

			Stencil line4End = new Stencil(
			new StencilFilter[,] 
			{
				{ StencilFilter.Ignored, StencilFilter.Ignored, StencilFilter.Ignored,  StencilFilter.Option,    StencilFilter.Ignored },
				{ StencilFilter.Fixed,   StencilFilter.Fixed, 	StencilFilter.Fixed,    StencilFilter.Free,      StencilFilter.Option },
				{ StencilFilter.Ignored, StencilFilter.Ignored, StencilFilter.Ignored,  StencilFilter.Option,    StencilFilter.Ignored }
			});

			Stencil line4Center = new Stencil(
			new StencilFilter[,] 
			{
				{ StencilFilter.Ignored, StencilFilter.Ignored, StencilFilter.Option,  StencilFilter.Ignored,    StencilFilter.Ignored },
				{ StencilFilter.Fixed,   StencilFilter.Fixed, 	StencilFilter.Free,    StencilFilter.Fixed,      StencilFilter.Fixed },
				{ StencilFilter.Ignored, StencilFilter.Ignored, StencilFilter.Option,  StencilFilter.Ignored,    StencilFilter.Ignored }
			});

			return new Stencil[] { line4End, line4Center, line3 };
		}

		private Stencil[] InitBlockShapes()
		{
			Stencil block = new Stencil(
			new StencilFilter[,] 
			{
				{ StencilFilter.Fixed, 	 StencilFilter.Fixed,  	StencilFilter.Ignored },
				{ StencilFilter.Fixed, 	 StencilFilter.Free,    StencilFilter.Option },
				{ StencilFilter.Ignored, StencilFilter.Option,  StencilFilter.Ignored }
			});

			Stencil blockFlipped = new Stencil(
			new StencilFilter[,] 
			{
				{ StencilFilter.Ignored, StencilFilter.Option,  StencilFilter.Ignored },
				{ StencilFilter.Fixed, 	 StencilFilter.Free,    StencilFilter.Option },
				{ StencilFilter.Fixed, 	 StencilFilter.Fixed,  	StencilFilter.Ignored }
			});
		
			return new Stencil[] { block, blockFlipped };
		}
	}
}