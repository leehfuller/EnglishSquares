using System;

namespace EnglishSquares
{
	/// <summary>
	/// Class for infantry, cavalry or artillery units
	/// </summary>
	public enum unitEnum 
	{
		Infantry, 
		Cavalry, 
		Artillery,
		Nothing
	}

	public enum stanceEnum 
	{
		Square, 
		Column, 
		Line,
		Nothing
	}

	public enum playerEnum
	{
		Red,
		Blue,
		Nothing
	}

	public enum playerControlEnum
	{
		Human,
		OnlineHuman,
		Computer
	}

	public enum unitCommands
	{
		MoveOnly,
		FireOnly,
		MoveAndFire
	}

	/// <summary>
	/// encapsulation of map units
	/// </summary>
	public class MapUnit : ICloneable
	{
		public int x;
		public int y;
		public int strength;
		public int morale;
		public int support;
		public int training;
		public playerEnum player;
		public int battlesWon;
		public string description;
		public unitEnum typeUnit;
		public stanceEnum stanceUnit;

		public MapUnit()
		{
			x = 0;
			y = 0;
			strength = 100;
			morale = 100;
			support = 0;
			training = 100;
			player = playerEnum.Red;
			description = "Standard Unit";
			battlesWon = 0;
			typeUnit = unitEnum.Infantry;
			stanceUnit = stanceEnum.Line;
		}

		public object Clone()
		{
			return this.MemberwiseClone();
		}
	}
}
