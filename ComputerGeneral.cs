//#define TraceMoves		// write to debug output

using System;
using System.Diagnostics;

namespace EnglishSquares
{
	public enum enumMoveType
	{
		Move,
		Attack,
		Fire,
		Stance,
		Nothing
	}

	public enum enumCharacter
	{
		Aggressive,
		Defensive,
		Neutral,
		Suicidal
	}

	/// <summary>
	/// computer controlled player
	/// </summary>
	public class ComputerGeneral
	{
		int mapX = 0;
		int mapY = 0;
		int lookaheadMoves = 0;
		public enumCharacter computerCharacter = enumCharacter.Aggressive;
		public int movesConsidered = 0;
		public int deadUnits = 0;
		public BattleEngine modelBattleEngine = null;

		public bool pruneFar = false;
		public bool quickCalc = false;

		// look for valid moves and firing positions for each unit of this player
		int bestU = -1;
		int bestF = -1;
		int bestX = -1;
		int bestY = -1;
		enumMoveType bestMoveType = enumMoveType.Move;
		stanceEnum bestStance = stanceEnum.Nothing;

		public ComputerGeneral()
		{
		}

		[Conditional("TraceMoves")]
		public void traceProgress(string progress)
		{
			Debug.WriteLine(progress);
		}

		public void newGeneral(int x, int y, int lookdepth)
		{
			lookaheadMoves = lookdepth - 1;
			mapX = x;
			mapY = y;
		}

		private int calculateWeightFacade(ref MapUnit[] modelUnits, playerEnum player, int maxUnits)
		{
			int calcWeight = Constants.MINIMUM_WEIGHT;

			if (quickCalc == true)
			{
				calcWeight = modelBattleEngine.calculateQuickPlayerWeight(ref modelUnits, player, maxUnits);
			}
			else
			{
				calcWeight = modelBattleEngine.calculatePlayerWeight(ref modelUnits, player, maxUnits);
			}

			return(calcWeight);
		}

		// heuristic evaluation function for minimax algorithm
		public int calculateWeight(ref MapUnit[] modelUnits, ref BattleEngine modelBattleEngine, playerEnum player, int maxUnits)
		{
			int battlefieldWeight = Constants.MINIMUM_WEIGHT;
			int playerRedWeight = 0;
			int playerBlueWeight = 0;
			playerEnum enemyPlayer = (player == playerEnum.Red)? playerEnum.Blue : playerEnum.Red;
			int numberRedUnits = modelBattleEngine.countLivingUnits(ref modelUnits, playerEnum.Red, maxUnits);
			int numberBlueUnits = modelBattleEngine.countLivingUnits(ref modelUnits, playerEnum.Blue, maxUnits);

			if (player == playerEnum.Blue)
			{
				switch (computerCharacter)
				{
					case (enumCharacter.Aggressive):
						playerRedWeight = calculateWeightFacade(ref modelUnits, playerEnum.Red, maxUnits);
						playerRedWeight += numberRedUnits;
						battlefieldWeight = 0-playerRedWeight;
						break;
					case (enumCharacter.Suicidal):
						playerRedWeight = calculateWeightFacade(ref modelUnits, playerEnum.Red, maxUnits);
						playerRedWeight += numberRedUnits;
						battlefieldWeight = playerRedWeight;
						break;
					case (enumCharacter.Defensive):
						playerBlueWeight = calculateWeightFacade(ref modelUnits, playerEnum.Blue, maxUnits);
						playerBlueWeight += numberBlueUnits;
						battlefieldWeight = playerBlueWeight;
						break;
					case (enumCharacter.Neutral):
						playerRedWeight = calculateWeightFacade(ref modelUnits, playerEnum.Red, maxUnits);
						playerBlueWeight = calculateWeightFacade(ref modelUnits, playerEnum.Blue, maxUnits);
						battlefieldWeight = (playerBlueWeight + numberBlueUnits) - (playerRedWeight + numberRedUnits);
						break;
					default:
						break;
				}
			}

			if (player == playerEnum.Red)
			{
				switch (computerCharacter)
				{
					case (enumCharacter.Aggressive):
						playerBlueWeight = calculateWeightFacade(ref modelUnits, playerEnum.Blue, maxUnits);
						playerBlueWeight += numberBlueUnits;
						battlefieldWeight = 0-playerBlueWeight;
						break;
					case (enumCharacter.Suicidal):
						playerBlueWeight = calculateWeightFacade(ref modelUnits, playerEnum.Blue, maxUnits);
						playerBlueWeight += numberBlueUnits;
						battlefieldWeight = playerBlueWeight;
						break;
					case (enumCharacter.Defensive):
						playerRedWeight = calculateWeightFacade(ref modelUnits, playerEnum.Red, maxUnits);
						playerRedWeight += numberRedUnits;
						battlefieldWeight = playerRedWeight;
						break;
					case (enumCharacter.Neutral):
						playerRedWeight = calculateWeightFacade(ref modelUnits, playerEnum.Red, maxUnits);
						playerBlueWeight = calculateWeightFacade(ref modelUnits, playerEnum.Blue, maxUnits);
						battlefieldWeight = (playerRedWeight + numberRedUnits) - (playerBlueWeight + numberBlueUnits);
						break;
					default:
						break;
				}				
			}

			return(battlefieldWeight);
		}

