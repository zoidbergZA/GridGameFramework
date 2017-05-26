using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GridGame;

namespace Match3
{
    public class CandidateMatcher : PatternFinder
    {
        public CandidateMatcher(Stencil[] stencils, BoardLayer<Field> fieldsLayer, BoardLayer<int> candidatesLayer)
            : base(stencils, fieldsLayer, candidatesLayer)
        {

        }

        protected override void TestMatchSample(Stencil kernel, Field[,] sample, BoardLayer<int> resultsLayer, List<MatchGroup> matches)
        {
            Field keyField = sample[kernel.Key.x, kernel.Key.y];

            if (keyField == null)
                return;
            if (keyField.Gem == null)
                return;

            var matchColor = keyField.Gem.color;
            var fixedGroup = new List<Field>();
            Field option = null;
            Field free = null;

            //get test samples
            for (int x = 0; x < sample.GetLength(0); x++)
            {
                for (int y = 0; y < sample.GetLength(1); y++)
                {
                    if (kernel.filter[x,y] == StencilFilter.Fixed)
                    {
                        fixedGroup.Add(sample[x,y]);
                    }
                    else if (kernel.filter[x,y] == StencilFilter.Option)
                    {
                        var field = sample[x,y];
                        if (field != null)
                        {
                            if (field.Gem != null)
                            {
                                if (field.Gem.color == matchColor)
                                {
                                    option = field;
                                }
                            }
                        }
                    }
                    else if (kernel.filter[x,y] == StencilFilter.Free)
                    {
                        free = sample[x,y];
                    }
                }
            }

            for (int i = 0; i < fixedGroup.Count; i++)
            {
                if (fixedGroup[i] == null)
                    return;

                var pos = fixedGroup[i].position;
                if (fixedGroup[i].Gem.color != matchColor)
                    return;
            }

            if (option == null || free == null)
                return;

            matches.Add(CreateNewCandidateGroup(resultsLayer, fixedGroup, free, option));
        }

        private MatchGroup CreateNewCandidateGroup(BoardLayer<int> candidatesLayer, List<Field> fixedFields, Field free, Field option)
        {
            Vec2[] fixedCells = new Vec2[fixedFields.Count];
            Vec2 freeCell = free.position;
            Vec2 optionCell = option.position;

            for (int i = 0; i < fixedFields.Count; i++)
            {
                fixedCells[i] = fixedFields[i].position;
            }

            //mark cells on candidates layer
            foreach (var cell in fixedCells)
            {
                candidatesLayer.cells[cell.x, cell.y] = 1;
            }
            candidatesLayer.cells[freeCell.x, freeCell.y] = 2;
            candidatesLayer.cells[optionCell.x, optionCell.y] = 3;

            var candidateCells = fixedCells.ToList();
            candidateCells.Add(freeCell);
            candidateCells.Add(optionCell);

            //return candidate group
            return new MatchGroup(optionCell, candidateCells.ToArray(), MatchType.LineMatch, option.Gem.color);
        }
    }
}