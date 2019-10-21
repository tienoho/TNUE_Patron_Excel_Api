using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TNUE_Patron_Excel.API;
using TNUE_Patron_Excel.DBConnect;
using TNUE_Patron_Excel.Ldap;
using TNUE_Patron_Excel.Properties;
using TNUE_Patron_Excel.Tool;
using TNUE_Patron_Excel.Z303;

namespace TNUE_Patron_Excel.ControlMember
{
	public class AddEditMember : UserControl
	{
		private List<Z308> listZ308 = null;

		private ToolP tool = new ToolP();

		private StringBuilder sbList = null;

		private StringBuilder sbPatronXml;

		private StringBuilder sb = null;

		private Patron p = null;

		private User u = null;

		private int countP = 1;

		private string directoryPath = DataDBLocal.pathUserLog;

        private IContainer components = null;

		private GroupBox groupBox1;

		private Button btnThoat;

		private TextBox txtPatronId;

		private Label label4;

		private Label label5;

		private ComboBox comboBox1;

		private Button btnConvert;

		private FolderBrowserDialog folderBrowserDialog1;

		private Button btnPush;

		private PictureBox pb_TaiChinh;

		private Label label7;

		private ComboBox cbLoaiBanDoc;

		private TextBox txtPassword;

		private Label label2;

		private TextBox txtMa;

		private Label label1;

		private Label label3;

		private ComboBox cbGioiTinh;

		private Label label6;

		private DateTimePicker dtpNgaySinh;

		private DateTimePicker dateHetHan;

		private Label label9;

		private TextBox txtPhone;

		private Label label8;

		private TextBox txtAddress;

		private Label label10;

		private Panel panel1;

		private GroupBox groupBox2;

		private RadioButton rbSinhVien;

		private RadioButton rbCanBo;

		private Panel panelSinhVien;

		private TextBox txtEmail;

		private Label label11;

		private Label label13;

		private TextBox txtLop;

		private Label label12;

		private TextBox txtKhoa;

		private Label label14;

		private TextBox txtKhoaHoc;

		private Panel panelCanBo;

		private Label label15;

		private TextBox txtChucDanh;

		private Label label16;

		private TextBox txtChucVu;

		private Label label17;

		private TextBox txtDonVi;

		private Label lbErrorBarcode;

		private Label label18;

		private TextBox txtHoTen;

		public AddEditMember()
		{
			InitializeComponent();
		}

		private void UCCanBo_Load(object sender, EventArgs e)
		{
			listZ308 = DataDBLocal.listZ308;
			ComboxBlock();
			ComboxLoaiBanDoc();
			ComboxGioiTinh();
			txtPatronId.Text = "1";
			countP = new QueryDB().CountPatron();
			txtPatronId.Text = (countP + 1).ToString();
			CreateFolder(directoryPath);
		}

		private void CreateFolder(string directoryPath)
		{
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
		}

