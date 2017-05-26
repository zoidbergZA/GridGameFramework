using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Match3
{
    public class GemMatcher : PatternFinder
    {
        public GemMatcher(Stencil[] stencils, BoardLayer<Field> fieldsLayer, BoardLayer<int> matchesLayer) 
            : base(stencils, fieldsLayer, matchesLayer)
        {

        }	

        protected override void TestMatchSample(Stencil kernel, Field[,] sample, BoardLayer<int> matchesLayer, List<MatchGroup> matches)
        {
            Field keyField = sample[kernel.Key.x, kernel.Key.y];

            if (keyField == null)
                return;
            if (keyField.Gem == null)
                return;
            if (keyField.Gem.color == GemColor.None)
                return;

            var matchColor = keyField.Gem.color;
            var group = new List<Field>();
            
            //get sample group
            for (int x = 0; x < sample.GetLength(0); x++)
            {
                for (int y = 0; y < sample.GetLength(1); y++)
                {
                    if (kernel.filter[x,y] == StencilFilter.Fixed || kernel.filter[x,y] == StencilFilter.Free)
                    {
                        group.Add(sample[x,y]);
                    }
                }
            }

            //validate sample group
            for (int i = 0; i < group.Count; i++)
            {
                if (group[i] == null)
                    return;
                if (group[i].Gem.color != matchColor)
                    return;
                if (matchesLayer.cells[group[i].position.x, group[i].position.y] > 0) //skip if cell already matched
                    return;
            }

            //add new match group
            AddNewMatchGroup(group.ToArray(), matchesLayer, matches);
        }

        private void AddNewMatchGroup(Field[] fields, BoardLayer<int> matchesLayer, List<MatchGroup> matches)
        {
            Vec2[] cells = new Vec2[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                cells[i] = fields[i].position;
            }

            foreach (var cell in cells)
            {
                matchesLayer.cells[cell.x, cell.y] = 1;
            }

            //add matchGroup
            var matchGroup = new MatchGroup(cells[0], cells, MatchType.LineMatch, fields[0].Gem.color);
            matches.Add(matchGroup);
        }
    }
}