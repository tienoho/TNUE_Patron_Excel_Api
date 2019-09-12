using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TNUE_Patron_Excel.Tool;

namespace TNUE_Patron_Excel
{
	public class Result : Form
	{
		public static List<Z308> listZ308 = null;

		private IContainer components = null;

		private Label lblInformation;

		private Panel _pnlRight;

		private Panel panel1;

		private Label label1;

		private Label label2;

		public Result()
		{
			InitializeComponent();
		}

		private void Result_Load(object sender, EventArgs e)
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TNUE_Patron_Excel.Result));
			lblInformation = new System.Windows.Forms.Label();
			_pnlRight = new System.Windows.Forms.Panel();
			panel1 = new System.Windows.Forms.Panel();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			SuspendLayout();
			lblInformation.AutoSize = true;
			lblInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
			lblInformation.ForeColor = System.Drawing.Color.Black;
			lblInformation.Location = new System.Drawing.Point(471, 3);
			lblInformation.Name = "lblInformation";
			lblInformation.Size = new System.Drawing.Size(93, 24);
			lblInformation.TabIndex = 48;
			lblInformation.Text = "KẾT QUẢ";
			_pnlRight.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			_pnlRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			_pnlRight.Location = new System.Drawing.Point(10, 54);
			_pnlRight.Name = "_pnlRight";
			_pnlRight.Size = new System.Drawing.Size(501, 474);
			_pnlRight.TabIndex = 47;
			panel1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			panel1.Location = new System.Drawing.Point(517, 54);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(489, 474);
			panel1.TabIndex = 48;
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
			label1.ForeColor = System.Drawing.Color.Black;
			label1.Location = new System.Drawing.Point(186, 27);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(108, 24);
			label1.TabIndex = 49;
			label1.Text = "API APEPH";
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
			label2.ForeColor = System.Drawing.Color.Black;
			label2.Location = new System.Drawing.Point(720, 27);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(58, 24);
			label2.TabIndex = 50;
			label2.Text = "LDAP";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1018, 533);
			base.Controls.Add(label2);
			base.Controls.Add(label1);
			base.Controls.Add(panel1);
			base.Controls.Add(lblInformation);
			base.Controls.Add(_pnlRight);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "Result";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "Result";
			base.Load += new System.EventHandler(Result_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