		private void btnThoat_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void btnConvert_Click(object sender, EventArgs e)
		{
			if (!new QueryDB().CheckBarcode(txtMa.Text.Trim()))
			{
				if (txtMa.Text != "")
				{
					WriterUserLdapPatron();
					WriteXML();
					WriteXmlApi();
					btnPush.Enabled = true;
					MessageBox.Show("chuyển đổi dữ liệu thành công!", "Thông báo!");
				}
			}
			else
			{
				MessageBox.Show("Mã này đã tồn tại.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void txtPatronId_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void ComboxBlock()
		{
			ComboboxItem comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Mở";
			comboboxItem.Value = "00";
			comboBox1.Items.Add(comboboxItem);
			comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Khóa";
			comboboxItem.Value = "05";
			comboBox1.Items.Add(comboboxItem);
			comboBox1.SelectedIndex = 0;
		}

		private void ComboxGioiTinh()
		{
			ComboboxItem comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Nam";
			comboboxItem.Value = "Nam";
			cbGioiTinh.Items.Add(comboboxItem);
			comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Nữ";
			comboboxItem.Value = "Nữ";
			cbGioiTinh.Items.Add(comboboxItem);
			cbGioiTinh.SelectedIndex = 0;
		}

		private void ComboxLoaiBanDoc()
		{
			ComboboxItem comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Cán Bộ";
			comboboxItem.Value = "01";
			cbLoaiBanDoc.Items.Add(comboboxItem);
			comboboxItem.Text = "Sinh Viên";
			comboboxItem.Value = "02";
			cbLoaiBanDoc.Items.Add(comboboxItem);
			comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Cao Học";
			comboboxItem.Value = "03";
			cbLoaiBanDoc.Items.Add(comboboxItem);
			comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Giảng Viên";
			comboboxItem.Value = "04";
			cbLoaiBanDoc.Items.Add(comboboxItem);
			comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Thư Viện Viên";
			comboboxItem.Value = "06";
			cbLoaiBanDoc.Items.Add(comboboxItem);
			comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Nghiên Cứu sinh";
			comboboxItem.Value = "05";
			cbLoaiBanDoc.Items.Add(comboboxItem);
			comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Loại Khác";
			comboboxItem.Value = "07";
			cbLoaiBanDoc.Items.Add(comboboxItem);
			cbLoaiBanDoc.SelectedIndex = 0;
		}

		private void WriterUserLdapPatron()
		{
			string text = (comboBox1.SelectedItem as ComboboxItem).Value.ToString();
			string text2 = (cbLoaiBanDoc.SelectedItem as ComboboxItem).Value.ToString();
			string genDer = (cbGioiTinh.SelectedItem as ComboboxItem).Value.ToString();
			p = new Patron();
			p.pationID = $"{countP + 1:000000000000}";
			p.MaSV_O = txtMa.Text.Trim();
			p.HoTen = txtHoTen.Text.Trim();
			p.password = txtPassword.Text.Trim();
			p.ngaySinh = dtpNgaySinh.Text.Trim();
			p.ngayHetHan = dateHetHan.Text.Trim();
			p.GT = new ToolP().convertGender(genDer);
			p.phone = txtPhone.Text.Trim();
			p.email = txtEmail.Text.Trim();
			p.DiaChi = txtAddress.Text.Trim();
			p.makh = "";
			if (rbSinhVien.Checked)
			{
				p.Khoa = txtKhoa.Text.Trim();
				p.lopHoc = txtLop.Text.Trim();
				p.khoaHoc = txtKhoaHoc.Text.Trim();
			}
			if (rbCanBo.Checked)
			{
				p.chucVu = txtChucVu.Text.Trim();
				p.Khoa = txtKhoa.Text.Trim();
				p.chucDanh = txtChucDanh.Text.Trim();
			}
			u = new User();
			u.cn = p.MaSV_O.Trim();
			u.sn = p.MaSV_O.Trim();
			u.userLogin = p.MaSV_O.Trim();
			u.userMail = p.email;
			u.userPassword = p.password;
			u.objectClass = "OpenLDAPPerson";
			u.telephoneNumber = p.phone;
		}

		private void WriteXML()
		{
			string block = (comboBox1.SelectedItem as ComboboxItem).Value.ToString();
			string status = (cbLoaiBanDoc.SelectedItem as ComboboxItem).Value.ToString();
			string text = (cbGioiTinh.SelectedItem as ComboboxItem).Value.ToString();
			sbPatronXml = new StringBuilder();
			sbPatronXml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			sbPatronXml.AppendLine("<p-file-20>");
			sbPatronXml.AppendLine("<patron-record>");
			sbPatronXml.Append(new z303().tab3(p));
			sbPatronXml.Append(new z304().tab4(p));
			sbPatronXml.Append(new z305().tab5(p, block, status));
			sbPatronXml.Append(new z308().tab8(p));
			sbPatronXml.AppendLine("</patron-record>");
			sbPatronXml.AppendLine("</p-file-20>");
		}

		private void WriteXmlApi()
		{
			string block = (comboBox1.SelectedItem as ComboboxItem).Value.ToString();
			string status = (cbLoaiBanDoc.SelectedItem as ComboboxItem).Value.ToString();
			sb = new StringBuilder();
			sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			sb.Append("<p-file-20>");
			sb.Append("<patron-record>");
			sb.Append(new z303().tab3(p));
			sb.Append(new z304().tab4(p));
			sb.Append(new z305().tab5(p, block, status));
			sb.Append(new z308().tab8(p));
			sb.Append("</patron-record>");
			sb.Append("</p-file-20>");
		}

		private void ExportDanhSachTT()
		{
			sbList = new StringBuilder();
			sbList.Append(p.pationID);
			sbList.Append("\t");
			sbList.AppendLine(p.MaSV_O);
			File.WriteAllText(txtPassword.Text + "/DanhSachTT-CanBo-" + tool.getDate() + ".txt", sbList.ToString());
		}

		private void CheckDataGridView(DataGridView gdv, Label lb)
		{
			if (gdv.ColumnCount > 0)
			{
				lb.Text = "Số lượng: " + gdv.RowCount.ToString();
			}
		}

		private void CreatePatron()
		{
			Loading_FS.text = "\tĐang đưa dữ liệu ...";
			Loading_FS.ShowSplash();
			using (StreamWriter streamWriter = new StreamWriter(directoryPath + "/Api-Patron-Log-" + tool.getDate() + ".txt"))
			{
				streamWriter.WriteLine(new AlephAPI().Url(sb.ToString()));
			}
			using (StreamWriter streamWriter2 = new StreamWriter(directoryPath + "/Ldap-Log-" + tool.getDate() + ".txt"))
			{
				streamWriter2.WriteLine(u.userLogin + "\t" + new ModelLdap().CreateUser(u));
			}
			Loading_FS.CloseSplash();
			MessageBox.Show("Thành công!", "Thông báo!");
		}

		private void btnPush_Click(object sender, EventArgs e)
		{
			CreatePatron();
		}

		private async void TxtMa_TextChanged(object sender, EventArgs e)
		{
			await Task.Delay(300);
			if (new QueryDB().CheckBarcode(txtMa.Text.Trim()))
			{
				lbErrorBarcode.Visible = true;
			}
			else
			{
				lbErrorBarcode.Visible = false;
			}
		}

		private void RbCanBo_CheckedChanged(object sender, EventArgs e)
		{
			panelCanBo.Visible = true;
			panelSinhVien.Visible = false;
		}

		private void RbSinhVien_CheckedChanged(object sender, EventArgs e)
		{
			panelCanBo.Visible = false;
			panelSinhVien.Visible = true;
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
			panel1 = new System.Windows.Forms.Panel();
			label18 = new System.Windows.Forms.Label();
			txtHoTen = new System.Windows.Forms.TextBox();
			lbErrorBarcode = new System.Windows.Forms.Label();
			panelCanBo = new System.Windows.Forms.Panel();
			label15 = new System.Windows.Forms.Label();
			txtChucDanh = new System.Windows.Forms.TextBox();
			label16 = new System.Windows.Forms.Label();
			txtChucVu = new System.Windows.Forms.TextBox();
			label17 = new System.Windows.Forms.Label();
			txtDonVi = new System.Windows.Forms.TextBox();
			txtEmail = new System.Windows.Forms.TextBox();
			label11 = new System.Windows.Forms.Label();
			groupBox2 = new System.Windows.Forms.GroupBox();
			rbSinhVien = new System.Windows.Forms.RadioButton();
			rbCanBo = new System.Windows.Forms.RadioButton();
			panelSinhVien = new System.Windows.Forms.Panel();
			label14 = new System.Windows.Forms.Label();
			txtKhoaHoc = new System.Windows.Forms.TextBox();
			label13 = new System.Windows.Forms.Label();
			txtLop = new System.Windows.Forms.TextBox();
			label12 = new System.Windows.Forms.Label();
			txtKhoa = new System.Windows.Forms.TextBox();
			txtPatronId = new System.Windows.Forms.TextBox();
			btnPush = new System.Windows.Forms.Button();
			label7 = new System.Windows.Forms.Label();
			btnConvert = new System.Windows.Forms.Button();
			btnThoat = new System.Windows.Forms.Button();
			txtAddress = new System.Windows.Forms.TextBox();
			cbLoaiBanDoc = new System.Windows.Forms.ComboBox();
			comboBox1 = new System.Windows.Forms.ComboBox();
			label10 = new System.Windows.Forms.Label();
			label5 = new System.Windows.Forms.Label();
			dateHetHan = new System.Windows.Forms.DateTimePicker();
			label4 = new System.Windows.Forms.Label();
			label9 = new System.Windows.Forms.Label();
			pb_TaiChinh = new System.Windows.Forms.PictureBox();
			txtPhone = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			label8 = new System.Windows.Forms.Label();
			txtMa = new System.Windows.Forms.TextBox();
			dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
			label2 = new System.Windows.Forms.Label();
			label6 = new System.Windows.Forms.Label();
			txtPassword = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			cbGioiTinh = new System.Windows.Forms.ComboBox();
			folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			groupBox1.SuspendLayout();
			panel1.SuspendLayout();
			panelCanBo.SuspendLayout();
			groupBox2.SuspendLayout();
			panelSinhVien.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).BeginInit();
			SuspendLayout();
			groupBox1.Controls.Add(panel1);
			groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold);
			groupBox1.Location = new System.Drawing.Point(0, 3);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(993, 559);
			groupBox1.TabIndex = 28;
			groupBox1.TabStop = false;
			groupBox1.Text = "Thêm người dùng";
			panel1.Controls.Add(label18);
			panel1.Controls.Add(txtHoTen);
			panel1.Controls.Add(lbErrorBarcode);
			panel1.Controls.Add(txtEmail);
			panel1.Controls.Add(label11);
			panel1.Controls.Add(groupBox2);
			panel1.Controls.Add(panelSinhVien);
			panel1.Controls.Add(txtPatronId);
			panel1.Controls.Add(btnPush);
			panel1.Controls.Add(label7);
			panel1.Controls.Add(btnConvert);
			panel1.Controls.Add(btnThoat);
			panel1.Controls.Add(txtAddress);
			panel1.Controls.Add(cbLoaiBanDoc);
			panel1.Controls.Add(comboBox1);
			panel1.Controls.Add(label10);
			panel1.Controls.Add(label5);
			panel1.Controls.Add(dateHetHan);
			panel1.Controls.Add(label4);
			panel1.Controls.Add(label9);
			panel1.Controls.Add(pb_TaiChinh);
			panel1.Controls.Add(txtPhone);
			panel1.Controls.Add(label1);
			panel1.Controls.Add(label8);
			panel1.Controls.Add(txtMa);
			panel1.Controls.Add(dtpNgaySinh);
			panel1.Controls.Add(label2);
			panel1.Controls.Add(label6);
			panel1.Controls.Add(txtPassword);
			panel1.Controls.Add(label3);
			panel1.Controls.Add(cbGioiTinh);
			panel1.Location = new System.Drawing.Point(6, 27);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(981, 535);
			panel1.TabIndex = 127;
			label18.AutoSize = true;
			label18.Location = new System.Drawing.Point(10, 79);
			label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label18.Name = "label18";
			label18.Size = new System.Drawing.Size(73, 25);
			label18.TabIndex = 135;
			label18.Text = "Họ tên";
			txtHoTen.Location = new System.Drawing.Point(143, 74);
			txtHoTen.Margin = new System.Windows.Forms.Padding(2);
			txtHoTen.Name = "txtHoTen";
			txtHoTen.Size = new System.Drawing.Size(313, 33);
			txtHoTen.TabIndex = 136;
			lbErrorBarcode.AutoSize = true;
			lbErrorBarcode.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
			lbErrorBarcode.ForeColor = System.Drawing.Color.Red;
			lbErrorBarcode.Location = new System.Drawing.Point(145, 7);
			lbErrorBarcode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			lbErrorBarcode.Name = "lbErrorBarcode";
			lbErrorBarcode.Size = new System.Drawing.Size(156, 21);
			lbErrorBarcode.TabIndex = 134;
			lbErrorBarcode.Text = "Mã SV/CB đã tồn tại";
			lbErrorBarcode.Visible = false;
			panelCanBo.Controls.Add(label15);
			panelCanBo.Controls.Add(txtChucDanh);
			panelCanBo.Controls.Add(label16);
			panelCanBo.Controls.Add(txtChucVu);
			panelCanBo.Controls.Add(label17);
			panelCanBo.Controls.Add(txtDonVi);
			panelCanBo.Location = new System.Drawing.Point(0, 0);
			panelCanBo.Name = "panelCanBo";
			panelCanBo.Size = new System.Drawing.Size(444, 122);
			panelCanBo.TabIndex = 129;
			label15.AutoSize = true;
			label15.Location = new System.Drawing.Point(6, 91);
			label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label15.Name = "label15";
			label15.Size = new System.Drawing.Size(107, 25);
			label15.TabIndex = 120;
			label15.Text = "Chức danh";
			txtChucDanh.Location = new System.Drawing.Point(125, 86);
			txtChucDanh.Margin = new System.Windows.Forms.Padding(2);
			txtChucDanh.Name = "txtChucDanh";
			txtChucDanh.Size = new System.Drawing.Size(316, 33);
			txtChucDanh.TabIndex = 121;
			label16.AutoSize = true;
			label16.Location = new System.Drawing.Point(6, 54);
			label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label16.Name = "label16";
			label16.Size = new System.Drawing.Size(84, 25);
			label16.TabIndex = 118;
			label16.Text = "Chức vụ";
			txtChucVu.Location = new System.Drawing.Point(125, 49);
			txtChucVu.Margin = new System.Windows.Forms.Padding(2);
			txtChucVu.Name = "txtChucVu";
			txtChucVu.Size = new System.Drawing.Size(316, 33);
			txtChucVu.TabIndex = 119;
			label17.AutoSize = true;
			label17.Location = new System.Drawing.Point(6, 14);
			label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label17.Name = "label17";
			label17.Size = new System.Drawing.Size(70, 25);
			label17.TabIndex = 116;
			label17.Text = "Đơn vị";
			txtDonVi.Location = new System.Drawing.Point(126, 9);
			txtDonVi.Margin = new System.Windows.Forms.Padding(2);
			txtDonVi.Name = "txtDonVi";
			txtDonVi.Size = new System.Drawing.Size(316, 33);
			txtDonVi.TabIndex = 117;
			txtEmail.Location = new System.Drawing.Point(478, 165);
			txtEmail.Margin = new System.Windows.Forms.Padding(2);
			txtEmail.Name = "txtEmail";
			txtEmail.Size = new System.Drawing.Size(314, 33);
			txtEmail.TabIndex = 133;
			label11.AutoSize = true;
			label11.Location = new System.Drawing.Point(473, 136);
			label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label11.Name = "label11";
			label11.Size = new System.Drawing.Size(59, 25);
			label11.TabIndex = 132;
			label11.Text = "Email";
			groupBox2.Controls.Add(rbSinhVien);
			groupBox2.Controls.Add(rbCanBo);
			groupBox2.Location = new System.Drawing.Point(14, 349);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(444, 50);
			groupBox2.TabIndex = 131;
			groupBox2.TabStop = false;
			rbSinhVien.AutoSize = true;
			rbSinhVien.Checked = true;
			rbSinhVien.Location = new System.Drawing.Point(126, 15);
			rbSinhVien.Name = "rbSinhVien";
			rbSinhVien.Size = new System.Drawing.Size(111, 29);
			rbSinhVien.TabIndex = 130;
			rbSinhVien.TabStop = true;
			rbSinhVien.Text = "Sinh viên";
			rbSinhVien.UseVisualStyleBackColor = true;
			rbSinhVien.CheckedChanged += new System.EventHandler(RbSinhVien_CheckedChanged);
			rbCanBo.AutoSize = true;
			rbCanBo.Location = new System.Drawing.Point(257, 16);
			rbCanBo.Name = "rbCanBo";
			rbCanBo.Size = new System.Drawing.Size(93, 29);
			rbCanBo.TabIndex = 129;
			rbCanBo.Text = "Cán bộ";
			rbCanBo.UseVisualStyleBackColor = true;
			rbCanBo.CheckedChanged += new System.EventHandler(RbCanBo_CheckedChanged);
			panelSinhVien.Controls.Add(label14);
			panelSinhVien.Controls.Add(txtKhoaHoc);
			panelSinhVien.Controls.Add(label13);
			panelSinhVien.Controls.Add(panelCanBo);
			panelSinhVien.Controls.Add(txtLop);
			panelSinhVien.Controls.Add(label12);
			panelSinhVien.Controls.Add(txtKhoa);
			panelSinhVien.Location = new System.Drawing.Point(14, 400);
			panelSinhVien.Name = "panelSinhVien";
			panelSinhVien.Size = new System.Drawing.Size(444, 122);
			panelSinhVien.TabIndex = 128;
			label14.AutoSize = true;
			label14.Location = new System.Drawing.Point(6, 91);
			label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label14.Name = "label14";
			label14.Size = new System.Drawing.Size(94, 25);
			label14.TabIndex = 120;
			label14.Text = "Khóa học";
			txtKhoaHoc.Location = new System.Drawing.Point(125, 86);
			txtKhoaHoc.Margin = new System.Windows.Forms.Padding(2);
			txtKhoaHoc.Name = "txtKhoaHoc";
			txtKhoaHoc.Size = new System.Drawing.Size(316, 33);
			txtKhoaHoc.TabIndex = 121;
			label13.AutoSize = true;
			label13.Location = new System.Drawing.Point(6, 55);
			label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label13.Name = "label13";
			label13.Size = new System.Drawing.Size(46, 25);
			label13.TabIndex = 118;
			label13.Text = "Lớp";
			txtLop.Location = new System.Drawing.Point(125, 49);
			txtLop.Margin = new System.Windows.Forms.Padding(2);
			txtLop.Name = "txtLop";
			txtLop.Size = new System.Drawing.Size(316, 33);
			txtLop.TabIndex = 119;
			label12.AutoSize = true;
			label12.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			label12.Location = new System.Drawing.Point(6, 14);
			label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label12.Name = "label12";
			label12.Size = new System.Drawing.Size(108, 21);
			label12.TabIndex = 116;
			label12.Text = "Khoa/Ngành";
			txtKhoa.Location = new System.Drawing.Point(126, 9);
			txtKhoa.Margin = new System.Windows.Forms.Padding(2);
			txtKhoa.Name = "txtKhoa";
			txtKhoa.Size = new System.Drawing.Size(315, 33);
			txtKhoa.TabIndex = 117;
			txtPatronId.Enabled = false;
			txtPatronId.Location = new System.Drawing.Point(573, 30);
			txtPatronId.Margin = new System.Windows.Forms.Padding(2);
			txtPatronId.Name = "txtPatronId";
			txtPatronId.Size = new System.Drawing.Size(219, 33);
			txtPatronId.TabIndex = 99;
			txtPatronId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPatronId_KeyPress);
			btnPush.AutoSize = true;
			btnPush.BackColor = System.Drawing.Color.Green;
			btnPush.Enabled = false;
			btnPush.FlatAppearance.BorderSize = 0;
			btnPush.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnPush.ForeColor = System.Drawing.Color.White;
			btnPush.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnPush.Location = new System.Drawing.Point(478, 442);
			btnPush.Name = "btnPush";
			btnPush.Size = new System.Drawing.Size(159, 38);
			btnPush.TabIndex = 107;
			btnPush.Text = "Thêm người dùng";
			btnPush.UseVisualStyleBackColor = false;
			btnPush.Click += new System.EventHandler(btnPush_Click);
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(9, 316);
			label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(128, 25);
			label7.TabIndex = 111;
			label7.Text = "Loại Bạn Đọc";
			btnConvert.AutoSize = true;
			btnConvert.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnConvert.FlatAppearance.BorderSize = 0;
			btnConvert.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnConvert.ForeColor = System.Drawing.Color.White;
			btnConvert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnConvert.Location = new System.Drawing.Point(478, 400);
			btnConvert.Name = "btnConvert";
			btnConvert.Size = new System.Drawing.Size(159, 38);
			btnConvert.TabIndex = 106;
			btnConvert.Text = "Chuyển dữ liệu";
			btnConvert.UseVisualStyleBackColor = false;
			btnConvert.Click += new System.EventHandler(btnConvert_Click);
			btnThoat.AutoSize = true;
			btnThoat.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
			btnThoat.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnThoat.FlatAppearance.BorderSize = 0;
			btnThoat.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnThoat.ForeColor = System.Drawing.Color.White;
			btnThoat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnThoat.Location = new System.Drawing.Point(478, 484);
			btnThoat.Name = "btnThoat";
			btnThoat.Size = new System.Drawing.Size(159, 38);
			btnThoat.TabIndex = 14;
			btnThoat.Text = "Trở về";
			btnThoat.UseVisualStyleBackColor = false;
			btnThoat.Click += new System.EventHandler(btnThoat_Click);
			txtAddress.Location = new System.Drawing.Point(478, 236);
			txtAddress.Margin = new System.Windows.Forms.Padding(2);
			txtAddress.Multiline = true;
			txtAddress.Name = "txtAddress";
			txtAddress.Size = new System.Drawing.Size(314, 108);
			txtAddress.TabIndex = 126;
			cbLoaiBanDoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbLoaiBanDoc.FormattingEnabled = true;
			cbLoaiBanDoc.Location = new System.Drawing.Point(142, 311);
			cbLoaiBanDoc.Name = "cbLoaiBanDoc";
			cbLoaiBanDoc.Size = new System.Drawing.Size(313, 33);
			cbLoaiBanDoc.TabIndex = 110;
			comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBox1.FormattingEnabled = true;
			comboBox1.Location = new System.Drawing.Point(142, 270);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new System.Drawing.Size(313, 33);
			comboBox1.TabIndex = 92;
			label10.AutoSize = true;
			label10.Location = new System.Drawing.Point(473, 204);
			label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(71, 25);
			label10.TabIndex = 125;
			label10.Text = "Địa chỉ";
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(9, 278);
			label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(102, 25);
			label5.TabIndex = 94;
			label5.Text = "Trạng thái";
			dateHetHan.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			dateHetHan.Location = new System.Drawing.Point(142, 191);
			dateHetHan.Name = "dateHetHan";
			dateHetHan.Size = new System.Drawing.Size(314, 33);
			dateHetHan.TabIndex = 124;
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(473, 33);
			label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(96, 25);
			label4.TabIndex = 98;
			label4.Text = "Patron Id";
			label9.AutoSize = true;
			label9.Location = new System.Drawing.Point(9, 196);
			label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(130, 25);
			label9.TabIndex = 123;
			label9.Text = "Ngày hết hạn";
			pb_TaiChinh.Image = TNUE_Patron_Excel.Properties.Resources.library_logo;
			pb_TaiChinh.Location = new System.Drawing.Point(806, 10);
			pb_TaiChinh.Name = "pb_TaiChinh";
			pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
			pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			pb_TaiChinh.TabIndex = 20;
			pb_TaiChinh.TabStop = false;
			txtPhone.Location = new System.Drawing.Point(478, 100);
			txtPhone.Margin = new System.Windows.Forms.Padding(2);
			txtPhone.Name = "txtPhone";
			txtPhone.Size = new System.Drawing.Size(314, 33);
			txtPhone.TabIndex = 122;
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(9, 38);
			label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(101, 25);
			label1.TabIndex = 112;
			label1.Text = "Mã SV/CB";
			label8.AutoSize = true;
			label8.Location = new System.Drawing.Point(473, 66);
			label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(129, 25);
			label8.TabIndex = 121;
			label8.Text = "Số điện thoại";
			txtMa.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
			txtMa.Location = new System.Drawing.Point(142, 34);
			txtMa.Margin = new System.Windows.Forms.Padding(2);
			txtMa.Name = "txtMa";
			txtMa.Size = new System.Drawing.Size(314, 33);
			txtMa.TabIndex = 113;
			txtMa.TextChanged += new System.EventHandler(TxtMa_TextChanged);
			dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;
			dtpNgaySinh.Location = new System.Drawing.Point(142, 151);
			dtpNgaySinh.Name = "dtpNgaySinh";
			dtpNgaySinh.Size = new System.Drawing.Size(314, 33);
			dtpNgaySinh.TabIndex = 120;
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(9, 117);
			label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(96, 25);
			label2.TabIndex = 114;
			label2.Text = "Mật khẩu";
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(9, 158);
			label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(100, 25);
			label6.TabIndex = 119;
			label6.Text = "Ngày sinh";
			txtPassword.Location = new System.Drawing.Point(142, 112);
			txtPassword.Margin = new System.Windows.Forms.Padding(2);
			txtPassword.Name = "txtPassword";
			txtPassword.Size = new System.Drawing.Size(313, 33);
			txtPassword.TabIndex = 115;
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(9, 238);
			label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(88, 25);
			label3.TabIndex = 117;
			label3.Text = "Giới tính";
			cbGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbGioiTinh.FormattingEnabled = true;
			cbGioiTinh.Location = new System.Drawing.Point(142, 231);
			cbGioiTinh.Name = "cbGioiTinh";
			cbGioiTinh.Size = new System.Drawing.Size(313, 33);
			cbGioiTinh.TabIndex = 116;
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox1);
			Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.Margin = new System.Windows.Forms.Padding(4);
			base.Name = "AddEditMember";
			base.Size = new System.Drawing.Size(1000, 565);
			base.Load += new System.EventHandler(UCCanBo_Load);
			groupBox1.ResumeLayout(false);
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			panelCanBo.ResumeLayout(false);
			panelCanBo.PerformLayout();
			groupBox2.ResumeLayout(false);
			groupBox2.PerformLayout();
			panelSinhVien.ResumeLayout(false);
			panelSinhVien.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).EndInit();
			ResumeLayout(false);
		}
	}
}
