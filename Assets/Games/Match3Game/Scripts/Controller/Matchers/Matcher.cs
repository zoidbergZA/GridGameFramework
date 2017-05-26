using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Match3
{
	public class Matcher
	{
		private Stencil[] shapes;
		private Stencil[] matchStencils;
		private Vec2 boardSize;
		private BoardLayer<Field> fieldsLayer;
		private BoardLayer<int> matchesLayer;
		private BoardLayer<int> candidatesLayer;
		private PatternFinder gemMatcher;
		private PatternFinder candidateMatcher;

		public Matcher(Stencil[] shapes, BoardLayer<Field> fieldsLayer, BoardLayer<int> matchesLayer, BoardLayer<int> candidatesLayer)
		{
			this.shapes = shapes;
			this.boardSize = fieldsLayer.GetDimensions();
			this.fieldsLayer = fieldsLayer;
			this.matchesLayer = matchesLayer;
			this.candidatesLayer = candidatesLayer;

			InitStencils();

			gemMatcher = new GemMatcher(matchStencils, fieldsLayer, matchesLayer);
			candidateMatcher = new CandidateMatcher(matchStencils, fieldsLayer, candidatesLayer);
		}

		public virtual List<MatchGroup> FindMatches()
		{
			return gemMatcher.ScanMatches();
		}

		public virtual List<MatchGroup> FindCandidates()
		{
			return candidateMatcher.ScanMatches();
		}

		private void InitStencils()
		{
			var stencils = new List<Stencil>();

			for (int i = 0; i < shapes.Length; i++)
			{
				var rotated90 = shapes[i].Rotate90();
				var rotated180 = rotated90.Rotate90();
				var rotated270 = rotated180.Rotate90();

				stencils.Add(shapes[i]);
				stencils.Add(rotated90);
				stencils.Add(rotated180);
				stencils.Add(rotated270);
			}

			matchStencils = stencils.ToArray();
		}
	}
}