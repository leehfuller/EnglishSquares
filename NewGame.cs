using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml;

namespace EnglishSquares
{
	/// <summary>
	/// Summary description for NewGame.
	/// </summary>
	public class NewGame : System.Windows.Forms.Form
	{
		private System.Windows.Forms.PictureBox pictureEnglish;
		private System.Windows.Forms.PictureBox pictureFrench;
		private System.Windows.Forms.PictureBox picturePrussian;
		private System.Windows.Forms.PictureBox pictureRussian;
		private System.Windows.Forms.PictureBox pictureAustria;
		private System.Windows.Forms.ListBox listScenarios;
		private System.Windows.Forms.Button buttonNew;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonRegister;
		private System.Windows.Forms.Label labelBritish;
		private System.Windows.Forms.Label labelPrussian;
		private System.Windows.Forms.Label labelFrench;
		private System.Windows.Forms.Label labelRussian;
		private System.Windows.Forms.Label labelAustrian;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.TextBox textDescription;

		public bool selectedMap;
		public string mapFilename = "";
		protected Hashtable _hashDescriptions = new Hashtable();
		protected Hashtable _hashFilenames = new Hashtable();

		public NewGame()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			selectedMap = false;
			showBritish();
			textDescription.Text = "";

			this.pictureEnglish.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.BRITISH_FLAG);
			this.pictureFrench.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.FRENCH_FLAG);
			this.picturePrussian.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.PRUSSIAN_FLAG);
			this.pictureRussian.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.RUSSIAN_FLAG);
			this.pictureAustria.Image = Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.AUSTRIAN_FLAG);
		}

		public void checkShareware(bool sharewareVersion)
		{
			// hide shareware parts if not shareware version
			if (sharewareVersion == false)
			{
				this.buttonRegister.Visible = false;
			}
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
			this.pictureEnglish = new System.Windows.Forms.PictureBox();
			this.pictureFrench = new System.Windows.Forms.PictureBox();
			this.picturePrussian = new System.Windows.Forms.PictureBox();
			this.pictureRussian = new System.Windows.Forms.PictureBox();
			this.pictureAustria = new System.Windows.Forms.PictureBox();
			this.listScenarios = new System.Windows.Forms.ListBox();
			this.buttonNew = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.labelBritish = new System.Windows.Forms.Label();
			this.labelPrussian = new System.Windows.Forms.Label();
			this.labelFrench = new System.Windows.Forms.Label();
			this.labelRussian = new System.Windows.Forms.Label();
			this.labelAustrian = new System.Windows.Forms.Label();
			this.buttonRegister = new System.Windows.Forms.Button();
			this.textDescription = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// pictureEnglish
			// 
			this.pictureEnglish.Location = new System.Drawing.Point(8, 16);
			this.pictureEnglish.Name = "pictureEnglish";
			this.pictureEnglish.Size = new System.Drawing.Size(80, 56);
			this.pictureEnglish.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureEnglish.TabIndex = 0;
			this.pictureEnglish.TabStop = false;
			this.pictureEnglish.Click += new System.EventHandler(this.pictureEnglish_Click);
			// 
			// pictureFrench
			// 
			this.pictureFrench.Location = new System.Drawing.Point(184, 16);
			this.pictureFrench.Name = "pictureFrench";
			this.pictureFrench.Size = new System.Drawing.Size(80, 56);
			this.pictureFrench.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureFrench.TabIndex = 1;
			this.pictureFrench.TabStop = false;
			this.pictureFrench.Click += new System.EventHandler(this.pictureFrench_Click);
			// 
			// picturePrussian
			// 
			this.picturePrussian.Location = new System.Drawing.Point(96, 16);
			this.picturePrussian.Name = "picturePrussian";
			this.picturePrussian.Size = new System.Drawing.Size(80, 56);
			this.picturePrussian.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.picturePrussian.TabIndex = 2;
			this.picturePrussian.TabStop = false;
			this.picturePrussian.Click += new System.EventHandler(this.picturePrussian_Click);
			// 
			// pictureRussian
			// 
			this.pictureRussian.Location = new System.Drawing.Point(272, 16);
			this.pictureRussian.Name = "pictureRussian";
			this.pictureRussian.Size = new System.Drawing.Size(80, 56);
			this.pictureRussian.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureRussian.TabIndex = 3;
			this.pictureRussian.TabStop = false;
			this.pictureRussian.Click += new System.EventHandler(this.pictureRussian_Click);
			// 
			// pictureAustria
			// 
			this.pictureAustria.Location = new System.Drawing.Point(360, 16);
			this.pictureAustria.Name = "pictureAustria";
			this.pictureAustria.Size = new System.Drawing.Size(80, 56);
			this.pictureAustria.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureAustria.TabIndex = 4;
			this.pictureAustria.TabStop = false;
			this.pictureAustria.Click += new System.EventHandler(this.pictureAustria_Click);
			// 
			// listScenarios
			// 
			this.listScenarios.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.listScenarios.ItemHeight = 16;
			this.listScenarios.Location = new System.Drawing.Point(16, 112);
			this.listScenarios.Name = "listScenarios";
			this.listScenarios.Size = new System.Drawing.Size(312, 148);
			this.listScenarios.TabIndex = 5;
			this.listScenarios.DoubleClick += new System.EventHandler(this.listScenarios_DoubleClick);
			this.listScenarios.SelectedIndexChanged += new System.EventHandler(this.listScenarios_SelectedIndexChanged);
			// 
			// buttonNew
			// 
			this.buttonNew.Location = new System.Drawing.Point(344, 112);
			this.buttonNew.Name = "buttonNew";
			this.buttonNew.Size = new System.Drawing.Size(96, 24);
			this.buttonNew.TabIndex = 6;
			this.buttonNew.Text = "Start...";
			this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(344, 144);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(96, 23);
			this.buttonCancel.TabIndex = 7;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// labelBritish
			// 
			this.labelBritish.Location = new System.Drawing.Point(8, 80);
			this.labelBritish.Name = "labelBritish";
			this.labelBritish.Size = new System.Drawing.Size(80, 16);
			this.labelBritish.TabIndex = 8;
			this.labelBritish.Text = "British";
			this.labelBritish.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.labelBritish.Click += new System.EventHandler(this.labelBritish_Click);
			// 
			// labelPrussian
			// 
			this.labelPrussian.Location = new System.Drawing.Point(96, 80);
			this.labelPrussian.Name = "labelPrussian";
			this.labelPrussian.Size = new System.Drawing.Size(80, 16);
			this.labelPrussian.TabIndex = 9;
			this.labelPrussian.Text = "Prussian";
			this.labelPrussian.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelPrussian.Click += new System.EventHandler(this.labelPrussian_Click);
			// 
			// labelFrench
			// 
			this.labelFrench.Location = new System.Drawing.Point(184, 80);
			this.labelFrench.Name = "labelFrench";
			this.labelFrench.Size = new System.Drawing.Size(80, 16);
			this.labelFrench.TabIndex = 10;
			this.labelFrench.Text = "French";
			this.labelFrench.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelFrench.Click += new System.EventHandler(this.labelFrench_Click);
			// 
			// labelRussian
			// 
			this.labelRussian.Location = new System.Drawing.Point(272, 80);
			this.labelRussian.Name = "labelRussian";
			this.labelRussian.Size = new System.Drawing.Size(80, 16);
			this.labelRussian.TabIndex = 11;
			this.labelRussian.Text = "Russian";
			this.labelRussian.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelRussian.Click += new System.EventHandler(this.labelRussian_Click);
			// 
			// labelAustrian
			// 
			this.labelAustrian.Location = new System.Drawing.Point(360, 80);
			this.labelAustrian.Name = "labelAustrian";
			this.labelAustrian.Size = new System.Drawing.Size(80, 16);
			this.labelAustrian.TabIndex = 12;
			this.labelAustrian.Text = "Austrian";
			this.labelAustrian.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.labelAustrian.Click += new System.EventHandler(this.labelAustrian_Click);
			// 
			// buttonRegister
			// 
			this.buttonRegister.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.buttonRegister.Location = new System.Drawing.Point(344, 304);
			this.buttonRegister.Name = "buttonRegister";
			this.buttonRegister.Size = new System.Drawing.Size(96, 24);
			this.buttonRegister.TabIndex = 13;
			this.buttonRegister.Text = "Register...";
			this.buttonRegister.Click += new System.EventHandler(this.buttonRegister_Click);
			// 
			// textDescription
			// 
			this.textDescription.Location = new System.Drawing.Point(16, 264);
			this.textDescription.Multiline = true;
			this.textDescription.Name = "textDescription";
			this.textDescription.ReadOnly = true;
			this.textDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textDescription.Size = new System.Drawing.Size(312, 64);
			this.textDescription.TabIndex = 14;
			this.textDescription.Text = "textDescription";
			this.textDescription.TextChanged += new System.EventHandler(this.textDescription_TextChanged);
			// 
			// NewGame
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(450, 343);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.textDescription,
																		  this.buttonRegister,
																		  this.labelAustrian,
																		  this.labelRussian,
																		  this.labelFrench,
																		  this.labelPrussian,
																		  this.labelBritish,
																		  this.buttonCancel,
																		  this.buttonNew,
																		  this.listScenarios,
																		  this.pictureAustria,
																		  this.pictureRussian,
																		  this.picturePrussian,
																		  this.pictureFrench,
																		  this.pictureEnglish});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewGame";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "New Game";
			this.Load += new System.EventHandler(this.NewGame_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void NewGame_Load(object sender, System.EventArgs e)
		{
			readScenarios("English");
		}

		private void buttonCancel_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void readScenarios(string xmlName)
		{
			listScenarios.Items.Clear();

			string[] mapList = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Maps");
			int index = 0;
			bool foundTitle = false;
			bool foundDescription = false;
			string scenarioName = "";
			string scenarioDescription = "";

			foreach (string mapFile in mapList)
			{
				index = mapFile.LastIndexOf("\\");
				scenarioName = mapFile.Substring(index+1);

				if (scenarioName.StartsWith(xmlName))
				{
					try
					{
						FileStream fRead = new FileStream(mapFile, FileMode.Open);
						XmlTextReader xmlReader = new XmlTextReader(fRead);
						foundTitle = false;
						foundDescription = false;
						while (xmlReader.Read() && (foundTitle == false || foundDescription == false))
						{
							if (xmlReader.NodeType == XmlNodeType.Element)
							{
								if (xmlReader.Name == "Title")
								{
									scenarioName = xmlReader.GetAttribute("Name");
									foundTitle = true;
								}

								if (xmlReader.Name == "Description")
								{
									xmlReader.Read();
									if (xmlReader.NodeType == XmlNodeType.Text)
									{
										scenarioDescription = xmlReader.Value;
										foundDescription = true;
									}
								}
							}
						}
						xmlReader.Close();
						fRead.Close();
					}
					catch (Exception e)
					{
						scenarioDescription = e.Message;
					}

					scenarioName = " " + scenarioName;
					listScenarios.Items.Add(scenarioName);
					_hashDescriptions[scenarioName] = scenarioDescription;
					_hashFilenames[scenarioName] = mapFile;
				}
			}	
		}
		
		private void resetDisplay()
		{	
			labelBritish.Font = new Font("Tahoma", 8, FontStyle.Regular);
			labelPrussian.Font = new Font("Tahoma", 8, FontStyle.Regular);
			labelFrench.Font = new Font("Tahoma", 8, FontStyle.Regular);
			labelRussian.Font = new Font("Tahoma", 8, FontStyle.Regular);
			labelAustrian.Font = new Font("Tahoma", 8, FontStyle.Regular);
		}

		private void showBritish()
		{
			resetDisplay();
			labelBritish.Font = new Font("Tahoma", 10, FontStyle.Bold);
			readScenarios("English");
		}

		private void showPrussian()
		{
			resetDisplay();
			labelPrussian.Font = new Font("Tahoma", 10, FontStyle.Bold);
			readScenarios("Prussia");
		}

		private void showFrench()
		{
			resetDisplay();
			labelFrench.Font = new Font("Tahoma", 10, FontStyle.Bold);
			readScenarios("French");
		}

		private void showRussian()
		{
			resetDisplay();
			labelRussian.Font = new Font("Tahoma", 10, FontStyle.Bold);
			readScenarios("Russia");
		}

		private void showAustrian()
		{
			resetDisplay();
			labelAustrian.Font = new Font("Tahoma", 10, FontStyle.Bold);
			readScenarios("Austria");
		}

		private void pictureEnglish_Click(object sender, System.EventArgs e)
		{
			showBritish();
		}

		private void labelBritish_Click(object sender, System.EventArgs e)
		{
			showBritish();		
		}

		private void picturePrussian_Click(object sender, System.EventArgs e)
		{
			showPrussian();		
		}

		private void labelPrussian_Click(object sender, System.EventArgs e)
		{
			showPrussian();
		}

		private void pictureFrench_Click(object sender, System.EventArgs e)
		{
			showFrench();
		}

		private void labelFrench_Click(object sender, System.EventArgs e)
		{
			showFrench();
		}

		private void pictureRussian_Click(object sender, System.EventArgs e)
		{
			showRussian();
		}

		private void labelRussian_Click(object sender, System.EventArgs e)
		{
			showRussian();
		}

		private void pictureAustria_Click(object sender, System.EventArgs e)
		{
			showAustrian();
		}

		private void labelAustrian_Click(object sender, System.EventArgs e)
		{
			showAustrian();
		}

		private void buttonRegister_Click(object sender, System.EventArgs e)
		{
			Process ieProcess = new Process();
			ieProcess.StartInfo.FileName = "iexplore.exe";
			ieProcess.StartInfo.Arguments = "http://www.fullerdata.com/englishsquares/default.aspx?Action=Register";		
			ieProcess.Start();		
		}

		private void buttonNew_Click(object sender, System.EventArgs e)
		{
			if (listScenarios.SelectedIndex > -1)
			{
				mapFilename =  (string) _hashFilenames[listScenarios.Items[listScenarios.SelectedIndex]];
				selectedMap = true;
				this.Close();	
			}
			else
			{
				MessageBox.Show("Please choose a scenario map to start or press cancel.", "No Map Selected", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void textDescription_TextChanged(object sender, System.EventArgs e)
		{
		}

		private void listScenarios_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			textDescription.Text = (string) _hashDescriptions[listScenarios.Items[listScenarios.SelectedIndex]];
		}

		private void listScenarios_DoubleClick(object sender, System.EventArgs e)
		{
			buttonNew_Click(sender, e);
		}
		
	}
}
