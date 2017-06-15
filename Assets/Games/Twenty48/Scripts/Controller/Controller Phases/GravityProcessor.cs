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

        private BoardController<MoveDirection> controller;
        private MoveDirection moveDirection;
        private Crawler[] crawlers;
        private Vec2 boardSize;
        
        public int Moves { get; set; }

        public GravityProcessor(BoardController<MoveDirection> controller, 
            BoardLayer<GravityState> gravityLayer, BoardLayer<int> tileLayer)
        {
            this.controller = controller;
            this.gravityLayer = gravityLayer;
            this.tileLayer = tileLayer;

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
            RefreshGravityLayer();
            Moves = 0;
 
            foreach (var crawler in crawlers)
            {
                crawler.Reset();

                while (crawler.State != Crawler.CrawlerState.Done)
                {
                    crawler.Crawl();
                }
            }

            if (Moves == 0)
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

            public void Crawl()
            {
                var nextPosition = position + direction;

                if (!nextPosition.IsValidPosition(processor.gravityLayer))
                {
                    State = CrawlerState.Done;
                    return;
                }

                var currentCell = processor.tileLayer.GetCell(position);
                var nextCell = processor.tileLayer.GetCell(nextPosition);

                if (nextCell > 0)
                {
                    if (currentCell == 0)
                    {
                        //move tile
                        processor.tileLayer.cells[position.x, position.y] = processor.tileLayer.cells[nextPosition.x, nextPosition.y];
                        processor.tileLayer.cells[nextPosition.x, nextPosition.y] = 0;

                        //todo: animate view
                        processor.Moves++;
                    }
                    else if (currentCell == nextCell)
                    {
                        //todo: score match
                        processor.Moves++;
                    }
                }

                position = nextPosition;
            }
        }
    }
}
