using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GridGame;
using System;

namespace Match3
{
	public enum TrickleState
	{
		Default = 0,
		Open = 1,
		Fixed = 2,
		Falling = 3,
		Landed = 4
	}

	public class Trickler : ControllerPhase
	{
		private BoardController<SwapInput> controller;
		private MatchProcessor matcher;
		private MatchResolver resolver;
		private Board board;
		private BoardLayer<Field> fieldsLayer;
		private BoardLayer<TrickleState> trickleLayer;
		private AnimationController animController;
		
		public Trickler(BoardController<SwapInput> controller, Board board, int fieldsLayerId, int trickleLayerId, 
			MatchProcessor matcher, MatchResolver resolver, AnimationController animController)
		{
			this.controller = controller;
			this.matcher = matcher;
			this.resolver = resolver;
			this.board = board;
			fieldsLayer = board.GetLayer<Field>(fieldsLayerId);
			trickleLayer = board.GetLayer<TrickleState>(trickleLayerId);
			this.animController = animController;
		}

		public override void Start()
		{
			base.Start();
			InitializeTrickleLayer();
		}

		public override BoardAlert[] Tick()
		{
			int trickles = Trickle();

			if (trickles == 0)
			{
				State = PhaseState.Done;

				//re-check matches after trickles
				matcher.FindAllMatches();

				//jump back to resolver if new matches after trickles
				if (matcher.MatchCount > 0)
				{
					controller.JumpToPhase(resolver);
				}			
			}
			
			return new BoardAlert[] { };
		}

		public override void Reset()
		{
			base.Reset();

			for (int i = 0; i < board.Size.x; i++)
			{
				for (int j = 0; j < board.Size.y; j++)
				{
					trickleLayer.cells[i,j] = TrickleState.Default;
				}
			}
		}

		private int Trickle()
		{
			int trickles = 0;

			for (int y = 0; y < board.Size.y; y++)
			{
				for (int x = 0; x < board.Size.x; x++)
				{
					var pos = new Vec2(x,y);
					var state = trickleLayer.cells[x,y];

					if (state == TrickleState.Open)
					{
						//try grab up
						Vec2 up = new Vec2(x,y+1);
						if (!up.IsValidPosition(trickleLayer))
						{
							var gemColor = RandomHelper.RandomEnum<GemColor>();
							gemColor = gemColor == GemColor.None ? GemColor.Blue : gemColor;
							var gem = new Gem(gemColor);

							fieldsLayer.cells[x,y].SetGem(gem);

							trickleLayer.cells[x,y] = TrickleState.Falling;
							animController.QueueAnimation(new SpawnAnimation(pos));
							trickles++;
							continue;
						}
						else
						{
							var upCell = trickleLayer.cells[up.x, up.y];
							if (upCell == TrickleState.Falling || upCell == TrickleState.Landed)
							{
								var toField = fieldsLayer.cells[x,y];
								var fromField = fieldsLayer.cells[up.x, up.y];
								var tempGem = toField.Gem;
								toField.SetGem(fromField.Gem);
								fromField.SetGem(tempGem);
							
								trickleLayer.cells[up.x, up.y] = TrickleState.Open;
								trickleLayer.cells[x,y] = TrickleState.Falling;
								animController.QueueAnimation(new TrickleAnimation(up, pos));
								trickles++;
								continue;
							}
						}

						// //try grab left-up
						// Vec2 leftUp = new Vec2(x-1,y+1);

					}
				}
			}

			return trickles;
		}

		private void InitializeTrickleLayer()
		{
			//check open fields
			for (int i = 0; i < board.Size.x; i++)
			{
				for (int j = 0; j < board.Size.y; j++)
				{
					if (fieldsLayer.cells[i,j].Gem == null)
					{
						trickleLayer.cells[i,j] = TrickleState.Open;
					}				
				}
			}

			CheckFixedGems();
		}

		private void CheckFixedGems()
		{
			for (int y = 0; y < board.Size.y; y++)
			{
				for (int x = 0; x < board.Size.x; x++)
				{
					if (fieldsLayer.cells[x,y].Gem == null)
						continue;

					Vec2 down = new Vec2(x, y-1);
					Vec2 downLeft = new Vec2(x-1, y-1);
					Vec2 downRight = new Vec2(x+1, y-1);

					bool isFixed = true;

					if (!down.IsValidPosition(trickleLayer))
					{					
						isFixed = true;
					}
					else
					{
						if (trickleLayer.cells[down.x, down.y] != TrickleState.Fixed)
						{
							isFixed = false;
						}
						if (downLeft.IsValidPosition(trickleLayer))
						{
							if (trickleLayer.cells[downLeft.x, downLeft.y] != TrickleState.Fixed)
								isFixed = false;
						}
						if (downRight.IsValidPosition(trickleLayer))
						{
							if (trickleLayer.cells[downRight.x, downRight.y] != TrickleState.Fixed)
								isFixed = false;
						}
					}

					if (isFixed)
					{
						trickleLayer.cells[x,y] = TrickleState.Fixed;
					}
					else
					{
						trickleLayer.cells[x,y] = TrickleState.Landed;
					}
				}
			}
		}
	}
}