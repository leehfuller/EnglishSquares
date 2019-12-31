using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.Timers;
using System.Threading;
using System.Xml;
using System.IO;

namespace EnglishSquares
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class EnglishSquares : System.Windows.Forms.Form
	{
		protected string _onlineGUID = "";
		protected bool _sharewareVersion = Constants.FREE_VERSION;
		protected bool _showOnce;
		protected bool _showTitle;
		protected bool gameOver = false;

		protected bool playMusic = true;
		protected bool playFX = true;

		//private System.Threading.Thread threadSoundFX = null;
		//private System.Threading.Thread threadMusic = null;
		protected int titleMix = 0;

		private bool computerPlayerRed = Constants.COMPUTER_RED;
		private bool computerPlayerBlue = Constants.COMPUTER_BLUE;

		protected Image _mapTitle;	
		protected Image _displayBuffer;		
		protected Graphics _display;
		protected Map _map = new Map();

		protected GameStatus gameStatus = null;
		protected UnitStatus unitStatus = null;
		protected BattleStatus battleStatus = null;

		public int numberTurns = 0;
		public int elapsedMinutes = 0;

		private System.Timers.Timer timeGameplay;
		private System.Timers.Timer timeThinking;

		private bool thinkingBlue = false;
		private System.Threading.Thread threadBlueComputerMove = null;

		private bool thinkingRed = false;
		private System.Threading.Thread threadRedComputerMove = null;

		private DateTime startThink;
		private DateTime endThink;
		private string thinkTime = "0"; 

		//*********************************************************************

		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuView;
		private System.Windows.Forms.MenuItem menuHelp;
		private System.Windows.Forms.MenuItem menuNew;
		private System.Windows.Forms.MenuItem menuOpen;
		private System.Windows.Forms.MenuItem menuSave;
		private System.Windows.Forms.MenuItem menuClose;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuExit;
		private System.Windows.Forms.MenuItem menuAbout;
		private System.Windows.Forms.MenuItem menuMove;
		private System.Windows.Forms.MenuItem menuFire;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuGrid;
		private System.Windows.Forms.MenuItem menuBackground;
		private System.Windows.Forms.ContextMenu contextMenu1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuSoundEffects;
		private System.Windows.Forms.MenuItem menuMusic;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuSaveOptions;
		private System.Windows.Forms.MenuItem menuUnit;
		private System.Windows.Forms.MenuItem menuSquare;
		private System.Windows.Forms.MenuItem menuColumn;
		private System.Windows.Forms.MenuItem menuLine;
		private System.Windows.Forms.MenuItem menuGame;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuUnitStats;
		private System.Windows.Forms.MenuItem menuGameStats;
		private System.Windows.Forms.MenuItem menuOnlineStats;
		private System.Windows.Forms.MenuItem menuUnitView;
		private System.Windows.Forms.MenuItem menuContextMove;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuContextFire;
		private System.Windows.Forms.MenuItem menuContextSquare;
		private System.Windows.Forms.MenuItem menuContextColumn;
		private System.Windows.Forms.MenuItem menuContextLine;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuContextStatistics;
		private System.Windows.Forms.MenuItem menuMoves;
		private System.Windows.Forms.MenuItem menuContextBoth;
		private System.Windows.Forms.MenuItem menuBoth;
        private IContainer components;

        //*********************************************************************

        public EnglishSquares()
		{
			// Required for Windows Form Designer support
			InitializeComponent();

			_mapTitle = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.TITLE_IMAGE);
			_showTitle = true;
			enableGameMenus(false);
			_showOnce = true;
			_map.currentTurn = playerEnum.Red;
			gameOver = false;

			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Opaque | ControlStyles.DoubleBuffer, true);

			if (this.playMusic == true)
			{
				SoundFX.stopMusic();
				SoundFX.playMusic();
				titleMix++;
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			stopThinking();

			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}

			base.Dispose( disposing );
		}

		protected void stopThinking()
		{
			if (threadBlueComputerMove != null)
			{
				threadBlueComputerMove.Abort();
				threadBlueComputerMove = null;
			}

			if (threadRedComputerMove != null)
			{
				threadBlueComputerMove.Abort();
				threadRedComputerMove = null;
			}

			this.thinkingBlue = false;
			this.thinkingRed = false;
			this.Cursor = Cursors.Hand;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			_display.Clear(this.BackColor);

			if (_showTitle == true)
			{
				_display.DrawImage(_mapTitle, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
			}
			else
			{
				if (_map.currentTurn == playerEnum.Red)		this.Text = "English Squares - Red Turn - " + _map.player1Name;
				if (_map.currentTurn == playerEnum.Blue)	this.Text = "English Squares - Blue Turn - " + _map.player2Name;

				Rectangle sourceRectangle = e.ClipRectangle;
				sourceRectangle.X -= this.AutoScrollPosition.X;
				sourceRectangle.Y -= this.AutoScrollPosition.Y;

				_map.drawMapWindow(_display, e.ClipRectangle, sourceRectangle);
			}

			g.DrawImage(_displayBuffer, 0, 0);
			g.Dispose();

			if (_showOnce == true && _sharewareVersion == true)
			{
				_showOnce = false;
				AboutEnglishSquares aboutForm = new AboutEnglishSquares();
				aboutForm.checkShareware(_sharewareVersion);
				aboutForm.ShowDialog();
			}
		}

		protected override void OnMouseDown(MouseEventArgs e) 
		{
		}

		protected void showUnitStatus(int x, int y)
		{
			if (unitStatus != null)
			{
				unitStatus.showUnit(_map.unitStance, _map.unitType, _map.unitPlayer, _map.unitSupport, _map.unitStrength, _map.unitMorale, _map.unitTraining, _map.unitDescription, _map.unitBattlesWon, _map.unitX, _map.unitY, _map.unitImage, _map.currentPlayerName);

				if (_map.unitStrength != -1)
				{
					unitStatus.Focus();
				}
			}			
		}

		public void moveBlue()
		{
			thinkingBlue = true;
			_map.makeComputerTurn(playerEnum.Blue);
			madeTurn();
		}

		public void moveRed()
		{
			thinkingRed = true;
			_map.makeComputerTurn(playerEnum.Red);
			madeTurn();
		}

		public void madeTurn()
		{
			if (battleStatus != null && battleStatus.IsDisposed == false)
			{
				showLastBattle();
			}

			if (playFX == true)
			{
                // removed fx threading given managed api
				//if (threadSoundFX != null)
				//{
				//	threadSoundFX.Abort();
				//	threadSoundFX = null;
				//}

				unitEnum lastUnitType = unitEnum.Nothing;
				if (_map.lastU1 > -1)
				{
					lastUnitType = _map.playerUnits[_map.lastU1].typeUnit;
				}
				
				switch (_map.lastBattleMove)
				{
					case "Move":
						if (lastUnitType == unitEnum.Cavalry)
							SoundFX.playCavalryMarch();
						else
							SoundFX.playInfantryMarch();		
						break;
					case "Attack":
						if (lastUnitType == unitEnum.Cavalry)
							SoundFX.playCavalry();
						else
							SoundFX.playMuskets();
						break;
					case "Fire":
						if (lastUnitType == unitEnum.Artillery)
							SoundFX.playArtillery();
						else
							SoundFX.playMuskets();
						break;
					case "Stance":
						if (lastUnitType == unitEnum.Cavalry)
							SoundFX.playCavalryMarch();
						else
							SoundFX.playInfantryMarch();
						break;
					case "Nothing":
						SoundFX.playInfantryMarch();
						break;	
				}

				//threadSoundFX.Priority = ThreadPriority.Normal;
				//threadSoundFX.Start();
			}

			_map.madeMove = false;
			if (_map.currentTurn == playerEnum.Red) _map.currentTurn = playerEnum.Blue;
			else if (_map.currentTurn == playerEnum.Blue) _map.currentTurn = playerEnum.Red;
			_map.updateSupportValues();
			numberTurns++;

			// check for end of game state
			gameOver = _map.checkGameOver();
			if (gameOver == true)
			{
				_map.createMapBitmap();
				this.Invalidate();				

				GameComplete gameComplete = new GameComplete();
				if ((_map.whoWon() == playerEnum.Red && computerPlayerRed == false) ||
					(_map.whoWon() == playerEnum.Blue && computerPlayerBlue == false))
				{
					gameComplete.labelResult.Text = "Victory !";
					gameComplete.Text = "Game Over - Victory";
					gameComplete.useFireworks();
					SoundFX.stopMusic();
					SoundFX.playWinMusic();
				}
				else
				{
					gameComplete.labelResult.Text = "Defeated";
					gameComplete.Text = "Game Over - Defeated";
					gameComplete.burnFire();
					SoundFX.stopMusic();
					SoundFX.playDefeatMusic();
				}

				gameComplete.ShowDialog();

				return;
			}

			// computer control Blue player?
			if (_map.currentTurn == playerEnum.Blue && computerPlayerBlue == true)
			{
				_map.createMapBitmap();
				this.Invalidate();
				threadRedComputerMove = null;
				startThink = DateTime.Now;
				this.Cursor = Cursors.WaitCursor;
                threadBlueComputerMove = new Thread(new ThreadStart(this.moveBlue));
				threadBlueComputerMove.Priority = ThreadPriority.Normal;
				threadBlueComputerMove.Start();
				timeThinking.Start();
			}

			// computer control Red player?
			if (_map.currentTurn == playerEnum.Red && computerPlayerRed == true)
			{
				_map.createMapBitmap();
				this.Invalidate();
				threadBlueComputerMove = null;
				startThink = DateTime.Now;
				this.Cursor = Cursors.WaitCursor;
				threadRedComputerMove = new Thread(new ThreadStart(this.moveRed));
				threadRedComputerMove.Priority = ThreadPriority.Normal;
				threadRedComputerMove.Start();
				timeThinking.Start();
			}


            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(showGameStatus);
                Invoke(method);
            }
            else
            {
                showGameStatus();
            }
		}

		protected override void OnMouseUp(MouseEventArgs e) 
		{
			if (gameOver == true)
			{
				MessageBox.Show("The current game is over, please use the New menu to start a new game or the Open menu to resume a saved game.", "Game Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (thinkingBlue == true || thinkingRed == true)
			{
				//MessageBox.Show("The computer player is currently thinking about the possible moves, this shouldn't take long.", "Thinking", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			if (_showTitle == false)
			{
				if (e.Button == MouseButtons.Right)
				{
					bool unitRightHit = _map.highlightUnit(e.X + System.Math.Abs(this.AutoScrollPosition.X), e.Y + System.Math.Abs(this.AutoScrollPosition.Y));

					if (unitRightHit == true)
					{
						enableUnitMenus(true);

						_map.createMapBitmap();
						this.Invalidate();

						checkUnitStance();
						showUnitStatus(e.X, e.Y);

						contextMenu1.Show(this, new Point (e.X,e.Y));
					}
					else
					{
						enableUnitMenus(false);
					}
				}

				if (e.Button == MouseButtons.Left)
				{
					//MessageBox.Show(this.AutoScrollPosition.X.ToString() + "," + this.AutoScrollPosition.Y.ToString() + " vs " + e.X.ToString() + "," + e.Y.ToString());
					
					bool unitLeftHit = _map.highlightUnit(e.X + System.Math.Abs(this.AutoScrollPosition.X), e.Y + System.Math.Abs(this.AutoScrollPosition.Y));

					if (unitLeftHit == true)
					{
						// unit is highlighted

						if (_map.madeMove == true)	madeTurn();
						enableUnitMenus(true);

						_map.createMapBitmap();
						this.Invalidate();

						checkUnitStance();
						showUnitStatus(e.X, e.Y);
					}
					else
					{
						// no unit is now highlighted

						if (_map.madeMove == true)	madeTurn();
						enableUnitMenus(false);

						_map.createMapBitmap();
						this.Invalidate();

						checkUnitStance();
						showUnitStatus(e.X, e.Y);
					}
				}
			}
		}

		protected override void OnClick(EventArgs e)
		{
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			if (_map.highlightedUnit > -1)
			{
				if (unitStatus == null || unitStatus.IsDisposed == true)
					unitStatus = new UnitStatus();
				
				unitStatus.Show();		
				showUnitStatus(_map.unitX, _map.unitY);
			}
		}

		private void EnglishSquares_Load(object sender, System.EventArgs e)
		{
			PerformHealthCheck();
			RegisterLogonOnline();

			_displayBuffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height, Graphics.FromHwnd(this.Handle));
			_display = Graphics.FromImage(_displayBuffer);

			_map.createMapBitmap();
			this.AutoScrollMinSize = new Size(_map.mapPixWidth, _map.mapPixHeight);
			this.AutoScroll=false;

			menuSquare.Checked = false;
			menuColumn.Checked = false;
			menuLine.Checked = false;
			this.Cursor = Cursors.Hand;

			setAllMove(false);
			menuBoth.Checked = true;
			menuContextBoth.Checked = true;
		}

		protected void PerformHealthCheck()
		{
			//
			// TODO: Check all dependent files exist before trying to run
			//
		}

		protected void RegisterLogonOnline()
		{
			//
			// TODO: Create GUID and use to logon to fullerdata (to allow high score updates)
			//

			_onlineGUID = Guid.NewGuid().ToString();
			//MessageBox.Show(_onlineGUID);
		}

		private void OnTimeThinking(Object src, ElapsedEventArgs e)
		{
			if (threadBlueComputerMove != null)
			{
				if (threadBlueComputerMove.IsAlive == false)
				{
					thinkingBlue = false;
					timeThinking.Stop();
					endThink = DateTime.Now;
					TimeSpan ts = endThink - startThink; 
					thinkTime = Convert.ToInt32(ts.TotalSeconds).ToString();
					this.Cursor = Cursors.Hand;
					_map.createMapBitmap();
					this.Invalidate();

					if (gameStatus != null)
					{
						gameStatus.updateFields(_map.player1Name, _map.player2Name, _map.player1Strength, _map.player2Strength, _map.player1Units, _map.player2Units, _map.player1Battles, _map.player2Battles, _map.player1DeadUnits, _map.player2DeadUnits, numberTurns.ToString(), thinkTime, _map.movesConsidered, _map.lastWeightBefore1, _map.lastWeightBefore2, _map.computerStance);
					}
				}
			}

			if (threadRedComputerMove != null)
			{
				if (threadRedComputerMove.IsAlive == false)
				{
					thinkingRed = false;
					timeThinking.Stop();
					endThink = DateTime.Now;
					TimeSpan ts = endThink - startThink; 
					thinkTime = Convert.ToInt32(ts.TotalSeconds).ToString();
					this.Cursor = Cursors.Hand;
					_map.createMapBitmap();
					this.Invalidate();

					if (gameStatus != null)
					{
						gameStatus.updateFields(_map.player1Name, _map.player2Name, _map.player1Strength, _map.player2Strength, _map.player1Units, _map.player2Units, _map.player1Battles, _map.player2Battles, _map.player1DeadUnits, _map.player2DeadUnits, numberTurns.ToString(), thinkTime, _map.movesConsidered, _map.lastWeightBefore1, _map.lastWeightBefore2, _map.computerStance);
					}
				}
			}
		}

		private void OnTimeGame(Object src, ElapsedEventArgs e)
		{
			elapsedMinutes++;

			if (gameStatus != null)
			{
				gameStatus.showStatus(elapsedMinutes);
				gameStatus.updateFields(_map.player1Name, _map.player2Name, _map.player1Strength, _map.player2Strength, _map.player1Units, _map.player2Units, _map.player1Battles, _map.player2Battles, _map.player1DeadUnits, _map.player2DeadUnits, numberTurns.ToString(), thinkTime, _map.movesConsidered, _map.lastWeightBefore1, _map.lastWeightBefore2, _map.computerStance);
			}

			if (this._showTitle == true && this.playMusic == true)
			{
				SoundFX.stopMusic();

				if (titleMix == 0)
				{				
					SoundFX.playMusic();
					titleMix+=1;
				}
				else
				{
					titleMix = 0;
					SoundFX.playBackgroundMusic();
				}
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);

			if (_displayBuffer != null)
			{
				if (ClientRectangle.Width > 0 && ClientRectangle.Height > 0)
				{
					_displayBuffer.Dispose();
					_display.Dispose();
					_displayBuffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
					_display = Graphics.FromImage(_displayBuffer);
				}
			}

			this.Invalidate();	
		}

		//*********************************************************************

		#region Windows Form Designer generated code
		//*********************************************************************
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnglishSquares));
            this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
            this.menuGame = new System.Windows.Forms.MenuItem();
            this.menuNew = new System.Windows.Forms.MenuItem();
            this.menuOpen = new System.Windows.Forms.MenuItem();
            this.menuClose = new System.Windows.Forms.MenuItem();
            this.menuSave = new System.Windows.Forms.MenuItem();
            this.menuItem7 = new System.Windows.Forms.MenuItem();
            this.menuOnlineStats = new System.Windows.Forms.MenuItem();
            this.menuGameStats = new System.Windows.Forms.MenuItem();
            this.menuItem8 = new System.Windows.Forms.MenuItem();
            this.menuExit = new System.Windows.Forms.MenuItem();
            this.menuUnit = new System.Windows.Forms.MenuItem();
            this.menuMove = new System.Windows.Forms.MenuItem();
            this.menuFire = new System.Windows.Forms.MenuItem();
            this.menuBoth = new System.Windows.Forms.MenuItem();
            this.menuItem1 = new System.Windows.Forms.MenuItem();
            this.menuSquare = new System.Windows.Forms.MenuItem();
            this.menuColumn = new System.Windows.Forms.MenuItem();
            this.menuLine = new System.Windows.Forms.MenuItem();
            this.menuItem11 = new System.Windows.Forms.MenuItem();
            this.menuUnitStats = new System.Windows.Forms.MenuItem();
            this.menuView = new System.Windows.Forms.MenuItem();
            this.menuUnitView = new System.Windows.Forms.MenuItem();
            this.menuGrid = new System.Windows.Forms.MenuItem();
            this.menuMoves = new System.Windows.Forms.MenuItem();
            this.menuBackground = new System.Windows.Forms.MenuItem();
            this.menuItem9 = new System.Windows.Forms.MenuItem();
            this.menuSoundEffects = new System.Windows.Forms.MenuItem();
            this.menuMusic = new System.Windows.Forms.MenuItem();
            this.menuItem10 = new System.Windows.Forms.MenuItem();
            this.menuSaveOptions = new System.Windows.Forms.MenuItem();
            this.menuHelp = new System.Windows.Forms.MenuItem();
            this.menuAbout = new System.Windows.Forms.MenuItem();
            this.contextMenu1 = new System.Windows.Forms.ContextMenu();
            this.menuContextMove = new System.Windows.Forms.MenuItem();
            this.menuContextFire = new System.Windows.Forms.MenuItem();
            this.menuContextBoth = new System.Windows.Forms.MenuItem();
            this.menuItem14 = new System.Windows.Forms.MenuItem();
            this.menuContextSquare = new System.Windows.Forms.MenuItem();
            this.menuContextColumn = new System.Windows.Forms.MenuItem();
            this.menuContextLine = new System.Windows.Forms.MenuItem();
            this.menuItem3 = new System.Windows.Forms.MenuItem();
            this.menuContextStatistics = new System.Windows.Forms.MenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timeGameplay = new System.Timers.Timer();
            this.timeThinking = new System.Timers.Timer();
            ((System.ComponentModel.ISupportInitialize)(this.timeGameplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeThinking)).BeginInit();
            this.SuspendLayout();
            // 
            // mainMenu1
            // 
            this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuGame,
            this.menuUnit,
            this.menuView,
            this.menuHelp});
            // 
            // menuGame
            // 
            this.menuGame.Index = 0;
            this.menuGame.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuNew,
            this.menuOpen,
            this.menuClose,
            this.menuSave,
            this.menuItem7,
            this.menuOnlineStats,
            this.menuGameStats,
            this.menuItem8,
            this.menuExit});
            this.menuGame.Text = "Game";
            // 
            // menuNew
            // 
            this.menuNew.Index = 0;
            this.menuNew.Text = "New...";
            this.menuNew.Click += new System.EventHandler(this.menuNew_Click);
            // 
            // menuOpen
            // 
            this.menuOpen.Index = 1;
            this.menuOpen.Text = "Open...";
            this.menuOpen.Click += new System.EventHandler(this.menuOpen_Click);
            // 
            // menuClose
            // 
            this.menuClose.Index = 2;
            this.menuClose.Text = "Close";
            this.menuClose.Click += new System.EventHandler(this.menuClose_Click);
            // 
            // menuSave
            // 
            this.menuSave.Index = 3;
            this.menuSave.Text = "Save As...";
            this.menuSave.Click += new System.EventHandler(this.menuSave_Click);
            // 
            // menuItem7
            // 
            this.menuItem7.Index = 4;
            this.menuItem7.Text = "-";
            // 
            // menuOnlineStats
            // 
            this.menuOnlineStats.Index = 5;
            this.menuOnlineStats.Text = "Battle Status...";
            this.menuOnlineStats.Click += new System.EventHandler(this.menuOnlineStats_Click);
            // 
            // menuGameStats
            // 
            this.menuGameStats.Index = 6;
            this.menuGameStats.Text = "Game Status...";
            this.menuGameStats.Click += new System.EventHandler(this.menuGameStats_Click);
            // 
            // menuItem8
            // 
            this.menuItem8.Index = 7;
            this.menuItem8.Text = "-";
            // 
            // menuExit
            // 
            this.menuExit.Index = 8;
            this.menuExit.Text = "Exit";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuUnit
            // 
            this.menuUnit.Index = 1;
            this.menuUnit.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuMove,
            this.menuFire,
            this.menuBoth,
            this.menuItem1,
            this.menuSquare,
            this.menuColumn,
            this.menuLine,
            this.menuItem11,
            this.menuUnitStats});
            this.menuUnit.Text = "Unit";
            // 
            // menuMove
            // 
            this.menuMove.Checked = true;
            this.menuMove.Index = 0;
            this.menuMove.Text = "Move";
            this.menuMove.Click += new System.EventHandler(this.menuMove_Click);
            // 
            // menuFire
            // 
            this.menuFire.Checked = true;
            this.menuFire.Index = 1;
            this.menuFire.Text = "Fire";
            this.menuFire.Click += new System.EventHandler(this.menuFire_Click);
            // 
            // menuBoth
            // 
            this.menuBoth.Checked = true;
            this.menuBoth.Index = 2;
            this.menuBoth.Text = "Both";
            this.menuBoth.Click += new System.EventHandler(this.menuBoth_Click);
            // 
            // menuItem1
            // 
            this.menuItem1.Index = 3;
            this.menuItem1.Text = "-";
            // 
            // menuSquare
            // 
            this.menuSquare.Checked = true;
            this.menuSquare.Index = 4;
            this.menuSquare.Text = "Square Stance";
            this.menuSquare.Click += new System.EventHandler(this.menuSquare_Click);
            // 
            // menuColumn
            // 
            this.menuColumn.Checked = true;
            this.menuColumn.Index = 5;
            this.menuColumn.Text = "Column Stance";
            this.menuColumn.Click += new System.EventHandler(this.menuColumn_Click);
            // 
            // menuLine
            // 
            this.menuLine.Checked = true;
            this.menuLine.Index = 6;
            this.menuLine.ShowShortcut = false;
            this.menuLine.Text = "Line Stance";
            this.menuLine.Click += new System.EventHandler(this.menuLine_Click);
            // 
            // menuItem11
            // 
            this.menuItem11.Index = 7;
            this.menuItem11.Text = "-";
            // 
            // menuUnitStats
            // 
            this.menuUnitStats.Index = 8;
            this.menuUnitStats.Text = "Show Unit Details";
            this.menuUnitStats.Click += new System.EventHandler(this.menuUnitStats_Click);
            // 
            // menuView
            // 
            this.menuView.Index = 2;
            this.menuView.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuUnitView,
            this.menuGrid,
            this.menuMoves,
            this.menuBackground,
            this.menuItem9,
            this.menuSoundEffects,
            this.menuMusic,
            this.menuItem10,
            this.menuSaveOptions});
            this.menuView.Text = "Options";
            // 
            // menuUnitView
            // 
            this.menuUnitView.Checked = true;
            this.menuUnitView.Index = 0;
            this.menuUnitView.Text = "View Units";
            this.menuUnitView.Click += new System.EventHandler(this.menuUnitView_Click);
            // 
            // menuGrid
            // 
            this.menuGrid.Checked = true;
            this.menuGrid.Index = 1;
            this.menuGrid.Text = "View Grid";
            this.menuGrid.Click += new System.EventHandler(this.menuGrid_Click);
            // 
            // menuMoves
            // 
            this.menuMoves.Checked = true;
            this.menuMoves.Index = 2;
            this.menuMoves.Text = "View Moves";
            this.menuMoves.Click += new System.EventHandler(this.menuMoves_Click);
            // 
            // menuBackground
            // 
            this.menuBackground.Checked = true;
            this.menuBackground.Index = 3;
            this.menuBackground.Text = "View Background";
            this.menuBackground.Click += new System.EventHandler(this.menuBackground_Click);
            // 
            // menuItem9
            // 
            this.menuItem9.Index = 4;
            this.menuItem9.Text = "-";
            // 
            // menuSoundEffects
            // 
            this.menuSoundEffects.Checked = true;
            this.menuSoundEffects.Index = 5;
            this.menuSoundEffects.Text = "Sound Effects";
            this.menuSoundEffects.Click += new System.EventHandler(this.menuSoundEffects_Click);
            // 
            // menuMusic
            // 
            this.menuMusic.Checked = true;
            this.menuMusic.Index = 6;
            this.menuMusic.Text = "Music";
            this.menuMusic.Click += new System.EventHandler(this.menuMusic_Click);
            // 
            // menuItem10
            // 
            this.menuItem10.Index = 7;
            this.menuItem10.Text = "-";
            // 
            // menuSaveOptions
            // 
            this.menuSaveOptions.Index = 8;
            this.menuSaveOptions.Text = "Save Options";
            this.menuSaveOptions.Click += new System.EventHandler(this.menuSaveOptions_Click);
            // 
            // menuHelp
            // 
            this.menuHelp.Index = 3;
            this.menuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuAbout});
            this.menuHelp.Text = "Help";
            // 
            // menuAbout
            // 
            this.menuAbout.Index = 0;
            this.menuAbout.Text = "About English Squares...";
            this.menuAbout.Click += new System.EventHandler(this.menuAbout_Click);
            // 
            // contextMenu1
            // 
            this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuContextMove,
            this.menuContextFire,
            this.menuContextBoth,
            this.menuItem14,
            this.menuContextSquare,
            this.menuContextColumn,
            this.menuContextLine,
            this.menuItem3,
            this.menuContextStatistics});
            // 
            // menuContextMove
            // 
            this.menuContextMove.Checked = true;
            this.menuContextMove.Index = 0;
            this.menuContextMove.Text = "Move";
            this.menuContextMove.Click += new System.EventHandler(this.menuContextMove_Click);
            // 
            // menuContextFire
            // 
            this.menuContextFire.Checked = true;
            this.menuContextFire.Index = 1;
            this.menuContextFire.Text = "Fire";
            this.menuContextFire.Click += new System.EventHandler(this.menuContextFire_Click);
            // 
            // menuContextBoth
            // 
            this.menuContextBoth.Checked = true;
            this.menuContextBoth.Index = 2;
            this.menuContextBoth.Text = "Both";
            this.menuContextBoth.Click += new System.EventHandler(this.menuContextBoth_Click);
            // 
            // menuItem14
            // 
            this.menuItem14.Index = 3;
            this.menuItem14.Text = "-";
            // 
            // menuContextSquare
            // 
            this.menuContextSquare.Checked = true;
            this.menuContextSquare.Index = 4;
            this.menuContextSquare.Text = "Square Stance";
            this.menuContextSquare.Click += new System.EventHandler(this.menuContextSquare_Click);
            // 
            // menuContextColumn
            // 
            this.menuContextColumn.Checked = true;
            this.menuContextColumn.Index = 5;
            this.menuContextColumn.Text = "Column Stance";
            this.menuContextColumn.Click += new System.EventHandler(this.menuContextColumn_Click);
            // 
            // menuContextLine
            // 
            this.menuContextLine.Checked = true;
            this.menuContextLine.Index = 6;
            this.menuContextLine.Text = "Line Stance";
            this.menuContextLine.Click += new System.EventHandler(this.menuContextLine_Click);
            // 
            // menuItem3
            // 
            this.menuItem3.Index = 7;
            this.menuItem3.Text = "-";
            // 
            // menuContextStatistics
            // 
            this.menuContextStatistics.Index = 8;
            this.menuContextStatistics.Text = "Show Unit Details";
            this.menuContextStatistics.Click += new System.EventHandler(this.menuContextStatistics_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "*.esq";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "*.esq";
            this.saveFileDialog1.FileName = "doc1";
            // 
            // timeGameplay
            // 
            this.timeGameplay.Enabled = true;
            this.timeGameplay.Interval = 60000D;
            this.timeGameplay.SynchronizingObject = this;
            this.timeGameplay.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimeGame);
            // 
            // timeThinking
            // 
            this.timeThinking.Interval = 1000D;
            this.timeThinking.SynchronizingObject = this;
            this.timeThinking.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimeThinking);
            // 
            // EnglishSquares
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(779, 441);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Menu = this.mainMenu1;
            this.Name = "EnglishSquares";
            this.Padding = new System.Windows.Forms.Padding(4);
            this.Text = "English Squares";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.exitEvent);
            this.Load += new System.EventHandler(this.EnglishSquares_Load);
            ((System.ComponentModel.ISupportInitialize)(this.timeGameplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeThinking)).EndInit();
            this.ResumeLayout(false);

		}
		//*********************************************************************
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new EnglishSquares());
		}

		#region Menu Handler Functions
		//*********************************************************************

		private void menuAbout_Click(object sender, System.EventArgs e)
		{
			AboutEnglishSquares aboutForm = new AboutEnglishSquares();
			aboutForm.checkShareware(_sharewareVersion);
			aboutForm.ShowDialog();
		}

		private void showSharewareMessage()
		{
			SoundFX.playPleaseRegister();
			MessageBox.Show("The feature you selected is not available in the free version of English Squares. If you would like to purchase the full version, please go to our web site of http://www.fullerdata.com/englishsquares. Thank you for your support!", "Full Version Only", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private bool checkSharewareMap(string mapFilename)
		{
			string filenameOnly = System.IO.Path.GetFileName(mapFilename);
			filenameOnly = filenameOnly.ToLower();
			bool sharewareFile = false;

			switch (filenameOnly)
			{
				case ("english0a.xml"):
				case ("english0b.xml"):
				case ("english0c.xml"):
				case ("english0d.xml"):
				case ("english0e.xml"):
				case ("english1.xml"):
				case ("austria1.xml"):
				case ("french1.xml"):
				case ("prussia1.xml"):
				case ("russia1.xml"):
					sharewareFile = true;
					break;
				default:
					sharewareFile = false;
					break;
			}

			return(sharewareFile);
		}

		private void menuNew_Click(object sender, System.EventArgs e)
		{
			if (_showTitle == false && gameOver == false)
			{
				DialogResult ok = MessageBox.Show("Are you sure you want to abandon your current game?", "Close Current Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

				if (ok == DialogResult.Yes)
				{
					_showTitle = true;
					enableGameMenus(false);

					stopThinking();

					_map.lastU = -1;
					if (battleStatus != null && battleStatus.IsDisposed == false)
					{
						showLastBattle();
					}

					this.AutoScroll=false;
					this.Invalidate();	

					if (this.playMusic == true)
					{
						SoundFX.stopMusic();
						SoundFX.playMusic();
						titleMix++;
					}
				}
				else
				{
					return;
				}
			}

			if (gameStatus != null && gameStatus.IsDisposed == false)
				gameStatus.Dispose();							

			if (unitStatus != null && unitStatus.IsDisposed == false)
				unitStatus.Dispose();							

			if (battleStatus != null && battleStatus.IsDisposed == false)
				battleStatus.Dispose();							

			NewGame newgameForm = new NewGame();
			newgameForm.checkShareware(_sharewareVersion);
			newgameForm.ShowDialog();

			if (newgameForm.selectedMap == true)
			{
				SoundFX.stopMusic();

				if (_sharewareVersion == true)
				{
					if (checkSharewareMap(newgameForm.mapFilename) == false)
					{
						showSharewareMessage();
						return;
					}
				}

				if (_map.loadMap(newgameForm.mapFilename, false) == true)
				{				
					_showTitle = false;
					gameOver = false;
					enableGameMenus(true);
					this.AutoScrollMinSize = new Size(_map.mapPixWidth, _map.mapPixHeight);
					this.AutoScroll=true;
					this.Invalidate();
					this.numberTurns = 0;
					this.elapsedMinutes = 0;
					_map.lastU = -1;

					//showGameStatus();
					//showLastBattle();

					if (_sharewareVersion == false)
					{
						loadOptions();
					}


                    if (InvokeRequired)
                    {
                        MethodInvoker method = new MethodInvoker(showGameStatus);
                        Invoke(method);
                    }
                    else
                    {
                        showGameStatus();
                    }
				}
			}
		}

		private void menuOpen_Click(object sender, System.EventArgs e)
		{
			if (_sharewareVersion == false)
			{
				openFileDialog1.DefaultExt = "xml";
				openFileDialog1.Filter = "Save files (*.xml)|*.xml|All files (*.*)|*.*";
				openFileDialog1.Title = "Open Saved Game";
				openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				DialogResult dr = openFileDialog1.ShowDialog();
				if (dr != DialogResult.Cancel)
				{
					SoundFX.stopMusic();

					bool openOK = _map.openSavedGame(openFileDialog1.FileName);
					if (openOK == false)
					{
						MessageBox.Show(openFileDialog1.FileName + " is not a valid English Squares game save.", "Load Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					else
					{
						if (gameStatus != null && gameStatus.IsDisposed == false)
							gameStatus.Dispose();							

						if (unitStatus != null && unitStatus.IsDisposed == false)
							unitStatus.Dispose();							

						if (battleStatus != null && battleStatus.IsDisposed == false)
							battleStatus.Dispose();							

						_showTitle = false;
						gameOver = false;
						enableGameMenus(true);
						this.AutoScrollMinSize = new Size(_map.mapPixWidth, _map.mapPixHeight);
						this.AutoScroll=true;
						this.Invalidate();
						this.numberTurns = 0;
						this.elapsedMinutes = 0;
						_map.lastU = -1;

						this.elapsedMinutes = _map.loadedElapsedMinutes;
						this.numberTurns = _map.loadedNumberTurns;

						loadOptions();
					}
				}
			}
			else
			{
				showSharewareMessage();
			}
		}

		private void menuSave_Click(object sender, System.EventArgs e)
		{
			menuSaveAs_Click(sender, e);
		}

		private void menuSaveAs_Click(object sender, System.EventArgs e)
		{
			if (_sharewareVersion == false)
			{
				saveFileDialog1.CheckFileExists = false;
				saveFileDialog1.DefaultExt = "xml";
				saveFileDialog1.Filter = "Save files (*.xml)|*.xml|All files (*.*)|*.*";
				saveFileDialog1.Title = "Save Game";
				saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				DialogResult dr = saveFileDialog1.ShowDialog();				
				if (dr != DialogResult.Cancel)
				{			
					_map.loadedElapsedMinutes = this.elapsedMinutes;
					_map.loadedNumberTurns = this.numberTurns;

					bool openOK = _map.saveGame(saveFileDialog1.FileName);
					if (openOK == false)
					{
						MessageBox.Show("Failed to save game to file: " + saveFileDialog1.FileName, "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
			else
			{
				showSharewareMessage();
			}
		}

		private void exitEvent(object sender, CancelEventArgs e)
		{
			DialogResult ok = MessageBox.Show("Are you sure you want to close English Squares?", "Exit English Squares", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (ok != DialogResult.Yes)
			{
				e.Cancel = true;
			}
		}

		private void menuExit_Click(object sender, System.EventArgs e)
		{
			//DialogResult ok = MessageBox.Show("Are you sure you want to close English Squares?", "Exit English Squares", MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
			//if (ok == DialogResult.Yes)
			//{
			this.Close();
			//}
		}

		private void menuOnlineHelp_Click(object sender, System.EventArgs e)
		{
			Process ieProcess = new Process();
			ieProcess.StartInfo.FileName = "iexplore.exe";
			ieProcess.StartInfo.Arguments = "http://www.fullerdata.com/englishsquares/default.aspx?Action=Help";
			ieProcess.Start();
		
		}

		private void menuRegister_Click(object sender, System.EventArgs e)
		{
			Process ieProcess = new Process();
			ieProcess.StartInfo.FileName = "iexplore.exe";
			ieProcess.StartInfo.Arguments = "http://www.fullerdata.com/englishsquares/default.aspx?Action=Register";
			ieProcess.Start();
		
		}

		private void menuClose_Click(object sender, System.EventArgs e)
		{
			DialogResult ok = MessageBox.Show("Are you sure you want to abandon your current game?", "Close Current Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (ok == DialogResult.Yes)
			{
				_showTitle = true;
				enableGameMenus(false);
				this.AutoScroll=false;
				this.Invalidate();				

				if (gameStatus != null && gameStatus.IsDisposed == false)
					gameStatus.Dispose();							

				if (unitStatus != null && unitStatus.IsDisposed == false)
					unitStatus.Dispose();							

				if (battleStatus != null && battleStatus.IsDisposed == false)
					battleStatus.Dispose();							
			}		
		}

		private void menuGrid_Click(object sender, System.EventArgs e)
		{
			if (menuGrid.Checked == false)
			{
				menuGrid.Checked = true;
				_map.showGrid = true;
				_map.createMapBitmap();
				this.Invalidate();
			}
			else
			{
				menuGrid.Checked = false;
				_map.showGrid = false;
				_map.createMapBitmap();
				this.Invalidate();
			}
		}

		private void menuUnitView_Click(object sender, System.EventArgs e)
		{
			if (menuUnitView.Checked == false)
			{
				menuUnitView.Checked = true;
				_map.showUnits = true;
				_map.createMapBitmap();
				this.Invalidate();
			}
			else
			{
				menuUnitView.Checked = false;
				_map.showUnits = false;
				_map.createMapBitmap();
				this.Invalidate();
			}	
		}

		private void menuMoves_Click(object sender, System.EventArgs e)
		{
			if (menuMoves.Checked == false)
			{
				menuMoves.Checked = true;
				_map.showAvailableMoves = true;
				_map.createMapBitmap();
				this.Invalidate();
			}
			else
			{
				menuMoves.Checked = false;
				_map.showAvailableMoves = false;
				_map.createMapBitmap();
				this.Invalidate();
			}			
		}

		private void menuBackground_Click(object sender, System.EventArgs e)
		{
			if (menuBackground.Checked == false)
			{
				menuBackground.Checked = true;
				_map.showBackground = true;
				_map.createMapBitmap();
				this.Invalidate();
			}
			else
			{
				menuBackground.Checked = false;
				_map.showBackground = false;
				_map.createMapBitmap();
				this.Invalidate();
			}		
		}

		public void showGameStatus()
		{
			if (gameStatus == null || gameStatus.IsDisposed == true)	
				gameStatus = new GameStatus();
			
			gameStatus.Show();
			gameStatus.showStatus(elapsedMinutes);
			gameStatus.updateFields(_map.player1Name, _map.player2Name, _map.player1Strength, _map.player2Strength, _map.player1Units, _map.player2Units, _map.player1Battles, _map.player2Battles, _map.player1DeadUnits, _map.player2DeadUnits, numberTurns.ToString(), thinkTime, _map.movesConsidered, _map.lastWeightBefore1, _map.lastWeightBefore2, _map.computerStance);
			gameStatus.Focus();
		}

		private void menuGameStats_Click(object sender, System.EventArgs e)
		{
			showGameStatus();
		}

		public void showLastBattle()
		{
            // do we need to switch threads?
            if (InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(showLastBattle);
                Invoke(method);
            }
            else
            {
                if (battleStatus == null || battleStatus.IsDisposed == true)
                    battleStatus = new BattleStatus();

                battleStatus.Show();
                battleStatus.updateFields(_map.lastBattleMove, _map.lastBattleResult1, _map.lastBattleResult2, _map.lastBattleUnit1Description, _map.lastBattleUnit2Description,
                                            _map.lastBattleUnit1Strength, _map.lastBattleUnit1Morale, _map.lastBattleUnit1Support, _map.lastBattleUnit1Training, _map.lastBattleUnit1Damage,
                                            _map.lastBattleUnit2Strength, _map.lastBattleUnit2Morale, _map.lastBattleUnit2Support, _map.lastBattleUnit2Training, _map.lastBattleUnit2Damage,
                                            _map.lastBattleUnit1Image, _map.lastBattleUnit2Image, _map.lastCalc1, _map.lastCalc2);
                battleStatus.Focus();
            }
		}

		private void menuOnlineStats_Click(object sender, System.EventArgs e)
		{
			showLastBattle();
		}

		private void showUnitStats()
		{
			if (unitStatus == null || unitStatus.IsDisposed== true)	
				unitStatus = new UnitStatus();
		
			unitStatus.Show();		
			showUnitStatus(_map.unitX, _map.unitY);
			unitStatus.Focus();
		}

		private void menuUnitStats_Click(object sender, System.EventArgs e)
		{
			showUnitStats();
		}

		private void checkUnitSquare(bool checkState)
		{
			menuSquare.Checked = checkState;
			menuContextSquare.Checked = checkState;
		}

		private void checkUnitLine(bool checkState)
		{
			menuLine.Checked = checkState;		
			menuContextLine.Checked = checkState;		
		}

		private void checkUnitColumn(bool checkState)
		{
			menuColumn.Checked = checkState;
			menuContextColumn.Checked = checkState;
		}

		private void checkUnitStance()
		{
			checkUnitSquare(false);
			checkUnitLine(false);
			checkUnitColumn(false);

			if (_map.unitStance == stanceEnum.Square)	checkUnitSquare(true);
			if (_map.unitStance == stanceEnum.Column)	checkUnitColumn(true);
			if (_map.unitStance == stanceEnum.Line)		checkUnitLine(true);
		}			

		private void menuSquare_Click(object sender, System.EventArgs e)
		{
			checkUnitSquare(true);
			checkUnitLine(false);
			checkUnitColumn(false);

			_map.unitToSquare();
			_map.createMapBitmap();
			this.Invalidate();

			if (_map.madeMove == true)	madeTurn();
		}

		private void menuColumn_Click(object sender, System.EventArgs e)
		{
			checkUnitSquare(false);
			checkUnitLine(false);
			checkUnitColumn(true);

			_map.unitToColumn();
			_map.createMapBitmap();
			this.Invalidate();

			if (_map.madeMove == true)	madeTurn();
		}

		private void menuLine_Click(object sender, System.EventArgs e)
		{
			checkUnitSquare(false);
			checkUnitLine(true);
			checkUnitColumn(false);

			_map.unitToLine();
			_map.createMapBitmap();
			this.Invalidate();

			if (_map.madeMove == true)	madeTurn();
		}

		private void enableUnitMenus(bool enabledState)
		{
			menuMove.Enabled = enabledState;
			menuFire.Enabled = enabledState;
			menuSquare.Enabled = enabledState;
			menuColumn.Enabled = enabledState;
			menuLine.Enabled = enabledState;
			menuBoth.Enabled = enabledState;
			menuUnitStats.Enabled = enabledState;
		}

		private void enableGameMenus(bool enabledState)
		{
			enableUnitMenus(enabledState);

			menuUnitView.Enabled = enabledState;
			menuGrid.Enabled = enabledState;
			menuMoves.Enabled = enabledState;
			menuBackground.Enabled = enabledState;
			menuSoundEffects.Enabled = enabledState;
			menuMusic.Enabled = enabledState;
			menuSaveOptions.Enabled = enabledState;
			menuGameStats.Enabled = enabledState;
			menuOnlineStats.Enabled = enabledState;
			menuSave.Enabled = enabledState;
			//menuSaveAs.Enabled = enabledState;
			menuClose.Enabled = enabledState;
		}

		private void setAllMove(bool checkVal)
		{
			menuMove.Checked = checkVal;
			menuContextMove.Checked = checkVal;
			menuFire.Checked = checkVal;
			menuContextFire.Checked = checkVal;
			menuBoth.Checked = checkVal;
			menuContextBoth.Checked = checkVal;
		}

		private void menuMove_Click(object sender, System.EventArgs e)
		{
			_map.commandType = unitCommands.MoveOnly;

			setAllMove(false);
			menuMove.Checked = true;
			menuContextMove.Checked = true;

			_map.createMapBitmap();
			this.Invalidate();
		}

		private void menuFire_Click(object sender, System.EventArgs e)
		{
			_map.commandType = unitCommands.FireOnly;

			setAllMove(false);
			menuFire.Checked = true;
			menuContextFire.Checked = true;

			_map.createMapBitmap();
			this.Invalidate();
		}

		private void menuBoth_Click(object sender, System.EventArgs e)
		{
			_map.commandType = unitCommands.MoveAndFire;

			setAllMove(false);
			menuBoth.Checked = true;
			menuContextBoth.Checked = true;

			_map.createMapBitmap();
			this.Invalidate();
		}

		private void menuContextStatistics_Click(object sender, System.EventArgs e)
		{
			menuUnitStats_Click(sender, e);
		}

		private void menuContextMove_Click(object sender, System.EventArgs e)
		{
			menuMove_Click(sender, e);		
		}

		private void menuContextFire_Click(object sender, System.EventArgs e)
		{
			menuFire_Click(sender, e);		
		}

		private void menuContextBoth_Click(object sender, System.EventArgs e)
		{
			menuBoth_Click(sender, e);
		}

		private void menuContextSquare_Click(object sender, System.EventArgs e)
		{
			menuSquare_Click(sender, e);		
		}

		private void menuContextColumn_Click(object sender, System.EventArgs e)
		{
			menuColumn_Click(sender, e);	
		}

		private void menuContextLine_Click(object sender, System.EventArgs e)
		{
			menuLine_Click(sender, e);		
		}

		private void menuSaveOptions_Click(object sender, System.EventArgs e)
		{
			if (_sharewareVersion == false)
			{
				saveOptions();
			}
			else
			{
				showSharewareMessage();
			}	
		}

		private void menuSoundEffects_Click(object sender, System.EventArgs e)
		{
			playFX = (playFX == true)? false : true;

			menuSoundEffects.Checked = this.playFX;

			//if (playFX == false)
			//{
			//	if (threadSoundFX != null)
			//	{
			//		threadSoundFX.Abort();
			//		threadSoundFX = null;
			//	}
			//}
		}

		private void menuMusic_Click(object sender, System.EventArgs e)
		{
			playMusic = (playMusic == true)? false : true;
		
			menuMusic.Checked = this.playMusic;

			if (playMusic == false)
			{
				SoundFX.stopMusic();
			}
			else
			{
				SoundFX.stopMusic();
				SoundFX.playMusic();
			}
		}

		//*********************************************************************
		#endregion

		#region GameOptions

		private void saveOptions()
		{
			try
			{
				FileStream fWrite = new FileStream(AppDomain.CurrentDomain.BaseDirectory + Constants.OPTIONS_FILENAME, FileMode.Create);
				XmlTextWriter xmlSave = new XmlTextWriter(fWrite, System.Text.Encoding.ASCII);

				xmlSave.WriteStartDocument();
				xmlSave.WriteStartElement("Options");

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("playMusic", playMusic==true?"Y":"N");
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("playFX", playFX==true?"Y":"N");
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("showGrid", _map.showGrid==true?"Y":"N");
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("showUnits", _map.showUnits==true?"Y":"N");
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("showAvailableMoves", _map.showAvailableMoves==true?"Y":"N");
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("showBackground", _map.showBackground==true?"Y":"N");
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("gameStatus", (gameStatus==null||gameStatus.IsDisposed==true)?"N":"Y");
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("battleStatus", (battleStatus==null||battleStatus.IsDisposed==true)?"N":"Y");
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("unitStatus", (unitStatus==null||unitStatus.IsDisposed==true)?"N":"Y");
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("Width", this.Size.Width.ToString());
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("Height", this.Size.Height.ToString());
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("LocationX", this.Location.X.ToString());
				xmlSave.WriteEndElement();

				xmlSave.WriteStartElement("Option");
				xmlSave.WriteAttributeString("LocationY", this.Location.Y.ToString());
				xmlSave.WriteEndElement();

				if (!(gameStatus==null || gameStatus.IsDisposed==true))
				{
					xmlSave.WriteStartElement("Option");
					xmlSave.WriteAttributeString("GameStatus_LocationX", gameStatus.Location.X.ToString());
					xmlSave.WriteEndElement();
					xmlSave.WriteStartElement("Option");
					xmlSave.WriteAttributeString("GameStatus_LocationY", gameStatus.Location.Y.ToString());
					xmlSave.WriteEndElement();
				}
				
				if (!(battleStatus==null || battleStatus.IsDisposed==true))
				{
					xmlSave.WriteStartElement("Option");
					xmlSave.WriteAttributeString("BattleStatus_LocationX",  battleStatus.Location.X.ToString());
					xmlSave.WriteEndElement();
					xmlSave.WriteStartElement("Option");
					xmlSave.WriteAttributeString("BattleStatus_LocationY",  battleStatus.Location.Y.ToString());
					xmlSave.WriteEndElement();
				}

				if (!(unitStatus==null || unitStatus.IsDisposed==true))
				{
					xmlSave.WriteStartElement("Option");
					xmlSave.WriteAttributeString("UnitStatus_LocationX", unitStatus.Location.X.ToString());
					xmlSave.WriteEndElement();
					xmlSave.WriteStartElement("Option");
					xmlSave.WriteAttributeString("UnitStatus_LocationY", unitStatus.Location.Y.ToString());
					xmlSave.WriteEndElement();
				}
				
				xmlSave.WriteEndElement();
				xmlSave.Close();
				fWrite.Close();

				MessageBox.Show("Options saved. They will be read whenever a game is started or a saved game opened.", "Options Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception e)
			{
				MessageBox.Show("Error when saving options - " + e.Message, "Failed Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	
		private void loadOptions()
		{
			int _locX = -1;
			int _locY = -1;
			int _locGameX = -1;
			int _locGameY = -1;
			int _locBattleX = -1;
			int _locBattleY = -1;
			int _locUnitX = -1;
			int _locUnitY = -1;

			try
			{
				if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + Constants.OPTIONS_FILENAME) == false)
					return;

				FileStream fRead = new FileStream(AppDomain.CurrentDomain.BaseDirectory + Constants.OPTIONS_FILENAME, FileMode.Open);
				XmlTextReader xmlReader = new XmlTextReader(fRead);
				while (xmlReader.Read())
				{
					if (xmlReader.NodeType == XmlNodeType.Element)
					{
						if (xmlReader.Name == "Option")
						{
							string opt_playMusic				= xmlReader.GetAttribute("playMusic");
							string opt_playFX					= xmlReader.GetAttribute("playFX");
							string opt_showGrid					= xmlReader.GetAttribute("showGrid");
							string opt_showUnits				= xmlReader.GetAttribute("showUnits");
							string opt_showAvailableMoves		= xmlReader.GetAttribute("showAvailableMoves");
							string opt_showBackground			= xmlReader.GetAttribute("showBackground");
							string opt_gameStatus				= xmlReader.GetAttribute("gameStatus");
							string opt_battleStatus				= xmlReader.GetAttribute("battleStatus");
							string opt_unitStatus				= xmlReader.GetAttribute("unitStatus");
							string opt_Width					= xmlReader.GetAttribute("Width");
							string opt_Height					= xmlReader.GetAttribute("Height");
							string opt_LocationX				= xmlReader.GetAttribute("LocationX");
							string opt_LocationY				= xmlReader.GetAttribute("LocationY");
							string opt_GameStatus_LocationX		= xmlReader.GetAttribute("GameStatus_LocationX");
							string opt_GameStatus_LocationY		= xmlReader.GetAttribute("GameStatus_LocationY");
							string opt_BattleStatus_LocationX	= xmlReader.GetAttribute("BattleStatus_LocationX");
							string opt_BattleStatus_LocationY	= xmlReader.GetAttribute("BattleStatus_LocationY");
							string opt_UnitStatus_LocationX		= xmlReader.GetAttribute("UnitStatus_LocationX");
							string opt_UnitStatus_LocationY		= xmlReader.GetAttribute("UnitStatus_LocationY");

							if (opt_playMusic != null && opt_playMusic.Length > 0)						this.playMusic = opt_playMusic=="Y"?true:false;
							if (opt_playFX != null && opt_playFX.Length > 0)							this.playFX = opt_playFX=="Y"?true:false;
							if (opt_showGrid != null && opt_showGrid.Length > 0)						_map.showGrid = opt_showGrid=="Y"?true:false;
							if (opt_showUnits != null && opt_showUnits.Length > 0)						_map.showUnits = opt_showUnits=="Y"?true:false;
							if (opt_showAvailableMoves != null && opt_showAvailableMoves.Length > 0)	_map.showAvailableMoves = opt_showAvailableMoves=="Y"?true:false;
							if (opt_showBackground != null && opt_showBackground.Length > 0)			_map.showBackground = opt_showBackground=="Y"?true:false;

							if (opt_Width != null && opt_Width.Length > 0)			this.Width = Convert.ToInt32(opt_Width);
							if (opt_Height != null && opt_Height.Length > 0)		this.Height = Convert.ToInt32(opt_Height);

							if (opt_LocationX != null && opt_LocationX.Length > 0)	_locX = Convert.ToInt32(opt_LocationX); 
							if (opt_LocationY != null && opt_LocationY.Length > 0)	_locY = Convert.ToInt32(opt_LocationY);
							if (_locX > -1 && _locY > -1) { this.SetDesktopLocation(_locX, _locY); _locX=-1; _locY=-1;}

							if (opt_gameStatus != null && opt_gameStatus.Length > 0 && opt_gameStatus == "Y")		showGameStatus();
							if (opt_battleStatus != null && opt_battleStatus.Length > 0 && opt_battleStatus == "Y")	showLastBattle();
							if (opt_unitStatus != null && opt_unitStatus.Length > 0 && opt_unitStatus == "Y")		showUnitStats();

							if (opt_GameStatus_LocationX != null && opt_GameStatus_LocationX.Length > 0)	_locGameX = Convert.ToInt32(opt_GameStatus_LocationX);
							if (opt_GameStatus_LocationY != null && opt_GameStatus_LocationY.Length > 0)	_locGameY = Convert.ToInt32(opt_GameStatus_LocationY);
							if (_locGameX > -1 && _locGameY > -1) { gameStatus.SetDesktopLocation(_locGameX, _locGameY); _locGameX=-1; _locGameY=-1;}

							if (opt_BattleStatus_LocationX != null && opt_BattleStatus_LocationX.Length > 0)	_locBattleX = Convert.ToInt32(opt_BattleStatus_LocationX);
							if (opt_BattleStatus_LocationY != null && opt_BattleStatus_LocationY.Length > 0)	_locBattleY = Convert.ToInt32(opt_BattleStatus_LocationY);
							if (_locBattleX > -1 && _locBattleY > -1) { battleStatus.SetDesktopLocation(_locBattleX, _locBattleY); _locBattleX=-1; _locBattleY=-1;}

							if (opt_UnitStatus_LocationX != null && opt_UnitStatus_LocationX.Length > 0)		_locUnitX = Convert.ToInt32(opt_UnitStatus_LocationX);
							if (opt_UnitStatus_LocationY != null && opt_UnitStatus_LocationY.Length > 0)		_locUnitY = Convert.ToInt32(opt_UnitStatus_LocationY);
							if (_locUnitX > -1 && _locUnitY > -1) { unitStatus.SetDesktopLocation(_locUnitX, _locUnitY); _locUnitX=-1; _locUnitY=-1;}
						}
					}
				}
				xmlReader.Close();
				fRead.Close();
			}
			catch (Exception e)
			{
				MessageBox.Show("Error when loading options - " + e.Message, "Failed Load", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		#endregion

	}
}
