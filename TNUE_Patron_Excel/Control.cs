using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TNUE_Patron_Excel.Config;
using TNUE_Patron_Excel.ControlMember;
using TNUE_Patron_Excel.DBConnect;
using TNUE_Patron_Excel.Properties;

namespace TNUE_Patron_Excel
{
	public class Control : Form
	{
		public string errorText = "Không kết nối được đến server";

		private IContainer components = null;

		private Label label7;

		private Panel _pnlLeft;

		private Label label5;

		private Label lblInformation;

		private Panel _pnlRight;

		private Button _btNhanVien;

		private Button btnSinhVien;

		private Label lbStatus;

		private Label label1;

		private Button btnAddMember;

		private Button btnRest;

		private ToolStripMenuItem danhMucToolStripMenuItem;

		private ToolStripMenuItem helpToolStripMenuItem;

		private ToolStripMenuItem trơGiupToolStripMenuItem;

		private ToolStripMenuItem thôngTinPhiênBanToolStripMenuItem;

		private ToolStripMenuItem liênHêToolStripMenuItem;

		private MenuStrip menuStrip1;

		private BackgroundWorker backgroundWorker1;

		private Button btnEditRemove;

		private ToolStripMenuItem càiĐặtToolStripMenuItem;

		private ToolStripMenuItem serverDatabaseToolStripMenuItem;

		private ToolStripMenuItem serverLdapToolStripMenuItem;

		private ToolStripMenuItem serverAlephToolStripMenuItem;

		private ToolStripMenuItem đăngNhậpToolStripMenuItem;

		private ToolStripMenuItem QLNDToolStripMenuItem;

		private ToolStripMenuItem homeToolStripMenuItem;

		private ToolStripMenuItem ConvertPicToolStripMenuItem;

		private ToolStripMenuItem dataPatronToolStripMenuItem;

		private Button btnXoa;

		private ToolStripMenuItem addLdapToolStripMenuItem;

		public Control()
		{
			InitializeComponent();
		}

		private void Control_Load(object sender, EventArgs e)
		{
		}

		private void btnSinhVien_Click(object sender, EventArgs e)
		{
			_pnlRight.Controls.Clear();
			UCSinhVien uCSinhVien = new UCSinhVien();
			_pnlRight.Controls.Add(uCSinhVien);
			uCSinhVien.Show();
			lblInformation.Text = "Chức năng: Sinh viên";
		}

		private void _btNhanVien_Click(object sender, EventArgs e)
		{
			_pnlRight.Controls.Clear();
			UCCanBo uCCanBo = new UCCanBo();
			_pnlRight.Controls.Add(uCCanBo);
			uCCanBo.Show();
			lblInformation.Text = "Chức năng: Cán bộ";
		}

		private void btnData_Click(object sender, EventArgs e)
		{
			_pnlRight.Controls.Clear();
			AddEditMember addEditMember = new AddEditMember();
			_pnlRight.Controls.Add(addEditMember);
			addEditMember.Show();
			lblInformation.Text = "Chức năng: Thêm bạn đọc";
		}

		private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
		{
			Invoke((MethodInvoker)delegate
			{
				lblInformation.Text = "Đang cập nhập lại dữ liệu từ server....";
				EnabledPanl(bl: false);
			});
			DataDBLocal.listZ308 = new QueryDB().listZ308TED();
			Invoke((MethodInvoker)delegate
			{
				EnabledPanl(bl: true);
				lblInformation.Text = "Cập nhập thành công!";
				LoadForm();
			});
		}

		private void btnRest_Click(object sender, EventArgs e)
		{
			restConTrol();
		}

		private void btnPic_Click(object sender, EventArgs e)
		{
			_pnlRight.Controls.Clear();
			UCUpdatePatron uCUpdatePatron = new UCUpdatePatron();
			_pnlRight.Controls.Add(uCUpdatePatron);
			uCUpdatePatron.Show();
			lblInformation.Text = "Chức năng: Cập nhập bạn đọc";
		}

