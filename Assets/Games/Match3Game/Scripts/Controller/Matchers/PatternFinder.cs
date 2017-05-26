using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Match3
{
	public abstract class PatternFinder 
	{
		private Stencil[] stencils;
		private Vec2 boardSize;
		private BoardLayer<Field> fieldsLayer;
		private BoardLayer<int> resultsLayer;

		public PatternFinder(Stencil[] stencils, BoardLayer<Field> fieldsLayer, BoardLayer<int> resultsLayer)
		{
			this.stencils = stencils;
			this.fieldsLayer = fieldsLayer;
			this.resultsLayer = resultsLayer;

			boardSize = fieldsLayer.GetDimensions();
		}

		public List<MatchGroup> ScanMatches()
		{
			List<MatchGroup> matches = new List<MatchGroup>();
			
			foreach (var kernel in stencils)
			{
				Vec2 kernelSize = kernel.Size;
				Vec2 min = new Vec2(kernelSize.x-1, kernelSize.y-1) * -1;    
			
				for (int i = min.x; i < boardSize.x; i++)
				{
					for (int j = min.y; j < boardSize.y; j++)
					{
						HandleMatchSearch(new Vec2(i, j), kernel, matches);
					}
				}
			}
			return matches;
		}

		protected abstract void TestMatchSample(Stencil kernel, Field[,] sample, BoardLayer<int> resultsLayer, List<MatchGroup> matches);

		private void HandleMatchSearch(Vec2 cell, Stencil stencil, List<MatchGroup> matches)
		{
			var sample = fieldsLayer.GetSample(stencil, cell);

			TestMatchSample(stencil, sample, resultsLayer, matches);
		}
	}
}