using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TNUE_Patron_Excel.DBConnect;
using TNUE_Patron_Excel.Tool;

namespace TNUE_Patron_Excel
{
	public class UCDataPatronZ308 : UserControl
	{
		private List<Z308> listZ308 = null;

		private IContainer components = null;

		private GroupBox groupBox3;

		private DataGridView dgvPatron;

		public UCDataPatronZ308()
		{
			InitializeComponent();
		}

		private void UCNhanVien_Load(object sender, EventArgs e)
		{
			listZ308 = DataDBLocal.listZ308;
			dgvPatron.DataSource = listZ308;
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			groupBox3 = new System.Windows.Forms.GroupBox();
			dgvPatron = new System.Windows.Forms.DataGridView();
			groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvPatron).BeginInit();
			SuspendLayout();
			groupBox3.Controls.Add(dgvPatron);
			groupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			groupBox3.Location = new System.Drawing.Point(3, 0);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(990, 552);
			groupBox3.TabIndex = 29;
			groupBox3.TabStop = false;
			groupBox3.Text = "DANH SÁCH";
			dgvPatron.AllowUserToAddRows = false;
			dgvPatron.AllowUserToDeleteRows = false;
			dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dgvPatron.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			dgvPatron.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dgvPatron.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvPatron.Dock = System.Windows.Forms.DockStyle.Fill;
			dgvPatron.Location = new System.Drawing.Point(3, 18);
			dgvPatron.Name = "dgvPatron";
			dgvPatron.ReadOnly = true;
			dgvPatron.RowHeadersWidth = 20;
			dgvPatron.Size = new System.Drawing.Size(984, 531);
			dgvPatron.TabIndex = 18;
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox3);
			Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.Margin = new System.Windows.Forms.Padding(4);
			base.Name = "UCDataPatronZ308";
			base.Size = new System.Drawing.Size(1000, 565);
			base.Load += new System.EventHandler(UCNhanVien_Load);
			groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvPatron).EndInit();
			ResumeLayout(false);
		}
	}
}
