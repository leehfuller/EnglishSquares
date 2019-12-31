using System;
using System.Collections;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Xml;

namespace EnglishSquares
{
	/// <summary>
	/// map of units and active background (e.g. towns)
	/// </summary>
	public class Map
	{
		public MapUnit[] playerUnits;
		public BattleEngine battleEngine = new BattleEngine();
		public ComputerGeneral computerGeneral = new ComputerGeneral();

		public playerEnum currentTurn;
		public playerControlEnum player1Control = playerControlEnum.Human;
		public playerControlEnum player2Control = playerControlEnum.Computer;

		public unitCommands commandType = unitCommands.MoveAndFire;
		public BattleStatus battleStatus = null;

		public bool madeMove = false;

		public int maxUnits = Constants.MAX_UNITS;
		public int numberUnits = 0;

		public int highlightedUnit = -1;
		public int moveOffset = 0;
		public int fireOffset = 0;

		public int loadedNumberTurns = 0;
		public int loadedElapsedMinutes = 0;

		public bool showGrid = true;
		public bool showBackground = true;
		public bool showStatus = true;
		public bool showUnits = true;
		public bool showAvailableMoves = true;

        // minmax algorithm depth
		public int movesConsidered = 0;
		public int lookaheadMoves = Constants.LOOKAHEAD_DEPTH;

		private readonly Image mapEmptyTile = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.EMPTY_TILE);