		// using minimax type of algorithm
		public int makeMove(ref MapUnit[] playerUnits, playerEnum player, int maxUnits, int thisDepth, int maxDepth, int bestWeight)
		{
			traceProgress("makeMove() - *** DEPTH *** " + thisDepth.ToString() + " bestweight:" + bestWeight.ToString());

			// already got end-condition, then this recurse not needed
			if (bestWeight == Constants.MAXIMUM_WEIGHT) return(Constants.MINIMUM_WEIGHT);

			if (modelBattleEngine == null)
			{
				modelBattleEngine = new BattleEngine();
				modelBattleEngine.newBattle(mapX, mapY);
			}

			MapUnit[] modelUnits = new MapUnit[maxUnits];
			for (int mu = 0; mu < maxUnits; mu++)
			{
				modelUnits[mu] = (MapUnit) playerUnits[mu].Clone();
			}

			if (thisDepth == 0) 
			{
				movesConsidered = 0;
				deadUnits = modelBattleEngine.countDeadUnits(ref modelUnits, player, maxUnits);

				bestU = -1;
				bestF = -1;
				bestX = -1;
				bestY = -1;
				bestMoveType = enumMoveType.Nothing;
				bestStance = stanceEnum.Nothing;

				traceProgress("makeMove() - *** INITIALISE *** max:" + mapX.ToString() + "," + mapY.ToString());
			}

			int startingWeight = calculateWeight(ref modelUnits, ref modelBattleEngine, player, maxUnits);
			traceProgress("makeMove() - startingWeight:" + startingWeight.ToString());

			int u = 0;
			while (u < maxUnits && bestWeight != Constants.MAXIMUM_WEIGHT)
			{
				if (player == modelUnits[u].player && modelUnits[u].strength > 0)
				{
					traceProgress("makeMove() - UNIT:" + modelUnits[u].description);

					// Moves
					int moveOffset = modelBattleEngine.possibleMoves(modelUnits[u].typeUnit, modelUnits[u].stanceUnit);

					int startX = modelUnits[u].x - moveOffset;
					int endX = modelUnits[u].x + moveOffset;
					if (startX < 0) startX = 0;
					if (endX >= mapX) endX = mapX-1;

					for (int i = startX; i <= endX; i++)
					{
						int startY = modelUnits[u].y - moveOffset;
						int endY = modelUnits[u].y + moveOffset;
						if (startY < 0) startY = 0;
						if (endY >= mapY) endY = mapY-1;

						for (int j = startY; j <= endY; j++)
						{
							if (bestWeight != Constants.MAXIMUM_WEIGHT)
							{
								playerEnum enemyPlayer = (player == playerEnum.Red)? playerEnum.Blue : playerEnum.Red;
								int livingEnemyBefore = modelBattleEngine.countLivingUnits(ref modelUnits, enemyPlayer, maxUnits);

								MapUnit oldUnit = (MapUnit) modelUnits[u].Clone();
								MapUnit oldAttackedUnit = null;
								int nearestEnemyBefore = modelBattleEngine.findNearestFriend(ref modelUnits, u, maxUnits, true);
								int nearestEnemyDistanceBefore = modelBattleEngine.calculateDistance(ref modelUnits, nearestEnemyBefore, u);

								int f = modelBattleEngine.findUnit(ref modelUnits, i, j, maxUnits);
								int playerWeight = Constants.MINIMUM_WEIGHT;
								bool noChange = false;
								if (f > -1)
								{
									if (f != u && modelUnits[f].player != modelUnits[u].player && modelUnits[f].strength > 0)
									{
										movesConsidered++;

										// nearby enemy unit, try attacking
										oldAttackedUnit = (MapUnit) modelUnits[f].Clone();

										modelUnits[u].x = i;
										modelUnits[u].y = j;

										modelBattleEngine.fightUnits(ref modelUnits, u, f, maxUnits, false);

										traceProgress("makeMove() - Attack:" + modelUnits[f].description);
									}
									else
									{
										noChange = true;
									}
								}
								else
								{
									movesConsidered++;

									// empty square, so try moving
									modelUnits[u].x = i;
									modelUnits[u].y = j;

									traceProgress("makeMove() - Move:" + i.ToString() + "," + j.ToString());
								}

								if (noChange == false)
								{
									if (thisDepth < maxDepth)
									{
										if (pruneFar == true)
										{
											// stop moves away from enemy
											int nearestEnemyAfter = modelBattleEngine.findNearestFriend(ref modelUnits, u, maxUnits, true);
											int nearestEnemyDistanceAfter = modelBattleEngine.calculateDistance(ref modelUnits, nearestEnemyAfter, u);

											if (nearestEnemyDistanceAfter > nearestEnemyDistanceBefore)
											{
												playerWeight = Constants.MINIMUM_WEIGHT;
											}
											else
											{
												// if not at maxDepth, then recurse for unpruned tree
												playerWeight = makeMove(ref modelUnits, player, maxUnits, thisDepth+1, maxDepth, bestWeight);
											}
										}
										else
										{
											// if all enemy dead, then this is end condition!
											int livingEnemyAfter = modelBattleEngine.countLivingUnits(ref modelUnits, enemyPlayer, maxUnits);
											if (livingEnemyAfter <= 0)
											{
												playerWeight = Constants.MAXIMUM_WEIGHT;
											}
											else
											{
												// if not at maxDepth, then recurse
												playerWeight = makeMove(ref modelUnits, player, maxUnits, thisDepth+1, maxDepth, bestWeight);

												if (livingEnemyAfter < livingEnemyBefore) playerWeight++;
											}
										}

										traceProgress("makeMove() - RecursedWeight:" + playerWeight.ToString());
									}
									else
									{
										// finally at max depth, what is the weighting?
										playerWeight = calculateWeight(ref modelUnits, ref modelBattleEngine, player, maxUnits);

										traceProgress("makeMove() - playerWeight:" + playerWeight.ToString());
									}

									if (playerWeight > bestWeight)
									{
										if (f > -1)
										{
											if (modelUnits[u].player != modelUnits[f].player)
											{
												bestWeight = playerWeight;
												bestX = i;
												bestY = j;
												bestU = u;
												bestF = f;
												bestMoveType = enumMoveType.Attack;

												traceProgress("makeMove() - Set bestWeight:" + playerWeight.ToString());
												traceProgress("makeMove() - To Attack:" + modelUnits[f].description);
											}
										}
										else
										{
											bestWeight = playerWeight;
											bestX = i;
											bestY = j;
											bestU = u;
											bestF = -1;
											bestMoveType = enumMoveType.Move;

											traceProgress("makeMove() - Set bestWeight:" + playerWeight.ToString());
											traceProgress("makeMove() - To Move:" + i.ToString() + "," + j.ToString());
										}
									}
								}

								// undo hypothetical move
								modelUnits[u] = (MapUnit) oldUnit.Clone();

								// undo hypothetical attack
								if (f > -1 && oldAttackedUnit != null) 
									modelUnits[f] = (MapUnit) oldAttackedUnit.Clone();
							}
						}

						// try changing stance 
						int stanceWeight = Constants.MINIMUM_WEIGHT;
						int numberStances = 0;
						MapUnit oldStanceUnit = (MapUnit) modelUnits[u].Clone();

						switch (modelUnits[u].typeUnit)
						{
							case (unitEnum.Artillery):
								numberStances = 2;
								break;
							case (unitEnum.Cavalry):
								numberStances = 2;
								break;
							case (unitEnum.Infantry):
								numberStances = 3;
								break;
							default:
								numberStances = 0;
								break;
						}

						int us = 1;
						while (us <= numberStances)
						{
							bool stanceChange = false;
							stanceEnum currentStance = stanceEnum.Nothing;

							switch (us)
							{
								case (1):
									if (modelUnits[u].stanceUnit != stanceEnum.Line)
									{
										modelUnits[u].stanceUnit = stanceEnum.Line;
										currentStance = stanceEnum.Line;
										stanceChange = true;
									}
									break;
								case (2):
									if (modelUnits[u].stanceUnit != stanceEnum.Column)
									{
										//modelUnits[u].stanceUnit = stanceEnum.Column;
										//currentStance = stanceEnum.Column;
										//stanceChange = true;
									}
									break;
								case (3):
									if (modelUnits[u].stanceUnit != stanceEnum.Square)
									{
										modelUnits[u].stanceUnit = stanceEnum.Square;
										currentStance = stanceEnum.Square;
										stanceChange = true;
									}
									break;
							}

							if (stanceChange == false)
							{
								stanceWeight = Constants.MINIMUM_WEIGHT;
							}
							else
							{
								traceProgress("makeMove() - Set stance:" + currentStance.ToString());

								movesConsidered++;

								if (thisDepth < maxDepth)
								{
									stanceWeight = makeMove(ref modelUnits, player, maxUnits, thisDepth+1, maxDepth, bestWeight);
									traceProgress("makeMove() - recursedWeight:" + stanceWeight.ToString());
								}
								else
								{
									stanceWeight = calculateWeight(ref modelUnits, ref modelBattleEngine, player, maxUnits);
									traceProgress("makeMove() - stanceWeight:" + stanceWeight.ToString());
								}
							}

							if (stanceWeight > bestWeight && currentStance != stanceEnum.Nothing)
							{								
								bestWeight = stanceWeight;
								bestU = u;
								bestStance = currentStance;
								bestMoveType = enumMoveType.Stance;

								traceProgress("makeMove() - Set bestWeight:" + stanceWeight.ToString());
								traceProgress("makeMove() - stance:" + currentStance.ToString());
							}

							// undo hypothetical stance change
							modelUnits[u] = (MapUnit) oldStanceUnit.Clone();
							us++;
						}
						
						// try firing at all nearby units
						int fireWeight = Constants.MINIMUM_WEIGHT;
						int fireOffset = modelBattleEngine.possibleFire(modelUnits[u].typeUnit, modelUnits[u].stanceUnit);
						if (fireOffset > 0)
						{
							int uf = 0;
							while (uf < maxUnits && bestWeight != Constants.MAXIMUM_WEIGHT)
							{
								if (modelUnits[u].player != modelUnits[uf].player && modelUnits[uf].strength > 0)
								{
									playerEnum enemyPlayer = (player == playerEnum.Red)? playerEnum.Blue : playerEnum.Red;
									int livingEnemyBefore = modelBattleEngine.countLivingUnits(ref modelUnits, enemyPlayer, maxUnits);

									MapUnit oldFireUnit = (MapUnit) modelUnits[uf].Clone();

									movesConsidered++;
									traceProgress("makeMove() - Fire At:" + modelUnits[uf].description);

									int distanceFire = modelBattleEngine.calculateDistance(ref modelUnits, uf, u);
									if (distanceFire <= fireOffset)
									{
										traceProgress("makeMove() - Distance:" + distanceFire.ToString());

										modelBattleEngine.fireAtUnit(ref modelUnits, u, uf, maxUnits, false);
							
										if (thisDepth < maxDepth)
										{
											// if all enemy dead, then this is end condition!
											int livingEnemyAfter = modelBattleEngine.countLivingUnits(ref modelUnits, enemyPlayer, maxUnits);
											if (livingEnemyAfter <= 0)
											{
												fireWeight = Constants.MAXIMUM_WEIGHT;
											}
											else
											{
												// if not at maxDepth, then recurse
												fireWeight = makeMove(ref modelUnits, player, maxUnits, thisDepth+1, maxDepth, bestWeight);

												if (livingEnemyAfter < livingEnemyBefore) fireWeight++;
											}

											traceProgress("makeMove() - recursedWeight:" + fireWeight.ToString());
										}
										else
										{
											fireWeight = calculateWeight(ref modelUnits, ref modelBattleEngine, player, maxUnits);

											traceProgress("makeMove() - fireWeight:" + fireWeight.ToString());
										}

										if (fireWeight > bestWeight)
										{
											bestWeight = fireWeight;
											bestU = u;
											bestF = uf;
											bestMoveType = enumMoveType.Fire;

											traceProgress("makeMove() - Set Fire:" + modelUnits[uf].description);
										}

										// undo hypothetical fire
										modelUnits[uf] = (MapUnit) oldFireUnit.Clone();
									}
								}

								uf++;
							}
						}
					}
				}				
			
				u++;
			}

			if (bestU > -1 && thisDepth == 0)
			{
				bool moveMade = false;
				traceProgress("makeMove() - *** MAKE COMPUTER MOVE ***");

				switch (bestMoveType)
				{
					case (enumMoveType.Move):
						if (bestX > -1 && bestY > -1)
						{
							modelBattleEngine.moveUnit(ref playerUnits, bestU, bestX, bestY);
							moveMade = true;

							traceProgress("makeMove() - *** MOVE UNIT ***");
							traceProgress("makeMove() - " + playerUnits[bestU].description + " to " + bestX.ToString() + "," + bestY.ToString());
						}
						break;
					case (enumMoveType.Attack):
						if (bestX > -1 && bestY > -1)
						{
							playerUnits[bestU].x = bestX;
							playerUnits[bestU].y = bestY;

							modelBattleEngine.fightUnits(ref playerUnits, bestU, bestF, maxUnits, true);
							moveMade = true;

							traceProgress("makeMove() - *** ATTACK UNIT ***");
							traceProgress("makeMove() - " + playerUnits[bestU].description + " to " + playerUnits[bestF].description);
						}
						break;
					case (enumMoveType.Fire):
						modelBattleEngine.fireAtUnit(ref playerUnits, bestU, bestF, maxUnits, true);
						moveMade = true;

						traceProgress("makeMove() - *** FIRE ON UNIT ***");
						traceProgress("makeMove() - " + playerUnits[bestU].description + " to " + playerUnits[bestF].description);
						break;
					case (enumMoveType.Stance):
						modelBattleEngine.changeUnitStance(ref playerUnits, bestU, bestStance);
						moveMade = true;

						traceProgress("makeMove() - *** CHANGE STANCE ***");
						traceProgress("makeMove() - " + playerUnits[bestU].description + " to " + bestStance.ToString());
						break;
					default:
						traceProgress("makeMove() - *** ERROR - DEFAULT CASE ***");
						moveMade = false;
						break;
				}

				traceProgress("makeMove() - moves considered:" + movesConsidered.ToString());

				Debug.Assert(moveMade == true, "No move made", "madeMove() failed to create a move!");

				if (moveMade == false) bestWeight = Constants.MINIMUM_WEIGHT;
			}

			traceProgress("makeMove() - return weight:" + bestWeight.ToString());
			return(bestWeight);
		}
	}
}
