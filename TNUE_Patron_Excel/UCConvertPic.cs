using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TNUE_Patron_Excel.Properties;

namespace TNUE_Patron_Excel
{
	public class UCConvertPic : UserControl
	{
		private string[] files = null;

		private DataTable temp = null;

		private List<listID> list;

		private string fileName;

		private string src;

		private int key = 0;

		private string directoryPath = Application.StartupPath + "\\log";

		private IContainer components = null;

		private GroupBox groupBox3;

		private GroupBox groupBox1;

		private Button btnThoat;

		private TextBox textBox2;

		private Label label2;

		private Button btnchooseTxt;

		private TextBox textBox1;

		private Label label1;

		private Button btnOpenDirectoryName;

		private Button btnConvert;

		private FolderBrowserDialog folderBrowserDialog1;

		private GroupBox groupBox2;

		private PictureBox pb_TaiChinh;

		private ListBox listBox3;

		private ListBox lb_showDirectory;

		private Label lb_tongfile;

		private Label lb_tong;

		public UCConvertPic()
		{
			InitializeComponent();
		}

		private void UCConvertPic_Load(object sender, EventArgs e)
		{
		}

		private void btnThoat_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void btnConvert_Click(object sender, EventArgs e)
		{
			Loading_FS.text = "\tĐang đưa dữ liệu ...";
			ChangeName();
			Loading_FS.CloseSplash();
			MessageBox.Show("Thành công!", "Thông báo!");
		}

		private void openDirectory(ListBox lb)
		{
			lb.Items.Clear();
			FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
			folderBrowserDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			folderBrowserDialog.ShowDialog();
			src = folderBrowserDialog.SelectedPath;
			textBox2.Text = src;
			int num = 0;
			try
			{
				files = Directory.GetFiles(src.Trim(), "*.*", SearchOption.AllDirectories);
				for (int i = 0; i < files.Length; i++)
				{
					lb.Items.Add(files[i]);
					num++;
				}
				lb_tongfile.Text = num.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show("Erro: " + ex.Message);
			}
		}

		private void readTxt()
		{
			list = new List<listID>();
			if (fileName == null)
			{
				MessageBox.Show("Chua chon file");
				return;
			}
			using (StreamReader streamReader = new StreamReader(fileName))
			{
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					listID listID = new listID();
					listID.id = text.Substring(0, text.IndexOf("\t"));
					listID.barcode = text.Substring(text.LastIndexOf("\t")).Trim();
					list.Add(listID);
					listBox3.Items.Add(listID.id + " - " + listID.barcode);
				}
			}
			lb_tong.Text = list.Count.ToString();
		}

		private void ChangeName()
		{
			if (list == null)
			{
				readTxt();
			}
			int num = 0;
			num = ((key == 0) ? temp.Rows.Count : list.Count);
			if (files == null)
			{
				return;
			}
			string text = "";
			string text2 = "";
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			for (int i = 0; i < files.Length; i++)
			{
				text = files[i];
				int num5 = text.LastIndexOf("\\");
				string text3 = text.Substring(num5 + 1);
				if (!text3.Contains("."))
				{
					continue;
				}
				text3 = text3.Substring(0, text3.LastIndexOf("."));
				string text4 = text.Substring(num5 + 1).Substring(text.Substring(num5 + 1).LastIndexOf("."));
				for (int j = 0; j < num; j++)
				{
					string str = (key == 0) ? temp.Rows[j]["PatronID"].ToString() : list[j].id.ToString();
					string text5 = (key == 0) ? temp.Rows[j]["PatronBarcode"].ToString() : list[j].barcode.ToString();
					if (text5.Trim().ToUpper().Equals(text3.ToUpper()))
					{
						try
						{
							if (num3 == 1000)
							{
								num3 = 0;
								num4++;
							}
							string text6 = src + "\\Converter\\pic" + num4;
							if (!Directory.Exists(text6))
							{
								Directory.CreateDirectory(text6);
							}
							text2 = text6 + "\\" + str + ".jpg";
							File.Move(text, text2);
							num3++;
							num2++;
						}
						catch
						{
							continue;
						}
						break;
					}
				}
			}
			stopwatch.Stop();
			MessageBox.Show("Thành công: " + num2 + "\nTime: " + stopwatch.Elapsed.ToString() + "s", "Thông báo!");
			files = null;
			loadItems(lb_showDirectory);
		}

