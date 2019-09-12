using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TNUE_Patron_Excel.Properties;

namespace TNUE_Patron_Excel.ControlMember
{
	public class UCControlMember : UserControl
	{
		private IContainer components = null;

		private GroupBox groupBox1;

		private FolderBrowserDialog folderBrowserDialog1;

		private PictureBox pb_TaiChinh;

		private Button btnSinhVien;

		private Button _btAdd;

		public UCControlMember()
		{
			InitializeComponent();
		}

		private void UCCanBo_Load(object sender, EventArgs e)
		{
		}

		private void _btAdd_Click(object sender, EventArgs e)
		{
			AddEditMember addEditMember = new AddEditMember();
			addEditMember.Show();
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
			groupBox1 = new System.Windows.Forms.GroupBox();
			pb_TaiChinh = new System.Windows.Forms.PictureBox();
			folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			btnSinhVien = new System.Windows.Forms.Button();
			_btAdd = new System.Windows.Forms.Button();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).BeginInit();
			SuspendLayout();
			groupBox1.Controls.Add(btnSinhVien);
			groupBox1.Controls.Add(_btAdd);
			groupBox1.Controls.Add(pb_TaiChinh);
			groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold);
			groupBox1.Location = new System.Drawing.Point(0, 3);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(993, 559);
			groupBox1.TabIndex = 28;
			groupBox1.TabStop = false;
			groupBox1.Text = "Control Member";
			pb_TaiChinh.Image = TNUE_Patron_Excel.Properties.Resources.library_logo;
			pb_TaiChinh.Location = new System.Drawing.Point(796, 20);
			pb_TaiChinh.Name = "pb_TaiChinh";
			pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
			pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			pb_TaiChinh.TabIndex = 20;
			pb_TaiChinh.TabStop = false;
			btnSinhVien.Cursor = System.Windows.Forms.Cursors.Hand;
			btnSinhVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btnSinhVien.Image = TNUE_Patron_Excel.Properties.Resources.username_login_50x50;
			btnSinhVien.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			btnSinhVien.Location = new System.Drawing.Point(451, 89);
			btnSinhVien.Name = "btnSinhVien";
			btnSinhVien.Size = new System.Drawing.Size(125, 82);
			btnSinhVien.TabIndex = 55;
			btnSinhVien.Text = "Sinh Viên";
			btnSinhVien.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btnSinhVien.UseVisualStyleBackColor = true;
			_btAdd.Cursor = System.Windows.Forms.Cursors.Hand;
			_btAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			_btAdd.Image = TNUE_Patron_Excel.Properties.Resources.customer_service_icon_48x48;
			_btAdd.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			_btAdd.Location = new System.Drawing.Point(290, 89);
			_btAdd.Name = "_btAdd";
			_btAdd.Size = new System.Drawing.Size(125, 82);
			_btAdd.TabIndex = 54;
			_btAdd.Text = "Add";
			_btAdd.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			_btAdd.UseVisualStyleBackColor = true;
			_btAdd.Click += new System.EventHandler(_btAdd_Click);
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox1);
			Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.Margin = new System.Windows.Forms.Padding(4);
			base.Name = "UCControlMember";
			base.Size = new System.Drawing.Size(1000, 565);
			base.Load += new System.EventHandler(UCCanBo_Load);
			groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).EndInit();
			ResumeLayout(false);
		}
	}
}
