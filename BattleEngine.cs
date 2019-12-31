using System;
using System.Windows.Forms;

namespace EnglishSquares
{
	public enum EnumResult
	{
		StandOff,
		Advance,
		Retreat,
		Destroyed,
		Victory,
		Damaged,
		Nothing
	}

	public class BattleHistory
	{
		int unitID = 0;
		int fromX = 0;
		int fromY = 0;
		int toX = 0;
		int toY = 0;
		int sequence = 0;

		EnumResult resultBattle = EnumResult.StandOff;

		public BattleHistory(int ID, int X1, int Y1, int X2, int Y2, EnumResult result, int s)
		{
			unitID = ID;
			fromX = X1;
			fromY = Y1;
			toX = X2;
			toY = Y2;
			resultBattle = result;
			sequence = s;
		}
	}

	public class BattleEngine
	{
		public const int MAX_HISTORY = 100;
		protected int historyMove = 0;
		protected int sequence = 0;
		protected BattleHistory[] battleHistory = null;
		string lastError = "";
		private int mapX = 0;
		private int mapY = 0;

		public MapUnit lastUnit1 = null;
		public MapUnit lastUnit2 = null;
		public EnumResult lastResult1 = EnumResult.Nothing;
		public EnumResult lastResult2 = EnumResult.Nothing;
		public enumMoveType lastMove = enumMoveType.Nothing;
		public int lastUnit1Damage = 0;
		public int lastUnit2Damage = 0;
		public int lastU1 = -1;
		public int lastU2 = -1;
		public string lastCalc1 = "";
		public string lastCalc2 = "";
		public int lastWeightBefore1 = -1;
		public int lastWeightBefore2 = -1;

		public BattleEngine()
		{
			//battleHistory = new BattleHistory[MAX_HISTORY];
		}

		public void newBattle(int mapWidth, int mapHeight)
		{
			mapX = mapWidth;
			mapY = mapHeight;
			lastUnit1Damage = 0;
			lastUnit2Damage = 0;
			lastU1 = -1;
			lastU2 = -1;
			lastCalc1 = "";
			lastCalc2 = "";
			lastResult1 = EnumResult.Nothing;
			lastResult2 = EnumResult.Nothing;
			lastMove = enumMoveType.Nothing;
			lastUnit1 = null;
			lastUnit2 = null;
		}

		public void addHistoryMove(int ID, int X1, int Y1, int X2, int Y2, EnumResult result)
		{
			sequence++;
			if (historyMove >= MAX_HISTORY) historyMove = 0;

			battleHistory[historyMove] = new BattleHistory(ID, X1, Y1, X2, Y2, result, sequence);
			historyMove++;
		}

		public void notePlayerWeights(ref MapUnit[] playerUnits, int maxUnits)
		{
			lastWeightBefore1 = this.calculatePlayerWeight(ref playerUnits, playerEnum.Red, maxUnits);
			lastWeightBefore2 = this.calculatePlayerWeight(ref playerUnits, playerEnum.Blue, maxUnits);
		}

		public int possibleMoves(unitEnum UnitType, stanceEnum UnitStance)
		{
			int moveOffset = 0;
			
			switch(UnitType)
			{
				case (unitEnum.Infantry):
					if (UnitStance == stanceEnum.Square)		moveOffset = Constants.MOVES_INFANTRY_SQUARE;
					if (UnitStance == stanceEnum.Line)			moveOffset = Constants.MOVES_INFANTRY_LINE;
					if (UnitStance == stanceEnum.Column)		moveOffset = Constants.MOVES_INFANTRY_COLUMN;
					break;		
				case (unitEnum.Cavalry):
					if (UnitStance == stanceEnum.Line)			moveOffset = Constants.MOVES_CAVALRY_LINE;
					if (UnitStance == stanceEnum.Column)		moveOffset = Constants.MOVES_CAVALRY_COLUMN;
					break;
				case (unitEnum.Artillery):
					if (UnitStance == stanceEnum.Line)			moveOffset = Constants.MOVES_ARTILLERY_LINE;
					if (UnitStance == stanceEnum.Column)		moveOffset = Constants.MOVES_ARTILLERY_COLUMN;
					break;
				default:
					moveOffset = 0;
					break;
			}

			return(moveOffset);
		}

