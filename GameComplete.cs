using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

namespace EnglishSquares
{
	/// <summary>
	/// Summary description for GameComplete.
	/// </summary>

	public class GameComplete : System.Windows.Forms.Form
	{
		public System.Windows.Forms.Label labelResult;
		private System.Windows.Forms.Button buttonClose;

		private System.Timers.Timer timeAnimate;
		bool win = false;

		protected Bitmap _workBuffer;
		protected Graphics _workG;
		protected Bitmap _displayBuffer;	
		protected Graphics _display;
		Pen penWhite = new Pen(Color.White);
		Pen penRed = new Pen(Color.Red);
		Pen penOrange = new Pen(Color.Orange);
		int webSize = 10;
		int webDir = 1;
		int fireLine = 0;
		const int fireHeight = 50;
		const int fireDivisions = 8;
		const int numParticles = 50;
		Random r = new Random();

		fireworkParticle[] particles = new fireworkParticle[numParticles];

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GameComplete()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Opaque | ControlStyles.DoubleBuffer, true);
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

		public void useFireworks()
		{
			win = true;
		}

		public void burnFire()
		{
			win = false;
		}

		private void OnTimeAnimate(Object src, ElapsedEventArgs e)
		{
			this.Invalidate();
			
			webSize += webDir;
			if (webSize > 20 || webSize < 10)
			{
				webDir = (webDir == 1)?-1:1;
			}

			if (fireLine <= fireHeight)
			{
				fireLine++;
			}
		}

		private void drawWeb(Graphics g, Pen p) 
		{
			float width = ClientRectangle.Width;
			float height = ClientRectangle.Height;
			float xDelta = width / webSize;
			float yDelta = height / webSize;

			for (int i = 0; i < webSize; i++) 
			{
				g.DrawLine(p, width, height - (yDelta * i), xDelta * i, height);
			}
		}

		private void drawRandom(Graphics g) 
		{
			Bitmap bm = new Bitmap(1,1);
			Random r = new Random();
			Color c;
			Pen penRandom;

			for (int i = 0; i < ClientRectangle.Width; i++)
			{
				c = Color.FromArgb(r.Next(155)+100, r.Next(150), r.Next(50));
				penRandom = new Pen(c);
				bm.SetPixel(0, 0, c);
				_workG.DrawImageUnscaled(bm, i, ClientRectangle.Height - fireLine);
			}

			g.DrawImage(_workBuffer, 0, 0);
		}

		private void drawFireOld(Graphics g) 
		{
			Random r = new Random();
			Color c;
			Pen penRandom;
			int meanColour = 0;

			Rectangle rectSource = new Rectangle(0, 2, _workBuffer.Width, _workBuffer.Height);
			Rectangle rectDest = new Rectangle(0, 0, _workBuffer.Width, _workBuffer.Height-2);
			_workG.DrawImage(_workBuffer, rectDest, rectSource, GraphicsUnit.Pixel);

			for (int j = 1; j < _workBuffer.Height; j++)
			{
				for (int i = 0; i < _workBuffer.Width; i++)
				{
					if (j == _workBuffer.Height - 1)
					{
						c = Color.FromArgb(r.Next(255), 0, 0);
					}
					else
					{
						meanColour = (_workBuffer.GetPixel(i, j) != Color.Empty)?
										_workBuffer.GetPixel(i, j).ToArgb()
										:Color.Empty.ToArgb();

						meanColour += (j < _workBuffer.Height-1)?
										_workBuffer.GetPixel(i, j+1).ToArgb()	
										:Color.Empty.ToArgb();

						meanColour += (i > 0 && j < _workBuffer.Height-1)?
										_workBuffer.GetPixel(i-1, j+1).ToArgb()	
										:Color.Empty.ToArgb();

						meanColour += (i < _workBuffer.Width-1 && j < _workBuffer.Height-1)?
										_workBuffer.GetPixel(i+1, j+1).ToArgb()	
										:Color.Empty.ToArgb();

						meanColour += (j < _workBuffer.Height-2)?
										_workBuffer.GetPixel(i, j+2).ToArgb()
										:Color.Empty.ToArgb();

						meanColour += (i > 0 && j < _workBuffer.Height-2)?
										_workBuffer.GetPixel(i-1, j+2).ToArgb()	
										:Color.Empty.ToArgb();

						meanColour += (i < _workBuffer.Width-1 && j < _workBuffer.Height-2)?
										_workBuffer.GetPixel(i+1, j+2).ToArgb()	
										:Color.Empty.ToArgb();

						meanColour /= 7;

						c = Color.FromArgb(meanColour);
					}

					penRandom = new Pen(c);
					_workBuffer.SetPixel(i, j, c);
					
					//bm.SetPixel(0, 0, c);
					//_workG.DrawImageUnscaled(bm, i, j);
				}
			}

			int divSpace = ClientRectangle.Width / fireDivisions;
			for (int i = 0; i < fireDivisions; i++)
			{
				if (i % 2 == 0)
				{
					g.DrawImage(_workBuffer, divSpace*i + (divSpace/2), ClientRectangle.Height - _workBuffer.Height - 10 - (i*4));
				}
			}
		}

