using System;
using System.Collections;
using System.Collections.Generic;
using GridGame;

namespace Twenty48
{
    public enum GravityState { Open, Ready, Fixed }

    public class GravityProcessor : ControllerPhase
    {
        public BoardLayer<GravityState> gravityLayer;
        public BoardLayer<int> tileLayer;
        public TileAnimator tileAnimator;

        private BoardController<MoveDirection> controller;
        private MoveDirection moveDirection;
        private Crawler[] crawlers;
        private Vec2 boardSize;
        
        public int Moves { get; private set; }

        public GravityProcessor(BoardController<MoveDirection> controller, TileAnimator animator,
            BoardLayer<GravityState> gravityLayer, BoardLayer<int> tileLayer)
        {
            this.controller = controller;
            this.gravityLayer = gravityLayer;
            this.tileLayer = tileLayer;
            tileAnimator = animator;

            boardSize = gravityLayer.GetDimensions();
            crawlers = new Crawler[boardSize.x];

            for (int i = 0; i < crawlers.Length; i++)
            {
                crawlers[i] = new Crawler(this);
            }
        }

        public override void Start()
        {
            base.Start();

            moveDirection = controller.LastInput;
            RefreshGravityLayer();

            //set crawler positions
            Vec2[] positions = new Vec2[boardSize.x];

            switch (moveDirection)
            {
                case MoveDirection.Left:
                    for (int i = 0; i < crawlers.Length; i++)
                        crawlers[i].Init(new Vec2(0, i), new Vec2(1, 0));
                    break;
                case MoveDirection.Down:
                    for (int i = 0; i < crawlers.Length; i++)
                        crawlers[i].Init(new Vec2(i, 0), new Vec2(0, 1));
                    break;
                case MoveDirection.Right:
                    for (int i = 0; i < crawlers.Length; i++)
                        crawlers[i].Init(new Vec2(crawlers.Length-1, crawlers.Length-1 - i), new Vec2(-1, 0));
                    break;
                case MoveDirection.Up:
                    for (int i = 0; i < crawlers.Length; i++)
                        crawlers[i].Init(new Vec2(crawlers.Length-1 - i, crawlers.Length-1), new Vec2(0, -1));
                    break;
            }
        }

        public override void Reset()
        {
            base.Reset();

            Moves = 0;
        }

        public override BoardAlert[] Tick()
        {
            // RefreshGravityLayer();
            bool tickMoved = false;
 
            foreach (var crawler in crawlers)
            {
                crawler.Reset();

                while (crawler.State != Crawler.CrawlerState.Done)
                {
                    var movedTile = crawler.Crawl();

                    if (movedTile)
                    {
                        Moves++;
                        tickMoved = true;
                    }
                }
            }

            if (!tickMoved)
            {
                State = PhaseState.Done;
            }

            return null;
        }

        private void RefreshGravityLayer()
        {
            for (int x = 0; x < boardSize.x; x++)
            {
                for (int y = 0; y < boardSize.y; y++)
                {
                    if (tileLayer.cells[x, y] == 0)
                        gravityLayer.cells[x, y] = GravityState.Open;
                    else
                        gravityLayer.cells[x, y] = GravityState.Ready;
                }
            }
        }

        private class Crawler
        {
            public enum CrawlerState { Working, Done }

            private GravityProcessor processor;
            private Vec2 startPosition;
            private Vec2 position;
            private Vec2 direction;

            public CrawlerState State { get; private set; }

            public Crawler(GravityProcessor processor)
            {
                this.processor = processor;
            }

            public void Init(Vec2 position, Vec2 direction)
            {
                this.startPosition = position;
                this.direction = direction;

                Reset();
            }

            public void Reset()
            {
                position = startPosition;
                State = CrawlerState.Working;
            }

            public bool Crawl() //return tile moves
            {
                bool moved = false;
                var nextPosition = position + direction;

                if (!nextPosition.IsValidPosition(processor.gravityLayer))
                {
                    State = CrawlerState.Done;
                    return moved;
                }

                var currentCell = processor.gravityLayer.GetCell(position);
                var nextCell = processor.gravityLayer.GetCell(nextPosition);

                if (nextCell == GravityState.Ready)
                {
                    if (currentCell == GravityState.Open)
                    {
                        //move tile data
                        processor.tileLayer.cells[position.x, position.y] = processor.tileLayer.cells[nextPosition.x, nextPosition.y];
                        processor.gravityLayer.cells[position.x, position.y] = GravityState.Ready;

                        processor.tileLayer.cells[nextPosition.x, nextPosition.y] = 0;
                        processor.gravityLayer.cells[nextPosition.x, nextPosition.y] = GravityState.Open;

                        //animate
                        processor.tileAnimator.QueueAnimation(new MoveAnimation(nextPosition, position));

                        moved = true;
                    }
                    else
                    {
                        var thisRank = processor.tileLayer.GetCell(position);
                        var otherRank = processor.tileLayer.GetCell(nextPosition);

                        if (thisRank == otherRank)
                        {
                            //move tile data
                            var newRank = thisRank + 1;
                            processor.tileLayer.cells[position.x, position.y] = newRank;
                            processor.gravityLayer.cells[position.x, position.y] = GravityState.Fixed;
            
                            processor.tileLayer.cells[nextPosition.x, nextPosition.y] = 0;
                            processor.gravityLayer.cells[nextPosition.x, nextPosition.y] = GravityState.Open;

                            //animate
                            processor.tileAnimator.QueueAnimation(new MergeAnimation(nextPosition, position, newRank));

                            moved = true;
                        }
                    }
                }

                position = nextPosition;
                return moved;
            }
        }
    }
}