		public int possibleFire(unitEnum UnitType, stanceEnum UnitStance)
		{
			int fireOffset = 0;
			
			switch(UnitType)
			{
				case (unitEnum.Infantry):
					fireOffset = Constants.FIRE_INFANTRY;
					break;		
				case (unitEnum.Cavalry):
					fireOffset = 0;
					break;
				case (unitEnum.Artillery):
					if (UnitStance == stanceEnum.Line)
						fireOffset = Constants.FIRE_ARTILLERY;
					else
						fireOffset = 0;
					break;
				default:
					break;
			}

			return(fireOffset);
		}

		public int calculateQuickPlayerWeight(ref MapUnit[] playerUnits, playerEnum player, int maxUnits)
		{
			int playerWeight = 0;

			int u = 0;
			while (u < maxUnits)
			{
				if (playerUnits[u].player == player)
				{
					playerWeight += calculateBiasedUnitPower(ref playerUnits, u);
				}

				u++;
			}

			if (playerWeight == 0) playerWeight = Constants.MINIMUM_WEIGHT;
			return(playerWeight);
		}

		public int calculatePlayerWeight(ref MapUnit[] playerUnits, playerEnum player, int maxUnits)
		{
			int playerWeight = 0;

			int u = 0;
			while (u < maxUnits)
			{
				if (playerUnits[u].player == player)
				{
					playerWeight += calculateBiasedUnitPower(ref playerUnits, u) + calculateSupport(ref playerUnits, u, maxUnits);
				}

				u++;
			}

			if (playerWeight == 0) playerWeight = Constants.MINIMUM_WEIGHT;
			return(playerWeight);
		}

		public int countUnits(ref MapUnit[] playerUnits, playerEnum player, int maxUnits)
		{
			int numUnits = 0;

			int u = 0;
			while (u < maxUnits)
			{
				if (playerUnits[u].player == player)
				{
					numUnits++;
				}

				u++;
			}

			return(numUnits);
		}

		public int countDeadUnits(ref MapUnit[] playerUnits, playerEnum player, int maxUnits)
		{
			int numUnits = 0;

			int u = 0;
			while (u < maxUnits)
			{
				if (playerUnits[u].player == player && playerUnits[u].strength == 0)
				{
					numUnits++;
				}

				u++;
			}

			return(numUnits);
		}

		public int countLivingUnits(ref MapUnit[] playerUnits, playerEnum player, int maxUnits)
		{
			int numUnits = 0;

			int u = 0;
			while (u < maxUnits)
			{
				if (playerUnits[u].player == player && playerUnits[u].strength != 0)
				{
					numUnits++;
				}

				u++;
			}

			return(numUnits);
		}

		public int countBattles(ref MapUnit[] playerUnits, playerEnum player, int maxUnits)
		{
			int numBattles = 0;

			int u = 0;
			while (u < maxUnits)
			{
				if (playerUnits[u].player == player)
				{
					numBattles += playerUnits[u].battlesWon;
				}

				u++;
			}

			return(numBattles);
		}

		public int calculateDistance(ref MapUnit[] playerUnits, int unit1, int unit2)
		{
			int distanceValue = 0;
			int x1 = playerUnits[unit1].x;
			int x2 = playerUnits[unit2].x;
			int y1 = playerUnits[unit1].y;
			int y2 = playerUnits[unit2].y;

			distanceValue = calculateDistance(x1, y1, x2, y2);

			return(distanceValue);
		}

		public int calculateDistance(int x1, int y1, int x2, int y2)
		{
			int distanceValue = 0;

			try
			{
				distanceValue = Convert.ToInt32(Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2)));
			}
			catch (Exception e)
			{
				lastError = e.Message;
				distanceValue = 0;
			}

