using System;

namespace EnglishSquares
{
	/// <summary>
	/// Global namespace constants, not specific to any class
	/// </summary>
	public class Constants
	{
		public const string VERSION_NAME				= "Version 1.0 (Nov 2003)";
		public const bool FREE_VERSION					= false;

		public const string OPTIONS_FILENAME			= "es_options.xml";
		public const int MUSIC_RESTART_SECONDS			= 60;

		// flag bitmaps
		public const string BRITISH_FLAG				= "images\\britflag.gif";
		public const string FRENCH_FLAG					= "images\\france.jpg";
		public const string RUSSIAN_FLAG				= "images\\russia.jpg";
		public const string PRUSSIAN_FLAG				= "images\\pflag3.gif";
		public const string AUSTRIAN_FLAG				= "images\\austria2.jpg";

		// misc bitmaps
		public const string TOWN_IMAGE					= "images\\town.jpg";
		public const string EAGLE_IMAGE					= "images\\eagle.gif";
		public const string EMPTY_TILE					= "images\\Empty.gif";
		public const string FIELD_DEFAULT				= "images\\Field1.jpg";	
		public const string TITLE_IMAGE					= "images\\Title.gif";			

		// red unit images
		public const string RED_INFANTRY_SQUARE			= "images\\RedSquareInfantry.gif";
		public const string RED_INFANTRY_LINE			= "images\\RedLineInfantry.gif";
		public const string RED_INFANTRY_COLUMN			= "images\\RedColumnInfantry.gif";
		public const string RED_CAVALRY_LINE			= "images\\RedLineCavalry.gif";
		public const string RED_CAVALRY_COLUMN			= "images\\RedColumnCavalry.gif";
		public const string RED_ARTILLERY_LINE			= "images\\RedLineArtillery.gif";
		public const string RED_ARTILLERY_COLUMN		= "images\\RedColumnArtillery.gif";

		// blue unit images
		public const string BLUE_INFANTRY_SQUARE		= "images\\BlueSquareInfantry.gif";
		public const string BLUE_INFANTRY_LINE			= "images\\BlueLineInfantry.gif";
		public const string BLUE_INFANTRY_COLUMN		= "images\\BlueColumnInfantry.gif";
		public const string BLUE_CAVALRY_LINE			= "images\\BlueLineCavalry.gif";
		public const string BLUE_CAVALRY_COLUMN			= "images\\BlueColumnCavalry.gif";
		public const string BLUE_ARTILLERY_LINE			= "images\\BlueLineArtillery.gif";
		public const string BLUE_ARTILLERY_COLUMN		= "images\\BlueColumnArtillery.gif";

		// movement / fire constants
		public const int MOVES_INFANTRY_SQUARE			= 1;
		public const int MOVES_INFANTRY_LINE			= 1;
		public const int MOVES_INFANTRY_COLUMN			= 2;
		public const int MOVES_CAVALRY_LINE				= 3;
		public const int MOVES_CAVALRY_COLUMN			= 4;
		public const int MOVES_ARTILLERY_LINE			= 0;
		public const int MOVES_ARTILLERY_COLUMN			= 1;
		public const int FIRE_INFANTRY					= 4;
		public const int FIRE_ARTILLERY					= 8;

		// battle resolution constants
		public const double SHOOT_INFANTRY_SQUARE		= 0.25;
		public const double SHOOT_INFANTRY_COLUMN		= 0.20;
		public const double SHOOT_INFANTRY_LINE			= 1.50;
		public const double SHOOT_ARTILLERY_LINE		= 4.00;

		public const double SHOOT_INFANTRY_ENERGYLOSS	= 0.90;
		public const double SHOOT_ARTILLERY_ENERGYLOSS	= 0.90;

		public const double DEFENSE_INFANTRY_SQUARE		= 5.00;
		public const double DEFENSE_INFANTRY_LINE		= 0.30;
		public const double DEFENSE_INFANTRY_COLUMN		= 0.10;
		public const double DEFENSE_CAVALRY				= 0.30;
		public const double DEFENSE_ARTILLERY			= 0.10;
		
		public const double ATTACK_INFANTRY_SQUARE		= 0.25;
		public const double ATTACK_INFANTRY_LINE		= 0.95;
		public const double ATTACK_INFANTRY_COLUMN		= 0.80;
		public const double ATTACK_CAVALRY_LINE			= 9.00;
		public const double ATTACK_CAVALRY_COLUMN		= 5.00;
		public const double ATTACK_ARTILLERY			= 0.01;

		public const double WEIGHT_SQUARE_CAVALRY		= 4.0;
		public const double WEIGHT_SQUARE_ARTILLERY		= 0.01;
		public const double WEIGHT_SQUARE_INFANTRY		= 0.5;

		public const double WEIGHT_FIRESQUARE_ARTILLERY	= 0.3;
		public const double WEIGHT_FIRESQUARE_INFANTRY	= 0.5;

		public const int SUPPORT_SQUARE					= 2;
		public const double SUPPORT_MULTIPLIER			= 0.5;
		public const double SUPPORT_ENEMY_INTERFERENCE	= 0.4;

		public const double BATTLE_WON_MORALEUP			= 2;
		public const double BATTLE_WON_MORALESHIFT		= 0.4;
		public const int RETREAT_MORALE_DIVIDE			= 2;

		public const int MINIMUM_EFFECTIVE_DAMAGE		= 1;
		public const int MAX_RANDOM_MORALE_LOSS			= 10;

		public const int TRAINING_ADJUSTER				= 5;

		// computer player constants
        // minmax algorithm
		public const int LOOKAHEAD_DEPTH				= 4;

		public const int MINIMUM_WEIGHT					= Int32.MinValue+1;
		public const int MAXIMUM_WEIGHT					= Int32.MaxValue-1;

		public const int MAX_UNITS						= 64;
		public const bool COMPUTER_RED					= false;
		public const bool COMPUTER_BLUE					= true;

		public const double BIAS_SQUARE					= 1.5;
		public const double BIAS_LINE					= 1.2;
		public const double BIAS_COLUMN					= 1.0;
		public const double BIAS_DEAD_UNITS				= 2.0;
	}
}
