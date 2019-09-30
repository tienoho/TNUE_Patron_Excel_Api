using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TNUE_Patron_Excel.DBConnect;
using TNUE_Patron_Excel.Ldap;
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
                int index = listZ308.FindIndex(delegate (Z308 dsd)
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
                txtMa.Text = dgvAleph.Rows[rowIndex].Cells[1].Value.ToString();
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
            txtSearch.Clear();
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvAleph = new System.Windows.Forms.DataGridView();
            this.pationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnUnSearch = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMa = new System.Windows.Forms.TextBox();
            this.btnCreat = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.pb_TaiChinh = new System.Windows.Forms.PictureBox();
            this.btnThoat = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvLdap = new System.Windows.Forms.DataGridView();
            this.lbCountListExcel = new System.Windows.Forms.Label();
            this.lbCountHad = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAleph)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLdap)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvAleph);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(4, 246);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(498, 277);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DANH SÁCH BẠN ĐỌC CHƯA CÓ TÀI KHOẢN LDAP";
            // 
            // dgvAleph
            // 
            this.dgvAleph.AllowUserToAddRows = false;
            this.dgvAleph.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAleph.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAleph.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvAleph.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pationID,
            this.Ma,
            this.HoTen});
            this.dgvAleph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAleph.Location = new System.Drawing.Point(3, 18);
            this.dgvAleph.Name = "dgvAleph";
            this.dgvAleph.ReadOnly = true;
            this.dgvAleph.RowHeadersWidth = 20;
            this.dgvAleph.Size = new System.Drawing.Size(492, 256);
            this.dgvAleph.TabIndex = 18;
            this.dgvAleph.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DgvAleph_CellClick);
            // 
            // pationID
            // 
            this.pationID.DataPropertyName = "PatronId";
            this.pationID.HeaderText = "Patron ID";
            this.pationID.Name = "pationID";
            this.pationID.ReadOnly = true;
            // 
            // Ma
            // 
            this.Ma.DataPropertyName = "Ma";
            this.Ma.HeaderText = "Mã";
            this.Ma.Name = "Ma";
            this.Ma.ReadOnly = true;
            // 
            // HoTen
            // 
            this.HoTen.DataPropertyName = "HoTen";
            this.HoTen.HeaderText = "Họ tên";
            this.HoTen.Name = "HoTen";
            this.HoTen.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnUnSearch);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtPassword);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtPhone);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtEmail);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtMa);
            this.groupBox1.Controls.Add(this.btnCreat);
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.pb_TaiChinh);
            this.groupBox1.Controls.Add(this.btnThoat);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(984, 240);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thêm bạn đọc ldap";
            // 
            // btnUnSearch
            // 
            this.btnUnSearch.AutoSize = true;
            this.btnUnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnUnSearch.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnUnSearch.FlatAppearance.BorderSize = 0;
            this.btnUnSearch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUnSearch.ForeColor = System.Drawing.Color.White;
            this.btnUnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUnSearch.Location = new System.Drawing.Point(688, 164);
            this.btnUnSearch.Name = "btnUnSearch";
            this.btnUnSearch.Size = new System.Drawing.Size(103, 38);
            this.btnUnSearch.TabIndex = 134;
            this.btnUnSearch.Text = "Bỏ tìm";
            this.btnUnSearch.UseVisualStyleBackColor = false;
            this.btnUnSearch.Click += new System.EventHandler(this.BtnUnSearch_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(615, 93);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 25);
            this.label5.TabIndex = 133;
            this.label5.Text = "Tìm kiếm mã";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(571, 121);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 33);
            this.txtSearch.TabIndex = 132;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 25);
            this.label4.TabIndex = 131;
            this.label4.Text = "Mật khẩu";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(152, 165);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(281, 33);
            this.txtPassword.TabIndex = 130;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 25);
            this.label3.TabIndex = 129;
            this.label3.Text = "Phone";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(152, 127);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(281, 33);
            this.txtPhone.TabIndex = 128;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 91);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 25);
            this.label2.TabIndex = 127;
            this.label2.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(152, 88);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(281, 33);
            this.txtEmail.TabIndex = 126;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 25);
            this.label1.TabIndex = 125;
            this.label1.Text = "Mã SV/CB";
            // 
            // txtMa
            // 
            this.txtMa.Enabled = false;
            this.txtMa.Location = new System.Drawing.Point(152, 49);
            this.txtMa.Name = "txtMa";
            this.txtMa.Size = new System.Drawing.Size(281, 33);
            this.txtMa.TabIndex = 124;
            // 
            // btnCreat
            // 
            this.btnCreat.AutoSize = true;
            this.btnCreat.BackColor = System.Drawing.Color.Green;
            this.btnCreat.FlatAppearance.BorderSize = 0;
            this.btnCreat.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreat.ForeColor = System.Drawing.Color.White;
            this.btnCreat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCreat.Location = new System.Drawing.Point(152, 201);
            this.btnCreat.Name = "btnCreat";
            this.btnCreat.Size = new System.Drawing.Size(281, 35);
            this.btnCreat.TabIndex = 123;
            this.btnCreat.Text = "Tạo người dùng ldap";
            this.btnCreat.UseVisualStyleBackColor = false;
            this.btnCreat.Click += new System.EventHandler(this.BtnCreat_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(571, 164);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(114, 38);
            this.btnSearch.TabIndex = 122;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // pb_TaiChinh
            // 
            this.pb_TaiChinh.Image = global::TNUE_Patron_Excel.Properties.Resources.library_logo;
            this.pb_TaiChinh.Location = new System.Drawing.Point(818, 21);
            this.pb_TaiChinh.Name = "pb_TaiChinh";
            this.pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
            this.pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_TaiChinh.TabIndex = 20;
            this.pb_TaiChinh.TabStop = false;
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
            this.btnThoat.Location = new System.Drawing.Point(869, 196);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(109, 38);
            this.btnThoat.TabIndex = 14;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = false;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvLdap);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(508, 249);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(486, 277);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DANH SÁCH LDAP";
            // 
            // dgvLdap
            // 
            this.dgvLdap.AllowUserToAddRows = false;
            this.dgvLdap.AllowUserToDeleteRows = false;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLdap.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvLdap.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLdap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLdap.Location = new System.Drawing.Point(3, 18);
            this.dgvLdap.Name = "dgvLdap";
            this.dgvLdap.ReadOnly = true;
            this.dgvLdap.RowHeadersWidth = 20;
            this.dgvLdap.Size = new System.Drawing.Size(480, 256);
            this.dgvLdap.TabIndex = 19;
            // 
            // lbCountListExcel
            // 
            this.lbCountListExcel.AutoSize = true;
            this.lbCountListExcel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCountListExcel.Location = new System.Drawing.Point(9, 526);
            this.lbCountListExcel.Name = "lbCountListExcel";
            this.lbCountListExcel.Size = new System.Drawing.Size(76, 21);
            this.lbCountListExcel.TabIndex = 31;
            this.lbCountListExcel.Text = "Số lượng:";
            // 
            // lbCountHad
            // 
            this.lbCountHad.AutoSize = true;
            this.lbCountHad.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCountHad.Location = new System.Drawing.Point(507, 526);
            this.lbCountHad.Name = "lbCountHad";
            this.lbCountHad.Size = new System.Drawing.Size(76, 21);
            this.lbCountHad.TabIndex = 32;
            this.lbCountHad.Text = "Số lượng:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.lbCountListExcel);
            this.panel1.Controls.Add(this.lbCountHad);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1000, 565);
            this.panel1.TabIndex = 135;
            this.panel1.Visible = false;
            // 
            // UCInsertUserLdap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCInsertUserLdap";
            this.Size = new System.Drawing.Size(1000, 565);
            this.Load += new System.EventHandler(this.UCInsertUserLdap_Load);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAleph)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLdap)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