			return(distanceValue);
		}

		public int calculateUnitPower(ref MapUnit[] playerUnits, int thisUnit)
		{
			if (thisUnit < 0) return(0);

			double trainingModifier = (double) playerUnits[thisUnit].training / Constants.TRAINING_ADJUSTER;
			double unitPower = 0;
			
			if (playerUnits[thisUnit].strength > 0)
			{
				unitPower = playerUnits[thisUnit].strength + playerUnits[thisUnit].morale + trainingModifier;
			}

			return(Convert.ToInt32(unitPower));
		}

		public int calculateBiasedUnitPower(ref MapUnit[] playerUnits, int thisUnit)
		{
			if (thisUnit < 0) return(0);

			double unitPower = 0;

			if (playerUnits[thisUnit].strength > 0)
			{	
				unitPower = calculateUnitPower(ref playerUnits, thisUnit);

				switch (playerUnits[thisUnit].stanceUnit)
				{
					case(stanceEnum.Square):
						unitPower *= Constants.BIAS_SQUARE;
						break;
					case(stanceEnum.Column):
						unitPower *= Constants.BIAS_COLUMN;
						break;
					case(stanceEnum.Line):
						unitPower *= Constants.BIAS_LINE;
						break;
				}
			}

			return(Convert.ToInt32(unitPower));
		}

		public int calculateSupport(ref MapUnit[] playerUnits, int thisUnit, int maxUnits)
		{
			int supportValue = 0;
			int singleSupport = 0;

			int u = 0;
			int distanceSupport = 0;
			while (u < maxUnits)
			{
				singleSupport = 0;

				if (u != thisUnit)
				{
					distanceSupport = calculateDistance(playerUnits[u].x, playerUnits[u].y, playerUnits[thisUnit].x, playerUnits[thisUnit].y);

					// support contribution
					if (distanceSupport <= Constants.SUPPORT_SQUARE)
					{
						singleSupport = Convert.ToInt32(calculateUnitPower(ref playerUnits, u) * Math.Pow(Constants.SUPPORT_MULTIPLIER, distanceSupport));
	
						// if enemy, then apply support as interference
						if (playerUnits[u].player != playerUnits[thisUnit].player)
						{
							singleSupport = Convert.ToInt32((0 - singleSupport) * Constants.SUPPORT_ENEMY_INTERFERENCE);
						}
					}

				}

				supportValue += singleSupport;
				u++;
			}

			if (supportValue < 0) supportValue = 0;
			return(supportValue);
		}

		public int calculateFireWeight(ref MapUnit[] playerUnits, int thisUnit)
		{
			int fireWeight = 0;

			fireWeight = calculateUnitPower(ref playerUnits, thisUnit);
			fireWeight += playerUnits[thisUnit].support;

			if (playerUnits[thisUnit].typeUnit == unitEnum.Infantry)
			{
				switch (playerUnits[thisUnit].stanceUnit)
				{
					case(stanceEnum.Square):
						fireWeight = Convert.ToInt32(fireWeight * Constants.SHOOT_INFANTRY_SQUARE);
						break;
					case(stanceEnum.Column):
						fireWeight = Convert.ToInt32(fireWeight * Constants.SHOOT_INFANTRY_COLUMN);
						break;
					case(stanceEnum.Line):
						fireWeight = Convert.ToInt32(fireWeight * Constants.SHOOT_INFANTRY_LINE);
						break;
					default:
						fireWeight = 0;
						break;
				}
			}
			
			if (playerUnits[thisUnit].typeUnit == unitEnum.Artillery)
			{
				if (playerUnits[thisUnit].stanceUnit == stanceEnum.Line)
				{
					fireWeight = Convert.ToInt32(fireWeight * Constants.SHOOT_ARTILLERY_LINE);
				}
				else
				{
					fireWeight = 0;
				}
			}

			return(fireWeight);
		}

		public int calculateDefenseWeight(ref MapUnit[] playerUnits, int thisUnit, bool includeSupport)
		{
			int defenseWeight = 0;

			defenseWeight = calculateUnitPower(ref playerUnits, thisUnit);
			if (includeSupport == true)	defenseWeight += playerUnits[thisUnit].support;

			if (playerUnits[thisUnit].typeUnit == unitEnum.Infantry)
			{
				switch (playerUnits[thisUnit].stanceUnit)
				{
					case(stanceEnum.Square):
						defenseWeight = Convert.ToInt32(defenseWeight * Constants.DEFENSE_INFANTRY_SQUARE);
						break;
					case(stanceEnum.Column):
						defenseWeight = Convert.ToInt32(defenseWeight * Constants.DEFENSE_INFANTRY_COLUMN);
						break;
					case(stanceEnum.Line):
						defenseWeight = Convert.ToInt32(defenseWeight * Constants.DEFENSE_INFANTRY_LINE);
						break;
					default:
						defenseWeight = 0;
						break;
				}
			}
			
			if (playerUnits[thisUnit].typeUnit == unitEnum.Artillery)
			{
				defenseWeight = Convert.ToInt32(defenseWeight * Constants.DEFENSE_ARTILLERY);					
			}

			if (playerUnits[thisUnit].typeUnit == unitEnum.Cavalry)
			{
				defenseWeight = Convert.ToInt32(defenseWeight * Constants.DEFENSE_CAVALRY);
			}

			return(defenseWeight);
		}

		public int fireAtUnit(ref MapUnit[] playerUnits, int fromUnit, int toUnit, int maxUnits, bool showBattle)
		{
			if (fromUnit > -1 && toUnit > -1)
			{
				if (playerUnits[fromUnit].player != playerUnits[toUnit].player)
				{
					if (showBattle == true)
					{
						lastUnit1 = (MapUnit) playerUnits[fromUnit].Clone();
						lastUnit2 = (MapUnit) playerUnits[toUnit].Clone();
						lastMove = enumMoveType.Fire;
						lastU1 = fromUnit;
						lastU2 = toUnit;
						lastCalc1 = "";
						lastCalc2 = "";
						notePlayerWeights(ref playerUnits, maxUnits);
					}

					int distanceFire = calculateDistance(playerUnits[fromUnit].x, playerUnits[fromUnit].y, playerUnits[toUnit].x, playerUnits[toUnit].y);

					playerUnits[fromUnit].support = calculateSupport(ref playerUnits, fromUnit, maxUnits);
					playerUnits[toUnit].support = calculateSupport(ref playerUnits, toUnit, maxUnits);

					int fireWeight = calculateFireWeight(ref playerUnits, fromUnit);
					int defenseWeight = calculateDefenseWeight(ref playerUnits, toUnit, false);
					if (defenseWeight <= 0) defenseWeight = 1;

					if (playerUnits[fromUnit].typeUnit == unitEnum.Infantry)
					{
						fireWeight = Convert.ToInt32(fireWeight * Math.Pow(Constants.SHOOT_INFANTRY_ENERGYLOSS, distanceFire));
					}

					if (playerUnits[fromUnit].typeUnit == unitEnum.Artillery)
					{
						fireWeight = Convert.ToInt32(fireWeight * Math.Pow(Constants.SHOOT_ARTILLERY_ENERGYLOSS, distanceFire));
					}

					// special case if artillery into infantry square
					if (playerUnits[fromUnit].typeUnit == unitEnum.Artillery && playerUnits[toUnit].typeUnit == unitEnum.Infantry && playerUnits[toUnit].stanceUnit == stanceEnum.Square)
					{
						defenseWeight = Convert.ToInt32(defenseWeight * Constants.WEIGHT_FIRESQUARE_ARTILLERY);
					}

					// special case if infantry into infantry square
					if (playerUnits[fromUnit].typeUnit == unitEnum.Infantry && playerUnits[toUnit].typeUnit == unitEnum.Infantry && playerUnits[toUnit].stanceUnit == stanceEnum.Square)
					{
						defenseWeight = Convert.ToInt32(defenseWeight * Constants.WEIGHT_FIRESQUARE_INFANTRY);
					}

					if (showBattle == true)
					{
						lastCalc1 = fireWeight.ToString();
						lastCalc2 = defenseWeight.ToString();
					}

					int effectiveDamage =  fireWeight / defenseWeight; 
					if (effectiveDamage == 0) effectiveDamage = Constants.MINIMUM_EFFECTIVE_DAMAGE;

					if (showBattle == true)
					{
						//MessageBox.Show("TESTING: fireWeight: " + fireWeight.ToString() + " - defenseWeight: " + defenseWeight.ToString() + " - effectiveDamage: " + effectiveDamage.ToString());
					}

					Random rand = new Random();
					int chanceMorale = playerUnits[fromUnit].morale / rand.Next(5, Constants.MAX_RANDOM_MORALE_LOSS);
					if (chanceMorale <= 0) chanceMorale = 1;
			
					playerUnits[toUnit].strength -= effectiveDamage;
					playerUnits[toUnit].morale -= (effectiveDamage + chanceMorale);
					if (playerUnits[toUnit].morale < 0) playerUnits[toUnit].morale = 0;

					if (showBattle == true)
					{
						lastUnit1Damage = 0;
						lastUnit2Damage = effectiveDamage;
						lastResult1 = EnumResult.StandOff;
						lastResult2 = EnumResult.Damaged;
					}

					if (playerUnits[toUnit].strength <= 0)
					{
						if (showBattle == true)
						{
							lastResult1 = EnumResult.Victory;
							lastResult2 = EnumResult.Destroyed;
						}
						killUnit(ref playerUnits, toUnit, fromUnit, maxUnits, showBattle);
					}

					return(effectiveDamage);
				}
				else
				{
					return(-1);
				}
			}
			else
			{
				return(-1);
			}
		}

		public void moraleShift(ref MapUnit[] playerUnits, int unitCause, int shiftMorale, int maxUnits)
		{
			int u = 0;
			while (u < maxUnits)
			{
				if (u != unitCause)
				{
					if (playerUnits[u].player == playerUnits[unitCause].player)
					{
						// shift all units morale belonging to player that won a battle up
						playerUnits[u].morale += shiftMorale;
					}
					else
					{
						// shift all units morale belonging to player that lost a battle up
						playerUnits[u].morale -= shiftMorale;
						if (playerUnits[u].morale < 0) playerUnits[u].morale = 0;
					}
				}

				u++;
			}
		}

		public bool isSquareFree(ref MapUnit[] playerUnits, int x, int y, int maxUnits)
		{
			int u = 0;
			bool freeSpace = true;
			while (u < maxUnits && freeSpace == true)
			{
				if (playerUnits[u].x == x && playerUnits[u].y == y)
				{
					freeSpace = false;	
				}
				u++;
			}

			return(freeSpace);
		}

		public int findUnit(ref MapUnit[] playerUnits, int testmapX, int testmapY, int maxUnits)
		{
			int hitUnit = -1;
			int u = 0;
			while (u < maxUnits && hitUnit < 0)
			{
				if (playerUnits[u].x == testmapX && playerUnits[u].y == testmapY)
				{
					hitUnit = u;
				}
				u++;
			}

			return(hitUnit);
		}

		public int findNearestFriend(ref MapUnit[] playerUnits, int thisUnit, int maxUnits, bool enemy)
		{
			int nearestUnit = -1;
			int nearestDistance = 99999;

			int thisDistance = -1;
			int u = 0;
			while (u < maxUnits)
			{
				if ((playerUnits[thisUnit].player == playerUnits[u].player && enemy == false) ||
					(playerUnits[thisUnit].player != playerUnits[u].player && enemy == true)	 )
				{
					thisDistance = calculateDistance(playerUnits[thisUnit].x, playerUnits[thisUnit].y, playerUnits[u].x, playerUnits[u].y);

					if (thisDistance < nearestDistance)
					{
						nearestUnit = u;
					}
				}				
			
				u++;
			}

			return(nearestUnit);
		}

		public bool retreatUnit(ref MapUnit[] playerUnits, int toUnit, int fromUnit, int maxUnits, bool showBattle)
		{
			// find available square for retreat closest to friendly units
			int ux = -1, uy = -1;
			int nearestUnit = findNearestFriend(ref playerUnits, toUnit, maxUnits, false);
			int bestDistance = 9999;

			for (int i = playerUnits[toUnit].x-1; i <= playerUnits[toUnit].x+1; i++)
			{
				for (int j = playerUnits[toUnit].y-1; j <= playerUnits[toUnit].y+1; j++)
				{
					if (isSquareFree(ref playerUnits, i, j, maxUnits) == true)
					{
						if (i < mapX && j < mapY && i >= 0 && j >= 0)
						{
							if (nearestUnit > -1)
							{
								int thisDistance = calculateDistance(i, j, playerUnits[nearestUnit].x, playerUnits[nearestUnit].y);	
								if (thisDistance < bestDistance)
								{
									bestDistance = thisDistance;
									ux = i;
									uy = j;
								}
							}
							else
							{
								ux = i;
								uy = j;
							}
						}
					}
				}
			}

			if (ux > -1 && uy > -1)
			{
				playerUnits[toUnit].morale = playerUnits[toUnit].morale / Constants.RETREAT_MORALE_DIVIDE;
				playerUnits[toUnit].stanceUnit = stanceEnum.Column;
				playerUnits[toUnit].x = ux;
				playerUnits[toUnit].y = uy;

				playerUnits[fromUnit].battlesWon++;
				playerUnits[fromUnit].morale += Convert.ToInt32(playerUnits[fromUnit].battlesWon*Constants.BATTLE_WON_MORALEUP);

				moraleShift(ref playerUnits, fromUnit, Convert.ToInt32(playerUnits[fromUnit].battlesWon*Constants.BATTLE_WON_MORALESHIFT), maxUnits);

				return(true);
			}
			else
			{
				return(killUnit(ref playerUnits, toUnit, fromUnit, maxUnits, showBattle));
			}
		}

		public bool killUnit(ref MapUnit[] playerUnits, int toUnit, int fromUnit, int maxUnits, bool showBattle)
		{
			if (showBattle == true && playerUnits[toUnit].strength <= 0)
			{
				MessageBox.Show(playerUnits[toUnit].description + " destroyed.", "Unit Routed", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			playerUnits[toUnit].strength = 0;
			playerUnits[toUnit].morale = 0;
			playerUnits[toUnit].support = 0;
			playerUnits[toUnit].training = 0;
			playerUnits[toUnit].x = -1;
			playerUnits[toUnit].y = -1;

			playerUnits[fromUnit].battlesWon++;
			playerUnits[fromUnit].morale += Convert.ToInt32(playerUnits[fromUnit].battlesWon*Constants.BATTLE_WON_MORALEUP);

			moraleShift(ref playerUnits, fromUnit, Convert.ToInt32(playerUnits[fromUnit].battlesWon*Constants.BATTLE_WON_MORALESHIFT), maxUnits);

			return(true);
		}

		public bool isUnitDead(ref MapUnit[] playerUnits, int thisUnit)
		{
			if (playerUnits[thisUnit].strength == 0)
			{
				return(true);
			}
			else
			{
				return(false);
			}
		}

		public int calculateAttackWeight(ref MapUnit[] playerUnits, int thisUnit)
		{
			int attackWeight = 0;

			attackWeight = calculateUnitPower(ref playerUnits, thisUnit);
			attackWeight += playerUnits[thisUnit].support;

			if (playerUnits[thisUnit].typeUnit == unitEnum.Infantry)
			{
				switch (playerUnits[thisUnit].stanceUnit)
				{
					case(stanceEnum.Square):
						attackWeight = Convert.ToInt32(attackWeight * Constants.ATTACK_INFANTRY_SQUARE);
						break;
					case(stanceEnum.Column):
						attackWeight = Convert.ToInt32(attackWeight * Constants.ATTACK_INFANTRY_COLUMN);
						break;
					case(stanceEnum.Line):
						attackWeight = Convert.ToInt32(attackWeight * Constants.ATTACK_INFANTRY_LINE);
						break;
					default:
						attackWeight = 0;
						break;
				}
			}

			if (playerUnits[thisUnit].typeUnit == unitEnum.Cavalry)
			{
				switch (playerUnits[thisUnit].stanceUnit)
				{
					case(stanceEnum.Column):
						attackWeight = Convert.ToInt32(attackWeight * Constants.ATTACK_CAVALRY_COLUMN);
						break;
					case(stanceEnum.Line):
						attackWeight = Convert.ToInt32(attackWeight * Constants.ATTACK_CAVALRY_LINE);
						break;
					default:
						attackWeight = 0;
						break;
				}				
			}

			if (playerUnits[thisUnit].typeUnit == unitEnum.Artillery)
			{
				if (playerUnits[thisUnit].stanceUnit == stanceEnum.Line)
				{
					attackWeight = Convert.ToInt32(attackWeight * Constants.ATTACK_ARTILLERY);
				}
				else
				{
					attackWeight = 0;
				}
			}

			return(attackWeight);
		}

		public int fightUnits(ref MapUnit[] playerUnits, int fromUnit, int toUnit, int maxUnits, bool showBattle)
		{
			if (fromUnit > -1 && toUnit > -1)
			{
				if (showBattle == true)
				{
					lastUnit1 = (MapUnit) playerUnits[fromUnit].Clone();
					lastUnit2 = (MapUnit) playerUnits[toUnit].Clone();
					lastMove = enumMoveType.Attack;
					lastU1 = fromUnit;
					lastU2 = toUnit;
					lastCalc1 = "";
					lastCalc2 = "";
					notePlayerWeights(ref playerUnits, maxUnits);
				}

				playerUnits[fromUnit].support = calculateSupport(ref playerUnits, fromUnit, maxUnits);
				playerUnits[toUnit].support = calculateSupport(ref playerUnits, toUnit, maxUnits);
				
				int fireWeight1 = calculateAttackWeight(ref playerUnits, fromUnit);
				int defenseWeight1 = calculateDefenseWeight(ref playerUnits, fromUnit, true);
				if (defenseWeight1 <= 0) defenseWeight1 = 1;

				int fireWeight2 = calculateAttackWeight(ref playerUnits, toUnit);
				int defenseWeight2 = calculateDefenseWeight(ref playerUnits, toUnit, true);
				if (defenseWeight2 <= 0) defenseWeight2 = 1;

				int effectiveDamage1 =  fireWeight1 / defenseWeight2; 
				if (effectiveDamage1 == 0) effectiveDamage1 = Constants.MINIMUM_EFFECTIVE_DAMAGE;

				int effectiveDamage2 =  fireWeight2 / defenseWeight1; 
				if (effectiveDamage2 == 0) effectiveDamage2 = Constants.MINIMUM_EFFECTIVE_DAMAGE;

				// special case if artillery into infantry square
				if (playerUnits[fromUnit].typeUnit == unitEnum.Artillery && 
					playerUnits[toUnit].typeUnit == unitEnum.Infantry && playerUnits[toUnit].stanceUnit == stanceEnum.Square)
				{
					defenseWeight2 = Convert.ToInt32(defenseWeight2 * Constants.WEIGHT_SQUARE_ARTILLERY);
				}

				// special case if cavalry into infantry square
				if (playerUnits[fromUnit].typeUnit == unitEnum.Cavalry && 
					playerUnits[toUnit].typeUnit == unitEnum.Infantry && playerUnits[toUnit].stanceUnit == stanceEnum.Square)
				{
					defenseWeight2 = Convert.ToInt32(defenseWeight2 * Constants.WEIGHT_SQUARE_CAVALRY);
				}

				// special case if infantry into infantry square
				if (playerUnits[fromUnit].typeUnit == unitEnum.Infantry && 
					playerUnits[toUnit].typeUnit == unitEnum.Infantry && playerUnits[toUnit].stanceUnit == stanceEnum.Square)
				{
					defenseWeight2 = Convert.ToInt32(defenseWeight2 * Constants.WEIGHT_SQUARE_INFANTRY);
				}

				if (showBattle == true)
				{
					lastResult1 = EnumResult.StandOff;
					lastResult2 = EnumResult.StandOff;
					lastCalc1 = fireWeight1.ToString() + "/" + defenseWeight1.ToString();
					lastCalc2 = fireWeight2.ToString() + "/" + defenseWeight2.ToString();
				}

				playerUnits[toUnit].strength -= effectiveDamage1;
				playerUnits[toUnit].morale -= effectiveDamage1;
				if (playerUnits[toUnit].morale < 0) playerUnits[toUnit].morale = 0;

				if (showBattle == true)
				{
					lastUnit2Damage = effectiveDamage1;
				}

				if (playerUnits[toUnit].strength <= 0)
				{
					if (showBattle == true)
					{
						lastResult2 = EnumResult.Destroyed;
					}
					killUnit(ref playerUnits, toUnit, fromUnit, maxUnits, showBattle);
				}

				playerUnits[fromUnit].strength -= effectiveDamage2;
				playerUnits[fromUnit].morale -= effectiveDamage2;

				if (showBattle == true)
				{
					lastUnit1Damage = effectiveDamage2;
				}

				if (playerUnits[fromUnit].morale < 0) playerUnits[fromUnit].morale = 0;
				if (playerUnits[fromUnit].strength <= 0)
				{
					if (showBattle == true)
					{
						lastResult1 = EnumResult.Destroyed;
					}
					killUnit(ref playerUnits, fromUnit, toUnit, maxUnits, showBattle);
				}

				if (showBattle == true)
				{
					if (lastResult1 == EnumResult.Destroyed && lastResult2 != EnumResult.Destroyed)
						lastResult2 = EnumResult.Victory;

					if (lastResult2 == EnumResult.Destroyed && lastResult1 != EnumResult.Destroyed)
						lastResult1 = EnumResult.Victory;
				}

				// Retreat or kill one of units neither already died
				if (isUnitDead(ref playerUnits, fromUnit) == false && isUnitDead(ref playerUnits, toUnit) == false)
				{
					if (effectiveDamage1 > effectiveDamage2)
					{
						if (showBattle == true)
						{
							lastResult1 = EnumResult.Advance;
							lastResult2 = EnumResult.Retreat;
						}
						retreatUnit(ref playerUnits, toUnit, fromUnit, maxUnits, showBattle);
					}
					else
					{
						if (showBattle == true)
						{
							lastResult1 = EnumResult.Retreat;
							lastResult2 = EnumResult.Advance;
						}
						retreatUnit(ref playerUnits, fromUnit, toUnit, maxUnits, showBattle);
					}
				}

				return(effectiveDamage1 + effectiveDamage2);
			}
			else
			{
				return(-1);
			}
		}

		public void moveUnit(ref MapUnit[] playerUnits, int moveUnit, int newX, int newY)
		{
			lastUnit1 = (MapUnit) playerUnits[moveUnit].Clone();
			lastUnit2 = null;
			lastMove = enumMoveType.Move;
			lastU1 = moveUnit;
			lastU2 = -1;
			lastResult1 = EnumResult.Advance;
			lastResult2 = EnumResult.Nothing;
			lastUnit1Damage = 0;
			lastUnit2Damage = 0;
			lastCalc1 = "";
			lastCalc2 = "";

			playerUnits[moveUnit].x = newX;
			playerUnits[moveUnit].y = newY;
		}

		public void changeUnitStance(ref MapUnit[] playerUnits, int thisUnit, stanceEnum toStance)
		{
			lastUnit1 = (MapUnit) playerUnits[thisUnit].Clone();
			lastUnit2 = null;
			lastMove = enumMoveType.Stance;
			lastU1 = thisUnit;
			lastU2 = -1;
			lastResult1 = EnumResult.Nothing;
			lastResult2 = EnumResult.Nothing;
			lastUnit1Damage = 0;
			lastUnit2Damage = 0;
			lastCalc1 = "";
			lastCalc2 = "";

			playerUnits[thisUnit].stanceUnit = toStance;
		}


	}
}