		private void QLNDToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_pnlRight.Controls.Clear();
			UCControlMember uCControlMember = new UCControlMember();
			_pnlRight.Controls.Add(uCControlMember);
			uCControlMember.Show();
			lblInformation.Text = "Chức năng: Control Member";
		}

		private void ConvertPicToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_pnlRight.Controls.Clear();
			UCConvertPic uCConvertPic = new UCConvertPic();
			_pnlRight.Enabled = true;
			_pnlRight.Controls.Add(uCConvertPic);
			uCConvertPic.Show();
			lblInformation.Text = "Chức năng: Chuyển đổi tên ảnh";
		}

		private void DataPatronToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_pnlRight.Controls.Clear();
			UCDataPatronZ308 uCDataPatronZ = new UCDataPatronZ308();
			_pnlRight.Controls.Add(uCDataPatronZ);
			uCDataPatronZ.Show();
			lblInformation.Text = "Chức năng: Dữ liệu Z308";
		}

		private bool TestConnecting()
		{
			bool result = false;
			try
			{
				DataOracle oracle = new ReadWriterConfig().ReadConfigDataBase();
				DBConnecting.conn = DBConnecting.GetDBConnection(oracle);
				DBConnecting.conn.Open();
				lbStatus.Text = "Kết nối thành công";
				result = true;
				DBConnecting.conn.Close();
			}
			catch (Exception ex)
			{
				DBConnecting.conn.Close();
				lbStatus.Text = "Kết nối không thành công";
				MessageBox.Show("Lỗi: " + ex.Message, "Thông báo!");
			}
			return result;
		}

		public void EnabledPanl(bool bl)
		{
			_pnlLeft.Enabled = bl;
			_pnlRight.Enabled = bl;
			addLdapToolStripMenuItem.Enabled = bl;
			homeToolStripMenuItem.Enabled = bl;
			QLNDToolStripMenuItem.Enabled = bl;
			dataPatronToolStripMenuItem.Enabled = bl;
		}

		public void LoadForm()
		{
			_pnlRight.Controls.Clear();
			UCCanBo uCCanBo = new UCCanBo();
			_pnlRight.Controls.Add(uCCanBo);
			uCCanBo.Show();
			lblInformation.Text = "Chức năng: Cán bộ";
		}

		public void restConTrol()
		{
			Process.Start(Application.ExecutablePath);
			Close();
		}

		private void _btNhanVien_MouseLeave(object sender, EventArgs e)
		{
			_btNhanVien.BackColor = SystemColors.Control;
			_btNhanVien.BackgroundImage = null;
		}

		private void _btNhanVien_MouseMove(object sender, MouseEventArgs e)
		{
			_btNhanVien.BackgroundImage = Resources.background;
		}

		private void btnSinhVien_MouseLeave(object sender, EventArgs e)
		{
			btnSinhVien.BackColor = SystemColors.Control;
			btnSinhVien.BackgroundImage = null;
		}

		private void btnSinhVien_MouseMove(object sender, MouseEventArgs e)
		{
			btnSinhVien.BackgroundImage = Resources.background;
		}

		private void btnData_MouseLeave(object sender, EventArgs e)
		{
			btnAddMember.BackColor = SystemColors.Control;
			btnAddMember.BackgroundImage = null;
		}

		private void btnData_MouseMove(object sender, MouseEventArgs e)
		{
			btnAddMember.BackgroundImage = Resources.background;
		}

		private void btnRest_MouseLeave(object sender, EventArgs e)
		{
			btnRest.BackColor = SystemColors.Control;
			btnRest.BackgroundImage = null;
		}

		private void btnRest_MouseMove(object sender, MouseEventArgs e)
		{
			btnRest.BackgroundImage = Resources.background;
		}

		private void serverDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ConfigDataBase configDataBase = new ConfigDataBase();
			configDataBase.ShowDialog();
		}

		private void serverLdapToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ConfigLdap configLdap = new ConfigLdap();
			configLdap.ShowDialog();
		}

		private void serverAlephToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ConfigAleph configAleph = new ConfigAleph();
			configAleph.ShowDialog();
		}

		private void đăngNhậpToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		public void checkMenu()
		{
			if (DTOlogin.isLogin == 0)
			{
				serverDatabaseToolStripMenuItem.Enabled = false;
				serverLdapToolStripMenuItem.Enabled = false;
				serverAlephToolStripMenuItem.Enabled = false;
				đăngNhậpToolStripMenuItem.Enabled = true;
			}
			else
			{
				serverDatabaseToolStripMenuItem.Enabled = true;
				serverLdapToolStripMenuItem.Enabled = true;
				serverAlephToolStripMenuItem.Enabled = true;
				đăngNhậpToolStripMenuItem.Enabled = false;
			}
		}

		private void Control_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void BtnXoa_Click(object sender, EventArgs e)
		{
			_pnlRight.Controls.Clear();
			UCDeleteUser uCDeleteUser = new UCDeleteUser();
			_pnlRight.Controls.Add(uCDeleteUser);
			uCDeleteUser.Show();
			lblInformation.Text = "Chức năng: Xóa bạn đọc";
		}

		private void AddLdapToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_pnlRight.Controls.Clear();
			UCInsertUserLdap uCInsertUserLdap = new UCInsertUserLdap();
			_pnlRight.Controls.Add(uCInsertUserLdap);
			uCInsertUserLdap.Show();
			lblInformation.Text = "Chức năng: Thêm bạn đọc ldap";
		}

		private void HomeToolStripMenuItem_Click(object sender, EventArgs e)
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Control));
            this.label7 = new System.Windows.Forms.Label();
            this._pnlLeft = new System.Windows.Forms.Panel();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnEditRemove = new System.Windows.Forms.Button();
            this.btnRest = new System.Windows.Forms.Button();
            this.btnAddMember = new System.Windows.Forms.Button();
            this.lbStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSinhVien = new System.Windows.Forms.Button();
            this._btNhanVien = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblInformation = new System.Windows.Forms.Label();
            this._pnlRight = new System.Windows.Forms.Panel();
            this.danhMucToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.đăngNhậpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.QLNDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ConvertPicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataPatronToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trơGiupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.thôngTinPhiênBanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.liênHêToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.addLdapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.càiĐặtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverLdapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.serverAlephToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this._pnlLeft.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label7.Location = new System.Drawing.Point(12, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(178, 29);
            this.label7.TabIndex = 45;
            this.label7.Text = "TRANG CHÍNH";
            // 
            // _pnlLeft
            // 
            this._pnlLeft.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this._pnlLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._pnlLeft.Controls.Add(this.btnXoa);
            this._pnlLeft.Controls.Add(this.btnEditRemove);
            this._pnlLeft.Controls.Add(this.btnRest);
            this._pnlLeft.Controls.Add(this.btnAddMember);
            this._pnlLeft.Controls.Add(this.lbStatus);
            this._pnlLeft.Controls.Add(this.label1);
            this._pnlLeft.Controls.Add(this.btnSinhVien);
            this._pnlLeft.Controls.Add(this._btNhanVien);
            this._pnlLeft.Controls.Add(this.label5);
            this._pnlLeft.Location = new System.Drawing.Point(12, 56);
            this._pnlLeft.Name = "_pnlLeft";
            this._pnlLeft.Size = new System.Drawing.Size(178, 591);
            this._pnlLeft.TabIndex = 46;
            // 
            // btnXoa
            // 
            this.btnXoa.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnXoa.Image = global::TNUE_Patron_Excel.Properties.Resources.zoom_search_2_icon___Copy;
            this.btnXoa.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnXoa.Location = new System.Drawing.Point(27, 340);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(125, 57);
            this.btnXoa.TabIndex = 54;
            this.btnXoa.Text = "Block - xóa";
            this.btnXoa.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.BtnXoa_Click);
            // 
            // btnEditRemove
            // 
            this.btnEditRemove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEditRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEditRemove.Image = global::TNUE_Patron_Excel.Properties.Resources.no_image_icon;
            this.btnEditRemove.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnEditRemove.Location = new System.Drawing.Point(27, 273);
            this.btnEditRemove.Name = "btnEditRemove";
            this.btnEditRemove.Size = new System.Drawing.Size(125, 61);
            this.btnEditRemove.TabIndex = 53;
            this.btnEditRemove.Text = "Cập nhập";
            this.btnEditRemove.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnEditRemove.UseVisualStyleBackColor = true;
            this.btnEditRemove.Click += new System.EventHandler(this.btnPic_Click);
            // 
            // btnRest
            // 
            this.btnRest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRest.Image = global::TNUE_Patron_Excel.Properties.Resources.reset_50x48;
            this.btnRest.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRest.Location = new System.Drawing.Point(27, 498);
            this.btnRest.Name = "btnRest";
            this.btnRest.Size = new System.Drawing.Size(125, 82);
            this.btnRest.TabIndex = 52;
            this.btnRest.Text = "Làm mới dữ liệu";
            this.btnRest.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRest.UseVisualStyleBackColor = true;
            this.btnRest.Click += new System.EventHandler(this.btnRest_Click);
            this.btnRest.MouseLeave += new System.EventHandler(this.btnRest_MouseLeave);
            this.btnRest.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnRest_MouseMove);
            // 
            // btnAddMember
            // 
            this.btnAddMember.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddMember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddMember.Image = global::TNUE_Patron_Excel.Properties.Resources.zoom_search_2_icon___Copy;
            this.btnAddMember.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddMember.Location = new System.Drawing.Point(27, 210);
            this.btnAddMember.Name = "btnAddMember";
            this.btnAddMember.Size = new System.Drawing.Size(125, 57);
            this.btnAddMember.TabIndex = 51;
            this.btnAddMember.Text = "Thêm bạn đọc";
            this.btnAddMember.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddMember.UseVisualStyleBackColor = true;
            this.btnAddMember.Click += new System.EventHandler(this.btnData_Click);
            this.btnAddMember.MouseLeave += new System.EventHandler(this.btnData_MouseLeave);
            this.btnAddMember.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnData_MouseMove);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lbStatus.ForeColor = System.Drawing.Color.Black;
            this.lbStatus.Location = new System.Drawing.Point(3, 467);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(102, 24);
            this.lbStatus.TabIndex = 50;
            this.lbStatus.Text = "Trạng Thái";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(33, 432);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 24);
            this.label1.TabIndex = 49;
            this.label1.Text = "Trạng Thái";
            // 
            // btnSinhVien
            // 
            this.btnSinhVien.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSinhVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSinhVien.Image = global::TNUE_Patron_Excel.Properties.Resources.username_login_50x50;
            this.btnSinhVien.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSinhVien.Location = new System.Drawing.Point(27, 122);
            this.btnSinhVien.Name = "btnSinhVien";
            this.btnSinhVien.Size = new System.Drawing.Size(125, 82);
            this.btnSinhVien.TabIndex = 39;
            this.btnSinhVien.Text = "Sinh Viên";
            this.btnSinhVien.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSinhVien.UseVisualStyleBackColor = true;
            this.btnSinhVien.Click += new System.EventHandler(this.btnSinhVien_Click);
            this.btnSinhVien.MouseLeave += new System.EventHandler(this.btnSinhVien_MouseLeave);
            this.btnSinhVien.MouseMove += new System.Windows.Forms.MouseEventHandler(this.btnSinhVien_MouseMove);
            // 
            // _btNhanVien
            // 
            this._btNhanVien.Cursor = System.Windows.Forms.Cursors.Hand;
            this._btNhanVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this._btNhanVien.Image = global::TNUE_Patron_Excel.Properties.Resources.customer_service_icon_48x48;
            this._btNhanVien.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this._btNhanVien.Location = new System.Drawing.Point(27, 34);
            this._btNhanVien.Name = "_btNhanVien";
            this._btNhanVien.Size = new System.Drawing.Size(125, 82);
            this._btNhanVien.TabIndex = 2;
            this._btNhanVien.Text = "Cán Bộ";
            this._btNhanVien.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this._btNhanVien.UseVisualStyleBackColor = true;
            this._btNhanVien.Click += new System.EventHandler(this._btNhanVien_Click);
            this._btNhanVien.MouseLeave += new System.EventHandler(this._btNhanVien_MouseLeave);
            this._btNhanVien.MouseMove += new System.Windows.Forms.MouseEventHandler(this._btNhanVien_MouseMove);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label5.Location = new System.Drawing.Point(32, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(120, 29);
            this.label5.TabIndex = 38;
            this.label5.Text = "Danh Mục";
            // 
            // lblInformation
            // 
            this.lblInformation.AutoSize = true;
            this.lblInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lblInformation.ForeColor = System.Drawing.Color.Black;
            this.lblInformation.Location = new System.Drawing.Point(556, 28);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(91, 24);
            this.lblInformation.TabIndex = 48;
            this.lblInformation.Text = "Xin chào ";
            // 
            // _pnlRight
            // 
            this._pnlRight.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this._pnlRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._pnlRight.Location = new System.Drawing.Point(196, 56);
            this._pnlRight.Name = "_pnlRight";
            this._pnlRight.Size = new System.Drawing.Size(1013, 591);
            this._pnlRight.TabIndex = 47;
            // 
            // danhMucToolStripMenuItem
            // 
            this.danhMucToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.đăngNhậpToolStripMenuItem,
            this.homeToolStripMenuItem,
            this.QLNDToolStripMenuItem,
            this.ConvertPicToolStripMenuItem,
            this.dataPatronToolStripMenuItem});
            this.danhMucToolStripMenuItem.Name = "danhMucToolStripMenuItem";
            this.danhMucToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.danhMucToolStripMenuItem.Text = "Danh Mục";
            // 
            // đăngNhậpToolStripMenuItem
            // 
            this.đăngNhậpToolStripMenuItem.Image = global::TNUE_Patron_Excel.Properties.Resources.login_icon50x50;
            this.đăngNhậpToolStripMenuItem.Name = "đăngNhậpToolStripMenuItem";
            this.đăngNhậpToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.đăngNhậpToolStripMenuItem.Text = "Đăng nhập";
            this.đăngNhậpToolStripMenuItem.Click += new System.EventHandler(this.đăngNhậpToolStripMenuItem_Click);
            // 
            // homeToolStripMenuItem
            // 
            this.homeToolStripMenuItem.Image = global::TNUE_Patron_Excel.Properties.Resources.home;
            this.homeToolStripMenuItem.Name = "homeToolStripMenuItem";
            this.homeToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.homeToolStripMenuItem.Text = "Trang Chính";
            this.homeToolStripMenuItem.Click += new System.EventHandler(this.HomeToolStripMenuItem_Click);
            // 
            // QLNDToolStripMenuItem
            // 
            this.QLNDToolStripMenuItem.Image = global::TNUE_Patron_Excel.Properties.Resources.username_login_50x50;
            this.QLNDToolStripMenuItem.Name = "QLNDToolStripMenuItem";
            this.QLNDToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.QLNDToolStripMenuItem.Text = "Quản Lý Người Dùng";
            this.QLNDToolStripMenuItem.Click += new System.EventHandler(this.QLNDToolStripMenuItem_Click);
            // 
            // ConvertPicToolStripMenuItem
            // 
            this.ConvertPicToolStripMenuItem.Image = global::TNUE_Patron_Excel.Properties.Resources.no_image_icon;
            this.ConvertPicToolStripMenuItem.Name = "ConvertPicToolStripMenuItem";
            this.ConvertPicToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.ConvertPicToolStripMenuItem.Text = "Chuyển đổi ảnh người dùng";
            this.ConvertPicToolStripMenuItem.Click += new System.EventHandler(this.ConvertPicToolStripMenuItem_Click);
            // 
            // dataPatronToolStripMenuItem
            // 
            this.dataPatronToolStripMenuItem.Name = "dataPatronToolStripMenuItem";
            this.dataPatronToolStripMenuItem.Size = new System.Drawing.Size(223, 22);
            this.dataPatronToolStripMenuItem.Text = "Data Patron";
            this.dataPatronToolStripMenuItem.Click += new System.EventHandler(this.DataPatronToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.trơGiupToolStripMenuItem,
            this.thôngTinPhiênBanToolStripMenuItem,
            this.liênHêToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // trơGiupToolStripMenuItem
            // 
            this.trơGiupToolStripMenuItem.Name = "trơGiupToolStripMenuItem";
            this.trơGiupToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.trơGiupToolStripMenuItem.Text = "Trợ Giúp";
            // 
            // thôngTinPhiênBanToolStripMenuItem
            // 
            this.thôngTinPhiênBanToolStripMenuItem.Name = "thôngTinPhiênBanToolStripMenuItem";
            this.thôngTinPhiênBanToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.thôngTinPhiênBanToolStripMenuItem.Text = "Thông Tin Phiên Bản";
            // 
            // liênHêToolStripMenuItem
            // 
            this.liênHêToolStripMenuItem.Name = "liênHêToolStripMenuItem";
            this.liênHêToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.liênHêToolStripMenuItem.Text = "Liên Hệ";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.danhMucToolStripMenuItem,
            this.addLdapToolStripMenuItem,
            this.càiĐặtToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1211, 24);
            this.menuStrip1.TabIndex = 44;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // addLdapToolStripMenuItem
            // 
            this.addLdapToolStripMenuItem.Name = "addLdapToolStripMenuItem";
            this.addLdapToolStripMenuItem.Size = new System.Drawing.Size(121, 20);
            this.addLdapToolStripMenuItem.Text = "Thêm bạn đọc ldap";
            this.addLdapToolStripMenuItem.Click += new System.EventHandler(this.AddLdapToolStripMenuItem_Click);
            // 
            // càiĐặtToolStripMenuItem
            // 
            this.càiĐặtToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.serverDatabaseToolStripMenuItem,
            this.serverLdapToolStripMenuItem,
            this.serverAlephToolStripMenuItem});
            this.càiĐặtToolStripMenuItem.Name = "càiĐặtToolStripMenuItem";
            this.càiĐặtToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.càiĐặtToolStripMenuItem.Text = "Cài Đặt";
            // 
            // serverDatabaseToolStripMenuItem
            // 
            this.serverDatabaseToolStripMenuItem.Image = global::TNUE_Patron_Excel.Properties.Resources.database;
            this.serverDatabaseToolStripMenuItem.Name = "serverDatabaseToolStripMenuItem";
            this.serverDatabaseToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.serverDatabaseToolStripMenuItem.Text = "Server Database";
            this.serverDatabaseToolStripMenuItem.Click += new System.EventHandler(this.serverDatabaseToolStripMenuItem_Click);
            // 
            // serverLdapToolStripMenuItem
            // 
            this.serverLdapToolStripMenuItem.Image = global::TNUE_Patron_Excel.Properties.Resources.simpleid_icon_adapt;
            this.serverLdapToolStripMenuItem.Name = "serverLdapToolStripMenuItem";
            this.serverLdapToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.serverLdapToolStripMenuItem.Text = "Server Ldap";
            this.serverLdapToolStripMenuItem.Click += new System.EventHandler(this.serverLdapToolStripMenuItem_Click);
            // 
            // serverAlephToolStripMenuItem
            // 
            this.serverAlephToolStripMenuItem.Image = global::TNUE_Patron_Excel.Properties.Resources.mindtouch;
            this.serverAlephToolStripMenuItem.Name = "serverAlephToolStripMenuItem";
            this.serverAlephToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.serverAlephToolStripMenuItem.Text = "Server Aleph";
            this.serverAlephToolStripMenuItem.Click += new System.EventHandler(this.serverAlephToolStripMenuItem_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // Control
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 650);
            this.Controls.Add(this.label7);
            this.Controls.Add(this._pnlLeft);
            this.Controls.Add(this.lblInformation);
            this.Controls.Add(this._pnlRight);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Control";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TNUE Patron";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Control_FormClosed);
            this.Load += new System.EventHandler(this.Control_Load);
            this._pnlLeft.ResumeLayout(false);
            this._pnlLeft.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