		private void loadItems(ListBox lb)
		{
			files = null;
			lb.Items.Clear();
			try
			{
				files = Directory.GetFiles(src.Trim(), "*.*", SearchOption.AllDirectories);
				for (int i = 0; i < files.Length; i++)
				{
					lb.Items.Add(files[i]);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void btnchooseTxt_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "text file |*.txt;*.txt";
			openFileDialog.FilterIndex = 1;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.Multiselect = false;
			openFileDialog.Title = "Chọn file txt";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				fileName = openFileDialog.FileName;
				textBox1.Text = fileName;
			}
			key = 1;
			readTxt();
		}

		private void btnOpenDirectoryName_Click(object sender, EventArgs e)
		{
			openDirectory(lb_showDirectory);
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
			groupBox3 = new System.Windows.Forms.GroupBox();
			listBox3 = new System.Windows.Forms.ListBox();
			groupBox1 = new System.Windows.Forms.GroupBox();
			pb_TaiChinh = new System.Windows.Forms.PictureBox();
			btnConvert = new System.Windows.Forms.Button();
			textBox2 = new System.Windows.Forms.TextBox();
			label2 = new System.Windows.Forms.Label();
			btnchooseTxt = new System.Windows.Forms.Button();
			textBox1 = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			btnOpenDirectoryName = new System.Windows.Forms.Button();
			btnThoat = new System.Windows.Forms.Button();
			folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			groupBox2 = new System.Windows.Forms.GroupBox();
			lb_showDirectory = new System.Windows.Forms.ListBox();
			lb_tongfile = new System.Windows.Forms.Label();
			lb_tong = new System.Windows.Forms.Label();
			groupBox3.SuspendLayout();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).BeginInit();
			groupBox2.SuspendLayout();
			SuspendLayout();
			groupBox3.Controls.Add(listBox3);
			groupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			groupBox3.Location = new System.Drawing.Point(3, 191);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(248, 332);
			groupBox3.TabIndex = 29;
			groupBox3.TabStop = false;
			groupBox3.Text = "DANH SÁCH";
			listBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			listBox3.FormattingEnabled = true;
			listBox3.HorizontalScrollbar = true;
			listBox3.Location = new System.Drawing.Point(3, 18);
			listBox3.Name = "listBox3";
			listBox3.RightToLeft = System.Windows.Forms.RightToLeft.No;
			listBox3.Size = new System.Drawing.Size(242, 311);
			listBox3.TabIndex = 47;
			groupBox1.Controls.Add(pb_TaiChinh);
			groupBox1.Controls.Add(btnConvert);
			groupBox1.Controls.Add(textBox2);
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(btnchooseTxt);
			groupBox1.Controls.Add(textBox1);
			groupBox1.Controls.Add(label1);
			groupBox1.Controls.Add(btnOpenDirectoryName);
			groupBox1.Controls.Add(btnThoat);
			groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold);
			groupBox1.Location = new System.Drawing.Point(0, 3);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(993, 182);
			groupBox1.TabIndex = 28;
			groupBox1.TabStop = false;
			groupBox1.Text = "Chuyển đổi tên ảnh";
			pb_TaiChinh.Image = TNUE_Patron_Excel.Properties.Resources.library_logo;
			pb_TaiChinh.Location = new System.Drawing.Point(811, 18);
			pb_TaiChinh.Name = "pb_TaiChinh";
			pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
			pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			pb_TaiChinh.TabIndex = 20;
			pb_TaiChinh.TabStop = false;
			btnConvert.AutoSize = true;
			btnConvert.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnConvert.FlatAppearance.BorderSize = 0;
			btnConvert.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnConvert.ForeColor = System.Drawing.Color.White;
			btnConvert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnConvert.Location = new System.Drawing.Point(329, 131);
			btnConvert.Name = "btnConvert";
			btnConvert.Size = new System.Drawing.Size(158, 38);
			btnConvert.TabIndex = 106;
			btnConvert.Text = "Chuyển dữ liệu";
			btnConvert.UseVisualStyleBackColor = false;
			btnConvert.Click += new System.EventHandler(btnConvert_Click);
			textBox2.Enabled = false;
			textBox2.Location = new System.Drawing.Point(144, 84);
			textBox2.Margin = new System.Windows.Forms.Padding(2);
			textBox2.Name = "textBox2";
			textBox2.Size = new System.Drawing.Size(470, 33);
			textBox2.TabIndex = 93;
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(15, 84);
			label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(128, 25);
			label2.TabIndex = 91;
			label2.Text = "Thư mục ảnh";
			btnchooseTxt.Location = new System.Drawing.Point(618, 46);
			btnchooseTxt.Margin = new System.Windows.Forms.Padding(2);
			btnchooseTxt.Name = "btnchooseTxt";
			btnchooseTxt.Size = new System.Drawing.Size(159, 33);
			btnchooseTxt.TabIndex = 90;
			btnchooseTxt.Text = "Browser...";
			btnchooseTxt.UseVisualStyleBackColor = true;
			btnchooseTxt.Click += new System.EventHandler(btnchooseTxt_Click);
			textBox1.Enabled = false;
			textBox1.Location = new System.Drawing.Point(144, 47);
			textBox1.Margin = new System.Windows.Forms.Padding(2);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(470, 33);
			textBox1.TabIndex = 89;
			textBox1.Tag = "";
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(20, 50);
			label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(91, 25);
			label1.TabIndex = 88;
			label1.Text = "Chọn file";
			btnOpenDirectoryName.Location = new System.Drawing.Point(618, 84);
			btnOpenDirectoryName.Margin = new System.Windows.Forms.Padding(2);
			btnOpenDirectoryName.Name = "btnOpenDirectoryName";
			btnOpenDirectoryName.Size = new System.Drawing.Size(159, 33);
			btnOpenDirectoryName.TabIndex = 87;
			btnOpenDirectoryName.Text = "Browser...";
			btnOpenDirectoryName.UseVisualStyleBackColor = true;
			btnOpenDirectoryName.Click += new System.EventHandler(btnOpenDirectoryName_Click);
			btnThoat.AutoSize = true;
			btnThoat.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
			btnThoat.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnThoat.FlatAppearance.BorderSize = 0;
			btnThoat.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnThoat.ForeColor = System.Drawing.Color.White;
			btnThoat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnThoat.Location = new System.Drawing.Point(493, 131);
			btnThoat.Name = "btnThoat";
			btnThoat.Size = new System.Drawing.Size(82, 38);
			btnThoat.TabIndex = 14;
			btnThoat.Text = "Thoát";
			btnThoat.UseVisualStyleBackColor = false;
			btnThoat.Click += new System.EventHandler(btnThoat_Click);
			groupBox2.Controls.Add(lb_showDirectory);
			groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			groupBox2.Location = new System.Drawing.Point(257, 191);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(736, 332);
			groupBox2.TabIndex = 30;
			groupBox2.TabStop = false;
			groupBox2.Text = "DANH SÁCH FILE";
			lb_showDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
			lb_showDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lb_showDirectory.FormattingEnabled = true;
			lb_showDirectory.Location = new System.Drawing.Point(3, 18);
			lb_showDirectory.Name = "lb_showDirectory";
			lb_showDirectory.ScrollAlwaysVisible = true;
			lb_showDirectory.Size = new System.Drawing.Size(730, 311);
			lb_showDirectory.TabIndex = 49;
			lb_tongfile.AutoSize = true;
			lb_tongfile.Location = new System.Drawing.Point(925, 526);
			lb_tongfile.Name = "lb_tongfile";
			lb_tongfile.Size = new System.Drawing.Size(46, 16);
			lb_tongfile.TabIndex = 54;
			lb_tongfile.Text = "Tổng: ";
			lb_tong.AutoSize = true;
			lb_tong.Location = new System.Drawing.Point(181, 526);
			lb_tong.Name = "lb_tong";
			lb_tong.Size = new System.Drawing.Size(46, 16);
			lb_tong.TabIndex = 53;
			lb_tong.Text = "Tổng: ";
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(lb_tongfile);
			base.Controls.Add(lb_tong);
			base.Controls.Add(groupBox2);
			base.Controls.Add(groupBox3);
			base.Controls.Add(groupBox1);
			Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.Margin = new System.Windows.Forms.Padding(4);
			base.Name = "UCConvertPic";
			base.Size = new System.Drawing.Size(1000, 565);
			base.Load += new System.EventHandler(UCConvertPic_Load);
			groupBox3.ResumeLayout(false);
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).EndInit();
			groupBox2.ResumeLayout(false);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
