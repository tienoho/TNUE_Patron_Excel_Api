using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TNUE_Patron_Excel.DBConnect;
using TNUE_Patron_Excel.Ldap;
using TNUE_Patron_Excel.Properties;
using TNUE_Patron_Excel.Tool;

namespace TNUE_Patron_Excel.ControlMember
{
	public class UCInsertUserLdap : UserControl
	{
		private List<Z308> listZ308 = null;

		private ToolP tool = new ToolP();

		private List<User> ldapUser = null;

		private List<ItemBlock> ListPatronNoLdap = null;

		private List<string> PatronLdap = null;

		private string directoryPath = Application.StartupPath + "\\log";

		private IContainer components = null;

		private GroupBox groupBox3;

		private DataGridView dgvAleph;

		private GroupBox groupBox1;

		private Button btnThoat;

		private FolderBrowserDialog folderBrowserDialog1;

		private GroupBox groupBox2;

		private Label lbCountListExcel;

		private Label lbCountHad;

		private PictureBox pb_TaiChinh;

		private DataGridView dgvLdap;

		private Button btnUnSearch;

		private Label label5;

		private TextBox txtSearch;

		private Label label4;

		private TextBox txtPassword;

		private Label label3;

		private TextBox txtPhone;

		private Label label2;

		private TextBox txtEmail;

		private Label label1;

		private TextBox txtMa;

		private Button btnSearch;

		private Button btnCreat;

		private DataGridViewTextBoxColumn pationID;

		private DataGridViewTextBoxColumn Ma;

		private DataGridViewTextBoxColumn HoTen;

		private Panel panel1;

		public UCInsertUserLdap()
		{
			InitializeComponent();
		}

		private void UCInsertUserLdap_Load(object sender, EventArgs e)
		{
			LoadForm();
		}

