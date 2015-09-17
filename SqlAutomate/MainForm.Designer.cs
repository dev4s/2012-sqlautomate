namespace SqlAutomate
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.checkedListBox = new System.Windows.Forms.CheckedListBox();
			this.buttonGenerateScripts = new System.Windows.Forms.Button();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
			this.timeStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.statusStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// checkedListBox
			// 
			this.checkedListBox.CheckOnClick = true;
			this.checkedListBox.FormattingEnabled = true;
			this.checkedListBox.Location = new System.Drawing.Point(12, 12);
			this.checkedListBox.Name = "checkedListBox";
			this.checkedListBox.Size = new System.Drawing.Size(347, 289);
			this.checkedListBox.TabIndex = 0;
			// 
			// buttonGenerateScripts
			// 
			this.buttonGenerateScripts.Location = new System.Drawing.Point(249, 307);
			this.buttonGenerateScripts.Name = "buttonGenerateScripts";
			this.buttonGenerateScripts.Size = new System.Drawing.Size(110, 23);
			this.buttonGenerateScripts.TabIndex = 1;
			this.buttonGenerateScripts.Text = "Generate Scripts";
			this.buttonGenerateScripts.UseVisualStyleBackColor = true;
			this.buttonGenerateScripts.Click += new System.EventHandler(this.ButtonGenerateScriptsClick);
			// 
			// backgroundWorker
			// 
			this.backgroundWorker.WorkerReportsProgress = true;
			this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorkerDoWork);
			this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorkerRunWorkerCompleted);
			// 
			// statusStrip
			// 
			this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.timeStripStatusLabel});
			this.statusStrip.Location = new System.Drawing.Point(0, 337);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(371, 22);
			this.statusStrip.SizingGrip = false;
			this.statusStrip.TabIndex = 2;
			// 
			// toolStripProgressBar
			// 
			this.toolStripProgressBar.Name = "toolStripProgressBar";
			this.toolStripProgressBar.Size = new System.Drawing.Size(200, 16);
			// 
			// timeStripStatusLabel
			// 
			this.timeStripStatusLabel.Name = "timeStripStatusLabel";
			this.timeStripStatusLabel.Size = new System.Drawing.Size(35, 17);
			this.timeStripStatusLabel.Text = "timer";
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = 1000;
			this.timer.Tick += new System.EventHandler(this.TimerTick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(371, 359);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.buttonGenerateScripts);
			this.Controls.Add(this.checkedListBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Sql Automate Scripts";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainFormFormClosed);
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox checkedListBox;
		private System.Windows.Forms.Button buttonGenerateScripts;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.ComponentModel.BackgroundWorker backgroundWorker;
		private System.Windows.Forms.StatusStrip statusStrip;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
		private System.Windows.Forms.Timer timer;
		private System.Windows.Forms.ToolStripStatusLabel timeStripStatusLabel;

	}
}

