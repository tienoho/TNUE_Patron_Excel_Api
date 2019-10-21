using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TNUE_Patron_Excel.Properties;

namespace TNUE_Patron_Excel
{
	public class Loading_FS : Form
	{
		private static Thread _splashThread;

		private static Loading_FS _splashForm;

		public static string text;

		private IContainer components = null;

		private Label label1;

		private PictureBox pictureBox1;

		public Loading_FS()
		{
			InitializeComponent();
			label1.Text = text;
		}

		public static void ShowSplash()
		{
			if (_splashThread == null)
			{
				_splashThread = new Thread(DoShowSplash);
				_splashThread.IsBackground = true;
				_splashThread.Start();
			}
			else
			{
				_splashThread = new Thread(DoShowSplash);
				_splashThread.IsBackground = true;
				_splashThread.Start();
				_splashForm = null;
			}
		}

		private static void DoShowSplash()
		{
			if (_splashForm == null)
			{
				_splashForm = new Loading_FS();
				_splashForm.StartPosition = FormStartPosition.CenterScreen;
				_splashForm.TopMost = true;
			}            
            Application.Run(_splashForm);
		}

		public static void CloseSplash()
		{
			if (_splashForm.InvokeRequired)
			{
				_splashForm.Invoke(new MethodInvoker(CloseSplash));
			}
			else
			{
				Application.ExitThread();
			}
		}

		private void Loading_FS_Load(object sender, EventArgs e)
		{
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			label1 = new System.Windows.Forms.Label();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			SuspendLayout();
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(21, 92);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(128, 13);
			label1.TabIndex = 3;
			label1.Text = "Converting data, please...";
			pictureBox1.Image = TNUE_Patron_Excel.Properties.Resources.ajax_loader_blue_round;
			pictureBox1.Location = new System.Drawing.Point(56, 21);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(63, 58);
			pictureBox1.TabIndex = 4;
			pictureBox1.TabStop = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(175, 126);
			base.Controls.Add(pictureBox1);
			base.Controls.Add(label1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			base.Name = "Loading_FS";
			Text = "Loading_FS";
			base.Load += new System.EventHandler(Loading_FS_Load);
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