		private void drawFire(Graphics g) 
		{
			Color c;
			Pen penRandom;
			int meanR = 0;
			int meanG = 0;
			int meanB = 0;
			int minCol = 30;

			// random seeded hot pixels
			for (int i = 0; i < _workBuffer.Width; i++)
			{
				c = Color.FromArgb(r.Next(155)+100, r.Next(50), r.Next(50));
				penRandom = new Pen(c);
				_workBuffer.SetPixel(i, _workBuffer.Height-1, c);
			}

			// random white spots
			int numSpots = r.Next(5)+5;
			for (int k = 0; k < numSpots; k++)
			{
				int xPos = r.Next(_workBuffer.Width-1);
				_workBuffer.SetPixel(xPos, _workBuffer.Height-1, Color.White);
			}

			// random maximum black spots
			numSpots = r.Next(5)+5;
			for (int k = 0; k < numSpots; k++)
			{
				int xPos = r.Next(_workBuffer.Width-1);
				_workBuffer.SetPixel(xPos, _workBuffer.Height-1, Color.Black);
			}

			// random maximum yellow spots
			numSpots = r.Next(5)+5;
			for (int k = 0; k < numSpots; k++)
			{
				int xPos = r.Next(_workBuffer.Width-1);
				_workBuffer.SetPixel(xPos, _workBuffer.Height-1, Color.Yellow);
			}

			// burn flame upwards
			for (int j = 0; j < _workBuffer.Height-1; j++)
			{
				// move line up
				for (int i = 0; i < _workBuffer.Width; i++)
				{
					c = _workBuffer.GetPixel(i, j+1);
					penRandom = new Pen(c);
					_workBuffer.SetPixel(i, j, c);
				}
			}

			// smoothe and cool
			for (int j = 1; j < _workBuffer.Height-2; j++)
			{
				for (int i = 1; i < _workBuffer.Width-1; i++)
				{
					meanR = (_workBuffer.GetPixel(i, j).R +
						     _workBuffer.GetPixel(i, j+1).R +	
						     _workBuffer.GetPixel(i-1, j+1).R +
						     _workBuffer.GetPixel(i+1, j+1).R) / 4;

					meanG = (_workBuffer.GetPixel(i, j).G +
							 _workBuffer.GetPixel(i, j+1).G +	
						     _workBuffer.GetPixel(i-1, j+1).G +
						     _workBuffer.GetPixel(i+1, j+1).G) / 4;

					meanB = (_workBuffer.GetPixel(i, j).B +
							 _workBuffer.GetPixel(i, j+1).B +	
							 _workBuffer.GetPixel(i-1, j+1).B +
							 _workBuffer.GetPixel(i+1, j+1).B) / 4;

                    meanR = (meanR < minCol)? 0: meanR-r.Next(minCol);
					meanG = (meanG < minCol)? 0: meanG-r.Next(minCol);
					meanB = (meanB < minCol)? 0: meanB-r.Next(minCol);

					c = (meanR==0 && meanG==0 && meanB==0)? Color.Empty : Color.FromArgb(meanR, meanG, meanB); 
					if (c == Color.Black) c = Color.Empty;

					penRandom = new Pen(c);
					_workBuffer.SetPixel(i, j, c);
				}

				_workBuffer.SetPixel(0, j, Color.Empty);
				_workBuffer.SetPixel(_workBuffer.Width-1, j, Color.Empty);
			}

			// display flame in a few places
			int divSpace = ClientRectangle.Width / fireDivisions;
			for (int i = 0; i < fireDivisions; i++)
			{
				if (i % 2 == 0)
				{
					g.DrawImage(_workBuffer, divSpace*i + (divSpace/2), ClientRectangle.Height - _workBuffer.Height - 20 - (i*8));
				}
			}
		}

