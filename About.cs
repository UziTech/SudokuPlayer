using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sudoku
{
	/// <summary>
	/// Summary description for About.
	/// </summary>
	public class About : System.Windows.Forms.Form
	{
		#region private stuff

		private System.Windows.Forms.Button btnOK;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		#region public About()
		public About()
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
		#endregion
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Black;
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.LightGreen;
            this.btnOK.Location = new System.Drawing.Point(43, 133);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            // 
            // About
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackColor = System.Drawing.Color.Gold;
            this.ClientSize = new System.Drawing.Size(162, 162);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "About";
            this.ShowInTaskbar = false;
            this.Text = "Sudoku v4.1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.About_Paint);
            this.ResumeLayout(false);

		}
		#endregion
		#region Global Variables
		int count = 0;
		int _left = 28;
		int _top = 35;
		int[,] _grid = new int[9,9];
		SudokuGrid grid = new SudokuGrid();
		int [,,] gridHints = new int[9,9,9];
		Pen _penThick = new Pen(Color.Black, 3);
		Rectangle[,] _boardArray = new Rectangle[9,9];
		Font _sudukoFont = new Font("Arial", 10, FontStyle.Regular);
		Font _hintsFont = new Font("Small Fonts", 4);
		Rectangle _board = new Rectangle(0,0, 0, 0);
		#endregion
		#region void DrawSudokuGrid(Graphics g)
		void DrawSudokuGrid(Graphics g)
		{
			if (count == 0)
			{
				_grid = grid.GenerateGrid();
				int r = 0;
				for (int row = 0; row < 5; row += 2)
				{
					for (int col = 0; col < 4; col += 2)
					{
						if (r == 0)
							r = new Random().Next();
						if (r % 2 == 1)
							_grid[row, col] = 0;
						r /= 2;
					}
				}
				for (int row = 1; row < 5; row += 2)
				{
					for (int col = 1; col < 4; col += 2)
					{
						if (r == 0)
							r = new Random().Next();
						if (r % 2 == 1)
							_grid[row, col] = 0;
						r /= 2;
					}
				}
				for (int row = 0; row < 5; row += 2)
				{
					for (int col = 1; col < 4; col += 2)
					{
						if (r == 0)
							r = new Random().Next();
						if (r % 2 == 1)
							_grid[row, col] = 0;
						r /= 2;
					}
				}
				for (int row = 1; row < 5; row += 2)
				{
					for (int col = 0; col < 4; col += 2)
					{
						if (r == 0)
							r = new Random().Next();
						if (r % 2 == 1)
							_grid[row, col] = 0;
						r /= 2;
					}
				}
				if (r % 2 == 0)
					_grid[4, 4] = 0;
				for (int row = 0; row < 5; row++)
				{
					for (int col = 0; col < 4; col++)
					{
						if (_grid[row, col] == 0)
						{
							_grid[8 - row, 8 - col] = 0;
							_grid[8 - col, row] = 0;
							_grid[col, 8 - row] = 0;
						}
					}
				}
				gridHints = grid.CalculateHints(_grid);
				_board = ClientRectangle;
			}
			// draw square
			g.DrawRectangle(_penThick, _board);

			int spacingX = _board.Width/9;
			int spacingY = _board.Height/9;

			for (int col = 0; col < 9; col++)
			{
				for (int row = 0; row < 9; row++)
				{
					_boardArray[row, col] = new Rectangle(_board.Left + col*spacingX, _board.Top + row*spacingY, spacingX, spacingY);
				}
			}
			g.FillRectangle(Brushes.Yellow, _boardArray[0, 3]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[0, 4]);
			g.FillRectangle(Brushes.Yellow, _boardArray[0, 5]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[0, 6]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[0, 7]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[1, 1]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[1, 2]);
			g.FillRectangle(Brushes.Yellow, _boardArray[1, 3]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[1, 4]);
			g.FillRectangle(Brushes.Yellow, _boardArray[1, 5]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[1, 6]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[1, 7]);
			g.FillRectangle(Brushes.Yellow, _boardArray[2, 3]);
			g.FillRectangle(Brushes.Yellow, _boardArray[2, 4]);
			g.FillRectangle(Brushes.Yellow, _boardArray[2, 5]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[3, 0]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[3, 1]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[3, 3]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[3, 4]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[3, 5]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[3, 7]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[4, 4]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[4, 5]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[5, 0]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[5, 1]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[5, 2]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[5, 4]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[5, 5]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[5, 6]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[5, 7]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[5, 8]);
			g.FillRectangle(Brushes.Yellow, _boardArray[6, 0]);
			g.FillRectangle(Brushes.Yellow, _boardArray[6, 1]);
			g.FillRectangle(Brushes.Yellow, _boardArray[6, 2]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[6, 4]);
			g.FillRectangle(Brushes.Yellow, _boardArray[6, 6]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[6, 7]);
			g.FillRectangle(Brushes.Yellow, _boardArray[6, 8]);
			g.FillRectangle(Brushes.Yellow, _boardArray[7, 0]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[7, 1]);
			g.FillRectangle(Brushes.Yellow, _boardArray[7, 2]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[7, 5]);
			g.FillRectangle(Brushes.Yellow, _boardArray[7, 6]);
			g.FillRectangle(Brushes.DeepPink, _boardArray[7, 7]);
			g.FillRectangle(Brushes.Yellow, _boardArray[7, 8]);
			g.FillRectangle(Brushes.Yellow, _boardArray[8, 0]);
			g.FillRectangle(Brushes.Yellow, _boardArray[8, 1]);
			g.FillRectangle(Brushes.Yellow, _boardArray[8, 2]);
			g.FillRectangle(Brushes.DarkViolet, _boardArray[8, 4]);
			g.FillRectangle(Brushes.Yellow, _boardArray[8, 6]);
			g.FillRectangle(Brushes.Yellow, _boardArray[8, 7]);
			g.FillRectangle(Brushes.Yellow, _boardArray[8, 8]);

			for (int i = 0; i < 10; i++)
			{
				if (i % 3 == 0)
				{
					g.DrawLine(_penThick, _board.Left, _board.Top + spacingY * i, _board.Right, _board.Top + spacingY*i);
					g.DrawLine(_penThick, _board.Left + spacingX * i, _board.Top, _board.Left + spacingX * i, _board.Bottom);
				}
				else
				{
					g.DrawLine(Pens.Black, _board.Left, _board.Top + spacingY * i, _board.Right, _board.Top + spacingY*i);
					g.DrawLine(Pens.Black, _board.Left + spacingX * i, _board.Top, _board.Left + spacingX * i, _board.Bottom);
				}
			}

			for (int col = 0; col < 9; col++)
			{
				for (int row = 0; row < 9; row++)
				{
					int val = _grid[row, col];

					if (val != 0)
					{
						g.DrawString(val.ToString(), _sudukoFont, Brushes.Black, _board.Left + col*spacingX + 4, _board.Top + row*spacingY + 1, new StringFormat());
					}
					else
					{
						DrawGridHints(g, row, col);
					}
				}				
			}
		}
		#endregion
		#region void DrawGridHints(Graphics g, int row, int col)
		void DrawGridHints(Graphics g, int row, int col)
		{
			int spacingX = _board.Width/9;
			int spacingY = _board.Height/9;
			int length;
			string hints = "";
			string hintss = "";
			string hintsss = "";
			for (int i = 0; i < 9; i++)
			{
				if (gridHints[row, col, i] != 0)
					hints = hints + gridHints[row, col, i].ToString() + ",";
			}

			if (hints.Length > 0)
			{
				hints  = hints.Remove(hints.Length - 1, 1);
			}
			System.Drawing.Brush brush = Brushes.Maroon;
			if (hints.Length == 1)
			{
				brush = Brushes.Blue;
			}
			if (hints.Length > 8)
			{
				length = hints.Length - 8;
				hintss = hints.Remove(0, 8);
				hints = hints.Remove(8, length);
				if (hintss.Length > 8)
				{
					hintsss = hintss.Remove(0, 8);
					hintss = hintss.Remove(8, 1);
					g.DrawString(hintsss.ToString(), _hintsFont, brush, _board.Left + col*spacingX + 1, _board.Top + row*spacingY + 14, new StringFormat());
				}
				g.DrawString(hintss.ToString(), _hintsFont, brush, _board.Left + col*spacingX + 1, _board.Top + row*spacingY + 8, new StringFormat());
			}
			g.DrawString(hints.ToString(), _hintsFont, brush, _board.Left + col*spacingX + 1, _board.Top + row*spacingY + 2, new StringFormat());
		}
		#endregion
		#region private void About_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		private void About_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			String S1 = "by Tony Brix";
			String S2 = "UziTech@gmail.com";
			String S3 = "Special Thanks to";
			String S4 = "Michael Gold";
			DrawSudokuGrid(e.Graphics);
			//e.Graphics.DrawString("Sudoku", new Font("Verdana", 20), Brushes.Black, 28, 10);
			//e.Graphics.DrawString("Sudoku", new Font("Verdana", 20), Brushes.Black, 27, 9);
			//e.Graphics.DrawString("Sudoku", new Font("Verdana", 20), Brushes.Yellow, 26, 8);
			e.Graphics.DrawString(S1, new Font("Verdana", 12), Brushes.Black, _left + 1, _top + 1);
			e.Graphics.DrawString(S1, new Font("Verdana", 12), Brushes.Black, _left + 1, _top - 1);
			e.Graphics.DrawString(S1, new Font("Verdana", 12), Brushes.Black, _left - 1, _top - 1);
			e.Graphics.DrawString(S1, new Font("Verdana", 12), Brushes.Black, _left - 1, _top + 1);
			e.Graphics.DrawString(S1, new Font("Verdana", 12), Brushes.Black, _left, _top - 1);
			e.Graphics.DrawString(S1, new Font("Verdana", 12), Brushes.Black, _left, _top + 1);
			e.Graphics.DrawString(S1, new Font("Verdana", 12), Brushes.Black, _left - 1, _top);
			e.Graphics.DrawString(S1, new Font("Verdana", 12), Brushes.Black, _left + 1, _top);
			e.Graphics.DrawString(S1, new Font("Verdana", 12), Brushes.Aqua, _left, _top);
			e.Graphics.DrawString(S2, new Font("Verdana", 12), Brushes.Black, _left - 29, _top + 20);
			e.Graphics.DrawString(S2, new Font("Verdana", 12), Brushes.Black, _left - 29, _top + 18);
			e.Graphics.DrawString(S2, new Font("Verdana", 12), Brushes.Black, _left - 31, _top + 18);
			e.Graphics.DrawString(S2, new Font("Verdana", 12), Brushes.Black, _left - 31, _top + 20);
			e.Graphics.DrawString(S2, new Font("Verdana", 12), Brushes.Black, _left - 30, _top + 18);
			e.Graphics.DrawString(S2, new Font("Verdana", 12), Brushes.Black, _left - 30, _top + 20);
			e.Graphics.DrawString(S2, new Font("Verdana", 12), Brushes.Black, _left - 31, _top + 19);
			e.Graphics.DrawString(S2, new Font("Verdana", 12), Brushes.Black, _left - 29, _top + 19);
			e.Graphics.DrawString(S2, new Font("Verdana", 12), Brushes.Aqua, _left - 30, _top + 19);
			e.Graphics.DrawString(S3, new Font("Verdana", 12), Brushes.Black, _left - 21, _top + 38);
			e.Graphics.DrawString(S3, new Font("Verdana", 12), Brushes.Black, _left - 21, _top + 36);
			e.Graphics.DrawString(S3, new Font("Verdana", 12), Brushes.Black, _left - 23, _top + 36);
			e.Graphics.DrawString(S3, new Font("Verdana", 12), Brushes.Black, _left - 23, _top + 38);
			e.Graphics.DrawString(S3, new Font("Verdana", 12), Brushes.Black, _left - 22, _top + 36);
			e.Graphics.DrawString(S3, new Font("Verdana", 12), Brushes.Black, _left - 22, _top + 38);
			e.Graphics.DrawString(S3, new Font("Verdana", 12), Brushes.Black, _left - 23, _top + 37);
			e.Graphics.DrawString(S3, new Font("Verdana", 12), Brushes.Black, _left - 21, _top + 37);
			e.Graphics.DrawString(S3, new Font("Verdana", 12), Brushes.Aqua, _left - 22, _top + 37);
			e.Graphics.DrawString(S4, new Font("Verdana", 12), Brushes.Black, _left, _top + 55);
			e.Graphics.DrawString(S4, new Font("Verdana", 12), Brushes.Black, _left, _top + 53);
			e.Graphics.DrawString(S4, new Font("Verdana", 12), Brushes.Black, _left - 2, _top + 53);
			e.Graphics.DrawString(S4, new Font("Verdana", 12), Brushes.Black, _left - 2, _top + 55);
			e.Graphics.DrawString(S4, new Font("Verdana", 12), Brushes.Black, _left - 1, _top + 53);
			e.Graphics.DrawString(S4, new Font("Verdana", 12), Brushes.Black, _left - 1, _top + 55);
			e.Graphics.DrawString(S4, new Font("Verdana", 12), Brushes.Black, _left - 2, _top + 54);
			e.Graphics.DrawString(S4, new Font("Verdana", 12), Brushes.Black, _left, _top + 54);
			e.Graphics.DrawString(S4, new Font("Verdana", 12), Brushes.Aqua, _left - 1, _top + 54);
			count = 1;
		}
		#endregion
	}
}
