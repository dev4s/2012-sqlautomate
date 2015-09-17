using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SqlAutomate
{
	public partial class MainForm : Form
	{
		private const string Version = "0.3";
		
		private List<SqlScript.DatabaseResults> _result = new List<SqlScript.DatabaseResults>();
		private readonly List<string> _selectedItems = new List<string>();

		private DateTime _startTime;

		private readonly SqlScript _sqlScript = new SqlScript();

		public MainForm()
		{
			InitializeComponent();
		}

		// loads version, database names, and sets timer label
		private void MainFormLoad(object sender, EventArgs e)
		{
			Text += string.Format(" - version {0}", Version);

			foreach (var dbName in _sqlScript.DatabaseNames)
			{
				checkedListBox.Items.Add(dbName);
			}

			timeStripStatusLabel.Text = _startTime.ToNormalTime();
		}

		// get selected databases, disables controls, set progressbar style
		// sets timer, run backgroundworker
		private void ButtonGenerateScriptsClick(object sender, EventArgs e)
		{
			// connect to sql
			foreach (string item in checkedListBox.SelectedItems)
			{
				_selectedItems.Add(item);
			}

			EnabledOnSomeControls(false);

			toolStripProgressBar.Style = ProgressBarStyle.Marquee;
			_startTime = new DateTime(0);
			timeStripStatusLabel.Text = _startTime.ToNormalTime();
			timer.Start();
			backgroundWorker.RunWorkerAsync();
		}

		// generates sql script in background
		private void BackgroundWorkerDoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			try
			{
				_result = _sqlScript.GenerateSqlScript(_selectedItems);
			}
			catch (Exception)
			{
				//if something bad happen
				//nothing to lose ;)
				_sqlScript.Disconnect();
			}
		}

		// stops timer, set default progressbar style, show dialog for save file
		// show message box, clearing results and selected items, enable controls
		private void BackgroundWorkerRunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			timer.Stop();
			toolStripProgressBar.Style = ProgressBarStyle.Blocks;

			//save file
			if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
			{
				folderBrowserDialog.SaveFile(_result);
				MessageBox.Show("Finished generating sql scripts for selected databases", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information,
							MessageBoxDefaultButton.Button1);
			}
			else
			{
				MessageBox.Show("Finished generating sql scripts for selected databases, but you have not save it. Mess of time ;)", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
			}

			_result.Clear();
			_selectedItems.Clear();
			EnabledOnSomeControls(true);
		}

		// disconnects from server
		private void MainFormFormClosed(object sender, FormClosedEventArgs e)
		{
			_sqlScript.Disconnect();
		}

		// checks if backgroundWorker is busy, if it is = count time
		private void TimerTick(object sender, EventArgs e)
		{
			if (!backgroundWorker.IsBusy) return;
			_startTime = _startTime.AddSeconds(1);
			timeStripStatusLabel.Text = _startTime.ToNormalTime();
		}

		private void EnabledOnSomeControls(bool value)
		{
			checkedListBox.Enabled = value;
			buttonGenerateScripts.Enabled = value;
		}

	}
}
