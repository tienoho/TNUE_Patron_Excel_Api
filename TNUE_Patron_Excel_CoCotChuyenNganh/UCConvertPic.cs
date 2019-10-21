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

		private string directoryPath = DataDBLocal.pathUserLog;

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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pb_TaiChinh = new System.Windows.Forms.PictureBox();
            this.btnConvert = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnchooseTxt = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenDirectoryName = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lb_showDirectory = new System.Windows.Forms.ListBox();
            this.lb_tongfile = new System.Windows.Forms.Label();
            this.lb_tong = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBox3);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(3, 191);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(248, 332);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DANH SÁCH";
            // 
            // listBox3
            // 
            this.listBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listBox3.FormattingEnabled = true;
            this.listBox3.HorizontalScrollbar = true;
            this.listBox3.Location = new System.Drawing.Point(3, 18);
            this.listBox3.Name = "listBox3";
            this.listBox3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.listBox3.Size = new System.Drawing.Size(242, 311);
            this.listBox3.TabIndex = 47;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pb_TaiChinh);
            this.groupBox1.Controls.Add(this.btnConvert);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnchooseTxt);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnOpenDirectoryName);
            this.groupBox1.Controls.Add(this.btnThoat);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(990, 182);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chuyển đổi tên ảnh";
            // 
            // pb_TaiChinh
            // 
            this.pb_TaiChinh.Image = global::TNUE_Patron_Excel.Properties.Resources.library_logo;
            this.pb_TaiChinh.Location = new System.Drawing.Point(811, 18);
            this.pb_TaiChinh.Name = "pb_TaiChinh";
            this.pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
            this.pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_TaiChinh.TabIndex = 20;
            this.pb_TaiChinh.TabStop = false;
            // 
            // btnConvert
            // 
            this.btnConvert.AutoSize = true;
            this.btnConvert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnConvert.FlatAppearance.BorderSize = 0;
            this.btnConvert.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConvert.ForeColor = System.Drawing.Color.White;
            this.btnConvert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConvert.Location = new System.Drawing.Point(329, 131);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(158, 38);
            this.btnConvert.TabIndex = 106;
            this.btnConvert.Text = "Chuyển dữ liệu";
            this.btnConvert.UseVisualStyleBackColor = false;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(144, 84);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(470, 33);
            this.textBox2.TabIndex = 93;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 25);
            this.label2.TabIndex = 91;
            this.label2.Text = "Thư mục ảnh";
            // 
            // btnchooseTxt
            // 
            this.btnchooseTxt.Location = new System.Drawing.Point(618, 46);
            this.btnchooseTxt.Margin = new System.Windows.Forms.Padding(2);
            this.btnchooseTxt.Name = "btnchooseTxt";
            this.btnchooseTxt.Size = new System.Drawing.Size(159, 33);
            this.btnchooseTxt.TabIndex = 90;
            this.btnchooseTxt.Text = "Browser...";
            this.btnchooseTxt.UseVisualStyleBackColor = true;
            this.btnchooseTxt.Click += new System.EventHandler(this.btnchooseTxt_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(144, 47);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(470, 33);
            this.textBox1.TabIndex = 89;
            this.textBox1.Tag = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 50);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 25);
            this.label1.TabIndex = 88;
            this.label1.Text = "Chọn file";
            // 
            // btnOpenDirectoryName
            // 
            this.btnOpenDirectoryName.Location = new System.Drawing.Point(618, 84);
            this.btnOpenDirectoryName.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenDirectoryName.Name = "btnOpenDirectoryName";
            this.btnOpenDirectoryName.Size = new System.Drawing.Size(159, 33);
            this.btnOpenDirectoryName.TabIndex = 87;
            this.btnOpenDirectoryName.Text = "Browser...";
            this.btnOpenDirectoryName.UseVisualStyleBackColor = true;
            this.btnOpenDirectoryName.Click += new System.EventHandler(this.btnOpenDirectoryName_Click);
            // 
            // btnThoat
            // 
            this.btnThoat.AutoSize = true;
            this.btnThoat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnThoat.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnThoat.FlatAppearance.BorderSize = 0;
            this.btnThoat.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnThoat.ForeColor = System.Drawing.Color.White;
            this.btnThoat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnThoat.Location = new System.Drawing.Point(493, 131);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(82, 38);
            this.btnThoat.TabIndex = 14;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = false;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lb_showDirectory);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(257, 191);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(736, 332);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DANH SÁCH FILE";
            // 
            // lb_showDirectory
            // 
            this.lb_showDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lb_showDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_showDirectory.FormattingEnabled = true;
            this.lb_showDirectory.Location = new System.Drawing.Point(3, 18);
            this.lb_showDirectory.Name = "lb_showDirectory";
            this.lb_showDirectory.ScrollAlwaysVisible = true;
            this.lb_showDirectory.Size = new System.Drawing.Size(730, 311);
            this.lb_showDirectory.TabIndex = 49;
            // 
            // lb_tongfile
            // 
            this.lb_tongfile.AutoSize = true;
            this.lb_tongfile.Location = new System.Drawing.Point(925, 526);
            this.lb_tongfile.Name = "lb_tongfile";
            this.lb_tongfile.Size = new System.Drawing.Size(46, 16);
            this.lb_tongfile.TabIndex = 54;
            this.lb_tongfile.Text = "Tổng: ";
            // 
            // lb_tong
            // 
            this.lb_tong.AutoSize = true;
            this.lb_tong.Location = new System.Drawing.Point(181, 526);
            this.lb_tong.Name = "lb_tong";
            this.lb_tong.Size = new System.Drawing.Size(46, 16);
            this.lb_tong.TabIndex = 53;
            this.lb_tong.Text = "Tổng: ";
            // 
            // UCConvertPic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lb_tongfile);
            this.Controls.Add(this.lb_tong);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCConvertPic";
            this.Size = new System.Drawing.Size(1000, 565);
            this.Load += new System.EventHandler(this.UCConvertPic_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
