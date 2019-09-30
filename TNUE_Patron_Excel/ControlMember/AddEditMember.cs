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

		private string directoryPath = Application.StartupPath + "\\log";

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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.txtHoTen = new System.Windows.Forms.TextBox();
            this.lbErrorBarcode = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbSinhVien = new System.Windows.Forms.RadioButton();
            this.rbCanBo = new System.Windows.Forms.RadioButton();
            this.panelSinhVien = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.txtKhoaHoc = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.panelCanBo = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.txtChucDanh = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtChucVu = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtDonVi = new System.Windows.Forms.TextBox();
            this.txtLop = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtKhoa = new System.Windows.Forms.TextBox();
            this.txtPatronId = new System.Windows.Forms.TextBox();
            this.btnPush = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnThoat = new System.Windows.Forms.Button();
            this.txtAddress = new System.Windows.Forms.TextBox();
            this.cbLoaiBanDoc = new System.Windows.Forms.ComboBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dateHetHan = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pb_TaiChinh = new System.Windows.Forms.PictureBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtMa = new System.Windows.Forms.TextBox();
            this.dtpNgaySinh = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbGioiTinh = new System.Windows.Forms.ComboBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panelSinhVien.SuspendLayout();
            this.panelCanBo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(993, 559);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Thêm người dùng";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.txtHoTen);
            this.panel1.Controls.Add(this.lbErrorBarcode);
            this.panel1.Controls.Add(this.panelCanBo);
            this.panel1.Controls.Add(this.txtEmail);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.panelSinhVien);
            this.panel1.Controls.Add(this.txtPatronId);
            this.panel1.Controls.Add(this.btnPush);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnConvert);
            this.panel1.Controls.Add(this.btnThoat);
            this.panel1.Controls.Add(this.txtAddress);
            this.panel1.Controls.Add(this.cbLoaiBanDoc);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.dateHetHan);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.pb_TaiChinh);
            this.panel1.Controls.Add(this.txtPhone);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtMa);
            this.panel1.Controls.Add(this.dtpNgaySinh);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtPassword);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbGioiTinh);
            this.panel1.Location = new System.Drawing.Point(6, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(981, 535);
            this.panel1.TabIndex = 127;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(10, 79);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(73, 25);
            this.label18.TabIndex = 135;
            this.label18.Text = "Họ tên";
            // 
            // txtHoTen
            // 
            this.txtHoTen.Location = new System.Drawing.Point(143, 74);
            this.txtHoTen.Margin = new System.Windows.Forms.Padding(2);
            this.txtHoTen.Name = "txtHoTen";
            this.txtHoTen.Size = new System.Drawing.Size(313, 33);
            this.txtHoTen.TabIndex = 136;
            // 
            // lbErrorBarcode
            // 
            this.lbErrorBarcode.AutoSize = true;
            this.lbErrorBarcode.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbErrorBarcode.ForeColor = System.Drawing.Color.Red;
            this.lbErrorBarcode.Location = new System.Drawing.Point(145, 7);
            this.lbErrorBarcode.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbErrorBarcode.Name = "lbErrorBarcode";
            this.lbErrorBarcode.Size = new System.Drawing.Size(156, 21);
            this.lbErrorBarcode.TabIndex = 134;
            this.lbErrorBarcode.Text = "Mã SV/CB đã tồn tại";
            this.lbErrorBarcode.Visible = false;
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(478, 165);
            this.txtEmail.Margin = new System.Windows.Forms.Padding(2);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(314, 33);
            this.txtEmail.TabIndex = 133;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(473, 136);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(59, 25);
            this.label11.TabIndex = 132;
            this.label11.Text = "Email";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbSinhVien);
            this.groupBox2.Controls.Add(this.rbCanBo);
            this.groupBox2.Location = new System.Drawing.Point(14, 349);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(444, 50);
            this.groupBox2.TabIndex = 131;
            this.groupBox2.TabStop = false;
            // 
            // rbSinhVien
            // 
            this.rbSinhVien.AutoSize = true;
            this.rbSinhVien.Checked = true;
            this.rbSinhVien.Location = new System.Drawing.Point(126, 15);
            this.rbSinhVien.Name = "rbSinhVien";
            this.rbSinhVien.Size = new System.Drawing.Size(111, 29);
            this.rbSinhVien.TabIndex = 130;
            this.rbSinhVien.TabStop = true;
            this.rbSinhVien.Text = "Sinh viên";
            this.rbSinhVien.UseVisualStyleBackColor = true;
            this.rbSinhVien.CheckedChanged += new System.EventHandler(this.RbSinhVien_CheckedChanged);
            // 
            // rbCanBo
            // 
            this.rbCanBo.AutoSize = true;
            this.rbCanBo.Location = new System.Drawing.Point(257, 16);
            this.rbCanBo.Name = "rbCanBo";
            this.rbCanBo.Size = new System.Drawing.Size(93, 29);
            this.rbCanBo.TabIndex = 129;
            this.rbCanBo.Text = "Cán bộ";
            this.rbCanBo.UseVisualStyleBackColor = true;
            this.rbCanBo.CheckedChanged += new System.EventHandler(this.RbCanBo_CheckedChanged);
            // 
            // panelSinhVien
            // 
            this.panelSinhVien.Controls.Add(this.label14);
            this.panelSinhVien.Controls.Add(this.txtKhoaHoc);
            this.panelSinhVien.Controls.Add(this.label13);
            this.panelSinhVien.Controls.Add(this.txtLop);
            this.panelSinhVien.Controls.Add(this.label12);
            this.panelSinhVien.Controls.Add(this.txtKhoa);
            this.panelSinhVien.Location = new System.Drawing.Point(12, 399);
            this.panelSinhVien.Name = "panelSinhVien";
            this.panelSinhVien.Size = new System.Drawing.Size(444, 122);
            this.panelSinhVien.TabIndex = 128;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 91);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(94, 25);
            this.label14.TabIndex = 120;
            this.label14.Text = "Khóa học";
            // 
            // txtKhoaHoc
            // 
            this.txtKhoaHoc.Location = new System.Drawing.Point(125, 86);
            this.txtKhoaHoc.Margin = new System.Windows.Forms.Padding(2);
            this.txtKhoaHoc.Name = "txtKhoaHoc";
            this.txtKhoaHoc.Size = new System.Drawing.Size(316, 33);
            this.txtKhoaHoc.TabIndex = 121;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 55);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(46, 25);
            this.label13.TabIndex = 118;
            this.label13.Text = "Lớp";
            // 
            // panelCanBo
            // 
            this.panelCanBo.Controls.Add(this.label15);
            this.panelCanBo.Controls.Add(this.txtChucDanh);
            this.panelCanBo.Controls.Add(this.label16);
            this.panelCanBo.Controls.Add(this.txtChucVu);
            this.panelCanBo.Controls.Add(this.label17);
            this.panelCanBo.Controls.Add(this.txtDonVi);
            this.panelCanBo.Location = new System.Drawing.Point(12, 403);
            this.panelCanBo.Name = "panelCanBo";
            this.panelCanBo.Size = new System.Drawing.Size(444, 122);
            this.panelCanBo.TabIndex = 129;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 91);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(107, 25);
            this.label15.TabIndex = 120;
            this.label15.Text = "Chức danh";
            // 
            // txtChucDanh
            // 
            this.txtChucDanh.Location = new System.Drawing.Point(125, 86);
            this.txtChucDanh.Margin = new System.Windows.Forms.Padding(2);
            this.txtChucDanh.Name = "txtChucDanh";
            this.txtChucDanh.Size = new System.Drawing.Size(316, 33);
            this.txtChucDanh.TabIndex = 121;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(6, 54);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(84, 25);
            this.label16.TabIndex = 118;
            this.label16.Text = "Chức vụ";
            // 
            // txtChucVu
            // 
            this.txtChucVu.Location = new System.Drawing.Point(125, 49);
            this.txtChucVu.Margin = new System.Windows.Forms.Padding(2);
            this.txtChucVu.Name = "txtChucVu";
            this.txtChucVu.Size = new System.Drawing.Size(316, 33);
            this.txtChucVu.TabIndex = 119;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(6, 14);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(70, 25);
            this.label17.TabIndex = 116;
            this.label17.Text = "Đơn vị";
            // 
            // txtDonVi
            // 
            this.txtDonVi.Location = new System.Drawing.Point(126, 9);
            this.txtDonVi.Margin = new System.Windows.Forms.Padding(2);
            this.txtDonVi.Name = "txtDonVi";
            this.txtDonVi.Size = new System.Drawing.Size(316, 33);
            this.txtDonVi.TabIndex = 117;
            // 
            // txtLop
            // 
            this.txtLop.Location = new System.Drawing.Point(125, 49);
            this.txtLop.Margin = new System.Windows.Forms.Padding(2);
            this.txtLop.Name = "txtLop";
            this.txtLop.Size = new System.Drawing.Size(316, 33);
            this.txtLop.TabIndex = 119;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(6, 14);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(108, 21);
            this.label12.TabIndex = 116;
            this.label12.Text = "Khoa/Ngành";
            // 
            // txtKhoa
            // 
            this.txtKhoa.Location = new System.Drawing.Point(126, 9);
            this.txtKhoa.Margin = new System.Windows.Forms.Padding(2);
            this.txtKhoa.Name = "txtKhoa";
            this.txtKhoa.Size = new System.Drawing.Size(315, 33);
            this.txtKhoa.TabIndex = 117;
            // 
            // txtPatronId
            // 
            this.txtPatronId.Enabled = false;
            this.txtPatronId.Location = new System.Drawing.Point(573, 30);
            this.txtPatronId.Margin = new System.Windows.Forms.Padding(2);
            this.txtPatronId.Name = "txtPatronId";
            this.txtPatronId.Size = new System.Drawing.Size(219, 33);
            this.txtPatronId.TabIndex = 99;
            this.txtPatronId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPatronId_KeyPress);
            // 
            // btnPush
            // 
            this.btnPush.AutoSize = true;
            this.btnPush.BackColor = System.Drawing.Color.Green;
            this.btnPush.Enabled = false;
            this.btnPush.FlatAppearance.BorderSize = 0;
            this.btnPush.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPush.ForeColor = System.Drawing.Color.White;
            this.btnPush.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPush.Location = new System.Drawing.Point(478, 442);
            this.btnPush.Name = "btnPush";
            this.btnPush.Size = new System.Drawing.Size(159, 38);
            this.btnPush.TabIndex = 107;
            this.btnPush.Text = "Thêm người dùng";
            this.btnPush.UseVisualStyleBackColor = false;
            this.btnPush.Click += new System.EventHandler(this.btnPush_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 316);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 25);
            this.label7.TabIndex = 111;
            this.label7.Text = "Loại Bạn Đọc";
            // 
            // btnConvert
            // 
            this.btnConvert.AutoSize = true;
            this.btnConvert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnConvert.FlatAppearance.BorderSize = 0;
            this.btnConvert.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConvert.ForeColor = System.Drawing.Color.White;
            this.btnConvert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConvert.Location = new System.Drawing.Point(478, 400);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(159, 38);
            this.btnConvert.TabIndex = 106;
            this.btnConvert.Text = "Chuyển dữ liệu";
            this.btnConvert.UseVisualStyleBackColor = false;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
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
            this.btnThoat.Location = new System.Drawing.Point(478, 484);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(159, 38);
            this.btnThoat.TabIndex = 14;
            this.btnThoat.Text = "Trở về";
            this.btnThoat.UseVisualStyleBackColor = false;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // txtAddress
            // 
            this.txtAddress.Location = new System.Drawing.Point(478, 236);
            this.txtAddress.Margin = new System.Windows.Forms.Padding(2);
            this.txtAddress.Multiline = true;
            this.txtAddress.Name = "txtAddress";
            this.txtAddress.Size = new System.Drawing.Size(314, 108);
            this.txtAddress.TabIndex = 126;
            // 
            // cbLoaiBanDoc
            // 
            this.cbLoaiBanDoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLoaiBanDoc.FormattingEnabled = true;
            this.cbLoaiBanDoc.Location = new System.Drawing.Point(142, 311);
            this.cbLoaiBanDoc.Name = "cbLoaiBanDoc";
            this.cbLoaiBanDoc.Size = new System.Drawing.Size(313, 33);
            this.cbLoaiBanDoc.TabIndex = 110;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(142, 270);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(313, 33);
            this.comboBox1.TabIndex = 92;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(473, 204);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 25);
            this.label10.TabIndex = 125;
            this.label10.Text = "Địa chỉ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 278);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 25);
            this.label5.TabIndex = 94;
            this.label5.Text = "Trạng thái";
            // 
            // dateHetHan
            // 
            this.dateHetHan.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateHetHan.Location = new System.Drawing.Point(142, 191);
            this.dateHetHan.Name = "dateHetHan";
            this.dateHetHan.Size = new System.Drawing.Size(314, 33);
            this.dateHetHan.TabIndex = 124;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(473, 33);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 25);
            this.label4.TabIndex = 98;
            this.label4.Text = "Patron Id";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 196);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(130, 25);
            this.label9.TabIndex = 123;
            this.label9.Text = "Ngày hết hạn";
            // 
            // pb_TaiChinh
            // 
            this.pb_TaiChinh.Image = global::TNUE_Patron_Excel.Properties.Resources.library_logo;
            this.pb_TaiChinh.Location = new System.Drawing.Point(806, 10);
            this.pb_TaiChinh.Name = "pb_TaiChinh";
            this.pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
            this.pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_TaiChinh.TabIndex = 20;
            this.pb_TaiChinh.TabStop = false;
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(478, 100);
            this.txtPhone.Margin = new System.Windows.Forms.Padding(2);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(314, 33);
            this.txtPhone.TabIndex = 122;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 25);
            this.label1.TabIndex = 112;
            this.label1.Text = "Mã SV/CB";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(473, 66);
            this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(129, 25);
            this.label8.TabIndex = 121;
            this.label8.Text = "Số điện thoại";
            // 
            // txtMa
            // 
            this.txtMa.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtMa.Location = new System.Drawing.Point(142, 34);
            this.txtMa.Margin = new System.Windows.Forms.Padding(2);
            this.txtMa.Name = "txtMa";
            this.txtMa.Size = new System.Drawing.Size(314, 33);
            this.txtMa.TabIndex = 113;
            this.txtMa.TextChanged += new System.EventHandler(this.TxtMa_TextChanged);
            // 
            // dtpNgaySinh
            // 
            this.dtpNgaySinh.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpNgaySinh.Location = new System.Drawing.Point(142, 151);
            this.dtpNgaySinh.Name = "dtpNgaySinh";
            this.dtpNgaySinh.Size = new System.Drawing.Size(314, 33);
            this.dtpNgaySinh.TabIndex = 120;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 117);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 25);
            this.label2.TabIndex = 114;
            this.label2.Text = "Mật khẩu";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(9, 158);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(100, 25);
            this.label6.TabIndex = 119;
            this.label6.Text = "Ngày sinh";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(142, 112);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(2);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(313, 33);
            this.txtPassword.TabIndex = 115;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 238);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 25);
            this.label3.TabIndex = 117;
            this.label3.Text = "Giới tính";
            // 
            // cbGioiTinh
            // 
            this.cbGioiTinh.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbGioiTinh.FormattingEnabled = true;
            this.cbGioiTinh.Location = new System.Drawing.Point(142, 231);
            this.cbGioiTinh.Name = "cbGioiTinh";
            this.cbGioiTinh.Size = new System.Drawing.Size(313, 33);
            this.cbGioiTinh.TabIndex = 116;
            // 
            // AddEditMember
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AddEditMember";
            this.Size = new System.Drawing.Size(1000, 565);
            this.Load += new System.EventHandler(this.UCCanBo_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panelSinhVien.ResumeLayout(false);
            this.panelSinhVien.PerformLayout();
            this.panelCanBo.ResumeLayout(false);
            this.panelCanBo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).EndInit();
            this.ResumeLayout(false);

		}
	}
}