		private void drawFireworks(Graphics g) 
		{
			if (particles[0].y >= _displayBuffer.Height-1)
			{
				int xx = r.Next(20, _displayBuffer.Width-20);
				int xvel = r.Next(1-particles[0].x_velocity, particles[0].x_velocity);

				for (int i = 0; i<numParticles; i++)
				{
					particles[i].launchFirework(xx, _displayBuffer.Height-10, _displayBuffer.Height-20, i, xvel);
				}
			}

			for (int i = 0; i<numParticles; i++)
			{
				particles[i].x += particles[i].velocity_x;
				particles[i].y += particles[i].velocity_y;
				if (particles[i].velocity_x > 0)
				{
					particles[i].velocity_x -= particles[i].friction;
				}
				if (particles[i].velocity_x < 0)
				{
					particles[i].velocity_x += particles[i].friction;
				}
			
				particles[i].velocity_y += particles[i].gravity;

				if (particles[i].exploded == true)
				{
					//particles[i].colour = Color.FromArgb((particles[i].colour.R < 20)? 0: particles[i].colour.R-r.Next(20), (particles[i].colour.G < 20)? 0: particles[i].colour.G-r.Next(20), (particles[i].colour.B < 20)? 0: particles[i].colour.B-r.Next(20));
					//if (particles[i].colour == Color.Black)
						//particles[i].colour = Color.Empty;
					particles[i].colour = Color.FromArgb(r.Next(200)+55, r.Next(200)+55, r.Next(200)+55); 
				}

				if (particles[i].velocity_y > 0 && particles[i].exploded == false)
				{
					particles[i].x = particles[i].x + r.Next(-particles[i].explosionDistance, particles[i].explosionDistance);
					particles[i].y = particles[i].y + r.Next(-particles[i].explosionDistance, particles[i].explosionDistance);
					particles[i].colour = Color.FromArgb(r.Next(100)+155, r.Next(100)+155, r.Next(100)+155); 
					particles[i].exploded = true;
					particles[i].velocity_x = 0;
				}		

				if (particles[i].x > _displayBuffer.Width-1) particles[i].x = _displayBuffer.Width-1;
				if (particles[i].y > _displayBuffer.Height-1) particles[i].y = _displayBuffer.Height-1;
				if (particles[i].x < 0) particles[i].x = 0;
				if (particles[i].y < 0) particles[i].y = 0;

				//_displayBuffer.SetPixel(particles[i].x, particles[i].y, particles[i].colour);
				Pen penPoint = new Pen(particles[i].colour);
				_display.DrawEllipse(penPoint, particles[i].x, particles[i].y, 2, 2);
			}

			g.DrawImage(_displayBuffer, 0, ClientRectangle.Height);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			_display.Clear(this.BackColor);

			Graphics g = e.Graphics;

			_display.DrawImage(Image.FromFile(AppDomain.CurrentDomain.BaseDirectory + Constants.TOWN_IMAGE), 0, 0, ClientRectangle.Width, ClientRectangle.Height);
			Font fontText = new System.Drawing.Font("Edwardian Script ITC", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			Brush brushText = new SolidBrush(Color.Black);
			_display.DrawString(labelResult.Text, fontText, brushText, 10, 10); 

			//drawWeb(_display, penWhite);

			if (win == true)
			{
				//drawFire(_display);
				drawFireworks(_display);
			}
			else
			{
				drawFire(_display);
			}

			g.DrawImage(_displayBuffer, 0, 0);
			g.Dispose();

			base.OnPaint(e);
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GameComplete));
			this.labelResult = new System.Windows.Forms.Label();
			this.buttonClose = new System.Windows.Forms.Button();
			this.timeAnimate = new System.Timers.Timer();
			((System.ComponentModel.ISupportInitialize)(this.timeAnimate)).BeginInit();
			this.SuspendLayout();
			// 
			// labelResult
			// 
			this.labelResult.Font = new System.Drawing.Font("Edwardian Script ITC", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.labelResult.ForeColor = System.Drawing.Color.Black;
			this.labelResult.Location = new System.Drawing.Point(26, 8);
			this.labelResult.Name = "labelResult";
			this.labelResult.Size = new System.Drawing.Size(248, 80);
			this.labelResult.TabIndex = 2;
			this.labelResult.Text = "Victory";
			this.labelResult.Visible = false;
			// 
			// buttonClose
			// 
			this.buttonClose.Location = new System.Drawing.Point(272, 8);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(88, 23);
			this.buttonClose.TabIndex = 3;
			this.buttonClose.Text = "Close";
			this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
			// 
			// timeAnimate
			// 
			this.timeAnimate.Enabled = true;
			this.timeAnimate.Interval = 100;
			this.timeAnimate.SynchronizingObject = this;
			this.timeAnimate.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimeAnimate);
			// 
			// GameComplete
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(368, 247);
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		  this.buttonClose,
																		  this.labelResult});
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			//this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GameComplete";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Game Complete";
			this.TopMost = true;
			this.Load += new System.EventHandler(this.GameComplete_Load);
			((System.ComponentModel.ISupportInitialize)(this.timeAnimate)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		private void GameComplete_Load(object sender, System.EventArgs e)
		{
			_workBuffer = new Bitmap(ClientRectangle.Width / fireDivisions, fireHeight, Graphics.FromHwnd(this.Handle));
			_workG = Graphics.FromImage(_workBuffer);	
			_workG.Clear(Color.Empty);
	
			_displayBuffer = new Bitmap(ClientRectangle.Width, ClientRectangle.Height, Graphics.FromHwnd(this.Handle));
			_display = Graphics.FromImage(_displayBuffer);	

			for (int i = 0; i<numParticles; i++)
			{
				particles[i] = new fireworkParticle();
				particles[i].y = _displayBuffer.Height-1;
			}		
		}

		private void buttonClose_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}
	}

	public class fireworkParticle
	{
		public bool exploded = false;
		public int x = 0;
		public int y = 0;
		public int velocity_x = 0;
		public int velocity_y = 0;
		public Color colour = Color.White;
		public int maxHeight = 0;
		Random r = new Random();
		public int index = 0;

		public int max_velocity = 28;
		public int min_velocity = 18;
		public int x_velocity = 4;
		public int gravity = 2;
		public int friction = 1;
		public int explosionDistance = 25;
		public int maxTimer = 1000;

		public void launchFirework(int startx, int starty, int height, int i, int startxvelocity)
		{
			x = startx;
			y = height;
			velocity_x = startxvelocity;
			velocity_y = -r.Next(min_velocity, max_velocity);
			colour = Color.FromArgb(r.Next(100), r.Next(100), r.Next(100));
			maxHeight = height;
			index = i;
			exploded = false;
		}

	}
}


