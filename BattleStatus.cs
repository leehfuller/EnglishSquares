using System.Drawing;

namespace EnglishSquares
{
	/// <summary>
	/// Summary description for BattleStatus.
	/// </summary>
	public class BattleStatus : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureUnit1;
		private System.Windows.Forms.PictureBox pictureUnit2;
		private System.Windows.Forms.Label labelAction;
		private System.Windows.Forms.Label labelUnit1;
		private System.Windows.Forms.Label labelUnit2;
		private System.Windows.Forms.Label labelStrength1;
		private System.Windows.Forms.Label labelMorale1;
		private System.Windows.Forms.Label labelTraining1;
		private System.Windows.Forms.Label labelStrength2;
		private System.Windows.Forms.Label labelMorale2;
		private System.Windows.Forms.Label labelTraining2;
		private System.Windows.Forms.Label labelSupport1;
		private System.Windows.Forms.Label labelSupport2;
		private System.Windows.Forms.Label labelTotal1;
		private System.Windows.Forms.Label labelTotal2;
		private System.Windows.Forms.Label labelResult1;
		private System.Windows.Forms.Label labelResult2;
		private System.Windows.Forms.Label Calc1;
		private System.Windows.Forms.Label Calc2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public BattleStatus()
		{
			//
			// Required for Windows Form Designer support
			//
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(BattleStatus));
			this.pictureUnit1 = new System.Windows.Forms.PictureBox();
			this.pictureUnit2 = new System.Windows.Forms.PictureBox();
			this.labelAction = new System.Windows.Forms.Label();
			this.labelUnit1 = new System.Windows.Forms.Label();
			this.labelUnit2 = new System.Windows.Forms.Label();
			this.labelStrength1 = new System.Windows.Forms.Label();
			this.labelMorale1 = new System.Windows.Forms.Label();
			this.labelTraining1 = new System.Windows.Forms.Label();
			this.labelStrength2 = new System.Windows.Forms.Label();
			this.labelMorale2 = new System.Windows.Forms.Label();
			this.labelTraining2 = new System.Windows.Forms.Label();
			this.labelSupport1 = new System.Windows.Forms.Label();
			this.labelSupport2 = new System.Windows.Forms.Label();
			this.labelTotal1 = new System.Windows.Forms.Label();
			this.labelTotal2 = new System.Windows.Forms.Label();
			this.labelResult1 = new System.Windows.Forms.Label();
			this.labelResult2 = new System.Windows.Forms.Label();
			this.Calc1 = new System.Windows.Forms.Label();
			this.Calc2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// pictureUnit1
			// 
			this.pictureUnit1.Location = new System.Drawing.Point(8, 8);
			this.pictureUnit1.Name = "pictureUnit1";
			this.pictureUnit1.Size = new System.Drawing.Size(32, 32);
			this.pictureUnit1.TabIndex = 2;
			this.pictureUnit1.TabStop = false;
			// 
			// pictureUnit2
			// 
			this.pictureUnit2.Location = new System.Drawing.Point(136, 8);
			this.pictureUnit2.Name = "pictureUnit2";
			this.pictureUnit2.Size = new System.Drawing.Size(32, 32);
			this.pictureUnit2.TabIndex = 3;
			this.pictureUnit2.TabStop = false;
			// 
			// labelAction
			// 
			this.labelAction.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelAction.Location = new System.Drawing.Point(40, 16);
			this.labelAction.Name = "labelAction";
			this.labelAction.Size = new System.Drawing.Size(88, 16);
			this.labelAction.TabIndex = 4;
			this.labelAction.Text = "Action Took";
			this.labelAction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// labelUnit1
			// 
			this.labelUnit1.Location = new System.Drawing.Point(8, 48);
			this.labelUnit1.Name = "labelUnit1";
			this.labelUnit1.Size = new System.Drawing.Size(120, 32);
			this.labelUnit1.TabIndex = 5;
			this.labelUnit1.Text = "Unit 1";
			// 
			// labelUnit2
			// 
			this.labelUnit2.Location = new System.Drawing.Point(136, 48);
			this.labelUnit2.Name = "labelUnit2";
			this.labelUnit2.Size = new System.Drawing.Size(120, 32);
			this.labelUnit2.TabIndex = 6;
			this.labelUnit2.Text = "Unit2";
			// 
			// labelStrength1
			// 
			this.labelStrength1.Location = new System.Drawing.Point(8, 88);
			this.labelStrength1.Name = "labelStrength1";
			this.labelStrength1.Size = new System.Drawing.Size(120, 16);
			this.labelStrength1.TabIndex = 7;
			this.labelStrength1.Text = "Strength1";
			// 
			// labelMorale1
			// 
			this.labelMorale1.Location = new System.Drawing.Point(8, 104);
			this.labelMorale1.Name = "labelMorale1";
			this.labelMorale1.Size = new System.Drawing.Size(120, 16);
			this.labelMorale1.TabIndex = 8;
			this.labelMorale1.Text = "Morale1";
			// 
			// labelTraining1
			// 
			this.labelTraining1.Location = new System.Drawing.Point(8, 120);
			this.labelTraining1.Name = "labelTraining1";
			this.labelTraining1.Size = new System.Drawing.Size(120, 16);
			this.labelTraining1.TabIndex = 9;
			this.labelTraining1.Text = "Training1";
			// 
			// labelStrength2
			// 
			this.labelStrength2.Location = new System.Drawing.Point(136, 88);
			this.labelStrength2.Name = "labelStrength2";
			this.labelStrength2.Size = new System.Drawing.Size(120, 16);
			this.labelStrength2.TabIndex = 10;
			this.labelStrength2.Text = "Strength2";
			// 
			// labelMorale2
			// 
			this.labelMorale2.Location = new System.Drawing.Point(136, 104);
			this.labelMorale2.Name = "labelMorale2";
			this.labelMorale2.Size = new System.Drawing.Size(120, 16);
			this.labelMorale2.TabIndex = 11;
			this.labelMorale2.Text = "Morale2";
			// 
			// labelTraining2
			// 
			this.labelTraining2.Location = new System.Drawing.Point(136, 120);
			this.labelTraining2.Name = "labelTraining2";
			this.labelTraining2.Size = new System.Drawing.Size(120, 16);
			this.labelTraining2.TabIndex = 12;
			this.labelTraining2.Text = "Training2";
			// 
			// labelSupport1
			// 
			this.labelSupport1.Location = new System.Drawing.Point(8, 136);
			this.labelSupport1.Name = "labelSupport1";
			this.labelSupport1.Size = new System.Drawing.Size(120, 16);
			this.labelSupport1.TabIndex = 13;
			this.labelSupport1.Text = "Support1";
			// 
			// labelSupport2
			// 
			this.labelSupport2.Location = new System.Drawing.Point(136, 136);
			this.labelSupport2.Name = "labelSupport2";
			this.labelSupport2.Size = new System.Drawing.Size(120, 16);
			this.labelSupport2.TabIndex = 14;
			this.labelSupport2.Text = "Support2";
			// 
			// labelTotal1
			// 
			this.labelTotal1.Location = new System.Drawing.Point(8, 160);
			this.labelTotal1.Name = "labelTotal1";
			this.labelTotal1.Size = new System.Drawing.Size(120, 16);
			this.labelTotal1.TabIndex = 15;
			this.labelTotal1.Text = "Total1";
			// 
			// labelTotal2
			// 
			this.labelTotal2.Location = new System.Drawing.Point(136, 160);
			this.labelTotal2.Name = "labelTotal2";
			this.labelTotal2.Size = new System.Drawing.Size(120, 16);
			this.labelTotal2.TabIndex = 16;
			this.labelTotal2.Text = "Total2";
			// 
			// labelResult1
			// 
			this.labelResult1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelResult1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelResult1.Location = new System.Drawing.Point(8, 176);
			this.labelResult1.Name = "labelResult1";
			this.labelResult1.Size = new System.Drawing.Size(120, 16);
			this.labelResult1.TabIndex = 17;
			this.labelResult1.Text = "Result1";
			this.labelResult1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// labelResult2
			// 
			this.labelResult2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelResult2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.labelResult2.Location = new System.Drawing.Point(136, 176);
			this.labelResult2.Name = "labelResult2";
			this.labelResult2.Size = new System.Drawing.Size(120, 16);
			this.labelResult2.TabIndex = 18;
			this.labelResult2.Text = "Result2";
			this.labelResult2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// Calc1
			// 
			this.Calc1.Location = new System.Drawing.Point(8, 200);
			this.Calc1.Name = "Calc1";
			this.Calc1.Size = new System.Drawing.Size(120, 16);
			this.Calc1.TabIndex = 19;
			this.Calc1.Text = "Calc1";
			// 
			// Calc2
			// 
			this.Calc2.Location = new System.Drawing.Point(136, 200);
			this.Calc2.Name = "Calc2";
			this.Calc2.Size = new System.Drawing.Size(120, 16);
			this.Calc2.TabIndex = 20;
			this.Calc2.Text = "Calc2";
			// 
			// BattleStatus
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(258, 215);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.Calc2,
																		  this.Calc1,
																		  this.labelResult2,
																		  this.labelResult1,
																		  this.labelTotal2,
																		  this.labelTotal1,
																		  this.labelSupport2,
																		  this.labelSupport1,
																		  this.labelTraining2,
																		  this.labelMorale2,
																		  this.labelStrength2,
																		  this.labelTraining1,
																		  this.labelMorale1,
																		  this.labelStrength1,
																		  this.labelUnit2,
																		  this.labelUnit1,
																		  this.labelAction,
																		  this.pictureUnit2,
																		  this.pictureUnit1});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BattleStatus";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Battle Status";
			this.Load += new System.EventHandler(this.BattleStatus_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void BattleStatus_Load(object sender, System.EventArgs e)
		{	
		}

		public void updateFields(string lastBattleMove, string lastBattleResult1, string lastBattleResult2, string lastBattleUnit1Description, string lastBattleUnit2Description, 
								 string lastBattleUnit1Strength, string lastBattleUnit1Morale, string lastBattleUnit1Support, string lastBattleUnit1Training, string lastBattleUnit1Damage, 
							     string lastBattleUnit2Strength, string lastBattleUnit2Morale, string lastBattleUnit2Support, string lastBattleUnit2Training, string lastBattleUnit2Damage,
								 Image imageUnit1, Image imageUnit2, string syscalc1, string syscalc2)
		{

			labelAction.Text = "";
			labelUnit1.Text = "";
			labelUnit2.Text = "";
			labelStrength1.Text = "";
			labelMorale1.Text = "";
			labelTraining1.Text = "";
			labelStrength2.Text = "";
			labelMorale2.Text = "";
			labelTraining2.Text = "";
			labelSupport1.Text = "";
			labelSupport2.Text = "";
			labelTotal1.Text = "";
			labelTotal2.Text = "";
			labelResult1.Text = "";
			labelResult2.Text = "";
			Calc1.Text = "";
			Calc2.Text = "";

			switch (lastBattleMove)
			{
				case "Move":
					labelAction.Text = "Moved";
					break;
				case "Attack":
					labelAction.Text = "Attacked";
					break;
				case "Fire":
					labelAction.Text = "Fired On";
					break;
				case "Stance":
					labelAction.Text = "New Stance";
					break;
				default:
					labelAction.Text = "";
					break;
			}

			labelUnit1.Text = lastBattleUnit1Description;
			labelUnit2.Text = lastBattleUnit2Description;

			if (lastBattleUnit1Strength.Length > 0)		
			{
				labelStrength1.Text = "Strength: " + lastBattleUnit1Strength;
				pictureUnit1.Visible = true;
			}
			else
			{
				pictureUnit1.Visible = false;
			}

			if (lastBattleUnit2Strength.Length > 0)		
			{
				labelStrength2.Text = "Strength: " + lastBattleUnit2Strength;
				pictureUnit2.Visible = true;
			}
			else
			{
				pictureUnit2.Visible = false;
			}

			pictureUnit1.Image = imageUnit1;
			pictureUnit2.Image = imageUnit2;

			if (lastBattleUnit1Morale.Length > 0)		
				labelMorale1.Text = "Morale: " + lastBattleUnit1Morale;

			if (lastBattleUnit2Morale.Length > 0)		
				labelMorale2.Text = "Morale: " + lastBattleUnit2Morale;

			if (lastBattleUnit1Training.Length > 0)		
				labelTraining1.Text = "Training: " + lastBattleUnit1Training;

			if (lastBattleUnit2Training.Length > 0)		
				labelTraining2.Text = "Training: " + lastBattleUnit2Training;

			if (lastBattleUnit1Support.Length > 0)		
				labelSupport1.Text = "Support: " + lastBattleUnit1Support;

			if (lastBattleUnit2Support.Length > 0)		
				labelSupport2.Text = "Support: " + lastBattleUnit2Support;

			if (lastBattleUnit1Damage.Length > 0 && lastBattleUnit1Damage != "0")		
				labelTotal1.Text = "Damage: " + lastBattleUnit1Damage;

			if (lastBattleUnit2Damage.Length > 0 && lastBattleUnit2Damage != "0")		
				labelTotal2.Text = "Damage: " + lastBattleUnit2Damage;

			if (lastBattleResult1.Length > 0 && lastBattleResult1 != "Nothing")		
				labelResult1.Text = lastBattleResult1;

			if (lastBattleResult2.Length > 0 && lastBattleResult2 != "Nothing")		
				labelResult2.Text = lastBattleResult2;

			if (syscalc1.Length > 0)
				Calc1.Text = syscalc1.ToString();

			if (syscalc2.Length > 0)
				Calc2.Text = syscalc2.ToString();
		}	
	}
}
