using System;
using System.Drawing;

namespace EnglishSquares
{
	/// <summary>
	/// Summary description for GameStatus.
	/// </summary>
	public class GameStatus : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox flagPlayer2;
		private System.Windows.Forms.Label labelPlayer2;
		private System.Windows.Forms.Label labelPlayer1;
		private System.Windows.Forms.PictureBox flagPlayer1;
		private System.Windows.Forms.Label labelUnits2;
		private System.Windows.Forms.Label labelLost2;
		private System.Windows.Forms.Label labelWon2;
		private System.Windows.Forms.Label labelUnits1;
		private System.Windows.Forms.Label labelLost1;
		private System.Windows.Forms.Label labelWon1;
		private System.Windows.Forms.Label labelStrength1;
		private System.Windows.Forms.Label labelStrength2;
		private System.Windows.Forms.Label labelGTText;
		private System.Windows.Forms.Label labelGameTime;
		private System.Windows.Forms.Label labelMovesMadeText;
		private System.Windows.Forms.Label labelMovesMade;
		private System.Windows.Forms.Label labelLastComputerMove;
		private System.Windows.Forms.Label labelStance;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GameStatus()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GameStatus));
			this.flagPlayer2 = new System.Windows.Forms.PictureBox();
			this.flagPlayer1 = new System.Windows.Forms.PictureBox();
			this.labelPlayer2 = new System.Windows.Forms.Label();
			this.labelPlayer1 = new System.Windows.Forms.Label();
			this.labelUnits2 = new System.Windows.Forms.Label();
			this.labelLost2 = new System.Windows.Forms.Label();
			this.labelWon2 = new System.Windows.Forms.Label();
			this.labelUnits1 = new System.Windows.Forms.Label();
			this.labelLost1 = new System.Windows.Forms.Label();
			this.labelWon1 = new System.Windows.Forms.Label();
			this.labelStrength1 = new System.Windows.Forms.Label();
			this.labelStrength2 = new System.Windows.Forms.Label();
			this.labelGTText = new System.Windows.Forms.Label();
			this.labelGameTime = new System.Windows.Forms.Label();
			this.labelMovesMadeText = new System.Windows.Forms.Label();
			this.labelMovesMade = new System.Windows.Forms.Label();
			this.labelLastComputerMove = new System.Windows.Forms.Label();
			this.labelStance = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// flagPlayer2
			// 
			this.flagPlayer2.Location = new System.Drawing.Point(120, 8);
			this.flagPlayer2.Name = "flagPlayer2";
			this.flagPlayer2.Size = new System.Drawing.Size(48, 32);
			this.flagPlayer2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.flagPlayer2.TabIndex = 0;
			this.flagPlayer2.TabStop = false;
			// 
			// flagPlayer1
			// 
			this.flagPlayer1.Location = new System.Drawing.Point(8, 8);
			this.flagPlayer1.Name = "flagPlayer1";
			this.flagPlayer1.Size = new System.Drawing.Size(48, 32);
			this.flagPlayer1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.flagPlayer1.TabIndex = 1;
			this.flagPlayer1.TabStop = false;
			// 
			// labelPlayer2
			// 
			this.labelPlayer2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelPlayer2.Location = new System.Drawing.Point(120, 48);
			this.labelPlayer2.Name = "labelPlayer2";
			this.labelPlayer2.Size = new System.Drawing.Size(104, 16);
			this.labelPlayer2.TabIndex = 2;
			this.labelPlayer2.Text = "Player 2 Text";
			// 
			// labelPlayer1
			// 
			this.labelPlayer1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelPlayer1.Location = new System.Drawing.Point(8, 48);
			this.labelPlayer1.Name = "labelPlayer1";
			this.labelPlayer1.Size = new System.Drawing.Size(104, 16);
			this.labelPlayer1.TabIndex = 3;
			this.labelPlayer1.Text = "Player 1 Text";
			// 
			// labelUnits2
			// 
			this.labelUnits2.Location = new System.Drawing.Point(120, 88);
			this.labelUnits2.Name = "labelUnits2";
			this.labelUnits2.Size = new System.Drawing.Size(96, 16);
			this.labelUnits2.TabIndex = 5;
			this.labelUnits2.Text = "Units: 0";
			// 
			// labelLost2
			// 
			this.labelLost2.Location = new System.Drawing.Point(120, 104);
			this.labelLost2.Name = "labelLost2";
			this.labelLost2.Size = new System.Drawing.Size(96, 16);
			this.labelLost2.TabIndex = 6;
			this.labelLost2.Text = "Lost: 0";
			// 
			// labelWon2
			// 
			this.labelWon2.Location = new System.Drawing.Point(120, 120);
			this.labelWon2.Name = "labelWon2";
			this.labelWon2.Size = new System.Drawing.Size(96, 16);
			this.labelWon2.TabIndex = 7;
			this.labelWon2.Text = "Won: 0";
			// 
			// labelUnits1
			// 
			this.labelUnits1.Location = new System.Drawing.Point(8, 88);
			this.labelUnits1.Name = "labelUnits1";
			this.labelUnits1.Size = new System.Drawing.Size(96, 16);
			this.labelUnits1.TabIndex = 8;
			this.labelUnits1.Text = "Units: 0";
			// 
			// labelLost1
			// 
			this.labelLost1.Location = new System.Drawing.Point(8, 104);
			this.labelLost1.Name = "labelLost1";
			this.labelLost1.Size = new System.Drawing.Size(96, 16);
			this.labelLost1.TabIndex = 9;
			this.labelLost1.Text = "Lost: 0";
			// 
			// labelWon1
			// 
			this.labelWon1.Location = new System.Drawing.Point(8, 120);
			this.labelWon1.Name = "labelWon1";
			this.labelWon1.Size = new System.Drawing.Size(96, 16);
			this.labelWon1.TabIndex = 10;
			this.labelWon1.Text = "Won: 0";
			// 
			// labelStrength1
			// 
			this.labelStrength1.Location = new System.Drawing.Point(8, 64);
			this.labelStrength1.Name = "labelStrength1";
			this.labelStrength1.Size = new System.Drawing.Size(104, 16);
			this.labelStrength1.TabIndex = 11;
			this.labelStrength1.Text = "Total Strength: 0 / 0";
			// 
			// labelStrength2
			// 
			this.labelStrength2.Location = new System.Drawing.Point(120, 64);
			this.labelStrength2.Name = "labelStrength2";
			this.labelStrength2.Size = new System.Drawing.Size(104, 16);
			this.labelStrength2.TabIndex = 12;
			this.labelStrength2.Text = "Total Strength: 0 / 0";
			// 
			// labelGTText
			// 
			this.labelGTText.Location = new System.Drawing.Point(8, 160);
			this.labelGTText.Name = "labelGTText";
			this.labelGTText.Size = new System.Drawing.Size(112, 16);
			this.labelGTText.TabIndex = 13;
			this.labelGTText.Text = "Game Time:";
			// 
			// labelGameTime
			// 
			this.labelGameTime.Location = new System.Drawing.Point(120, 160);
			this.labelGameTime.Name = "labelGameTime";
			this.labelGameTime.Size = new System.Drawing.Size(96, 16);
			this.labelGameTime.TabIndex = 14;
			this.labelGameTime.Text = "0";
			// 
			// labelMovesMadeText
			// 
			this.labelMovesMadeText.Location = new System.Drawing.Point(8, 176);
			this.labelMovesMadeText.Name = "labelMovesMadeText";
			this.labelMovesMadeText.Size = new System.Drawing.Size(112, 16);
			this.labelMovesMadeText.TabIndex = 15;
			this.labelMovesMadeText.Text = "Moves Made:";
			// 
			// labelMovesMade
			// 
			this.labelMovesMade.Location = new System.Drawing.Point(120, 176);
			this.labelMovesMade.Name = "labelMovesMade";
			this.labelMovesMade.Size = new System.Drawing.Size(96, 16);
			this.labelMovesMade.TabIndex = 16;
			this.labelMovesMade.Text = "0";
			// 
			// labelLastComputerMove
			// 
			this.labelLastComputerMove.Location = new System.Drawing.Point(8, 208);
			this.labelLastComputerMove.Name = "labelLastComputerMove";
			this.labelLastComputerMove.Size = new System.Drawing.Size(208, 16);
			this.labelLastComputerMove.TabIndex = 17;
			this.labelLastComputerMove.Text = "Last Computer Move: 0 seconds";
			// 
			// labelStance
			// 
			this.labelStance.Location = new System.Drawing.Point(8, 224);
			this.labelStance.Name = "labelStance";
			this.labelStance.Size = new System.Drawing.Size(208, 16);
			this.labelStance.TabIndex = 18;
			// 
			// GameStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(226, 239);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.labelStance,
																		  this.labelLastComputerMove,
																		  this.labelMovesMade,
																		  this.labelMovesMadeText,
																		  this.labelGameTime,
																		  this.labelGTText,
																		  this.labelStrength2,
																		  this.labelStrength1,
																		  this.labelWon1,
																		  this.labelLost1,
																		  this.labelUnits1,
																		  this.labelWon2,
																		  this.labelLost2,
																		  this.labelUnits2,
																		  this.labelPlayer1,
																		  this.labelPlayer2,
																		  this.flagPlayer1,
																		  this.flagPlayer2});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GameStatus";
			this.Text = "Game Status";
			this.Load += new System.EventHandler(this.GameStatus_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void GameStatus_Load(object sender, System.EventArgs e)
		{
		}

		public void showStatus(int gameTime)
		{
			labelGameTime.Text = gameTime.ToString() + " minutes";
		}

		private Image setPlayerFlag(string playerName)
		{
			switch (playerName)
			{
				case ("British"):
				case ("Britain"):
				case ("English"):
				case ("England"):
				case ("Scottish"):
				case ("Scots"):
				case ("Scotland"):
				case ("Irish"):
				case ("Ireland"):
				case ("Welsh"):
				case ("Wales"):
					return(Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.BRITISH_FLAG));
				case ("French"):
				case ("France"):
					return(Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.FRENCH_FLAG));
				case ("Austrian"):
				case ("Austria"):
					return(Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.AUSTRIAN_FLAG));
				case ("Russian"):
				case ("Russia"):
					return(Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.RUSSIAN_FLAG));
				case ("Prussian"):
				case ("Germania"):
				case ("German"):
				case ("Prussia"):
					return(Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.PRUSSIAN_FLAG));
			}

			return(Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.EAGLE_IMAGE));
		}

		public void updateFields(string player1Name, string player2Name, string player1Strength, string player2Strength, 
								 string player1Units, string player2Units, string player1Battles, string player2Battles, 
								 string player1DeadUnits, string player2DeadUnits, string numberTurns, string thinkTime, int movesConsidered,
								 int lastWeight1, int lastWeight2, string compStance)
		{
			labelPlayer1.Text = player1Name;
			labelPlayer2.Text = player2Name;

			this.flagPlayer1.Image = setPlayerFlag(player1Name);
			this.flagPlayer2.Image = setPlayerFlag(player2Name);

			labelStrength1.Text = player1Strength + " / " + lastWeight1.ToString();
			labelStrength2.Text = player2Strength + " / " + lastWeight2.ToString();

			labelUnits1.Text = "Units: " + player1Units;
			labelUnits2.Text = "Units: " + player2Units;

			labelWon1.Text = "Battles Won: " + player1Battles;
			labelWon2.Text = "Battles Won: " + player2Battles;

			labelLost1.Text = "Lost Units: " + player1DeadUnits;
			labelLost2.Text = "Lost Units: " + player2DeadUnits;

			labelMovesMade.Text = numberTurns + " turns";

			labelLastComputerMove.Text = "Computer Move: " + thinkTime + " s / " + movesConsidered.ToString() + " moves";
			labelStance.Text = compStance;
		}
	}
}
