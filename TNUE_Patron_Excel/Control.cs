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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TNUE_Patron_Excel.Control));
			label7 = new System.Windows.Forms.Label();
			_pnlLeft = new System.Windows.Forms.Panel();
			btnXoa = new System.Windows.Forms.Button();
			btnEditRemove = new System.Windows.Forms.Button();
			btnRest = new System.Windows.Forms.Button();
			btnAddMember = new System.Windows.Forms.Button();
			lbStatus = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			btnSinhVien = new System.Windows.Forms.Button();
			_btNhanVien = new System.Windows.Forms.Button();
			label5 = new System.Windows.Forms.Label();
			lblInformation = new System.Windows.Forms.Label();
			_pnlRight = new System.Windows.Forms.Panel();
			danhMucToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			đăngNhậpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			homeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			QLNDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			ConvertPicToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			dataPatronToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			trơGiupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			thôngTinPhiênBanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			liênHêToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			menuStrip1 = new System.Windows.Forms.MenuStrip();
			addLdapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			càiĐặtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			serverDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			serverLdapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			serverAlephToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
			_pnlLeft.SuspendLayout();
			menuStrip1.SuspendLayout();
			SuspendLayout();
			label7.AutoSize = true;
			label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			label7.ForeColor = System.Drawing.Color.FromArgb(0, 192, 0);
			label7.Location = new System.Drawing.Point(12, 24);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(178, 29);
			label7.TabIndex = 45;
			label7.Text = "TRANG CHÍNH";
			_pnlLeft.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			_pnlLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			_pnlLeft.Controls.Add(btnXoa);
			_pnlLeft.Controls.Add(btnEditRemove);
			_pnlLeft.Controls.Add(btnRest);
			_pnlLeft.Controls.Add(btnAddMember);
			_pnlLeft.Controls.Add(lbStatus);
			_pnlLeft.Controls.Add(label1);
			_pnlLeft.Controls.Add(btnSinhVien);
			_pnlLeft.Controls.Add(_btNhanVien);
			_pnlLeft.Controls.Add(label5);
			_pnlLeft.Location = new System.Drawing.Point(12, 56);
			_pnlLeft.Name = "_pnlLeft";
			_pnlLeft.Size = new System.Drawing.Size(178, 555);
			_pnlLeft.TabIndex = 46;
			btnXoa.Cursor = System.Windows.Forms.Cursors.Hand;
			btnXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btnXoa.Image = TNUE_Patron_Excel.Properties.Resources.zoom_search_2_icon___Copy;
			btnXoa.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			btnXoa.Location = new System.Drawing.Point(27, 340);
			btnXoa.Name = "btnXoa";
			btnXoa.Size = new System.Drawing.Size(125, 57);
			btnXoa.TabIndex = 54;
			btnXoa.Text = "Xóa";
			btnXoa.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btnXoa.UseVisualStyleBackColor = true;
			btnXoa.Click += new System.EventHandler(BtnXoa_Click);
			btnEditRemove.Cursor = System.Windows.Forms.Cursors.Hand;
			btnEditRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btnEditRemove.Image = TNUE_Patron_Excel.Properties.Resources.no_image_icon;
			btnEditRemove.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			btnEditRemove.Location = new System.Drawing.Point(27, 273);
			btnEditRemove.Name = "btnEditRemove";
			btnEditRemove.Size = new System.Drawing.Size(125, 61);
			btnEditRemove.TabIndex = 53;
			btnEditRemove.Text = "Cập nhập";
			btnEditRemove.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btnEditRemove.UseVisualStyleBackColor = true;
			btnEditRemove.Click += new System.EventHandler(btnPic_Click);
			btnRest.Cursor = System.Windows.Forms.Cursors.Hand;
			btnRest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btnRest.Image = TNUE_Patron_Excel.Properties.Resources.reset_50x48;
			btnRest.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			btnRest.Location = new System.Drawing.Point(27, 466);
			btnRest.Name = "btnRest";
			btnRest.Size = new System.Drawing.Size(125, 82);
			btnRest.TabIndex = 52;
			btnRest.Text = "Làm mới dữ liệu";
			btnRest.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btnRest.UseVisualStyleBackColor = true;
			btnRest.Click += new System.EventHandler(btnRest_Click);
			btnRest.MouseLeave += new System.EventHandler(btnRest_MouseLeave);
			btnRest.MouseMove += new System.Windows.Forms.MouseEventHandler(btnRest_MouseMove);
			btnAddMember.Cursor = System.Windows.Forms.Cursors.Hand;
			btnAddMember.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btnAddMember.Image = TNUE_Patron_Excel.Properties.Resources.zoom_search_2_icon___Copy;
			btnAddMember.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			btnAddMember.Location = new System.Drawing.Point(27, 210);
			btnAddMember.Name = "btnAddMember";
			btnAddMember.Size = new System.Drawing.Size(125, 57);
			btnAddMember.TabIndex = 51;
			btnAddMember.Text = "Thêm bạn đọc";
			btnAddMember.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btnAddMember.UseVisualStyleBackColor = true;
			btnAddMember.Click += new System.EventHandler(btnData_Click);
			btnAddMember.MouseLeave += new System.EventHandler(btnData_MouseLeave);
			btnAddMember.MouseMove += new System.Windows.Forms.MouseEventHandler(btnData_MouseMove);
			lbStatus.AutoSize = true;
			lbStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
			lbStatus.ForeColor = System.Drawing.Color.Black;
			lbStatus.Location = new System.Drawing.Point(3, 435);
			lbStatus.Name = "lbStatus";
			lbStatus.Size = new System.Drawing.Size(102, 24);
			lbStatus.TabIndex = 50;
			lbStatus.Text = "Trạng Thái";
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
			label1.ForeColor = System.Drawing.Color.Black;
			label1.Location = new System.Drawing.Point(33, 400);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(102, 24);
			label1.TabIndex = 49;
			label1.Text = "Trạng Thái";
			btnSinhVien.Cursor = System.Windows.Forms.Cursors.Hand;
			btnSinhVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			btnSinhVien.Image = TNUE_Patron_Excel.Properties.Resources.username_login_50x50;
			btnSinhVien.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			btnSinhVien.Location = new System.Drawing.Point(27, 122);
			btnSinhVien.Name = "btnSinhVien";
			btnSinhVien.Size = new System.Drawing.Size(125, 82);
			btnSinhVien.TabIndex = 39;
			btnSinhVien.Text = "Sinh Viên";
			btnSinhVien.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			btnSinhVien.UseVisualStyleBackColor = true;
			btnSinhVien.Click += new System.EventHandler(btnSinhVien_Click);
			btnSinhVien.MouseLeave += new System.EventHandler(btnSinhVien_MouseLeave);
			btnSinhVien.MouseMove += new System.Windows.Forms.MouseEventHandler(btnSinhVien_MouseMove);
			_btNhanVien.Cursor = System.Windows.Forms.Cursors.Hand;
			_btNhanVien.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			_btNhanVien.Image = TNUE_Patron_Excel.Properties.Resources.customer_service_icon_48x48;
			_btNhanVien.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
			_btNhanVien.Location = new System.Drawing.Point(27, 34);
			_btNhanVien.Name = "_btNhanVien";
			_btNhanVien.Size = new System.Drawing.Size(125, 82);
			_btNhanVien.TabIndex = 2;
			_btNhanVien.Text = "Cán Bộ";
			_btNhanVien.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
			_btNhanVien.UseVisualStyleBackColor = true;
			_btNhanVien.Click += new System.EventHandler(_btNhanVien_Click);
			_btNhanVien.MouseLeave += new System.EventHandler(_btNhanVien_MouseLeave);
			_btNhanVien.MouseMove += new System.Windows.Forms.MouseEventHandler(_btNhanVien_MouseMove);
			label5.AutoSize = true;
			label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			label5.ForeColor = System.Drawing.SystemColors.Highlight;
			label5.Location = new System.Drawing.Point(32, 4);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(120, 29);
			label5.TabIndex = 38;
			label5.Text = "Danh Mục";
			lblInformation.AutoSize = true;
			lblInformation.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 163);
			lblInformation.ForeColor = System.Drawing.Color.Black;
			lblInformation.Location = new System.Drawing.Point(556, 28);
			lblInformation.Name = "lblInformation";
			lblInformation.Size = new System.Drawing.Size(91, 24);
			lblInformation.TabIndex = 48;
			lblInformation.Text = "Xin chào ";
			_pnlRight.BackColor = System.Drawing.SystemColors.ButtonHighlight;
			_pnlRight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			_pnlRight.Location = new System.Drawing.Point(196, 56);
			_pnlRight.Name = "_pnlRight";
			_pnlRight.Size = new System.Drawing.Size(1000, 555);
			_pnlRight.TabIndex = 47;
			danhMucToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[5]
			{
				đăngNhậpToolStripMenuItem,
				homeToolStripMenuItem,
				QLNDToolStripMenuItem,
				ConvertPicToolStripMenuItem,
				dataPatronToolStripMenuItem
			});
			danhMucToolStripMenuItem.Name = "danhMucToolStripMenuItem";
			danhMucToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
			danhMucToolStripMenuItem.Text = "Danh Mu\u0323c";
			đăngNhậpToolStripMenuItem.Image = TNUE_Patron_Excel.Properties.Resources.login_icon50x50;
			đăngNhậpToolStripMenuItem.Name = "đăngNhậpToolStripMenuItem";
			đăngNhậpToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			đăngNhậpToolStripMenuItem.Text = "Đăng nhập";
			đăngNhậpToolStripMenuItem.Click += new System.EventHandler(đăngNhậpToolStripMenuItem_Click);
			homeToolStripMenuItem.Image = TNUE_Patron_Excel.Properties.Resources.home;
			homeToolStripMenuItem.Name = "homeToolStripMenuItem";
			homeToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			homeToolStripMenuItem.Text = "Trang Chính";
			homeToolStripMenuItem.Click += new System.EventHandler(HomeToolStripMenuItem_Click);
			QLNDToolStripMenuItem.Image = TNUE_Patron_Excel.Properties.Resources.username_login_50x50;
			QLNDToolStripMenuItem.Name = "QLNDToolStripMenuItem";
			QLNDToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			QLNDToolStripMenuItem.Text = "Quản Lý Người Dùng";
			QLNDToolStripMenuItem.Click += new System.EventHandler(QLNDToolStripMenuItem_Click);
			ConvertPicToolStripMenuItem.Image = TNUE_Patron_Excel.Properties.Resources.no_image_icon;
			ConvertPicToolStripMenuItem.Name = "ConvertPicToolStripMenuItem";
			ConvertPicToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			ConvertPicToolStripMenuItem.Text = "Chuyển đổi ảnh người dùng";
			ConvertPicToolStripMenuItem.Click += new System.EventHandler(ConvertPicToolStripMenuItem_Click);
			dataPatronToolStripMenuItem.Name = "dataPatronToolStripMenuItem";
			dataPatronToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
			dataPatronToolStripMenuItem.Text = "Data Patron";
			dataPatronToolStripMenuItem.Click += new System.EventHandler(DataPatronToolStripMenuItem_Click);
			helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3]
			{
				trơGiupToolStripMenuItem,
				thôngTinPhiênBanToolStripMenuItem,
				liênHêToolStripMenuItem
			});
			helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
			helpToolStripMenuItem.Text = "Help";
			trơGiupToolStripMenuItem.Name = "trơGiupToolStripMenuItem";
			trơGiupToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
			trơGiupToolStripMenuItem.Text = "Trơ\u0323 Giu\u0301p";
			thôngTinPhiênBanToolStripMenuItem.Name = "thôngTinPhiênBanToolStripMenuItem";
			thôngTinPhiênBanToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
			thôngTinPhiênBanToolStripMenuItem.Text = "Thông Tin Phiên Ba\u0309n";
			liênHêToolStripMenuItem.Name = "liênHêToolStripMenuItem";
			liênHêToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
			liênHêToolStripMenuItem.Text = "Liên Hê\u0323";
			menuStrip1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
			menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4]
			{
				danhMucToolStripMenuItem,
				addLdapToolStripMenuItem,
				càiĐặtToolStripMenuItem,
				helpToolStripMenuItem
			});
			menuStrip1.Location = new System.Drawing.Point(0, 0);
			menuStrip1.Name = "menuStrip1";
			menuStrip1.Size = new System.Drawing.Size(1209, 24);
			menuStrip1.TabIndex = 44;
			menuStrip1.Text = "menuStrip1";
			addLdapToolStripMenuItem.Name = "addLdapToolStripMenuItem";
			addLdapToolStripMenuItem.Size = new System.Drawing.Size(109, 20);
			addLdapToolStripMenuItem.Text = "Thêm bạn đọc ldap";
			addLdapToolStripMenuItem.Click += new System.EventHandler(AddLdapToolStripMenuItem_Click);
			càiĐặtToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[3]
			{
				serverDatabaseToolStripMenuItem,
				serverLdapToolStripMenuItem,
				serverAlephToolStripMenuItem
			});
			càiĐặtToolStripMenuItem.Name = "càiĐặtToolStripMenuItem";
			càiĐặtToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
			càiĐặtToolStripMenuItem.Text = "Cài Đặt";
			serverDatabaseToolStripMenuItem.Image = TNUE_Patron_Excel.Properties.Resources.database;
			serverDatabaseToolStripMenuItem.Name = "serverDatabaseToolStripMenuItem";
			serverDatabaseToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			serverDatabaseToolStripMenuItem.Text = "Server Database";
			serverDatabaseToolStripMenuItem.Click += new System.EventHandler(serverDatabaseToolStripMenuItem_Click);
			serverLdapToolStripMenuItem.Image = TNUE_Patron_Excel.Properties.Resources.simpleid_icon_adapt;
			serverLdapToolStripMenuItem.Name = "serverLdapToolStripMenuItem";
			serverLdapToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			serverLdapToolStripMenuItem.Text = "Server Ldap";
			serverLdapToolStripMenuItem.Click += new System.EventHandler(serverLdapToolStripMenuItem_Click);
			serverAlephToolStripMenuItem.Image = TNUE_Patron_Excel.Properties.Resources.mindtouch;
			serverAlephToolStripMenuItem.Name = "serverAlephToolStripMenuItem";
			serverAlephToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
			serverAlephToolStripMenuItem.Text = "Server Aleph";
			serverAlephToolStripMenuItem.Click += new System.EventHandler(serverAlephToolStripMenuItem_Click);
			backgroundWorker1.WorkerSupportsCancellation = true;
			backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker1_DoWork);
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(1209, 618);
			base.Controls.Add(label7);
			base.Controls.Add(_pnlLeft);
			base.Controls.Add(lblInformation);
			base.Controls.Add(_pnlRight);
			base.Controls.Add(menuStrip1);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Name = "Control";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "TNUE Patron";
			base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(Control_FormClosed);
			base.Load += new System.EventHandler(Control_Load);
			_pnlLeft.ResumeLayout(false);
			_pnlLeft.PerformLayout();
			menuStrip1.ResumeLayout(false);
			menuStrip1.PerformLayout();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
