using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using Microsoft.Win32;

namespace Sudoku {
	public class Form1 : System.Windows.Forms.Form {
		private IContainer components;
		#region private stuff

		private System.Drawing.Printing.PrintDocument printDocument1;
		private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem mnuRestart;
		private System.Windows.Forms.MenuItem mnuOpen;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.MenuItem mnuSave;
		private System.Windows.Forms.MenuItem mnuNew;
		private System.Windows.Forms.MenuItem mnuSaveTemplate;
		private System.Windows.Forms.MenuItem mnuTemplateMode;
		private System.Windows.Forms.MenuItem mnuCheckSolution;
		private System.Windows.Forms.MenuItem mnuEasy;
		private System.Windows.Forms.MenuItem mnuHard;
		private System.Windows.Forms.MenuItem mnuFile;
		private System.Windows.Forms.MenuItem mnuGenerate;
		private System.Windows.Forms.MenuItem mnuPrint;
		private System.Windows.Forms.MenuItem mnuPrintPreview;
		private System.Windows.Forms.MenuItem mnuSaveImage;
		private System.Windows.Forms.MenuItem mnuExit;
		private System.Windows.Forms.MenuItem mnuUtilities;
		private System.Windows.Forms.MenuItem mnuMakePermanent;
		private System.Windows.Forms.MenuItem mnuDifficult;
		private System.Windows.Forms.MenuItem mnuHelp;
		private System.Windows.Forms.MenuItem mnuAbout;
		private System.Windows.Forms.MenuItem mnuHow;
		private System.Windows.Forms.MenuItem mnuSolve;
		private System.Windows.Forms.Label statusBar1;
		private System.Windows.Forms.MenuItem mnuMedium;
		private System.Windows.Forms.MenuItem mnuClearWrong;
		private System.Timers.Timer timer1;
		private System.Windows.Forms.Label timer;
		private System.Windows.Forms.MenuItem mnuTimer;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem mnuStartTimer;
		private System.Windows.Forms.MenuItem mnuStopTimer;
		private System.Windows.Forms.MenuItem mnuResetTimer;
		private System.Windows.Forms.MenuItem mnuShowHints;
		private System.Windows.Forms.MenuItem mnuEditHints;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem mnuClearHints;
		private System.Windows.Forms.MenuItem mnuAutoCalcHints;
		private System.Windows.Forms.MenuItem menuItem1;
		#endregion
		#region public Form1(string open)
		public Form1(string open) {
			InitializeComponent();
			InitializeBoardArray();
			InitializeGrid();
			Bitmap bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
			Graphics g = Graphics.FromImage(bmp);
			//g.TranslateTransform(-(statusBar1.Height + 1), -(statusBar1.Height + 1));
			// white background
			DrawSudokuBackground(g);
			BackgroundImage = bmp;
			_sudoku.CreateSubKey(@"Software\SudoKu");
			_sudoku = _sudoku.OpenSubKey(@"Software\SudoKu", true);
			if (_sudoku.GetValue("Save", "").ToString() != "" || open != "") {
				statusBar1.Text = "Open...";
				statusBar1.Update();
				if (open == "") {
					openFileDialog1.FileName = _sudoku.GetValue("Save", "").ToString();
				} else {
					openFileDialog1.FileName = open;
				}
				_filename = openFileDialog1.FileName.Remove(openFileDialog1.FileName.LastIndexOf("."), openFileDialog1.FileName.Length - openFileDialog1.FileName.LastIndexOf("."));
				_filename = _filename.Remove(0, _filename.LastIndexOf("\\") + 1);
				mnuTemplateMode.Checked = false;
				_grid = SudokuReader.Reader.Read(openFileDialog1.FileName);
				_checkBoxes = 0;
				if (_grid.ACH()) {
					mnuAutoCalcHints.Checked = true;
					_checkBoxes += 1;
				} else
					mnuAutoCalcHints.Checked = false;
				if (_grid.EH()) {
					mnuEditHints.Checked = true;
					_checkBoxes += 2;
				} else
					mnuEditHints.Checked = false;
				if (_grid.T()) {
					mnuTimer.Checked = true;
					_checkBoxes += 4;
				} else
					mnuTimer.Checked = false;
				if (_grid.TM()) {
					mnuTemplateMode.Checked = true;
					_checkBoxes += 8;
				} else
					mnuTemplateMode.Checked = false;
				if (_grid.E()) {
					mnuEasy.Checked = true;
					_checkBoxes += 16;
				} else
					mnuEasy.Checked = false;
				if (_grid.M()) {
					mnuMedium.Checked = true;
					_checkBoxes += 32;
				} else
					mnuMedium.Checked = false;
				if (_grid.H()) {
					mnuHard.Checked = true;
					_checkBoxes += 64;
				} else
					mnuHard.Checked = false;
				if (mnuAutoCalcHints.Checked)
					_grid.CalculateHints();
				_Tsec = 0;
				_Tmin = 0;
				timer.Text = "0:00";
				statusBar1.Text = "Ready...";
				statusBar1.Update();
			}
			Invalidate();
		}
		#endregion
		#region void InitializeGrid()
		/// <summary>
		/// Declares "_grid" a SudokuGrid();
		/// </summary>
		void InitializeGrid() {
			_grid = new SudokuGrid();
		}
		#endregion
		#region Global Variables
		string _filename = "MyPuzzle";
		int _checkBoxes = 36;
		bool _highlightWrong = false;
		bool _cellLegal = true;
		bool _timing = false;
		int _Tmin = 0;
		int _Tsec = 0;
		ManualResetEvent _blocker = new ManualResetEvent(true);
		Pen _penThick = new Pen(Color.Black, 3);
		Font _sudukoFont = new Font("Arial", 16, FontStyle.Regular);
		Rectangle _board = new Rectangle(0, 0, 0, 0);
		Rectangle[,] _boardArray = new Rectangle[9, 9];
		Font _hintsFont = new Font("Small Fonts", 6);
		SudokuGrid _grid;
		int _selectedRow = -1;
		int _selectedCol = -1;
		Rectangle _selectedRect;
		RegistryKey _sudoku = Registry.CurrentUser;
		#endregion
		#region protected override void Dispose( bool disposing )
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing) {
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		#endregion
		#region private void InitializeComponent()
		/// <summary>
		/// Required method for Designer support
		/// </summary>
		private void InitializeComponent() {
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.printDocument1 = new System.Drawing.Printing.PrintDocument();
			this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.mnuFile = new System.Windows.Forms.MenuItem();
			this.mnuNew = new System.Windows.Forms.MenuItem();
			this.mnuOpen = new System.Windows.Forms.MenuItem();
			this.mnuSave = new System.Windows.Forms.MenuItem();
			this.mnuSaveTemplate = new System.Windows.Forms.MenuItem();
			this.mnuSaveImage = new System.Windows.Forms.MenuItem();
			this.mnuPrint = new System.Windows.Forms.MenuItem();
			this.mnuPrintPreview = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.mnuExit = new System.Windows.Forms.MenuItem();
			this.mnuUtilities = new System.Windows.Forms.MenuItem();
			this.mnuGenerate = new System.Windows.Forms.MenuItem();
			this.mnuSolve = new System.Windows.Forms.MenuItem();
			this.mnuTemplateMode = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.mnuMakePermanent = new System.Windows.Forms.MenuItem();
			this.mnuCheckSolution = new System.Windows.Forms.MenuItem();
			this.mnuRestart = new System.Windows.Forms.MenuItem();
			this.mnuClearWrong = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.mnuStartTimer = new System.Windows.Forms.MenuItem();
			this.mnuStopTimer = new System.Windows.Forms.MenuItem();
			this.mnuResetTimer = new System.Windows.Forms.MenuItem();
			this.mnuTimer = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.mnuClearHints = new System.Windows.Forms.MenuItem();
			this.mnuShowHints = new System.Windows.Forms.MenuItem();
			this.mnuEditHints = new System.Windows.Forms.MenuItem();
			this.mnuAutoCalcHints = new System.Windows.Forms.MenuItem();
			this.mnuDifficult = new System.Windows.Forms.MenuItem();
			this.mnuEasy = new System.Windows.Forms.MenuItem();
			this.mnuMedium = new System.Windows.Forms.MenuItem();
			this.mnuHard = new System.Windows.Forms.MenuItem();
			this.mnuHelp = new System.Windows.Forms.MenuItem();
			this.mnuHow = new System.Windows.Forms.MenuItem();
			this.mnuAbout = new System.Windows.Forms.MenuItem();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.statusBar1 = new System.Windows.Forms.Label();
			this.timer1 = new System.Timers.Timer();
			this.timer = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.timer1)).BeginInit();
			this.SuspendLayout();
			// 
			// printDocument1
			// 
			this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
			// 
			// printPreviewDialog1
			// 
			this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
			this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
			this.printPreviewDialog1.Document = this.printDocument1;
			this.printPreviewDialog1.Enabled = true;
			this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
			this.printPreviewDialog1.Name = "printPreviewDialog1";
			this.printPreviewDialog1.Visible = false;
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.mnuFile,
						this.mnuUtilities,
						this.mnuDifficult,
						this.mnuHelp});
			// 
			// mnuFile
			// 
			this.mnuFile.Index = 0;
			this.mnuFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.mnuNew,
						this.mnuOpen,
						this.mnuSave,
						this.mnuSaveTemplate,
						this.mnuSaveImage,
						this.mnuPrint,
						this.mnuPrintPreview,
						this.menuItem7,
						this.mnuExit});
			this.mnuFile.Text = "File";
			// 
			// mnuNew
			// 
			this.mnuNew.Index = 0;
			this.mnuNew.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.mnuNew.Text = "&New";
			this.mnuNew.Click += new System.EventHandler(this.mnuNew_Click);
			// 
			// mnuOpen
			// 
			this.mnuOpen.Index = 1;
			this.mnuOpen.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.mnuOpen.Text = "&Open";
			this.mnuOpen.Click += new System.EventHandler(this.mnuOpen_Click);
			// 
			// mnuSave
			// 
			this.mnuSave.Index = 2;
			this.mnuSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.mnuSave.Text = "&Save as";
			this.mnuSave.Click += new System.EventHandler(this.mnuSaveAs_Click);
			// 
			// mnuSaveTemplate
			// 
			this.mnuSaveTemplate.Index = 3;
			this.mnuSaveTemplate.Shortcut = System.Windows.Forms.Shortcut.CtrlT;
			this.mnuSaveTemplate.Text = "Save as &Template";
			this.mnuSaveTemplate.Click += new System.EventHandler(this.mnuSaveTemplate_Click);
			// 
			// mnuSaveImage
			// 
			this.mnuSaveImage.Index = 4;
			this.mnuSaveImage.Shortcut = System.Windows.Forms.Shortcut.CtrlI;
			this.mnuSaveImage.Text = "Save as &Image";
			this.mnuSaveImage.Click += new System.EventHandler(this.mnuSaveImage_Click);
			// 
			// mnuPrint
			// 
			this.mnuPrint.Index = 5;
			this.mnuPrint.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
			this.mnuPrint.Text = "&Print";
			this.mnuPrint.Click += new System.EventHandler(this.mnuPrint_Click);
			// 
			// mnuPrintPreview
			// 
			this.mnuPrintPreview.Index = 6;
			this.mnuPrintPreview.Shortcut = System.Windows.Forms.Shortcut.CtrlR;
			this.mnuPrintPreview.Text = "P&rint Preview";
			this.mnuPrintPreview.Click += new System.EventHandler(this.mnuPrintPreview_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 7;
			this.menuItem7.Text = "-";
			// 
			// mnuExit
			// 
			this.mnuExit.Index = 8;
			this.mnuExit.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.mnuExit.Text = "E&xit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// mnuUtilities
			// 
			this.mnuUtilities.Index = 1;
			this.mnuUtilities.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.mnuGenerate,
						this.mnuSolve,
						this.mnuTemplateMode,
						this.menuItem3,
						this.mnuMakePermanent,
						this.mnuCheckSolution,
						this.mnuRestart,
						this.mnuClearWrong,
						this.menuItem1,
						this.mnuStartTimer,
						this.mnuStopTimer,
						this.mnuResetTimer,
						this.mnuTimer,
						this.menuItem5,
						this.mnuClearHints,
						this.mnuShowHints,
						this.mnuEditHints,
						this.mnuAutoCalcHints});
			this.mnuUtilities.Text = "Utilities";
			// 
			// mnuGenerate
			// 
			this.mnuGenerate.Index = 0;
			this.mnuGenerate.Shortcut = System.Windows.Forms.Shortcut.CtrlG;
			this.mnuGenerate.Text = "&Generate";
			this.mnuGenerate.Click += new System.EventHandler(this.mnuGenerate_Click);
			// 
			// mnuSolve
			// 
			this.mnuSolve.Index = 1;
			this.mnuSolve.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.mnuSolve.Text = "Sol&ve";
			this.mnuSolve.Click += new System.EventHandler(this.mnuSolve_Click);
			// 
			// mnuTemplateMode
			// 
			this.mnuTemplateMode.Index = 2;
			this.mnuTemplateMode.Shortcut = System.Windows.Forms.Shortcut.CtrlM;
			this.mnuTemplateMode.Text = "Template &Mode";
			this.mnuTemplateMode.Click += new System.EventHandler(this.mnuTemplateMode_Click);
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 3;
			this.menuItem3.Text = "-";
			// 
			// mnuMakePermanent
			// 
			this.mnuMakePermanent.Index = 4;
			this.mnuMakePermanent.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftP;
			this.mnuMakePermanent.Text = "Make Guesses &Permanent";
			this.mnuMakePermanent.Click += new System.EventHandler(this.mnuMakePermanent_Click);
			// 
			// mnuCheckSolution
			// 
			this.mnuCheckSolution.Index = 5;
			this.mnuCheckSolution.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.mnuCheckSolution.Text = "&Check Guesses";
			this.mnuCheckSolution.Click += new System.EventHandler(this.mnuCheckGuesses_Click);
			// 
			// mnuRestart
			// 
			this.mnuRestart.Index = 6;
			this.mnuRestart.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.mnuRestart.Text = "Clear &All Guesses";
			this.mnuRestart.Click += new System.EventHandler(this.mnuClearGuesses_Click);
			// 
			// mnuClearWrong
			// 
			this.mnuClearWrong.Index = 7;
			this.mnuClearWrong.Shortcut = System.Windows.Forms.Shortcut.CtrlW;
			this.mnuClearWrong.Text = "Clear &Wrong Guesses";
			this.mnuClearWrong.Click += new System.EventHandler(this.mnuClearWrong_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 8;
			this.menuItem1.Text = "-";
			// 
			// mnuStartTimer
			// 
			this.mnuStartTimer.Index = 9;
			this.mnuStartTimer.Text = "Start Timer";
			this.mnuStartTimer.Click += new System.EventHandler(this.mnuStartTimer_Click);
			// 
			// mnuStopTimer
			// 
			this.mnuStopTimer.Index = 10;
			this.mnuStopTimer.Text = "Stop Timer";
			this.mnuStopTimer.Click += new System.EventHandler(this.mnuStopTimer_Click);
			// 
			// mnuResetTimer
			// 
			this.mnuResetTimer.Index = 11;
			this.mnuResetTimer.Text = "Reset Timer";
			this.mnuResetTimer.Click += new System.EventHandler(this.mnuResetTimer_Click);
			// 
			// mnuTimer
			// 
			this.mnuTimer.Checked = true;
			this.mnuTimer.Index = 12;
			this.mnuTimer.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftT;
			this.mnuTimer.Text = "&Timer";
			this.mnuTimer.Click += new System.EventHandler(this.mnuTimer_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 13;
			this.menuItem5.Text = "-";
			// 
			// mnuClearHints
			// 
			this.mnuClearHints.Index = 14;
			this.mnuClearHints.Text = "Clear Hints";
			this.mnuClearHints.Click += new System.EventHandler(this.mnuClearHints_Click);
			// 
			// mnuShowHints
			// 
			this.mnuShowHints.Index = 15;
			this.mnuShowHints.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
			this.mnuShowHints.Text = "Show All &Hints";
			this.mnuShowHints.Click += new System.EventHandler(this.mnuShowHints_Click);
			// 
			// mnuEditHints
			// 
			this.mnuEditHints.Index = 16;
			this.mnuEditHints.Shortcut = System.Windows.Forms.Shortcut.CtrlE;
			this.mnuEditHints.Text = "&Edit Hints";
			this.mnuEditHints.Click += new System.EventHandler(this.mnuEditHints_Click);
			// 
			// mnuAutoCalcHints
			// 
			this.mnuAutoCalcHints.Index = 17;
			this.mnuAutoCalcHints.Text = "Auto Calc Hints";
			this.mnuAutoCalcHints.Click += new System.EventHandler(this.mnuAutoCalcHints_Click);
			// 
			// mnuDifficult
			// 
			this.mnuDifficult.Index = 2;
			this.mnuDifficult.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.mnuEasy,
						this.mnuMedium,
						this.mnuHard});
			this.mnuDifficult.Text = "Difficulty";
			// 
			// mnuEasy
			// 
			this.mnuEasy.Index = 0;
			this.mnuEasy.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.mnuEasy.Text = "Easy";
			this.mnuEasy.Click += new System.EventHandler(this.mnuEasy_Click);
			// 
			// mnuMedium
			// 
			this.mnuMedium.Checked = true;
			this.mnuMedium.Index = 1;
			this.mnuMedium.Shortcut = System.Windows.Forms.Shortcut.F6;
			this.mnuMedium.Text = "Medium";
			this.mnuMedium.Click += new System.EventHandler(this.mnuMedium_Click);
			// 
			// mnuHard
			// 
			this.mnuHard.Index = 2;
			this.mnuHard.Shortcut = System.Windows.Forms.Shortcut.F7;
			this.mnuHard.Text = "Hard";
			this.mnuHard.Click += new System.EventHandler(this.mnuHard_Click);
			// 
			// mnuHelp
			// 
			this.mnuHelp.Index = 3;
			this.mnuHelp.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
						this.mnuHow,
						this.mnuAbout});
			this.mnuHelp.Text = "Help";
			// 
			// mnuHow
			// 
			this.mnuHow.Index = 0;
			this.mnuHow.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.mnuHow.Text = "How To Play";
			this.mnuHow.Click += new System.EventHandler(this.mnuHow_Click);
			// 
			// mnuAbout
			// 
			this.mnuAbout.Index = 1;
			this.mnuAbout.Shortcut = System.Windows.Forms.Shortcut.F2;
			this.mnuAbout.Text = "About";
			this.mnuAbout.Click += new System.EventHandler(this.mnuAbout_Click);
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.DefaultExt = "jpg";
			this.saveFileDialog1.Filter = "JPeg Files | *.jpg ||";
			// 
			// statusBar1
			// 
			this.statusBar1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.statusBar1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
			this.statusBar1.Location = new System.Drawing.Point(8, 304);
			this.statusBar1.Name = "statusBar1";
			this.statusBar1.Size = new System.Drawing.Size(312, 22);
			this.statusBar1.TabIndex = 0;
			this.statusBar1.Text = "Ready...";
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 1000;
			this.timer1.SynchronizingObject = this;
			this.timer1.Elapsed += new System.Timers.ElapsedEventHandler(this.timer1_Elapsed);
			// 
			// timer
			// 
			this.timer.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.timer.ForeColor = System.Drawing.Color.White;
			this.timer.Location = new System.Drawing.Point(256, 304);
			this.timer.Name = "timer";
			this.timer.Size = new System.Drawing.Size(56, 16);
			this.timer.TabIndex = 1;
			this.timer.Text = "0:00";
			this.timer.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(318, 327);
			this.Controls.Add(this.timer);
			this.Controls.Add(this.statusBar1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "Sudoku Puzzle";
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
			((System.ComponentModel.ISupportInitialize)(this.timer1)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion
		#region static void Main(string[] args)
		[STAThread]
		static void Main(string[] args) {
			string openFile;
			if (args.Length > 0) {
				openFile = args[0];
			} else {
				openFile = "";
			}
			try {
				Application.Run(new Form1(openFile));
			} catch (Exception ex) {
				System.Windows.Forms.MessageBox.Show("Error -->\n" + ex.ToString());
			}
		}
		#endregion
		#region void InitializeBoardArray()
		void InitializeBoardArray() {
			_board = ClientRectangle;
			_board.Inflate(-statusBar1.Height - 2, -statusBar1.Height - 2);
			int spacingX = _board.Width / 9;
			int spacingY = _board.Height / 9;
			for (int col = 0; col < 9; col++) {
				for (int row = 0; row < 9; row++) {
					_boardArray[row, col] = new Rectangle(_board.Left + col * spacingX, _board.Top + row * spacingY, spacingX, spacingY);
				}
			}

		}
		#endregion
		#region void DrawSudokuBackground(Graphics g)
		void DrawSudokuBackground(Graphics g) {
			_board = ClientRectangle;
			_board.Inflate(-statusBar1.Height - 2, -statusBar1.Height - 2);
			g.DrawRectangle(_penThick, _board);

			int spacingX = _board.Width / 9;
			int spacingY = _board.Height / 9;

			for (int r = 0; r < 9; r++) {
				for (int c = 0; c < 9; c++) {
					if (r < 3) {
						if (c < 3)
							g.FillRectangle(Brushes.DeepSkyBlue, _boardArray[r, c]);
						else if (c < 6)
							g.FillRectangle(Brushes.LightGreen, _boardArray[r, c]);
						else
							g.FillRectangle(Brushes.DeepSkyBlue, _boardArray[r, c]);
					} else if (r < 6) {
						if (c < 3)
							g.FillRectangle(Brushes.LightGreen, _boardArray[r, c]);
						else if (c < 6)
							g.FillRectangle(Brushes.DeepSkyBlue, _boardArray[r, c]);
						else
							g.FillRectangle(Brushes.LightGreen, _boardArray[r, c]);
					} else {
						if (c < 3)
							g.FillRectangle(Brushes.DeepSkyBlue, _boardArray[r, c]);
						else if (c < 6)
							g.FillRectangle(Brushes.LightGreen, _boardArray[r, c]);
						else
							g.FillRectangle(Brushes.DeepSkyBlue, _boardArray[r, c]);
					}
				}
			}
		}
		#endregion
		#region void DrawSudokuGrid(Graphics g)
		/// <summary>
		/// Draw background colors and _grid numbers.
		/// </summary>
		void DrawSudokuGrid(Graphics g) {
			int spacingX = _board.Width / 9;
			int spacingY = _board.Height / 9;
			if (_highlightWrong) {
				for (int r = 0; r < 9; r++) {
					for (int c = 0; c < 9; c++) {
						if (!_grid.CheckCell(r, c))
							if (_grid.IsKnownElement(r, c) == mnuTemplateMode.Checked)
								g.FillRectangle(Brushes.Yellow, _boardArray[r, c]);
					}
				}
			} else if (_selectedRow >= 0 && _selectedCol >= 0) {
				if (_cellLegal) {
					g.FillRectangle(Brushes.White, _boardArray[_selectedRow, _selectedCol]);
				} else {
					g.FillRectangle(Brushes.Yellow, _boardArray[_selectedRow, _selectedCol]);
				}
			}
			for (int i = 0; i < 10; i++) {
				if (i % 3 == 0) {
					g.DrawLine(_penThick, _board.Left, _board.Top + spacingY * i, _board.Right, _board.Top + spacingY * i);
					g.DrawLine(_penThick, _board.Left + spacingX * i, _board.Top, _board.Left + spacingX * i, _board.Bottom);
				} else {
					g.DrawLine(Pens.Black, _board.Left, _board.Top + spacingY * i, _board.Right, _board.Top + spacingY * i);
					g.DrawLine(Pens.Black, _board.Left + spacingX * i, _board.Top, _board.Left + spacingX * i, _board.Bottom);
				}
			}

			if (_grid.CheckEmpty())
				return;


			for (int col = 0; col < 9; col++) {
				for (int row = 0; row < 9; row++) {
					int val = _grid[row, col];

					if (val != 0) {
						if (_grid.IsKnownElement(row, col) || mnuTemplateMode.Checked) {
							g.DrawString(val.ToString(), _sudukoFont, Brushes.Black, _board.Left + col * spacingX + 5, _board.Top + row * spacingY + 5, new StringFormat());
						} else {
							g.DrawString(val.ToString(), _sudukoFont, Brushes.Red, _board.Left + col * spacingX + 5, _board.Top + row * spacingY + 5, new StringFormat());
						}

					} else {
						DrawGridHints(g, row, col);
					}

				}

			}
		}
		#endregion
		#region void DrawGridHints(Graphics g, int row, int col)
		/// <summary>
		/// Draw _grid hints.
		/// </summary>
		void DrawGridHints(Graphics g, int row, int col) {
			int spacingX = _board.Width / 9;
			int spacingY = _board.Height / 9;
			int length;
			string hints = "";
			string hintss = "";
			string hintsss = "";
			for (int i = 0; i < 9; i++) {
				if (_grid.GridHints[row, col, i] != 0)
					hints = hints + _grid.GridHints[row, col, i].ToString() + ",";
			}

			if (hints.Length > 0) {
				hints = hints.Remove(hints.Length - 1, 1);
				System.Drawing.Brush brush = Brushes.Maroon;
				if (mnuEasy.Checked)//
				{
					int tempcol;
					int temprow;
					int tempcell;
					int crow = row / 3 * 3;
					int ccol = col / 3 * 3;
					for (int i = 0; i < 9; i++) {
						if (brush != Brushes.Blue && _grid.GridHints[row, col, i] != 0) {
							temprow = 0;
							tempcol = 0;
							tempcell = 0;
							for (int j = 0; j < 9; j++) {
								if (_grid.GridHints[row, j, i] != 0)
									tempcol++;
								if (_grid.GridHints[j, col, i] != 0)
									temprow++;
							}
							for (int k = 0; k < 3; k++) {
								for (int l = 0; l < 3; l++) {
									if (_grid.GridHints[crow + k, ccol + l, i] != 0)
										tempcell++;
								}
							}
							if (tempcol == 1 || temprow == 1 || tempcell == 1 || hints.Length == 1) {
								hints = (i + 1).ToString();
								brush = Brushes.Blue;
							}
						}
					}
				}//
				if (hints.Length == 1) {
					brush = Brushes.Blue;
				} else if (hints.Length > 8) {
					length = hints.Length - 8;
					hintss = hints.Remove(0, 8);
					hints = hints.Remove(8, length);
					if (hintss.Length > 8) {
						hintsss = hintss.Remove(0, 8);
						hintss = hintss.Remove(8, 1);
						g.DrawString(hintsss.ToString(), _hintsFont, brush, _board.Left + col * spacingX + 2, _board.Top + row * spacingY + 20, new StringFormat());
					}
					g.DrawString(hintss.ToString(), _hintsFont, brush, _board.Left + col * spacingX + 2, _board.Top + row * spacingY + 11, new StringFormat());
				}
				g.DrawString(hints.ToString(), _hintsFont, brush, _board.Left + col * spacingX + 2, _board.Top + row * spacingY + 2, new StringFormat());
			}
		}
		#endregion
		#region private Rectangle TranslateToRectBounds(Point p, out int selectedRow, out int selectedCol)
		private Rectangle TranslateToRectBounds(Point p, out int selectedRow, out int selectedCol) {
			int spacingX = _board.Width / 9;
			int spacingY = _board.Height / 9;
			for (int col = 0; col < 9; col++) {
				for (int row = 0; row < 9; row++) {
					if (_boardArray[row, col].Contains(p)) {
						selectedRow = row;
						selectedCol = col;
						return _boardArray[row, col];
					}
				}
			}

			selectedRow = -1;
			selectedCol = -1;
			return new Rectangle(0, 0, 0, 0);
		}
		#endregion
		#region private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e) {
			DrawSudokuGrid(e.Graphics);
		}
		#endregion
		#region private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		private void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			DrawSudokuGrid(e.Graphics);
		}
		#endregion
		#region private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		private void Form1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
			if (_highlightWrong) {
				_highlightWrong = false;
				Invalidate();
			} else {
				Invalidate(_selectedRect);
			}
			_selectedRect = TranslateToRectBounds(new Point(e.X, e.Y), out _selectedRow, out _selectedCol);
			if (_selectedRow != -1) {
				_cellLegal = _grid.CheckCell(_selectedRow, _selectedCol);

				if (_selectedRow >= 0) {
					Invalidate(_selectedRect);
				}
			}

		}
		#endregion
		#region private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		private void Form1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (_selectedRow >= 0 && _selectedCol >= 0) {
				if (_highlightWrong) {
					_highlightWrong = false;
					Invalidate();
				} else {
					Invalidate(_selectedRect);
				}
				//System.Windows.Forms.MessageBox.Show(e.KeyValue.ToString());
				if (e.KeyValue == 39) {
					_selectedCol++;
					if (_selectedCol == 9)
						_selectedCol = 0;
				} else if (e.KeyValue == 37) {
					_selectedCol--;
					if (_selectedCol == -1)
						_selectedCol = 8;
				} else if (e.KeyValue == 40) {
					_selectedRow++;
					if (_selectedRow == 9)
						_selectedRow = 0;
				} else if (e.KeyValue == 38) {
					_selectedRow--;
					if (_selectedRow == -1)
						_selectedRow = 8;
				} else if (e.KeyValue == 46) {
					_grid[_selectedRow, _selectedCol] = 0;
				}
				_cellLegal = _grid.CheckCell(_selectedRow, _selectedCol);
				_selectedRect = _boardArray[_selectedRow, _selectedCol];
				Invalidate(_selectedRect);
			}
		}
		#endregion
		#region private void Form1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		private void Form1_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e) {
			if (_highlightWrong) {
				_highlightWrong = false;
				Invalidate();
			}
			if (_grid == null)
				return;
			else if ((e.KeyChar >= '1' && e.KeyChar <= '9') || e.KeyChar == ' ') {
				if (_selectedRow >= 0 && _selectedCol >= 0) {
					if (mnuEditHints.Checked) {
						if (_grid[_selectedRow, _selectedCol] == 0) {
							if (e.KeyChar == ' ') {
								_grid.ClearRCHints(_selectedRow, _selectedCol);
							} else {
								_grid.ChangeHints(_selectedRow, _selectedCol, Convert.ToInt32(e.KeyChar.ToString()));
							}
							if (mnuAutoCalcHints.Checked) {
								Invalidate();
							} else {
								Invalidate(_selectedRect);
							}
						}
						return;
					}

					if (e.KeyChar == ' ') {
						if (!_grid.IsKnownElement(_selectedRow, _selectedCol) || mnuTemplateMode.Checked)
							_grid[_selectedRow, _selectedCol] = 0;
					} else {
						if (!_grid.IsKnownElement(_selectedRow, _selectedCol) || mnuTemplateMode.Checked)
							_grid[_selectedRow, _selectedCol] = Convert.ToInt32(e.KeyChar.ToString());
					}
					if (mnuTemplateMode.Checked) {
						_grid.SetKnownElement(_selectedRow, _selectedCol, _grid[_selectedRow, _selectedCol]);
					}
					if (mnuAutoCalcHints.Checked) {
						_grid.CalculateHints();
					}
					_cellLegal = _grid.CheckCell(_selectedRow, _selectedCol);
					if (mnuAutoCalcHints.Checked) {
						Invalidate();
					} else {
						Invalidate(_selectedRect);
					}
					if (_grid.GridCompleted())
						if (_grid.CheckAll()) {
							if (_timing) {
								_timing = false;
								MessageBox.Show("You solved the sudoku puzzle in " + (_Tmin != 0 ? _Tmin.ToString() + " minute" + (_Tmin == 1 ? "" : "s") : "") + (_Tmin != 0 && _Tsec != 0 ? " and " : "") + (_Tsec != 0 ? _Tsec.ToString() + " second" + (_Tsec == 1 ? "" : "s") : "") + "!", "Congratulations!!!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
							} else
								MessageBox.Show("You Solved the Sudoku Puzzle!", "Congratulations!!!", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Asterisk);
						}
				}
			}
		}
		#endregion
		#region private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if (!_grid.CheckEmpty() && MessageBox.Show("Do you want to save this game?", "Save?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
				statusBar1.Text = "Save as...";
				statusBar1.Update();
				saveFileDialog1.FileName = _filename;
				saveFileDialog1.DefaultExt = "sav";
				saveFileDialog1.Filter = "Sudoku Puzzle|*.sav";
				saveFileDialog1.AddExtension = true;
				if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
					_sudoku.SetValue("Save", saveFileDialog1.FileName);
					SudokuReader.Reader.Save(saveFileDialog1.FileName, _grid, _checkBoxes);
				} else {
					_sudoku.SetValue("Save", "");
				}
				statusBar1.Text = "Ready...";
				statusBar1.Update();
			} else {
				_sudoku.SetValue("Save", "");
			}
			_sudoku.Close();
		}
		#endregion
		#region private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
			if (timer.Visible && _timing) {
				_Tsec++;
				if (_Tsec == 60) {
					_Tmin++;
					_Tsec = 0;
				}
				timer.Text = _Tmin + ":" + (_Tsec < 10 ? "0" : "") + _Tsec;
			}
		}
		#endregion
		#region private void mnuNew_Click(object sender, System.EventArgs e)
		private void mnuNew_Click(object sender, System.EventArgs e) {
			if (!_grid.CheckEmpty()) {
				_grid.Clear();
				_grid.ClearHints();
				Invalidate();
			}
			_Tsec = 0;
			_Tmin = 0;
			timer.Text = "0:00";
			_timing = false;
		}
		#endregion
		#region private void mnuGenerate_Click(object sender, System.EventArgs e)
		private void mnuGenerate_Click(object sender, System.EventArgs e) {
			Invalidate();
			statusBar1.Text = "Generating...";
			statusBar1.Update();
			mnuTemplateMode.Checked = false;
			_grid.Generate();
			_grid.ClearHints();
			Invalidate();
			statusBar1.Text = "Ready...";
			statusBar1.Update();
			if (mnuTimer.Checked) {
				_Tsec = 0;
				_Tmin = 0;
				timer.Text = "0:00";
				_timing = true;
			}
		}
		#endregion
		#region private void mnuOpen_Click(object sender, System.EventArgs e)
		private void mnuOpen_Click(object sender, System.EventArgs e) {
			statusBar1.Text = "Open...";
			statusBar1.Update();
			openFileDialog1.FileName = _filename;
			openFileDialog1.DefaultExt = "sav";
			openFileDialog1.Filter = "Sudoku Puzzle|*.sav|Sudoku Template|*.xml|All Files|*";
			if (openFileDialog1.ShowDialog() == DialogResult.OK) {
				_filename = openFileDialog1.FileName.Remove(openFileDialog1.FileName.LastIndexOf("."), openFileDialog1.FileName.Length - openFileDialog1.FileName.LastIndexOf("."));
				_filename = _filename.Remove(0, _filename.LastIndexOf("\\") + 1);
				mnuTemplateMode.Checked = false;
				_grid = SudokuReader.Reader.Read(openFileDialog1.FileName);
				_checkBoxes = 0;
				if (_grid.ACH()) {
					mnuAutoCalcHints.Checked = true;
					_checkBoxes += 1;
				} else
					mnuAutoCalcHints.Checked = false;
				if (_grid.EH()) {
					mnuEditHints.Checked = true;
					_checkBoxes += 2;
				} else
					mnuEditHints.Checked = false;
				if (_grid.T()) {
					mnuTimer.Checked = true;
					_checkBoxes += 4;
				} else
					mnuTimer.Checked = false;
				if (_grid.TM()) {
					mnuTemplateMode.Checked = true;
					_checkBoxes += 8;
				} else
					mnuTemplateMode.Checked = false;
				if (_grid.E()) {
					mnuEasy.Checked = true;
					_checkBoxes += 16;
				} else
					mnuEasy.Checked = false;
				if (_grid.M()) {
					mnuMedium.Checked = true;
					_checkBoxes += 32;
				} else
					mnuMedium.Checked = false;
				if (_grid.H()) {
					mnuHard.Checked = true;
					_checkBoxes += 64;
				} else
					mnuHard.Checked = false;
				if (mnuAutoCalcHints.Checked)
					_grid.CalculateHints();
				Invalidate();
				_Tsec = 0;
				_Tmin = 0;
				timer.Text = "0:00";
			}
			statusBar1.Text = "Ready...";
			statusBar1.Update();
		}
		#endregion
		#region private void mnuSaveAs_Click(object sender, System.EventArgs e)
		private void mnuSaveAs_Click(object sender, System.EventArgs e) {
			statusBar1.Text = "Save as...";
			statusBar1.Update();
			saveFileDialog1.FileName = _filename;
			saveFileDialog1.DefaultExt = "sav";
			saveFileDialog1.Filter = "Sudoku Puzzle|*.sav";
			saveFileDialog1.AddExtension = true;
			if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
				SudokuReader.Reader.Save(saveFileDialog1.FileName, _grid, _checkBoxes);
			}
			statusBar1.Text = "Ready...";
			statusBar1.Update();
		}
		#endregion
		#region private void mnuSaveTemplate_Click(object sender, System.EventArgs e)
		private void mnuSaveTemplate_Click(object sender, System.EventArgs e) {
			statusBar1.Text = "Save as Template...";
			statusBar1.Update();
			saveFileDialog1.FileName = _filename;
			saveFileDialog1.DefaultExt = "xml";
			saveFileDialog1.Filter = "Sudoku Puzzle Template|*.xml";
			saveFileDialog1.AddExtension = true;
			if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
				SudokuReader.Reader.SaveTemplate(saveFileDialog1.FileName, _grid);
			}
			statusBar1.Text = "Ready...";
			statusBar1.Update();
		}
		#endregion
		#region private void mnuSaveImage_Click(object sender, System.EventArgs e)
		private void mnuSaveImage_Click(object sender, System.EventArgs e) {
			statusBar1.Text = "Save as Image...";
			statusBar1.Update();
			_blocker.Reset();
			saveFileDialog1.FileName = "MyPuzzle.jpg";
			saveFileDialog1.DefaultExt = "jpg";
			saveFileDialog1.Filter = "JPEG File|*.jpg";
			saveFileDialog1.AddExtension = true;
			if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
				Bitmap bmp = new Bitmap(_board.Width + 3, _board.Height + 3);
				Graphics g = Graphics.FromImage(bmp);
				g.TranslateTransform(-(statusBar1.Height + 1), -(statusBar1.Height + 1));
				// white background
				DrawSudokuGrid(g);
				bmp.Save(saveFileDialog1.FileName, ImageFormat.Jpeg);
			}
			_blocker.Set();
			statusBar1.Text = "Ready...";
			statusBar1.Update();
		}
		#endregion
		#region private void mnuPrint_Click(object sender, System.EventArgs e)
		private void mnuPrint_Click(object sender, System.EventArgs e) {
			printDocument1.Print();
		}
		#endregion
		#region private void mnuPrintPreview_Click(object sender, System.EventArgs e)
		private void mnuPrintPreview_Click(object sender, System.EventArgs e) {
			statusBar1.Text = "Print Preview...";
			statusBar1.Update();
			_blocker.Reset();
			printPreviewDialog1.ShowDialog();
			_blocker.Set();
			statusBar1.Text = "Ready...";
			statusBar1.Update();
		}
		#endregion
		#region private void mnuExit_Click(object sender, System.EventArgs e)
		private void mnuExit_Click(object sender, System.EventArgs e) {
			Application.Exit();
		}
		#endregion
		#region private void mnuClearGuesses_Click(object sender, System.EventArgs e)
		private void mnuClearGuesses_Click(object sender, System.EventArgs e) {
			for (int row = 0; row < 9; row++)
				for (int col = 0; col < 9; col++)
					if (!_grid.IsKnownElement(row, col))
						_grid[row, col] = 0;
			_grid.CalculateHints();
			Invalidate();
		}
		#endregion
		#region private void mnuClearWrong_Click(object sender, System.EventArgs e)
		private void mnuClearWrong_Click(object sender, System.EventArgs e) {
			if (!_grid.CheckEmpty()) {
				_grid.ClearWrong();
				Invalidate();
			}
		}
		#endregion
		#region private void mnuSolve_Click(object sender, System.EventArgs e)
		private void mnuSolve_Click(object sender, System.EventArgs e) {
			if (!_grid.CheckEmpty()) {
				_Tsec = 0;
				_Tmin = 0;
				timer.Text = "0:00";
				_timing = false;
				mnuTemplateMode.Checked = false;
				_grid.Solve(false, true, false);
				_grid.twoSlns(false);
				_grid.twoSlnsShow(false);
				for (int row = 0; row < 9; row++)
					for (int col = 0; col < 9; col++)
						_grid[row, col] = _grid.GetSolvedGrid(row, col);
				_grid.CalculateHints();
				Invalidate();
				Invalidate(_selectedRect);
			}
		}
		#endregion
		#region private void mnuMakePermanent_Click(object sender, System.EventArgs e)
		private void mnuMakePermanent_Click(object sender, System.EventArgs e) {
			for (int z = 0; z < 9; z++)
				for (int y = 0; y < 9; y++)
					_grid.SetKnownElement(z, y, _grid[z, y]);
			Invalidate();
		}
		#endregion
		#region private void mnuTimer_Click(object sender, System.EventArgs e)
		private void mnuTimer_Click(object sender, System.EventArgs e) {
			timer.Text = "0:00";
			_Tmin = 0;
			_Tsec = 0;
			_timing = false;
			if (mnuTimer.Checked) {
				mnuTimer.Checked = false;
				_checkBoxes -= 4;
			} else {
				mnuTimer.Checked = true;
				_checkBoxes += 4;
			}
			timer.Visible = mnuTimer.Checked;
		}
		#endregion
		#region private void mnuTemplateMode_Click(object sender, System.EventArgs e)
		private void mnuTemplateMode_Click(object sender, System.EventArgs e) {
			if (mnuTemplateMode.Checked) {
				mnuTemplateMode.Checked = false;
				_checkBoxes -= 8;
			} else {
				mnuTemplateMode.Checked = true;
				_checkBoxes += 8;
				if (mnuEditHints.Checked) {
					mnuEditHints.Checked = false;
					_checkBoxes -= 2;
				}
			}

			Invalidate();
		}
		#endregion
		#region private void mnuCheckGuesses_Click(object sender, System.EventArgs e)
		private void mnuCheckGuesses_Click(object sender, System.EventArgs e) {
			if (_grid.CheckAll()) {
				MessageBox.Show("Correct!", "Correct", System.Windows.Forms.MessageBoxButtons.OK);
			} else {
				Invalidate(_selectedRect);
				_highlightWrong = true;
				Invalidate();
			}
		}
		#endregion
		#region private void mnuEasy_Click(object sender, System.EventArgs e)
		private void mnuEasy_Click(object sender, System.EventArgs e) {
			mnuEasy.Checked = true;
			_checkBoxes += 16;
			if (mnuMedium.Checked) {
				mnuMedium.Checked = false;
				_checkBoxes -= 32;
			}
			if (mnuHard.Checked) {
				mnuHard.Checked = false;
				_checkBoxes -= 64;
			}
			_grid.hard(false);
			_grid.easy(true);
			Invalidate();
		}
		#endregion
		#region private void mnuMedium_Click(object sender, System.EventArgs e)
		private void mnuMedium_Click(object sender, System.EventArgs e) {
			mnuMedium.Checked = true;
			_checkBoxes += 32;
			if (mnuEasy.Checked) {
				mnuEasy.Checked = false;
				_checkBoxes -= 16;
			}
			if (mnuHard.Checked) {
				mnuHard.Checked = false;
				_checkBoxes -= 64;
			}
			_grid.hard(false);
			_grid.easy(false);
			Invalidate();
		}
		#endregion
		#region private void mnuHard_Click(object sender, System.EventArgs e)
		private void mnuHard_Click(object sender, System.EventArgs e) {
			mnuHard.Checked = true;
			_checkBoxes += 64;
			if (mnuMedium.Checked) {
				mnuMedium.Checked = false;
				_checkBoxes -= 32;
			}
			if (mnuEasy.Checked) {
				mnuEasy.Checked = false;
				_checkBoxes -= 16;
			}
			_grid.hard(true);
			_grid.easy(false);
			Invalidate();
		}
		#endregion
		#region private void mnuHow_Click(object sender, System.EventArgs e)
		private void mnuHow_Click(object sender, System.EventArgs e) {
			HowToPlay HowToPlayDialog = new HowToPlay();
			HowToPlayDialog.ShowDialog();
		}
		#endregion
		#region private void mnuAbout_Click(object sender, System.EventArgs e)
		private void mnuAbout_Click(object sender, System.EventArgs e) {
			About aboutDialog = new About();
			aboutDialog.ShowDialog();
		}
		#endregion
		#region private void mnuStartTimer_Click(object sender, System.EventArgs e)
		private void mnuStartTimer_Click(object sender, System.EventArgs e) {
			_timing = true;
		}
		#endregion
		#region private void mnuStopTimer_Click(object sender, System.EventArgs e)
		private void mnuStopTimer_Click(object sender, System.EventArgs e) {
			_timing = false;
		}
		#endregion
		#region private void mnuResetTimer_Click(object sender, System.EventArgs e)
		private void mnuResetTimer_Click(object sender, System.EventArgs e) {
			_Tsec = 0;
			_Tmin = 0;
			timer.Text = "0:00";
			_timing = false;
		}
		#endregion
		#region private void mnuShowHints_Click(object sender, System.EventArgs e)
		private void mnuShowHints_Click(object sender, System.EventArgs e) {
			_grid.CalculateHints();
			Invalidate();
		}
		#endregion
		#region private void mnuEditHints_Click(object sender, System.EventArgs e)
		private void mnuEditHints_Click(object sender, System.EventArgs e) {
			if (mnuEditHints.Checked) {
				mnuEditHints.Checked = false;
				_checkBoxes -= 2;
			} else {
				mnuEditHints.Checked = true;
				_checkBoxes += 2;
				if (mnuTemplateMode.Checked) {
					mnuTemplateMode.Checked = false;
					_checkBoxes -= 8;
				}
			}
		}
		#endregion
		#region private void mnuClearHints_Click(object sender, System.EventArgs e)
		private void mnuClearHints_Click(object sender, System.EventArgs e) {
			_grid.ClearHints();
			Invalidate();
		}
		#endregion
		#region private void mnuAutoCalcHints_Click(object sender, System.EventArgs e)
		private void mnuAutoCalcHints_Click(object sender, System.EventArgs e) {
			if (mnuAutoCalcHints.Checked) {
				mnuAutoCalcHints.Checked = false;
				_checkBoxes--;
			} else {
				mnuAutoCalcHints.Checked = true;
				_checkBoxes++;
				_grid.CalculateHints();
				Invalidate();
			}
		}
		#endregion
	}
}