		// background fields
		private Image field1 = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.FIELD_DEFAULT);
		private string fieldFilename = "";

		// flags
		private readonly Image flagBritish = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.BRITISH_FLAG);
		private readonly Image flagFrench = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.FRENCH_FLAG);
		private readonly Image flagRussian = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.RUSSIAN_FLAG);
		private readonly Image flagPrussian = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.PRUSSIAN_FLAG);
		private readonly Image flagAustrian = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.AUSTRIAN_FLAG);

		// red unit images
		private readonly Image redSquareInfantry = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.RED_INFANTRY_SQUARE);
		private readonly Image redLineInfantry = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.RED_INFANTRY_LINE);
		private readonly Image redColumnInfantry = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.RED_INFANTRY_COLUMN);
		private readonly Image redLineCavalry = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.RED_CAVALRY_LINE);
		private readonly Image redColumnCavalry = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.RED_CAVALRY_COLUMN);
		private readonly Image redLineArtillery = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.RED_ARTILLERY_LINE);
		private readonly Image redColumnArtillery = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.RED_ARTILLERY_COLUMN);

		// blue unit images
		private readonly Image blueSquareInfantry = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.BLUE_INFANTRY_SQUARE);
		private readonly Image blueLineInfantry = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.BLUE_INFANTRY_LINE);
		private readonly Image blueColumnInfantry = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.BLUE_INFANTRY_COLUMN);
		private readonly Image blueLineCavalry = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.BLUE_CAVALRY_LINE);
		private readonly Image blueColumnCavalry = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.BLUE_CAVALRY_COLUMN);
		private readonly Image blueLineArtillery = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.BLUE_ARTILLERY_LINE);
		private readonly Image blueColumnArtillery = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.BLUE_ARTILLERY_COLUMN);

		protected Image logicalMap;
		protected Graphics logicalG;

		private int mapWidth = 20;
		private int mapHeight = 12;

		public int mapPixWidth = 0;
		public int mapPixHeight = 0;

		public string mapTitle = "";
		public string mapDescription = "";

		public string player1 = "";
		public string player2 = "";

		public Map()
		{
			playerUnits = new MapUnit[maxUnits];
		}

		public void createMapBitmap()
		{
			mapPixWidth = mapWidth*mapEmptyTile.Width;
			mapPixHeight = mapHeight*mapEmptyTile.Height;

			// create logical map
			if (logicalMap != null)
			{
				logicalMap.Dispose();
				logicalG.Dispose();
			}

			logicalMap = new Bitmap(mapPixWidth, mapPixHeight);
			logicalG = Graphics.FromImage(logicalMap);

			// background graphic
			if (showBackground == true)
			{
				logicalG.DrawImage(field1, 0, 0, mapPixWidth, mapPixHeight);
			}

			// Re-Draw map from array of units
			if (showUnits == true)
			{
				drawUnits();
			}

			// overlay grid
			if (showGrid == true)
			{
				paintGrid();
			}
		
			// show highlighted units / squares
			if (showAvailableMoves == true)
			{
				paintHighlights();
			}
		}

		protected void paintGrid()
		{
			for (int i=0; i<=mapPixWidth; i+=mapEmptyTile.Width)
				logicalG.DrawLine(new Pen(Color.Black, 1), i, 0, i, mapPixHeight);

			for (int j=0; j<=mapPixHeight; j+=mapEmptyTile.Height)
				logicalG.DrawLine(new Pen(Color.Black, 1), 0, j, mapPixWidth, j);

			logicalG.DrawLine(new Pen(Color.Black, 1), mapPixWidth-1, 0, mapPixWidth-1, mapPixHeight-1);
			logicalG.DrawLine(new Pen(Color.Black, 1), 0, mapPixHeight-1, mapPixWidth-1, mapPixHeight-1);
		}

		protected void highlightSquare(int sqX, int sqY, Color sqColour)
		{
			logicalG.DrawRectangle(	new Pen(sqColour, 2), 
				sqX*mapEmptyTile.Width, 
				sqY*mapEmptyTile.Height, 
				mapEmptyTile.Width, 
				mapEmptyTile.Height);
		}

		protected void paintHighlights()
		{
			int startSquareX = 0;
			int endSquareX = 0;
			int startSquareY = 0;
			int endSquareY = 0;

			// last unit battle move
			if (battleEngine.lastU1 > -1)
			{
				highlightSquare(playerUnits[battleEngine.lastU1].x, playerUnits[battleEngine.lastU1].y, Color.Yellow);
			}
			if (battleEngine.lastU2 > -1)
			{
				highlightSquare(playerUnits[battleEngine.lastU2].x, playerUnits[battleEngine.lastU2].y, Color.Yellow);
			}

			if (highlightedUnit > -1)
			{
				moveOffset = 0;
				fireOffset = 0;

				// possible fire
				if (commandType == unitCommands.MoveAndFire || commandType == unitCommands.FireOnly)
				{
					fireOffset = battleEngine.possibleFire(playerUnits[highlightedUnit].typeUnit, playerUnits[highlightedUnit].stanceUnit);
				}
				else
				{
					fireOffset = 0;
				}

				startSquareX = playerUnits[highlightedUnit].x - fireOffset;
				endSquareX = playerUnits[highlightedUnit].x + fireOffset;
				startSquareY = playerUnits[highlightedUnit].y - fireOffset;
				endSquareY = playerUnits[highlightedUnit].y + fireOffset;

				for (int i = startSquareX; i <= endSquareX; i++)
				{
					for (int j = startSquareY; j <= endSquareY; j++)
					{
						if (i >= 0 && j >= 0 && i < mapWidth && j < mapHeight)
						{
							highlightSquare(i, j, Color.DarkRed);
						}
					}
				}

				// possible moves
				if (commandType == unitCommands.MoveAndFire || commandType == unitCommands.MoveOnly)
				{
					moveOffset = battleEngine.possibleMoves(playerUnits[highlightedUnit].typeUnit, playerUnits[highlightedUnit].stanceUnit);
				}
				else
				{
					moveOffset = 0;
				}

				startSquareX = playerUnits[highlightedUnit].x - moveOffset;
				endSquareX = playerUnits[highlightedUnit].x + moveOffset;
				startSquareY = playerUnits[highlightedUnit].y - moveOffset;
				endSquareY = playerUnits[highlightedUnit].y + moveOffset;

				for (int i = startSquareX; i <= endSquareX; i++)
				{
					for (int j = startSquareY; j <= endSquareY; j++)
					{
						if (i >= 0 && j >= 0 && i < mapWidth && j < mapHeight)
						{
							highlightSquare(i, j, Color.Red);
						}
					}
				}

				// actual unit
				highlightSquare(playerUnits[highlightedUnit].x, playerUnits[highlightedUnit].y, Color.Yellow);
			}
		}

		public void drawMapWindow(Graphics g, Rectangle rectClip, Rectangle rectSource)
		{
			g.DrawImage(logicalMap, rectClip, rectSource, GraphicsUnit.Pixel);
		}

		public bool drawUnits()
		{
			Image unitImage;
			for (int u=0; u<numberUnits; u++)
			{
				unitImage = lookupUnitGraphic(playerUnits[u].player, playerUnits[u].typeUnit, playerUnits[u].stanceUnit);
				logicalG.DrawImage(unitImage, playerUnits[u].x*mapEmptyTile.Width+1, playerUnits[u].y*mapEmptyTile.Height+1, mapEmptyTile.Width, mapEmptyTile.Height);	
			}

			return(true);
		}

		protected Image lookupUnitGraphic(playerEnum player, unitEnum type, stanceEnum stance)
		{
			Image unitImage;

			switch (player)
			{
				case (playerEnum.Red):	
					switch(type)
					{
						case (unitEnum.Infantry):
							switch(stance)
							{
								case (stanceEnum.Square):
									unitImage = redSquareInfantry;
									break;
								case (stanceEnum.Line):
									unitImage = redLineInfantry;
									break;
								case (stanceEnum.Column):
									unitImage = redColumnInfantry;
									break;
								default:
									unitImage = mapEmptyTile;
									break;
							}
							break;
						case (unitEnum.Cavalry):
							switch(stance)
							{
								case (stanceEnum.Line):
									unitImage = redLineCavalry;
									break;
								case (stanceEnum.Column):
									unitImage = redColumnCavalry;
									break;
								default:
									unitImage = mapEmptyTile;
									break;
							}
							break;
						case (unitEnum.Artillery):
							switch(stance)
							{
								case (stanceEnum.Line):
									unitImage = redLineArtillery;
									break;
								case (stanceEnum.Column):
									unitImage = redColumnArtillery;
									break;
								default:
									unitImage = mapEmptyTile;
									break;
							}
							break;
						default:
							unitImage = mapEmptyTile;
							break;
					}
					break;
				case (playerEnum.Blue):	
					switch(type)
					{
						case (unitEnum.Infantry):
							switch(stance)
							{
								case (stanceEnum.Square):
									unitImage = blueSquareInfantry;
									break;
								case (stanceEnum.Line):
									unitImage = blueLineInfantry;
									break;
								case (stanceEnum.Column):
									unitImage = blueColumnInfantry;
									break;
								default:
									unitImage = mapEmptyTile;
									break;
							}
							break;
						case (unitEnum.Cavalry):
							switch(stance)
							{
								case (stanceEnum.Line):
									unitImage = blueLineCavalry;
									break;
								case (stanceEnum.Column):
									unitImage = blueColumnCavalry;
									break;
								default:
									unitImage = mapEmptyTile;
									break;
							}
							break;
						case (unitEnum.Artillery):
							switch(stance)
							{
								case (stanceEnum.Line):
									unitImage = blueLineArtillery;
									break;
								case (stanceEnum.Column):
									unitImage = blueColumnArtillery;
									break;
								default:
									unitImage = mapEmptyTile;
									break;
							}
							break;
						default:
							unitImage = mapEmptyTile;
							break;
					}						
					break;
				default: 
					unitImage = mapEmptyTile;
					break;
			}

			return(unitImage);
		}

		public bool highlightUnit(int x, int y)
		{
			bool hitUnit = false;
			bool commandDecided = false;
			int testmapX = 0;
			int testmapY = 0;
			
			testmapX = x / mapEmptyTile.Width;
			testmapY = y / mapEmptyTile.Height;

			if (testmapX >= mapWidth || testmapX < 0 || testmapY >= mapHeight || testmapY < 0)
			{
				highlightedUnit = -1;
				return(false);
			}

			// make sure all support values are up to date
			updateSupportValues();

			madeMove = false;

			// test for move / fire command on highlighted unit
			if (highlightedUnit > -1)
			{
				if (playerUnits[highlightedUnit].player != currentTurn)
				{
					// do nothing, it's not your turn (removed messagebox, which gets annoying)
				}
				else
				{
					if (testmapX >= (playerUnits[highlightedUnit].x - moveOffset) && testmapX <= (playerUnits[highlightedUnit].x + moveOffset))
					{
						if (testmapY >= (playerUnits[highlightedUnit].y - moveOffset) && testmapY <= (playerUnits[highlightedUnit].y + moveOffset))
						{
							int f = battleEngine.findUnit(ref playerUnits, testmapX, testmapY, numberUnits);
							if (f > -1)
							{
								if (playerUnits[f].player != playerUnits[highlightedUnit].player)
								{
									playerUnits[highlightedUnit].x = testmapX;
									playerUnits[highlightedUnit].y = testmapY;

									//
									// Close up battle
									//
									battleEngine.fightUnits(ref playerUnits, highlightedUnit, f, numberUnits, true);

									madeMove = true;
									commandDecided = true;
								}
							}
							else
							{
								battleEngine.moveUnit(ref playerUnits, highlightedUnit, testmapX, testmapY);

								madeMove = true;
								commandDecided = true;
							}
						}
					}

					if (commandDecided == false)
					{
						if (testmapX >= (playerUnits[highlightedUnit].x - fireOffset) && testmapX <= (playerUnits[highlightedUnit].x + fireOffset))
						{
							if (testmapY >= (playerUnits[highlightedUnit].y - fireOffset) && testmapY <= (playerUnits[highlightedUnit].y + fireOffset))
							{
								int f = battleEngine.findUnit(ref playerUnits, testmapX, testmapY, numberUnits);
								if (f > -1 && playerUnits[f].player != playerUnits[highlightedUnit].player)
								{
									//
									// artillery or long-range infantry shots
									//
									battleEngine.fireAtUnit(ref playerUnits, highlightedUnit, f, numberUnits, true);

									madeMove = true;
									commandDecided = true;
								}
							}
						}
					}
				}

				highlightedUnit = -1;
			}

			// test if a unit is to be highlighted
			if (commandDecided == false)
			{
				int f = battleEngine.findUnit(ref playerUnits, testmapX, testmapY, numberUnits);
				if (f > -1)
				{
					hitUnit = true;
					highlightedUnit = f;
				}
			}

			return(hitUnit);
		}

		public void clearMap()
		{
			player1 = "";
			player2 = "";
			numberUnits = 0;
			currentTurn = playerEnum.Red;
		}

		public void updateSupportValues()
		{
			int u = 0;
			while (u < numberUnits)
			{
				playerUnits[u].support = battleEngine.calculateSupport(ref playerUnits, u, numberUnits);
				u++;
			}
		}

		public bool loadMap(string fileMap, bool encrypMe)
		{
			bool success = false;
			string tempBuffer = "";
			int currentPlayer = 0;
			string typeChar = "";
			string stanceChar = "";
			numberUnits = 0;

			try
			{
				clearMap();

				FileStream fRead = new FileStream(fileMap, FileMode.Open);
				XmlTextReader xmlReader = new XmlTextReader(fRead);
				while (xmlReader.Read())
				{
					if (xmlReader.NodeType == XmlNodeType.Element)
					{
						if (xmlReader.Name == "Setup")
						{
							mapWidth = Convert.ToInt32(xmlReader.GetAttribute("MapSizeX"));
							mapHeight = Convert.ToInt32(xmlReader.GetAttribute("MapSizeY"));

							loadedNumberTurns = Convert.ToInt32(xmlReader.GetAttribute("NumberTurns"));
							loadedElapsedMinutes = Convert.ToInt32(xmlReader.GetAttribute("ElapsedMinutes"));
						
							field1.Dispose();
							field1 = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + "\\Images\\" + xmlReader.GetAttribute("Background"));
							fieldFilename = xmlReader.GetAttribute("Background");
						}

						if (xmlReader.Name == "Title")
						{
							mapTitle = xmlReader.GetAttribute("Name");
						}

						if (xmlReader.Name == "Description")
						{
							xmlReader.Read();
							if (xmlReader.NodeType == XmlNodeType.Text)
							{
								mapDescription = xmlReader.Value;
							}
						}

						if (xmlReader.Name == "Units")
						{
							tempBuffer = xmlReader.GetAttribute("Side");

							if (player1.Length == 0)
							{
								player1 = tempBuffer;
								currentPlayer = 1;
							}
							else
							{
								if (player2.Length == 0)
								{
									player2 = tempBuffer;
									currentPlayer = 2;
								}
								else
								{
									MessageBox.Show("Error is map XML file - only 2 players are supported in this version.", "Error in map XML", MessageBoxButtons.OK, MessageBoxIcon.Error);
									return(false);
								}
							}

							string computerStance = xmlReader.GetAttribute("ComputerStance");
							string computerDepth = xmlReader.GetAttribute("Depth");

							if (computerDepth != null && computerDepth.Length > 0)
							{
								lookaheadMoves = Convert.ToInt32(computerDepth);
							}

							if (computerStance != null && computerStance.Length > 0)
							{
								switch (computerStance)
								{
									case ("Aggressive"):
										computerGeneral.computerCharacter = enumCharacter.Aggressive;
										break;
									case ("Defensive"):
										computerGeneral.computerCharacter = enumCharacter.Defensive;
										break;
									case ("Neutral"):
										computerGeneral.computerCharacter = enumCharacter.Neutral;
										break;
									case ("Suicidal"):
										computerGeneral.computerCharacter = enumCharacter.Suicidal;
										break;
								}
							}

							string pruneFar = xmlReader.GetAttribute("PruneFar");
							if (pruneFar != null && pruneFar.Length > 0)
							{
								if (pruneFar == "Y")
								{
									computerGeneral.pruneFar = true;
								}
								else
								{
									computerGeneral.pruneFar = false;
								}
							}

							string quickCalc = xmlReader.GetAttribute("QuickCalc");
							if (quickCalc != null && quickCalc.Length > 0)
							{
								if (quickCalc == "Y")
								{
									computerGeneral.quickCalc = true;
								}
								else
								{
									computerGeneral.quickCalc = false;
								}
							}
						}

						if (xmlReader.Name == "Unit")
						{
							playerUnits[numberUnits] = new MapUnit();

							playerUnits[numberUnits].x = Convert.ToInt32(xmlReader.GetAttribute("X"));
							playerUnits[numberUnits].y = Convert.ToInt32(xmlReader.GetAttribute("Y"));
							playerUnits[numberUnits].description = xmlReader.GetAttribute("Description");
							playerUnits[numberUnits].morale = Convert.ToInt32(xmlReader.GetAttribute("Morale"));
							playerUnits[numberUnits].strength = Convert.ToInt32(xmlReader.GetAttribute("Strength"));
							playerUnits[numberUnits].training = Convert.ToInt32(xmlReader.GetAttribute("Training"));

							if (currentPlayer == 1) playerUnits[numberUnits].player = playerEnum.Red;
							if (currentPlayer == 2) playerUnits[numberUnits].player = playerEnum.Blue;
							
							typeChar = xmlReader.GetAttribute("Type");
							stanceChar = xmlReader.GetAttribute("Stance");

							if (typeChar == "I" || typeChar == "Infantry")		playerUnits[numberUnits].typeUnit = unitEnum.Infantry;
							if (typeChar == "C" || typeChar == "Cavalry")		playerUnits[numberUnits].typeUnit = unitEnum.Cavalry;
							if (typeChar == "A" || typeChar == "Artillery")		playerUnits[numberUnits].typeUnit = unitEnum.Artillery;

							if (stanceChar == "L" || stanceChar == "Line")		playerUnits[numberUnits].stanceUnit = stanceEnum.Line;
							if (stanceChar == "C" || stanceChar == "Column")	playerUnits[numberUnits].stanceUnit = stanceEnum.Column;
							if (stanceChar == "S" || stanceChar == "Square")	playerUnits[numberUnits].stanceUnit = stanceEnum.Square;

							numberUnits++;
						}
					}
				}
				xmlReader.Close();
				fRead.Close();
				highlightedUnit = -1;

				battleEngine.newBattle(mapWidth, mapHeight);
				computerGeneral.newGeneral(mapWidth, mapHeight, Constants.LOOKAHEAD_DEPTH);
				computerGeneral.modelBattleEngine = battleEngine;
				this.lastU = -1;

				updateSupportValues();

				success = true;
			}
			catch (Exception e)
			{
				MessageBox.Show("Error is map file - " + e.Message, "Invalid Map", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return(false);
			}

			if (success == true)
			{
				createMapBitmap();
			}

			return(success);
		}

		public bool saveMap(string filename, bool encrypMe)
		{
			bool success = false;

			try
			{
				FileStream fWrite = new FileStream(filename, FileMode.Create);
				XmlTextWriter xmlSave = new XmlTextWriter(fWrite, System.Text.Encoding.ASCII);

				xmlSave.WriteStartDocument();
				xmlSave.WriteStartElement("Map");

				xmlSave.WriteStartElement("Setup");
				xmlSave.WriteAttributeString("MapSizeX", mapWidth.ToString());
				xmlSave.WriteAttributeString("MapSizeY", mapHeight.ToString());
				xmlSave.WriteAttributeString("Background", fieldFilename);
				xmlSave.WriteAttributeString("NumberTurns", loadedNumberTurns.ToString());
				xmlSave.WriteAttributeString("ElapsedMinutes", loadedElapsedMinutes.ToString());
		
				xmlSave.WriteStartElement("Title");
				xmlSave.WriteAttributeString("Name", mapTitle);
				xmlSave.WriteEndElement();
					
				xmlSave.WriteStartElement("Description");
				xmlSave.WriteString(mapDescription);
				xmlSave.WriteEndElement();
					
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Units");
				xmlSave.WriteAttributeString("Side", player1);
				xmlSave.WriteAttributeString("ComputerStance", computerGeneral.computerCharacter.ToString());
				xmlSave.WriteAttributeString("Depth", lookaheadMoves.ToString());
				xmlSave.WriteAttributeString("PruneFar", computerGeneral.pruneFar == true ?"Y":"N");
				xmlSave.WriteAttributeString("QuickCalc", computerGeneral.quickCalc == true ?"Y":"N");
				for (int p1=0; p1 < numberUnits; p1++)
				{
					if (playerUnits[p1].player == playerEnum.Red)
					{
						xmlSave.WriteStartElement("Unit");
						xmlSave.WriteAttributeString("X", playerUnits[p1].x.ToString());
						xmlSave.WriteAttributeString("Y", playerUnits[p1].y.ToString());
						xmlSave.WriteAttributeString("Description", playerUnits[p1].description);
						xmlSave.WriteAttributeString("Morale", playerUnits[p1].morale.ToString());
						xmlSave.WriteAttributeString("Strength", playerUnits[p1].strength.ToString());
						xmlSave.WriteAttributeString("Training", playerUnits[p1].training.ToString());
						xmlSave.WriteAttributeString("Type", playerUnits[p1].typeUnit.ToString());
						xmlSave.WriteAttributeString("Stance", playerUnits[p1].stanceUnit.ToString());
						xmlSave.WriteEndElement();
					}
				}	
				xmlSave.WriteEndElement();
				
				xmlSave.WriteStartElement("Units");
				xmlSave.WriteAttributeString("Side", player2);
				xmlSave.WriteAttributeString("ComputerStance", computerGeneral.computerCharacter.ToString());
				xmlSave.WriteAttributeString("Depth", lookaheadMoves.ToString());
				xmlSave.WriteAttributeString("PruneFar", computerGeneral.pruneFar == true ?"Y":"N");
				xmlSave.WriteAttributeString("QuickCalc", computerGeneral.quickCalc == true ?"Y":"N");
				for (int p2=0; p2 < numberUnits; p2++)
				{
					if (playerUnits[p2].player == playerEnum.Blue)
					{
						xmlSave.WriteStartElement("Unit");
						xmlSave.WriteAttributeString("X", playerUnits[p2].x.ToString());
						xmlSave.WriteAttributeString("Y", playerUnits[p2].y.ToString());
						xmlSave.WriteAttributeString("Description", playerUnits[p2].description);
						xmlSave.WriteAttributeString("Morale", playerUnits[p2].morale.ToString());
						xmlSave.WriteAttributeString("Strength", playerUnits[p2].strength.ToString());
						xmlSave.WriteAttributeString("Training", playerUnits[p2].training.ToString());
						xmlSave.WriteAttributeString("Type", playerUnits[p2].typeUnit.ToString());
						xmlSave.WriteAttributeString("Stance", playerUnits[p2].stanceUnit.ToString());
						xmlSave.WriteEndElement();
					}				
				}	
				xmlSave.WriteEndElement();
				xmlSave.Close();
				fWrite.Close();

				success = true;
			}
			catch (Exception e)
			{
				MessageBox.Show("Error when saving file - " + e.Message, "Failed Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return(false);
			}

			return(success);
		}

		public bool openSavedGame(string filename)
		{
			return(loadMap(filename, true));
		}

		public bool saveGame(string filename)
		{
			return(saveMap(filename, true));
		}

		public int makeComputerTurn(playerEnum player)
		{
			int bestWeight = computerGeneral.makeMove(ref playerUnits, player, numberUnits, 0, lookaheadMoves-1, Constants.MINIMUM_WEIGHT);
			movesConsidered = computerGeneral.movesConsidered;

			if (bestWeight == Constants.MINIMUM_WEIGHT)
			{
				//MessageBox.Show("No computer move made! Error in minimax algorithm to find move, please report error to lee@fullerdata.com.", "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}

			return(bestWeight);
		}

		public bool checkGameOver()
		{
			int countBlue = battleEngine.countLivingUnits(ref playerUnits, playerEnum.Blue, numberUnits);
			int countRed = battleEngine.countLivingUnits(ref playerUnits, playerEnum.Red, numberUnits);

			if (countBlue == 0 || countRed == 0)
			{
				return(true);
			}
			else
			{
				return(false);
			}
		}

		public playerEnum whoWon()
		{
			int countBlue = battleEngine.countLivingUnits(ref playerUnits, playerEnum.Blue, numberUnits);
			int countRed = battleEngine.countLivingUnits(ref playerUnits, playerEnum.Red, numberUnits);

			if (countRed > countBlue)
			{
				return(playerEnum.Red);
			}
			else
			{
				return(playerEnum.Blue);
			}
		}

		#region Unit Formation Changes

		public void unitToSquare()
		{
			if (highlightedUnit > -1)
			{
				if (playerUnits[highlightedUnit].player != currentTurn)
				{
					highlightedUnit = -1;
				}
				else
				{
					if (playerUnits[highlightedUnit].typeUnit == unitEnum.Infantry)
					{
						if (playerUnits[highlightedUnit].stanceUnit != stanceEnum.Square)
						{
							battleEngine.changeUnitStance(ref playerUnits, highlightedUnit, stanceEnum.Square);
							highlightedUnit = -1;
							madeMove = true;
						}
					}
					else
					{
						MessageBox.Show("Only Infantry can form into the British Square defensive formation", "Invalid Formation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					}
				}
			}
		}

		public void unitToLine()
		{
			if (highlightedUnit > -1)
			{
				if (playerUnits[highlightedUnit].player != currentTurn)
				{
					highlightedUnit = -1;
				}
				else
				{
					if (playerUnits[highlightedUnit].stanceUnit != stanceEnum.Line)
					{
						battleEngine.changeUnitStance(ref playerUnits, highlightedUnit, stanceEnum.Line);
						highlightedUnit = -1;
						madeMove = true;
					}
				}
			}
		}

		public void unitToColumn()
		{
			if (highlightedUnit > -1)
			{
				if (playerUnits[highlightedUnit].player != currentTurn)
				{
					highlightedUnit = -1;
				}
				else
				{
					if (playerUnits[highlightedUnit].stanceUnit != stanceEnum.Column)
					{
						battleEngine.changeUnitStance(ref playerUnits, highlightedUnit, stanceEnum.Column);
						highlightedUnit = -1;
						madeMove = true;
					}
				}
			}
		}

		#endregion

		#region Player Properties

		//
		// Player Properties
		//

		public string currentPlayerTurn
		{
			get 
			{ 
				if (currentTurn == playerEnum.Red)	return(player1);
				if (currentTurn == playerEnum.Blue) return(player2);

				return("");
			}
		}

		public string player1Strength
		{
			get
			{
				int strength = battleEngine.calculatePlayerWeight(ref playerUnits, playerEnum.Red, numberUnits);
				return(Convert.ToString(strength));
			}
		}
		
		public string player2Strength
		{
			get
			{
				int strength = battleEngine.calculatePlayerWeight(ref playerUnits, playerEnum.Blue, numberUnits);
				return(Convert.ToString(strength));
			}
		}

		public string player1Units
		{
			get
			{
				int units = battleEngine.countUnits(ref playerUnits, playerEnum.Red, numberUnits);
				return(Convert.ToString(units));
			}
		}

		public string player2Units
		{
			get
			{
				int units = battleEngine.countUnits(ref playerUnits, playerEnum.Blue, numberUnits);
				return(Convert.ToString(units));
			}
		}

		public string player1DeadUnits
		{
			get
			{
				int units = battleEngine.countDeadUnits(ref playerUnits, playerEnum.Red, numberUnits);
				return(Convert.ToString(units));
			}
		}

		public string player2DeadUnits
		{
			get
			{
				int units = battleEngine.countDeadUnits(ref playerUnits, playerEnum.Blue, numberUnits);
				return(Convert.ToString(units));
			}
		}

		public string player1Battles
		{
			get
			{
				int battles = battleEngine.countBattles(ref playerUnits, playerEnum.Red, numberUnits);
				return(Convert.ToString(battles));
			}
		}

		public string player2Battles
		{
			get
			{
				int battles = battleEngine.countBattles(ref playerUnits, playerEnum.Blue, numberUnits);
				return(Convert.ToString(battles));
			}
		}

		public string computerStance
		{
			get
			{
				return(computerGeneral.computerCharacter.ToString());
			}
		}

		#endregion

		#region Last Battle Properties

		//
		// Properties for the last Battle
		//

		public string lastBattleResult1
		{
			get
			{
				return(Convert.ToString(battleEngine.lastResult1));
			}
		}

		public string lastBattleResult2
		{
			get
			{
				return(Convert.ToString(battleEngine.lastResult2));
			}
		}

		public string lastBattleUnit1Damage
		{
			get
			{
				return(battleEngine.lastUnit1Damage.ToString());
			}
		}

		public string lastBattleUnit2Damage
		{
			get
			{
				return(battleEngine.lastUnit2Damage.ToString());
			}
		}

		public string lastBattleMove
		{
			get
			{
				return(battleEngine.lastMove.ToString());
			}
		}

		public string lastBattleUnit1Strength
		{
			get
			{
				if (battleEngine.lastU1 >= 0)
					return(battleEngine.lastUnit1.strength.ToString());
				else
					return("");
			}
		}

		public string lastBattleUnit2Strength
		{
			get
			{
				if (battleEngine.lastU2 >= 0)
					return(battleEngine.lastUnit2.strength.ToString());
				else
					return("");
			}
		}

		public string lastBattleUnit1Morale
		{
			get
			{
				if (battleEngine.lastU1 >= 0)
					return(battleEngine.lastUnit1.morale.ToString());
				else
					return("");
			}
		}

		public string lastBattleUnit2Morale
		{
			get
			{
				if (battleEngine.lastU2 >= 0)
					return(battleEngine.lastUnit2.morale.ToString());
				else
					return("");
			}
		}

		public string lastBattleUnit1Training
		{
			get
			{
				if (battleEngine.lastU1 >= 0)
					return(battleEngine.lastUnit1.training.ToString());
				else
					return("");
			}
		}

		public string lastBattleUnit2Training
		{
			get
			{
				if (battleEngine.lastU2 >= 0)
					return(battleEngine.lastUnit2.training.ToString());
				else
					return("");
			}
		}

		public string lastBattleUnit1Support
		{
			get
			{
				if (battleEngine.lastU1 >= 0)
					return(battleEngine.lastUnit1.support.ToString());
				else
					return("");
			}
		}

		public string lastBattleUnit2Support
		{
			get
			{
				if (battleEngine.lastU2 >= 0)
					return(battleEngine.lastUnit2.support.ToString());
				else
					return("");
			}
		}

		public string lastBattleUnit1Description
		{
			get
			{
				if (battleEngine.lastU1 >= 0)
					return(battleEngine.lastUnit1.description);
				else
					return("");
			}
		}

		public string lastBattleUnit2Description
		{
			get
			{
				if (battleEngine.lastU2 >= 0)
					return(battleEngine.lastUnit2.description);
				else
					return("");
			}
		}

		public string lastCalc1
		{
			get
			{
				if (battleEngine.lastU1 >= 0)
					return(battleEngine.lastCalc1);
				else
					return("");
			}
		}
		
		public string lastCalc2
		{
			get
			{
				if (battleEngine.lastU2 >= 0)
					return(battleEngine.lastCalc2);
				else
					return("");
			}
		}

		public int lastWeightBefore1
		{
			get
			{
				if (battleEngine.lastWeightBefore1 > -1)
					return(battleEngine.lastWeightBefore1);
				else
					return(0);
			}
		}

		public int lastWeightBefore2
		{
			get
			{
				if (battleEngine.lastWeightBefore2 > -1)
					return(battleEngine.lastWeightBefore2);
				else
					return(0);
			}
		}

		public int lastU
		{
			set
			{
				battleEngine.lastU1 = value;
				battleEngine.lastU2 = value;
				battleEngine.lastResult1 = EnumResult.Nothing;
				battleEngine.lastResult2 = EnumResult.Nothing;
				battleEngine.lastMove = enumMoveType.Nothing;
			}
		}

		public int lastU1
		{
			get
			{
				return(battleEngine.lastU1);
			}
		}

		public int lastU2
		{
			get
			{
				return(battleEngine.lastU2);
			}
		}

		public Image lastBattleUnit1Image
		{
			get
			{
				if (battleEngine.lastU1 >= 0)
				{
					Image unitImage;
					unitImage = lookupUnitGraphic(playerUnits[battleEngine.lastU1].player, playerUnits[battleEngine.lastU1].typeUnit, playerUnits[battleEngine.lastU1].stanceUnit);
					return(unitImage);
				}
				else
				{
					return(mapEmptyTile);
				}
			}
		}

		public Image lastBattleUnit2Image
		{
			get
			{
				if (battleEngine.lastU2 >= 0)
				{
					Image unitImage;
					unitImage = lookupUnitGraphic(playerUnits[battleEngine.lastU2].player, playerUnits[battleEngine.lastU2].typeUnit, playerUnits[battleEngine.lastU2].stanceUnit);
					return(unitImage);
				}
				else
				{
					return(mapEmptyTile);
				}
			}
		}

		#endregion

		#region Highlighted Unit Properties
		
		//
		// properties to control highlighted unit
		//

		public stanceEnum unitStance
		{
			get 
			{ 
				if (highlightedUnit > -1)
				{
					return(playerUnits[highlightedUnit].stanceUnit);
				}
				else
				{
					return(stanceEnum.Nothing);
				}
			}
			set 
			{ 
				if (highlightedUnit > -1)
				{
					playerUnits[highlightedUnit].stanceUnit = value;
				}
			}
		}

		public unitEnum unitType
		{
			get
			{
				if (highlightedUnit > -1)
				{
					return(playerUnits[highlightedUnit].typeUnit);
				}
				else
				{
					return(unitEnum.Nothing);
				}
			}
			set
			{
				if (highlightedUnit > -1)
				{
					playerUnits[highlightedUnit].typeUnit = value;
				}
			}
		}

		public playerEnum unitPlayer
		{
			get
			{
				if (highlightedUnit > -1)
				{
					return(playerUnits[highlightedUnit].player);
				}
				else
				{
					return(playerEnum.Nothing);
				}
			}
			set
			{
				if (highlightedUnit > -1)
				{
					playerUnits[highlightedUnit].player = value;
				}
			}
		}

		public int unitX
		{
			get
			{
				if (highlightedUnit > -1)
				{
					return(playerUnits[highlightedUnit].x);
				}
				else
				{
					return(-1);
				}
			}
			set
			{
				if (highlightedUnit > -1)
				{
					playerUnits[highlightedUnit].x = value;
				}
			}
		}

		public int unitY
		{
			get
			{
				if (highlightedUnit > -1)
				{
					return(playerUnits[highlightedUnit].y);
				}
				else
				{
					return(-1);
				}
			}
			set
			{
				if (highlightedUnit > -1)
				{
					playerUnits[highlightedUnit].y = value;
				}
			}
		}

		public int unitStrength
		{
			get
			{
				if (highlightedUnit > -1)
				{
					return(playerUnits[highlightedUnit].strength);
				}
				else
				{
					return(-1);
				}
			}
			set
			{
				if (highlightedUnit > -1)
				{
					playerUnits[highlightedUnit].strength = value;
				}
			}
		}

		public int unitMorale
		{
			get
			{
				if (highlightedUnit > -1)
				{
					return(playerUnits[highlightedUnit].morale);
				}
				else
				{
					return(-1);
				}
			}
			set
			{
				if (highlightedUnit > -1)
				{
					playerUnits[highlightedUnit].morale = value;
				}
			}
		}

		public int unitSupport
		{
			get
			{
				if (highlightedUnit > -1)
				{
					return(playerUnits[highlightedUnit].support);
				}
				else
				{
					return(-1);
				}
			}
			set
			{
				if (highlightedUnit > -1)
				{
					playerUnits[highlightedUnit].support = value;
				}
			}
		}

		public int unitTraining
		{
			get
			{
				if (highlightedUnit > -1)
				{
					return(playerUnits[highlightedUnit].training);
				}
				else
				{
					return(-1);
				}
			}
			set
			{
				if (highlightedUnit > -1)
				{
					playerUnits[highlightedUnit].training = value;
				}
			}
		}

		public int unitBattlesWon
		{
			get
			{
				if (highlightedUnit > -1)
				{
					return(playerUnits[highlightedUnit].battlesWon);
				}
				else
				{
					return(-1);
				}
			}
			set
			{
				if (highlightedUnit > -1)
				{
					playerUnits[highlightedUnit].battlesWon = value;
				}
			}
		}

		public string unitDescription
		{
			get
			{
				if (highlightedUnit > -1)
				{
					return(playerUnits[highlightedUnit].description);
				}
				else
				{
					return("No Unit Selected");
				}
			}
			set
			{
				if (highlightedUnit > -1)
				{
					playerUnits[highlightedUnit].description = value;
				}
			}
		}

		public Image unitImage
		{
			get
			{
				if (highlightedUnit > -1)
				{
					Image unitImage;
					unitImage = lookupUnitGraphic(playerUnits[highlightedUnit].player, playerUnits[highlightedUnit].typeUnit, playerUnits[highlightedUnit].stanceUnit);
					return(unitImage);
				}
				else
				{
					return(mapEmptyTile);
				}
			}
		}

		public string player1Name
		{
			get { return(player1); }
			set { player1 = value; }
		}

		public string player2Name
		{
			get { return(player2); }
			set { player2 = value; }
		}

		public string currentPlayerName
		{
			get 
			{ 
				if (highlightedUnit > -1)
				{
					if (playerUnits[highlightedUnit].player == playerEnum.Red)	return(player1);
					if (playerUnits[highlightedUnit].player == playerEnum.Blue) return(player2);
				}

				return("");
			}
		}

		#endregion

	}
}
