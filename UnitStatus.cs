using System;
using System.Drawing;

namespace EnglishSquares
{
	/// <summary>
	/// Summary description for UnitStatus.
	/// </summary>
	public class UnitStatus : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label labelUnitWon;
		private System.Windows.Forms.Label labelUnitStance;
		private System.Windows.Forms.Label labelUnitDescription;
		private System.Windows.Forms.Label labelUnitSupport;
		private System.Windows.Forms.Label labelUnitStrength;
		private System.Windows.Forms.Label labelUnitMorale;
		private System.Windows.Forms.PictureBox pictureUnit;
		private System.Windows.Forms.Label labelUnitType;
		private System.Windows.Forms.Label labelX;
		private System.Windows.Forms.Label labelY;
		private System.Windows.Forms.Label labelPlayerName;
		private System.Windows.Forms.Label labelUnitTraining;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public UnitStatus()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(UnitStatus));
			this.labelUnitWon = new System.Windows.Forms.Label();
			this.labelUnitStance = new System.Windows.Forms.Label();
			this.labelUnitDescription = new System.Windows.Forms.Label();
			this.labelUnitSupport = new System.Windows.Forms.Label();
			this.labelUnitStrength = new System.Windows.Forms.Label();
			this.labelUnitMorale = new System.Windows.Forms.Label();
			this.pictureUnit = new System.Windows.Forms.PictureBox();
			this.labelUnitType = new System.Windows.Forms.Label();
			this.labelX = new System.Windows.Forms.Label();
			this.labelY = new System.Windows.Forms.Label();
			this.labelPlayerName = new System.Windows.Forms.Label();
			this.labelUnitTraining = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// labelUnitWon
			// 
			this.labelUnitWon.Location = new System.Drawing.Point(8, 176);
			this.labelUnitWon.Name = "labelUnitWon";
			this.labelUnitWon.Size = new System.Drawing.Size(112, 16);
			this.labelUnitWon.TabIndex = 28;
			this.labelUnitWon.Text = "Won: 0";
			// 
			// labelUnitStance
			// 
			this.labelUnitStance.Location = new System.Drawing.Point(8, 240);
			this.labelUnitStance.Name = "labelUnitStance";
			this.labelUnitStance.Size = new System.Drawing.Size(112, 16);
			this.labelUnitStance.TabIndex = 27;
			this.labelUnitStance.Text = "Stance: Square";
			// 
			// labelUnitDescription
			// 
			this.labelUnitDescription.Location = new System.Drawing.Point(8, 48);
			this.labelUnitDescription.Name = "labelUnitDescription";
			this.labelUnitDescription.Size = new System.Drawing.Size(112, 40);
			this.labelUnitDescription.TabIndex = 26;
			this.labelUnitDescription.Text = "Descriptive Text";
			// 
			// labelUnitSupport
			// 
			this.labelUnitSupport.Location = new System.Drawing.Point(8, 128);
			this.labelUnitSupport.Name = "labelUnitSupport";
			this.labelUnitSupport.Size = new System.Drawing.Size(112, 16);
			this.labelUnitSupport.TabIndex = 25;
			this.labelUnitSupport.Text = "Support: 0";
			// 
			// labelUnitStrength
			// 
			this.labelUnitStrength.Location = new System.Drawing.Point(8, 96);
			this.labelUnitStrength.Name = "labelUnitStrength";
			this.labelUnitStrength.Size = new System.Drawing.Size(112, 16);
			this.labelUnitStrength.TabIndex = 24;
			this.labelUnitStrength.Text = "Stength: 100%";
			// 
			// labelUnitMorale
			// 
			this.labelUnitMorale.Location = new System.Drawing.Point(8, 112);
			this.labelUnitMorale.Name = "labelUnitMorale";
			this.labelUnitMorale.Size = new System.Drawing.Size(112, 16);
			this.labelUnitMorale.TabIndex = 23;
			this.labelUnitMorale.Text = "Morale: 100%";
			// 
			// pictureUnit
			// 
			this.pictureUnit.Location = new System.Drawing.Point(8, 8);
			this.pictureUnit.Name = "pictureUnit";
			this.pictureUnit.Size = new System.Drawing.Size(32, 32);
			this.pictureUnit.TabIndex = 21;
			this.pictureUnit.TabStop = false;
			// 
			// labelUnitType
			// 
			this.labelUnitType.Location = new System.Drawing.Point(8, 256);
			this.labelUnitType.Name = "labelUnitType";
			this.labelUnitType.Size = new System.Drawing.Size(112, 16);
			this.labelUnitType.TabIndex = 22;
			this.labelUnitType.Text = "Unit Type";
			// 
			// labelX
			// 
			this.labelX.Location = new System.Drawing.Point(8, 192);
			this.labelX.Name = "labelX";
			this.labelX.Size = new System.Drawing.Size(112, 16);
			this.labelX.TabIndex = 29;
			this.labelX.Text = "X: 0";
			// 
			// labelY
			// 
			this.labelY.Location = new System.Drawing.Point(8, 208);
			this.labelY.Name = "labelY";
			this.labelY.Size = new System.Drawing.Size(112, 16);
			this.labelY.TabIndex = 30;
			this.labelY.Text = "Y: 0";
			// 
			// labelPlayerName
			// 
			this.labelPlayerName.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelPlayerName.Location = new System.Drawing.Point(48, 8);
			this.labelPlayerName.Name = "labelPlayerName";
			this.labelPlayerName.Size = new System.Drawing.Size(72, 23);
			this.labelPlayerName.TabIndex = 31;
			this.labelPlayerName.Text = "Player";
			// 
			// labelUnitTraining
			// 
			this.labelUnitTraining.Location = new System.Drawing.Point(8, 144);
			this.labelUnitTraining.Name = "labelUnitTraining";
			this.labelUnitTraining.Size = new System.Drawing.Size(112, 16);
			this.labelUnitTraining.TabIndex = 32;
			this.labelUnitTraining.Text = "Training: 0";
			// 
			// UnitStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(122, 279);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.labelUnitTraining,
																		  this.labelPlayerName,
																		  this.labelY,
																		  this.labelX,
																		  this.labelUnitWon,
																		  this.labelUnitStance,
																		  this.labelUnitDescription,
																		  this.labelUnitSupport,
																		  this.labelUnitStrength,
																		  this.labelUnitMorale,
																		  this.pictureUnit,
																		  this.labelUnitType});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UnitStatus";
			this.Text = "Unit";
			this.Load += new System.EventHandler(this.UnitStatus_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void UnitStatus_Load(object sender, System.EventArgs e)
		{
			this.pictureUnit.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.EMPTY_TILE);
		}

		public void showUnit(stanceEnum stance, unitEnum unitType, playerEnum player, int support, int strength, int morale, int training, string description, int won, int x, int y, Image image, string currentPlayer)
		{
			if (strength == -1)
			{
				labelX.Text = "";
				labelX.Text = "";
				labelY.Text = "";
				labelUnitDescription.Text = "";
				labelUnitWon.Text = "";
				labelUnitStance.Text = "";
				labelUnitSupport.Text = "";
				labelUnitStrength.Text = "";
				labelUnitMorale.Text = "";
				labelUnitTraining.Text = "";
				labelUnitType.Text = "";
				labelPlayerName.Text = "";
				this.pictureUnit.Image = image;
				this.pictureUnit.Visible = false;
			}
			else
			{
				labelX.Text = "X: " + x.ToString();
				labelY.Text = "Y: " + y.ToString();
				labelUnitDescription.Text = description;
				labelUnitWon.Text = "Battles Won: " + won.ToString();
				labelUnitStance.Text = "Stance: " + stance.ToString();
				labelUnitSupport.Text = "Support: " + support.ToString();
				labelUnitStrength.Text = "Strength: " + strength.ToString();
				labelUnitMorale.Text = "Morale: " + morale.ToString();
				labelUnitTraining.Text = "Training: " + training.ToString();
				labelUnitType.Text = "Unit Type: " + unitType.ToString();
				labelPlayerName.Text = currentPlayer;
				this.pictureUnit.Image = image;
				this.pictureUnit.Visible = true;
			}
		}
	}
}
