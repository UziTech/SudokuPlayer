using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace Sudoku {
	/// <summary>
	/// Summary description for HowToPlay.
	/// </summary>
	public class HowToPlay : System.Windows.Forms.Form {
		#region private stuff
		private System.Windows.Forms.Label HowTo;
		private System.Windows.Forms.Label KeysHowTo;
		private System.Windows.Forms.Label HLColorHowTo;
		private System.Windows.Forms.Label HighLightColorHowTo;
		private System.Windows.Forms.Button OkBtn;
		private System.Windows.Forms.Label label1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion
		#region public HowToPlay()
		public HowToPlay() {
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
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
		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HowToPlay));
			this.HowTo = new System.Windows.Forms.Label();
			this.KeysHowTo = new System.Windows.Forms.Label();
			this.HLColorHowTo = new System.Windows.Forms.Label();
			this.HighLightColorHowTo = new System.Windows.Forms.Label();
			this.OkBtn = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// HowTo
			// 
			this.HowTo.ForeColor = System.Drawing.Color.Red;
			this.HowTo.Location = new System.Drawing.Point(8, 8);
			this.HowTo.Name = "HowTo";
			this.HowTo.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.HowTo.Size = new System.Drawing.Size(168, 48);
			this.HowTo.TabIndex = 0;
			this.HowTo.Text = "     Enter numbers 1 - 9 in each row, column and 3x3 square.";
			// 
			// KeysHowTo
			// 
			this.KeysHowTo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
			this.KeysHowTo.Location = new System.Drawing.Point(8, 56);
			this.KeysHowTo.Name = "KeysHowTo";
			this.KeysHowTo.Size = new System.Drawing.Size(168, 80);
			this.KeysHowTo.TabIndex = 1;
			this.KeysHowTo.Text = "   Select a cell by clicking on it with your mouse or moving to it with your arro" +
					"w keys.";
			// 
			// HLColorHowTo
			// 
			this.HLColorHowTo.ForeColor = System.Drawing.Color.Yellow;
			this.HLColorHowTo.Location = new System.Drawing.Point(8, 120);
			this.HLColorHowTo.Name = "HLColorHowTo";
			this.HLColorHowTo.Size = new System.Drawing.Size(168, 32);
			this.HLColorHowTo.TabIndex = 2;
			this.HLColorHowTo.Text = "     Space Bar clears the number.";
			// 
			// HighLightColorHowTo
			// 
			this.HighLightColorHowTo.ForeColor = System.Drawing.Color.Lime;
			this.HighLightColorHowTo.Location = new System.Drawing.Point(8, 152);
			this.HighLightColorHowTo.Name = "HighLightColorHowTo";
			this.HighLightColorHowTo.Size = new System.Drawing.Size(168, 48);
			this.HighLightColorHowTo.TabIndex = 3;
			this.HighLightColorHowTo.Text = "     If the cell is highlighted with yellow that means the number is wrong.";
			// 
			// OkBtn
			// 
			this.OkBtn.BackColor = System.Drawing.SystemColors.HotTrack;
			this.OkBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.OkBtn.ForeColor = System.Drawing.Color.Purple;
			this.OkBtn.Location = new System.Drawing.Point(64, 248);
			this.OkBtn.Name = "OkBtn";
			this.OkBtn.Size = new System.Drawing.Size(48, 23);
			this.OkBtn.TabIndex = 0;
			this.OkBtn.Text = "Ok";
			this.OkBtn.UseVisualStyleBackColor = false;
			// 
			// label1
			// 
			this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
			this.label1.Location = new System.Drawing.Point(8, 200);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(168, 48);
			this.label1.TabIndex = 4;
			this.label1.Text = "Use template mode to create your own SudoKu puzzle.";
			// 
			// HowToPlay
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(7, 15);
			this.BackColor = System.Drawing.Color.Navy;
			this.ClientSize = new System.Drawing.Size(184, 280);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.OkBtn);
			this.Controls.Add(this.HighLightColorHowTo);
			this.Controls.Add(this.HLColorHowTo);
			this.Controls.Add(this.KeysHowTo);
			this.Controls.Add(this.HowTo);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "HowToPlay";
			this.ShowInTaskbar = false;
			this.Text = "How To Play";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