		private void btnThoat_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void LoadForm()
		{
			try
			{
				Loading_FS.text = "\tĐang cập nhập dữ liệu...";
				Loading_FS.ShowSplash();
				CloneList();
				LoadData();
				CreateFolder(directoryPath);
				CheckDataGridView(dgvAleph, lbCountListExcel);
				CheckDataGridView(dgvLdap, lbCountHad);
				Loading_FS.CloseSplash();
				panel1.Visible = true;
			}
			catch (Exception ex)
			{
				Loading_FS.CloseSplash();
				MessageBox.Show("Lỗi không mong muốn !\n" + ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void LoadData()
		{
			ldapUser = new ModelLdap().GetAllListUser();
			compreRemovePatron();
			dgvAleph.DataSource = ListPatronNoLdap;
			dgvLdap.DataSource = ldapUser;
		}

		private void CloneList()
		{
			listZ308 = new List<Z308>();
			foreach (Z308 item in DataDBLocal.listZ308)
			{
				listZ308.Add(item);
			}
		}

		private void CreateFolder(string directoryPath)
		{
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
		}

		private void WriterUserLdap()
		{
			if (txtMa.Text == "" && txtMa.Text == null)
			{
				MessageBox.Show("Không thể để trống mã!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (txtPassword.Text == "" && txtPassword.Text == null)
			{
				MessageBox.Show("Không thể để trống mật khẩu !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			User user = new User();
			user.cn = txtMa.Text.Trim();
			user.sn = txtMa.Text.Trim();
			user.userLogin = txtMa.Text.Trim();
			user.userMail = txtEmail.Text.Trim();
			user.telephoneNumber = txtPhone.Text.Trim();
			user.userPassword = txtPassword.Text.Trim();
			user.objectClass = "OpenLDAPPerson";
			using (StreamWriter streamWriter = new StreamWriter(directoryPath + "/Ldap-Log-" + tool.getDate() + ".txt"))
			{
				streamWriter.WriteLine(user.userLogin + "\t" + new ModelLdap().CreateUser(user));
			}
			try
			{
				Loading_FS.text = "\tĐang tạo người dùng...";
				Loading_FS.ShowSplash();
				LoadData();
				Loading_FS.CloseSplash();
			}
			catch
			{
			}
			MessageBox.Show("Thành công!", "Thông báo!");
		}

		private void compreRemovePatron()
		{
			PatronLdap = new List<string>();
			ListPatronNoLdap = new List<ItemBlock>();
			foreach (Z308 item in listZ308)
			{
				string text = item.Z308_REC_KEY.Trim();
				text = text.Substring(2);
				foreach (User item2 in ldapUser)
				{
					if (text.Equals(item2.userLogin))
					{
						PatronLdap.Add(item2.userLogin);
					}
				}
			}
			RemovePatron();
		}

		private void RemovePatron()
		{
			List<Z308> list = listZ308;
			foreach (string s in PatronLdap)
			{
				int index = listZ308.FindIndex(delegate(Z308 dsd)
				{
					string text = dsd.Z308_REC_KEY.Substring(2);
					return text.Equals(s);
				});
				list.RemoveAt(index);
			}
			ListPatronNoLdap = new List<ItemBlock>();
			foreach (Z308 item in list)
			{
				ItemBlock itemBlock = new ItemBlock();
				itemBlock.PatronId = item.Z308_ID;
				itemBlock.Ma = item.Z308_REC_KEY.Substring(2);
				itemBlock.HoTen = item.Z308_ENCRYPTION;
				ListPatronNoLdap.Add(itemBlock);
			}
		}

		private void CheckDataGridView(DataGridView gdv, Label lb)
		{
			if (gdv.ColumnCount > 0)
			{
				lb.Text = "Số lượng: " + gdv.RowCount.ToString();
			}
		}

		private void BtnCreat_Click(object sender, EventArgs e)
		{
			WriterUserLdap();
		}

		private void DgvAleph_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			int rowIndex = e.RowIndex;
			try
			{
				txtMa.Text = dgvAleph.Rows[rowIndex].Cells[0].Value.ToString();
			}
			catch
			{
			}
		}

		private void BtnSearch_Click(object sender, EventArgs e)
		{
			string inputText = txtSearch.Text.Trim().ToUpper();
			dgvAleph.Columns.Clear();
			dgvAleph.DataSource = (from r in ListPatronNoLdap.AsEnumerable()
				where r.Ma.Contains(inputText)
				select r).ToList();
		}

		private void BtnUnSearch_Click(object sender, EventArgs e)
		{
			dgvAleph.DataSource = ListPatronNoLdap;
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			groupBox3 = new System.Windows.Forms.GroupBox();
			dgvAleph = new System.Windows.Forms.DataGridView();
			pationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Ma = new System.Windows.Forms.DataGridViewTextBoxColumn();
			HoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
			groupBox1 = new System.Windows.Forms.GroupBox();
			btnUnSearch = new System.Windows.Forms.Button();
			label5 = new System.Windows.Forms.Label();
			txtSearch = new System.Windows.Forms.TextBox();
			label4 = new System.Windows.Forms.Label();
			txtPassword = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			txtPhone = new System.Windows.Forms.TextBox();
			label2 = new System.Windows.Forms.Label();
			txtEmail = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			txtMa = new System.Windows.Forms.TextBox();
			btnCreat = new System.Windows.Forms.Button();
			btnSearch = new System.Windows.Forms.Button();
			pb_TaiChinh = new System.Windows.Forms.PictureBox();
			btnThoat = new System.Windows.Forms.Button();
			folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			groupBox2 = new System.Windows.Forms.GroupBox();
			dgvLdap = new System.Windows.Forms.DataGridView();
			lbCountListExcel = new System.Windows.Forms.Label();
			lbCountHad = new System.Windows.Forms.Label();
			panel1 = new System.Windows.Forms.Panel();
			groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvAleph).BeginInit();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).BeginInit();
			groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvLdap).BeginInit();
			panel1.SuspendLayout();
			SuspendLayout();
			groupBox3.Controls.Add(dgvAleph);
			groupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			groupBox3.Location = new System.Drawing.Point(4, 246);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(498, 277);
			groupBox3.TabIndex = 29;
			groupBox3.TabStop = false;
			groupBox3.Text = "DANH SÁCH ALEPH";
			dgvAleph.AllowUserToAddRows = false;
			dgvAleph.AllowUserToDeleteRows = false;
			dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dgvAleph.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			dgvAleph.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dgvAleph.Columns.AddRange(pationID, Ma, HoTen);
			dgvAleph.Dock = System.Windows.Forms.DockStyle.Fill;
			dgvAleph.Location = new System.Drawing.Point(3, 18);
			dgvAleph.Name = "dgvAleph";
			dgvAleph.ReadOnly = true;
			dgvAleph.RowHeadersWidth = 20;
			dgvAleph.Size = new System.Drawing.Size(492, 256);
			dgvAleph.TabIndex = 18;
			dgvAleph.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(DgvAleph_CellClick);
			pationID.DataPropertyName = "PatronId";
			pationID.HeaderText = "Patron ID";
			pationID.Name = "pationID";
			pationID.ReadOnly = true;
			pationID.Width = 81;
			Ma.DataPropertyName = "Ma";
			Ma.HeaderText = "Mã";
			Ma.Name = "Ma";
			Ma.ReadOnly = true;
			Ma.Width = 49;
			HoTen.DataPropertyName = "HoTen";
			HoTen.HeaderText = "Họ tên";
			HoTen.Name = "HoTen";
			HoTen.ReadOnly = true;
			HoTen.Width = 67;
			groupBox1.Controls.Add(btnUnSearch);
			groupBox1.Controls.Add(label5);
			groupBox1.Controls.Add(txtSearch);
			groupBox1.Controls.Add(label4);
			groupBox1.Controls.Add(txtPassword);
			groupBox1.Controls.Add(label3);
			groupBox1.Controls.Add(txtPhone);
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(txtEmail);
			groupBox1.Controls.Add(label1);
			groupBox1.Controls.Add(txtMa);
			groupBox1.Controls.Add(btnCreat);
			groupBox1.Controls.Add(btnSearch);
			groupBox1.Controls.Add(pb_TaiChinh);
			groupBox1.Controls.Add(btnThoat);
			groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold);
			groupBox1.Location = new System.Drawing.Point(4, 3);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(984, 240);
			groupBox1.TabIndex = 28;
			groupBox1.TabStop = false;
			groupBox1.Text = "Thêm bạn đọc ldap";
			btnUnSearch.AutoSize = true;
			btnUnSearch.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
			btnUnSearch.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnUnSearch.FlatAppearance.BorderSize = 0;
			btnUnSearch.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnUnSearch.ForeColor = System.Drawing.Color.White;
			btnUnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnUnSearch.Location = new System.Drawing.Point(688, 164);
			btnUnSearch.Name = "btnUnSearch";
			btnUnSearch.Size = new System.Drawing.Size(103, 38);
			btnUnSearch.TabIndex = 134;
			btnUnSearch.Text = "Bỏ tìm";
			btnUnSearch.UseVisualStyleBackColor = false;
			btnUnSearch.Click += new System.EventHandler(BtnUnSearch_Click);
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(615, 93);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(125, 25);
			label5.TabIndex = 133;
			label5.Text = "Tìm kiếm mã";
			txtSearch.Location = new System.Drawing.Point(571, 121);
			txtSearch.Name = "txtSearch";
			txtSearch.Size = new System.Drawing.Size(220, 33);
			txtSearch.TabIndex = 132;
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(12, 169);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(96, 25);
			label4.TabIndex = 131;
			label4.Text = "Mật khẩu";
			txtPassword.Location = new System.Drawing.Point(152, 166);
			txtPassword.Name = "txtPassword";
			txtPassword.Size = new System.Drawing.Size(281, 33);
			txtPassword.TabIndex = 130;
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(12, 130);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(69, 25);
			label3.TabIndex = 129;
			label3.Text = "Phone";
			txtPhone.Location = new System.Drawing.Point(152, 127);
			txtPhone.Name = "txtPhone";
			txtPhone.Size = new System.Drawing.Size(281, 33);
			txtPhone.TabIndex = 128;
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(12, 91);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(59, 25);
			label2.TabIndex = 127;
			label2.Text = "Email";
			txtEmail.Location = new System.Drawing.Point(152, 88);
			txtEmail.Name = "txtEmail";
			txtEmail.Size = new System.Drawing.Size(281, 33);
			txtEmail.TabIndex = 126;
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(12, 52);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(101, 25);
			label1.TabIndex = 125;
			label1.Text = "Mã SV/CB";
			txtMa.Enabled = false;
			txtMa.Location = new System.Drawing.Point(152, 49);
			txtMa.Name = "txtMa";
			txtMa.Size = new System.Drawing.Size(281, 33);
			txtMa.TabIndex = 124;
			btnCreat.AutoSize = true;
			btnCreat.BackColor = System.Drawing.Color.Green;
			btnCreat.FlatAppearance.BorderSize = 0;
			btnCreat.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnCreat.ForeColor = System.Drawing.Color.White;
			btnCreat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnCreat.Location = new System.Drawing.Point(152, 202);
			btnCreat.Name = "btnCreat";
			btnCreat.Size = new System.Drawing.Size(281, 35);
			btnCreat.TabIndex = 123;
			btnCreat.Text = "Tạo người dùng ldap";
			btnCreat.UseVisualStyleBackColor = false;
			btnCreat.Click += new System.EventHandler(BtnCreat_Click);
			btnSearch.AutoSize = true;
			btnSearch.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnSearch.FlatAppearance.BorderSize = 0;
			btnSearch.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnSearch.ForeColor = System.Drawing.Color.White;
			btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnSearch.Location = new System.Drawing.Point(571, 164);
			btnSearch.Name = "btnSearch";
			btnSearch.Size = new System.Drawing.Size(114, 38);
			btnSearch.TabIndex = 122;
			btnSearch.Text = "Tìm kiếm";
			btnSearch.UseVisualStyleBackColor = false;
			btnSearch.Click += new System.EventHandler(BtnSearch_Click);
			pb_TaiChinh.Image = TNUE_Patron_Excel.Properties.Resources.library_logo;
			pb_TaiChinh.Location = new System.Drawing.Point(818, 21);
			pb_TaiChinh.Name = "pb_TaiChinh";
			pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
			pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			pb_TaiChinh.TabIndex = 20;
			pb_TaiChinh.TabStop = false;
			btnThoat.AutoSize = true;
			btnThoat.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
			btnThoat.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnThoat.FlatAppearance.BorderSize = 0;
			btnThoat.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnThoat.ForeColor = System.Drawing.Color.White;
			btnThoat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnThoat.Location = new System.Drawing.Point(869, 196);
			btnThoat.Name = "btnThoat";
			btnThoat.Size = new System.Drawing.Size(109, 38);
			btnThoat.TabIndex = 14;
			btnThoat.Text = "Thoát";
			btnThoat.UseVisualStyleBackColor = false;
			btnThoat.Click += new System.EventHandler(btnThoat_Click);
			groupBox2.Controls.Add(dgvLdap);
			groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			groupBox2.Location = new System.Drawing.Point(508, 249);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(486, 277);
			groupBox2.TabIndex = 30;
			groupBox2.TabStop = false;
			groupBox2.Text = "DANH SÁCH LDAP";
			dgvLdap.AllowUserToAddRows = false;
			dgvLdap.AllowUserToDeleteRows = false;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dgvLdap.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
			dgvLdap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dgvLdap.Dock = System.Windows.Forms.DockStyle.Fill;
			dgvLdap.Location = new System.Drawing.Point(3, 18);
			dgvLdap.Name = "dgvLdap";
			dgvLdap.ReadOnly = true;
			dgvLdap.RowHeadersWidth = 20;
			dgvLdap.Size = new System.Drawing.Size(480, 256);
			dgvLdap.TabIndex = 19;
			lbCountListExcel.AutoSize = true;
			lbCountListExcel.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lbCountListExcel.Location = new System.Drawing.Point(9, 526);
			lbCountListExcel.Name = "lbCountListExcel";
			lbCountListExcel.Size = new System.Drawing.Size(76, 21);
			lbCountListExcel.TabIndex = 31;
			lbCountListExcel.Text = "Số lượng:";
			lbCountHad.AutoSize = true;
			lbCountHad.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lbCountHad.Location = new System.Drawing.Point(507, 526);
			lbCountHad.Name = "lbCountHad";
			lbCountHad.Size = new System.Drawing.Size(76, 21);
			lbCountHad.TabIndex = 32;
			lbCountHad.Text = "Số lượng:";
			panel1.Controls.Add(groupBox1);
			panel1.Controls.Add(lbCountListExcel);
			panel1.Controls.Add(lbCountHad);
			panel1.Controls.Add(groupBox3);
			panel1.Controls.Add(groupBox2);
			panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			panel1.Location = new System.Drawing.Point(0, 0);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(1000, 565);
			panel1.TabIndex = 135;
			panel1.Visible = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(panel1);
			Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.Margin = new System.Windows.Forms.Padding(4);
			base.Name = "UCInsertUserLdap";
			base.Size = new System.Drawing.Size(1000, 565);
			base.Load += new System.EventHandler(UCInsertUserLdap_Load);
			groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvAleph).EndInit();
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).EndInit();
			groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvLdap).EndInit();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			ResumeLayout(false);
		}
	}
}
