using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using TNUE_Patron_Excel.API;
using TNUE_Patron_Excel.DBConnect;
using TNUE_Patron_Excel.Ldap;
using TNUE_Patron_Excel.Properties;
using TNUE_Patron_Excel.Tool;
using TNUE_Patron_Excel.Z303;

namespace TNUE_Patron_Excel
{
	public class UCCanBo : UserControl
	{
		private List<Z308> listZ308 = null;

		private ToolP tool = new ToolP();

		private Microsoft.Office.Interop.Excel.Application fileEx = null;

		private StringBuilder sbList = null;

		private List<Patron> listPatron = null;

		private StringBuilder sbPatronXml;

		private List<StringBuilder> listSb = null;

		private List<User> ldapUser = null;

		private List<Patron> DSTonTai = null;

		private string fileName = "";

		private int countP = 1;

		private string directoryPath = System.Windows.Forms.Application.StartupPath + "\\log";

		private IContainer components = null;

		private GroupBox groupBox3;

		private GroupBox groupBox1;

		private Button btnThoat;

		private Label label6;

		private Button btn_ldap;

		private Button btn_api;

		private TextBox txtPatronId;

		private Label label4;

		private TextBox txtLine;

		private Label label5;

		private ComboBox comboBox1;

		private TextBox textBox3;

		private TextBox textBox2;

		private Label label2;

		private Button btnBrowserFile;

		private TextBox textBox1;

		private Label label1;

		private Button btnGetData;

		private Label label3;

		private Button btnConvert;

		private Button btnXml;

		private FolderBrowserDialog folderBrowserDialog1;

		private GroupBox groupBox2;

		private Label lbCountListExcel;

		private Label lbCountHad;

		private Button btnPush;

		private PictureBox pb_TaiChinh;

		private Label label7;

		private ComboBox cbLoaiBanDoc;

		private DataGridView dgvPatron;

		private DataGridView dgvHad;

		private DataGridViewTextBoxColumn pationID;

		private DataGridViewTextBoxColumn MaSV_O;

		private DataGridViewTextBoxColumn HoTen;

		private DataGridViewTextBoxColumn GT;

		private DataGridViewTextBoxColumn ngaySinh;

		private DataGridViewTextBoxColumn password;

		private DataGridViewTextBoxColumn phone;

		private DataGridViewTextBoxColumn email;

		private DataGridViewTextBoxColumn DiaChi;

		private DataGridViewTextBoxColumn khoaHoc;

		private DataGridViewTextBoxColumn khoa;

		private DataGridViewTextBoxColumn lopHoc;

		private DataGridViewTextBoxColumn makh;

		private DataGridViewTextBoxColumn chucVu;

		private DataGridViewTextBoxColumn chucDanh;

		private DataGridViewTextBoxColumn QuocTich;

		private DataGridViewTextBoxColumn hocBong;

		private DataGridViewTextBoxColumn qdCongNhan;

		private DataGridViewTextBoxColumn ngayHetHan;

		private DataGridViewTextBoxColumn Day;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;

		private DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;

		private Button btnUpdateLdap;

		public UCCanBo()
		{
			InitializeComponent();
		}

		private void UCCanBo_Load(object sender, EventArgs e)
		{
			listZ308 = DataDBLocal.listZ308;
			ComboxBlock();
			ComboxLoaiBanDoc();
			txtPatronId.Text = "1";
			txtLine.Text = "3";
			countP = new QueryDB().CountPatron();
			txtPatronId.Text = $"{countP + 1:000000000000}";
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
			System.Windows.Forms.Application.Exit();
		}

		private void txtLine_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void btnBrowserFile_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "excel file |*.xls;*.xlsx";
			openFileDialog.FilterIndex = 1;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.Multiselect = false;
			openFileDialog.Title = "Chọn file excel";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				textBox1.Text = openFileDialog.FileName;
			}
			if (textBox1.Text != "")
			{
				readExcel2();
				btnConvert.Enabled = true;
				MessageBox.Show("Chuyển dữ liệu thành công!");
			}
		}

		private void btnGetData_Click(object sender, EventArgs e)
		{
			if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
			{
				textBox2.Text = folderBrowserDialog1.SelectedPath;
			}
		}

		private void btnConvert_Click(object sender, EventArgs e)
		{
			if (textBox1.Text != "")
			{
				compreRemovePatron();
				WriteXML();
				WriteXmlApi();
				WriterUserLdap();
				dgvPatron.DataSource = listPatron;
				dgvHad.DataSource = DSTonTai;
				CheckDataGridView(dgvPatron, lbCountListExcel);
				CheckDataGridView(dgvHad, lbCountHad);
				if (listPatron.Count > 0)
				{
					btn_api.Enabled = true;
					btnXml.Enabled = true;
					btn_ldap.Enabled = true;
					btnPush.Enabled = true;
					btnUpdateLdap.Enabled = true;
					btnConvert.Enabled = false;
				}
				MessageBox.Show("chuyển đổi dữ liệu thành công!", "Thông báo!");
			}
		}

		private void btnXml_Click(object sender, EventArgs e)
		{
			try
			{
				ExportDanhSachTT();
				File.WriteAllText(textBox2.Text + "/PatronTNUE-CanBo-" + tool.getDate() + ".xml", sbPatronXml.ToString());
				MessageBox.Show("Xuất file thành công!", "Thông báo!");
			}
			catch
			{
				MessageBox.Show("Xuất file không thành công!", "Lỗi!");
			}
		}

		private void btn_ldap_Click(object sender, EventArgs e)
		{
			using (StreamWriter streamWriter = new StreamWriter(directoryPath + "/Ldap-Log-" + tool.getDate() + ".txt"))
			{
				foreach (User item in ldapUser)
				{
					streamWriter.WriteLine(item.userLogin + "\t" + new ModelLdap().CreateUser(item));
				}
			}
			MessageBox.Show("Thành công!", "Thông báo!");
		}

		private void btn_api_Click(object sender, EventArgs e)
		{
			using (StreamWriter streamWriter = new StreamWriter(directoryPath + "/Api-Patron-Log-" + tool.getDate() + ".txt"))
			{
				foreach (StringBuilder item in listSb)
				{
					streamWriter.WriteLine(new AlephAPI().Url(item.ToString()));
				}
			}
			MessageBox.Show("Thành công!", "Thông báo!");
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

		private void ComboxLoaiBanDoc()
		{
			ComboboxItem comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Cán Bộ";
			comboboxItem.Value = "01";
			cbLoaiBanDoc.Items.Add(comboboxItem);
			comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Giảng Viên";
			comboboxItem.Value = "04";
			cbLoaiBanDoc.Items.Add(comboboxItem);
			comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Thư Viện Viên";
			comboboxItem.Value = "06";
			cbLoaiBanDoc.Items.Add(comboboxItem);
			cbLoaiBanDoc.SelectedIndex = 0;
		}

		private void readExcel2()
		{
			fileName = textBox1.Text;
			if (fileName == null)
			{
				MessageBox.Show("Chưa chọn file");
				return;
			}
			fileEx = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
            Excel.Workbook workbook = fileEx.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
			DateTime dateTime = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
			DateTime dateTime2 = DateTime.Parse(dateTime.AddYears(4).ToString("dd/MM/yyyy"));
			listPatron = new List<Patron>();
			sbList = new StringBuilder();
			int count = fileEx.Worksheets.Count;
			string str = txtLine.Text.Trim();
			int num = int.Parse(txtPatronId.Text);
			for (int i = 1; i < count + 1; i++)
			{
                Excel.Worksheet worksheet = (Excel.Worksheet)(dynamic)fileEx.Sheets[i];
				try
				{
					int count2 = worksheet.UsedRange.Rows.Count;
                    Excel.Range range = ((Excel.Worksheet)worksheet).get_Range((object)("A" + str), (object)("K" + count2));
					int count3 = range.Rows.Count;
					int count4 = range.Columns.Count;
					object[,] array = (object[,])(dynamic)range.Value2;
					for (int j = 1; j <= array.GetLength(0); j++)
					{
						string text = Convert.ToString(array[j, 2]);
						if (text != null && !text.Equals(""))
						{
							Patron patron = new Patron();
							patron.pationID = $"{num:000000000000}";
							patron.MaSV_O = Unicode.compound2Unicode(Convert.ToString(array[j, 2])).ToUpper().Trim();
							patron.GT = tool.convertGender(Convert.ToString(array[j, 6]));
							string text2 = Unicode.compound2Unicode(Convert.ToString(array[j, 3]) + " " + Convert.ToString(array[j, 4]));
							patron.HoTen = text2.Trim();
							patron.ngaySinh = tool.formatDate(Convert.ToString(array[j, 5]));
							patron.password = tool.formatDatePassword(Convert.ToString(array[j, 5]));
							patron.phone = Convert.ToString(array[j, 7]);
							patron.email = Convert.ToString(array[j, 8]);
							patron.makh = Convert.ToString(array[j, 9]);
							string text3 = Unicode.compound2Unicode(Convert.ToString(array[j, 10]));
							patron.chucVu = text3.Trim();
							string text4 = Convert.ToString(array[j, 11]);
							patron.chucDanh = Unicode.compound2Unicode(text4.Trim());
							patron.ngayHetHan = tool.formatDate(dateTime2.ToString());
							string text6 = patron.Day = tool.formatDate(dateTime.ToString());
							patron.DiaChi = "";
							patron.khoaHoc = "";
							patron.lopHoc = "";
							listPatron.Add(patron);
							num++;
						}
					}
				}
				catch (Exception arg)
				{
					MessageBox.Show("Lỗi: " + arg);
				}
			}
			workbook.Close(false, Type.Missing, Type.Missing);
			fileEx.Quit();
			Marshal.ReleaseComObject(workbook);
			Marshal.ReleaseComObject(fileEx);
			listPatron.RemoveAll((Patron item) => item.MaSV_O == "");
		}

		private void WriterUserLdap()
		{
			ldapUser = new List<User>();
			foreach (Patron item in listPatron)
			{
				User user = new User();
				user.cn = item.MaSV_O.Trim();
				user.sn = item.MaSV_O.Trim();
				user.userLogin = item.MaSV_O.Trim();
				user.userMail = item.email;
				user.userPassword = item.password;
				user.objectClass = "OpenLDAPPerson";
				user.telephoneNumber = item.phone;
				ldapUser.Add(user);
			}
		}

		private void WriteXML()
		{
			string block = (comboBox1.SelectedItem as ComboboxItem).Value.ToString();
			string status = (cbLoaiBanDoc.SelectedItem as ComboboxItem).Value.ToString();
			sbPatronXml = new StringBuilder();
			sbPatronXml.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			sbPatronXml.AppendLine("<p-file-20>");
			foreach (Patron item in listPatron)
			{
				sbPatronXml.AppendLine("<patron-record>");
				sbPatronXml.Append(new z303().tab3(item));
				sbPatronXml.Append(new z304().tab4(item));
				sbPatronXml.Append(new z305().tab5(item, block, status));
				sbPatronXml.Append(new z308().tab8(item));
				sbPatronXml.AppendLine("</patron-record>");
			}
			sbPatronXml.AppendLine("</p-file-20>");
		}

		private void WriteXmlApi()
		{
			listSb = new List<StringBuilder>();
			StringBuilder stringBuilder = null;
			string block = (comboBox1.SelectedItem as ComboboxItem).Value.ToString();
			string status = (cbLoaiBanDoc.SelectedItem as ComboboxItem).Value.ToString();
			foreach (Patron item in listPatron)
			{
				stringBuilder = new StringBuilder();
				stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
				stringBuilder.Append("<p-file-20>");
				stringBuilder.Append("<patron-record>");
				stringBuilder.Append(new z303().tab3(item));
				stringBuilder.Append(new z304().tab4(item));
				stringBuilder.Append(new z305().tab5(item, block, status));
				stringBuilder.Append(new z308().tab8(item));
				stringBuilder.Append("</patron-record>");
				stringBuilder.Append("</p-file-20>");
				listSb.Add(stringBuilder);
			}
			ExportDanhSachTT();
		}

		private void ExportDanhSachTT()
		{
			if (listPatron.Count > 0)
			{
				sbList = new StringBuilder();
				foreach (Patron item in listPatron)
				{
					sbList.Append(item.pationID);
					sbList.Append("\t");
					sbList.AppendLine(item.MaSV_O);
				}
				File.WriteAllText(textBox2.Text + "/DanhSachTT-CanBo-" + tool.getDate() + ".txt", sbList.ToString());
			}
		}

		private void compreRemovePatron()
		{
			DSTonTai = new List<Patron>();
			foreach (Z308 item in listZ308)
			{
				string text = item.Z308_REC_KEY.Trim();
				text = text.Substring(2);
				foreach (Patron item2 in listPatron)
				{
					if (text.Equals(item2.MaSV_O))
					{
						item2.pationID = item.Z308_ID;
						DSTonTai.Add(item2);
					}
				}
			}
			List<Patron> list = new List<Patron>();
			list = listPatron;
			foreach (Patron s in DSTonTai)
			{
				int index = list.FindIndex((Patron dsd) => dsd.MaSV_O.Equals(s.MaSV_O));
				listPatron.RemoveAt(index);
			}
		}

		private void CheckDataGridView(DataGridView gdv, Label lb)
		{
			if (gdv.ColumnCount > 0)
			{
				lb.Text = "Số lượng: " + gdv.RowCount.ToString();
			}
		}

		private void btnPush_Click(object sender, EventArgs e)
		{
			if (textBox2.Text != "")
			{
				Loading_FS.text = "\tĐang đưa dữ liệu ...";
				Loading_FS.ShowSplash();
				using (StreamWriter streamWriter = new StreamWriter(directoryPath + "/Api-Patron-Log-" + tool.getDate() + ".txt"))
				{
					foreach (StringBuilder item in listSb)
					{
						streamWriter.WriteLine(new AlephAPI().Url(item.ToString()));
					}
				}
				using (StreamWriter streamWriter2 = new StreamWriter(directoryPath + "/Ldap-Log-" + tool.getDate() + ".txt"))
				{
					foreach (User item2 in ldapUser)
					{
						streamWriter2.WriteLine(item2.userLogin + "\t" + new ModelLdap().CreateUser(item2));
					}
				}
				DataDBLocal.listZ308 = new QueryDB().listZ308TED();
				listZ308 = DataDBLocal.listZ308;
				Loading_FS.CloseSplash();
				MessageBox.Show("Thành công!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			else
			{
				MessageBox.Show("Chưa chọn đường dẫn lưu !", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void btnUpdateLdap_Click(object sender, EventArgs e)
		{
			foreach (User item in ldapUser)
			{
				new ModelLdap().SetAdInfo(item.userLogin, ModelLdap.Property.mail, item.userMail);
				new ModelLdap().SetAdInfo(item.userLogin, ModelLdap.Property.telephoneNumber, item.telephoneNumber);
			}
			MessageBox.Show("Thành công!", "Thông báo!");
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
			dgvPatron = new System.Windows.Forms.DataGridView();
			pationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			MaSV_O = new System.Windows.Forms.DataGridViewTextBoxColumn();
			HoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
			GT = new System.Windows.Forms.DataGridViewTextBoxColumn();
			ngaySinh = new System.Windows.Forms.DataGridViewTextBoxColumn();
			password = new System.Windows.Forms.DataGridViewTextBoxColumn();
			phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
			email = new System.Windows.Forms.DataGridViewTextBoxColumn();
			DiaChi = new System.Windows.Forms.DataGridViewTextBoxColumn();
			khoaHoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
			khoa = new System.Windows.Forms.DataGridViewTextBoxColumn();
			lopHoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
			makh = new System.Windows.Forms.DataGridViewTextBoxColumn();
			chucVu = new System.Windows.Forms.DataGridViewTextBoxColumn();
			chucDanh = new System.Windows.Forms.DataGridViewTextBoxColumn();
			QuocTich = new System.Windows.Forms.DataGridViewTextBoxColumn();
			hocBong = new System.Windows.Forms.DataGridViewTextBoxColumn();
			qdCongNhan = new System.Windows.Forms.DataGridViewTextBoxColumn();
			ngayHetHan = new System.Windows.Forms.DataGridViewTextBoxColumn();
			Day = new System.Windows.Forms.DataGridViewTextBoxColumn();
			groupBox1 = new System.Windows.Forms.GroupBox();
			btnUpdateLdap = new System.Windows.Forms.Button();
			label7 = new System.Windows.Forms.Label();
			pb_TaiChinh = new System.Windows.Forms.PictureBox();
			cbLoaiBanDoc = new System.Windows.Forms.ComboBox();
			btnPush = new System.Windows.Forms.Button();
			btnConvert = new System.Windows.Forms.Button();
			btnXml = new System.Windows.Forms.Button();
			label6 = new System.Windows.Forms.Label();
			btn_ldap = new System.Windows.Forms.Button();
			btn_api = new System.Windows.Forms.Button();
			txtPatronId = new System.Windows.Forms.TextBox();
			label4 = new System.Windows.Forms.Label();
			txtLine = new System.Windows.Forms.TextBox();
			label5 = new System.Windows.Forms.Label();
			comboBox1 = new System.Windows.Forms.ComboBox();
			textBox3 = new System.Windows.Forms.TextBox();
			textBox2 = new System.Windows.Forms.TextBox();
			label2 = new System.Windows.Forms.Label();
			btnBrowserFile = new System.Windows.Forms.Button();
			textBox1 = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			btnGetData = new System.Windows.Forms.Button();
			label3 = new System.Windows.Forms.Label();
			btnThoat = new System.Windows.Forms.Button();
			folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			groupBox2 = new System.Windows.Forms.GroupBox();
			dgvHad = new System.Windows.Forms.DataGridView();
			dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
			lbCountListExcel = new System.Windows.Forms.Label();
			lbCountHad = new System.Windows.Forms.Label();
			groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvPatron).BeginInit();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).BeginInit();
			groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvHad).BeginInit();
			SuspendLayout();
			groupBox3.Controls.Add(dgvPatron);
			groupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			groupBox3.Location = new System.Drawing.Point(3, 244);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(620, 302);
			groupBox3.TabIndex = 29;
			groupBox3.TabStop = false;
			groupBox3.Text = "DANH SÁCH";
			dgvPatron.AllowUserToAddRows = false;
			dgvPatron.AllowUserToDeleteRows = false;
			dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dgvPatron.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			dgvPatron.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dgvPatron.Columns.AddRange(pationID, MaSV_O, HoTen, GT, ngaySinh, password, phone, email, DiaChi, khoaHoc, khoa, lopHoc, makh, chucVu, chucDanh, QuocTich, hocBong, qdCongNhan, ngayHetHan, Day);
			dgvPatron.Dock = System.Windows.Forms.DockStyle.Fill;
			dgvPatron.Location = new System.Drawing.Point(3, 18);
			dgvPatron.Name = "dgvPatron";
			dgvPatron.ReadOnly = true;
			dgvPatron.RowHeadersWidth = 20;
			dgvPatron.Size = new System.Drawing.Size(614, 281);
			dgvPatron.TabIndex = 19;
			pationID.DataPropertyName = "pationID";
			pationID.HeaderText = "Patron ID";
			pationID.Name = "pationID";
			pationID.ReadOnly = true;
			pationID.Width = 81;
			MaSV_O.DataPropertyName = "MaSV_O";
			MaSV_O.HeaderText = "Mã Cán Bộ";
			MaSV_O.Name = "MaSV_O";
			MaSV_O.ReadOnly = true;
			MaSV_O.Width = 89;
			HoTen.DataPropertyName = "HoTen";
			HoTen.HeaderText = "Họ Tên";
			HoTen.Name = "HoTen";
			HoTen.ReadOnly = true;
			HoTen.Width = 68;
			GT.DataPropertyName = "GT";
			GT.HeaderText = "Giới Tính";
			GT.Name = "GT";
			GT.ReadOnly = true;
			GT.Width = 79;
			ngaySinh.DataPropertyName = "ngaySinh";
			ngaySinh.HeaderText = "Ngày Sinh";
			ngaySinh.Name = "ngaySinh";
			ngaySinh.ReadOnly = true;
			ngaySinh.Width = 86;
			password.DataPropertyName = "password";
			password.HeaderText = "Mật Khẩu";
			password.Name = "password";
			password.ReadOnly = true;
			password.Width = 83;
			phone.DataPropertyName = "phone";
			phone.HeaderText = "Điện Thoại";
			phone.Name = "phone";
			phone.ReadOnly = true;
			phone.Width = 88;
			email.DataPropertyName = "email";
			email.HeaderText = "EMail";
			email.Name = "email";
			email.ReadOnly = true;
			email.Width = 61;
			DiaChi.DataPropertyName = "DiaChi";
			DiaChi.HeaderText = "Địa Chỉ";
			DiaChi.Name = "DiaChi";
			DiaChi.ReadOnly = true;
			DiaChi.Width = 69;
			khoaHoc.DataPropertyName = "khoaHoc";
			khoaHoc.HeaderText = "Khoa";
			khoaHoc.Name = "khoaHoc";
			khoaHoc.ReadOnly = true;
			khoaHoc.Visible = false;
			khoaHoc.Width = 57;
			khoa.DataPropertyName = "khoa";
			khoa.HeaderText = "Khóa";
			khoa.Name = "khoa";
			khoa.ReadOnly = true;
			khoa.Visible = false;
			khoa.Width = 57;
			lopHoc.DataPropertyName = "lopHoc";
			lopHoc.HeaderText = "Lớp Học";
			lopHoc.Name = "lopHoc";
			lopHoc.ReadOnly = true;
			lopHoc.Visible = false;
			lopHoc.Width = 73;
			makh.DataPropertyName = "makh";
			makh.HeaderText = "Phòng";
			makh.Name = "makh";
			makh.ReadOnly = true;
			makh.Width = 67;
			chucVu.DataPropertyName = "chucVu";
			chucVu.HeaderText = "Chức Vụ";
			chucVu.Name = "chucVu";
			chucVu.ReadOnly = true;
			chucVu.Width = 75;
			chucDanh.DataPropertyName = "chucDanh";
			chucDanh.HeaderText = "Chức Danh";
			chucDanh.Name = "chucDanh";
			chucDanh.ReadOnly = true;
			chucDanh.Width = 89;
			QuocTich.DataPropertyName = "QuocTich";
			QuocTich.HeaderText = "Quốc Tịch";
			QuocTich.Name = "QuocTich";
			QuocTich.ReadOnly = true;
			QuocTich.Width = 83;
			hocBong.DataPropertyName = "hocBong";
			hocBong.HeaderText = "Học Bổng";
			hocBong.Name = "hocBong";
			hocBong.ReadOnly = true;
			hocBong.Visible = false;
			hocBong.Width = 80;
			qdCongNhan.DataPropertyName = "qdCongNhan";
			qdCongNhan.HeaderText = "QĐ Công Nhận";
			qdCongNhan.Name = "qdCongNhan";
			qdCongNhan.ReadOnly = true;
			qdCongNhan.Visible = false;
			qdCongNhan.Width = 105;
			ngayHetHan.DataPropertyName = "ngayHetHan";
			ngayHetHan.HeaderText = "Ngày Hết Hạn Thẻ";
			ngayHetHan.Name = "ngayHetHan";
			ngayHetHan.ReadOnly = true;
			ngayHetHan.Width = 127;
			Day.DataPropertyName = "Day";
			Day.HeaderText = "Ngày Hiện Tại";
			Day.Name = "Day";
			Day.ReadOnly = true;
			Day.Width = 105;
			groupBox1.Controls.Add(btnUpdateLdap);
			groupBox1.Controls.Add(label7);
			groupBox1.Controls.Add(pb_TaiChinh);
			groupBox1.Controls.Add(cbLoaiBanDoc);
			groupBox1.Controls.Add(btnPush);
			groupBox1.Controls.Add(btnConvert);
			groupBox1.Controls.Add(btnXml);
			groupBox1.Controls.Add(label6);
			groupBox1.Controls.Add(btn_ldap);
			groupBox1.Controls.Add(btn_api);
			groupBox1.Controls.Add(txtPatronId);
			groupBox1.Controls.Add(label4);
			groupBox1.Controls.Add(txtLine);
			groupBox1.Controls.Add(label5);
			groupBox1.Controls.Add(comboBox1);
			groupBox1.Controls.Add(textBox3);
			groupBox1.Controls.Add(textBox2);
			groupBox1.Controls.Add(label2);
			groupBox1.Controls.Add(btnBrowserFile);
			groupBox1.Controls.Add(textBox1);
			groupBox1.Controls.Add(label1);
			groupBox1.Controls.Add(btnGetData);
			groupBox1.Controls.Add(label3);
			groupBox1.Controls.Add(btnThoat);
			groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold);
			groupBox1.Location = new System.Drawing.Point(0, 3);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(993, 240);
			groupBox1.TabIndex = 28;
			groupBox1.TabStop = false;
			groupBox1.Text = "Cán bộ";
			btnUpdateLdap.Enabled = false;
			btnUpdateLdap.Location = new System.Drawing.Point(628, 154);
			btnUpdateLdap.Margin = new System.Windows.Forms.Padding(2);
			btnUpdateLdap.Name = "btnUpdateLdap";
			btnUpdateLdap.Size = new System.Drawing.Size(135, 35);
			btnUpdateLdap.TabIndex = 112;
			btnUpdateLdap.Text = "Update Ldap";
			btnUpdateLdap.UseVisualStyleBackColor = true;
			btnUpdateLdap.Visible = false;
			btnUpdateLdap.Click += new System.EventHandler(btnUpdateLdap_Click);
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(11, 198);
			label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(128, 25);
			label7.TabIndex = 111;
			label7.Text = "Loại Bạn Đọc";
			pb_TaiChinh.Image = TNUE_Patron_Excel.Properties.Resources.library_logo;
			pb_TaiChinh.Location = new System.Drawing.Point(796, 20);
			pb_TaiChinh.Name = "pb_TaiChinh";
			pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
			pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			pb_TaiChinh.TabIndex = 20;
			pb_TaiChinh.TabStop = false;
			cbLoaiBanDoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbLoaiBanDoc.FormattingEnabled = true;
			cbLoaiBanDoc.Location = new System.Drawing.Point(144, 195);
			cbLoaiBanDoc.Name = "cbLoaiBanDoc";
			cbLoaiBanDoc.Size = new System.Drawing.Size(316, 33);
			cbLoaiBanDoc.TabIndex = 110;
			btnPush.AutoSize = true;
			btnPush.BackColor = System.Drawing.Color.Green;
			btnPush.Enabled = false;
			btnPush.FlatAppearance.BorderSize = 0;
			btnPush.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnPush.ForeColor = System.Drawing.Color.White;
			btnPush.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnPush.Location = new System.Drawing.Point(464, 194);
			btnPush.Name = "btnPush";
			btnPush.Size = new System.Drawing.Size(159, 38);
			btnPush.TabIndex = 107;
			btnPush.Text = "Tạo người dùng";
			btnPush.UseVisualStyleBackColor = false;
			btnPush.Click += new System.EventHandler(btnPush_Click);
			btnConvert.AutoSize = true;
			btnConvert.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnConvert.FlatAppearance.BorderSize = 0;
			btnConvert.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnConvert.ForeColor = System.Drawing.Color.White;
			btnConvert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnConvert.Location = new System.Drawing.Point(464, 151);
			btnConvert.Name = "btnConvert";
			btnConvert.Size = new System.Drawing.Size(159, 38);
			btnConvert.TabIndex = 106;
			btnConvert.Text = "Chuyển dữ liệu";
			btnConvert.UseVisualStyleBackColor = false;
			btnConvert.Click += new System.EventHandler(btnConvert_Click);
			btnXml.AutoSize = true;
			btnXml.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnXml.FlatAppearance.BorderSize = 0;
			btnXml.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnXml.ForeColor = System.Drawing.Color.White;
			btnXml.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnXml.Location = new System.Drawing.Point(628, 113);
			btnXml.Name = "btnXml";
			btnXml.Size = new System.Drawing.Size(120, 38);
			btnXml.TabIndex = 105;
			btnXml.Text = "Xuất File Xml";
			btnXml.UseVisualStyleBackColor = false;
			btnXml.Visible = false;
			btnXml.Click += new System.EventHandler(btnXml_Click);
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(358, 36);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(135, 25);
			label6.TabIndex = 104;
			label6.Text = "Dòng bắt đầu";
			btn_ldap.Enabled = false;
			btn_ldap.Location = new System.Drawing.Point(628, 195);
			btn_ldap.Margin = new System.Windows.Forms.Padding(2);
			btn_ldap.Name = "btn_ldap";
			btn_ldap.Size = new System.Drawing.Size(120, 35);
			btn_ldap.TabIndex = 102;
			btn_ldap.Text = "Ldap";
			btn_ldap.UseVisualStyleBackColor = true;
			btn_ldap.Visible = false;
			btn_ldap.Click += new System.EventHandler(btn_ldap_Click);
			btn_api.Enabled = false;
			btn_api.Location = new System.Drawing.Point(752, 195);
			btn_api.Margin = new System.Windows.Forms.Padding(2);
			btn_api.Name = "btn_api";
			btn_api.Size = new System.Drawing.Size(121, 35);
			btn_api.TabIndex = 100;
			btn_api.Text = "API";
			btn_api.UseVisualStyleBackColor = true;
			btn_api.Visible = false;
			btn_api.Click += new System.EventHandler(btn_api_Click);
			txtPatronId.Enabled = false;
			txtPatronId.Location = new System.Drawing.Point(144, 31);
			txtPatronId.Margin = new System.Windows.Forms.Padding(2);
			txtPatronId.Name = "txtPatronId";
			txtPatronId.Size = new System.Drawing.Size(209, 33);
			txtPatronId.TabIndex = 99;
			txtPatronId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPatronId_KeyPress);
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(21, 36);
			label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(96, 25);
			label4.TabIndex = 98;
			label4.Text = "Patron Id";
			txtLine.Location = new System.Drawing.Point(499, 30);
			txtLine.Name = "txtLine";
			txtLine.Size = new System.Drawing.Size(76, 33);
			txtLine.TabIndex = 103;
			txtLine.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLine_KeyPress);
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(15, 154);
			label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(102, 25);
			label5.TabIndex = 94;
			label5.Text = "Trạng thái";
			comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBox1.FormattingEnabled = true;
			comboBox1.Location = new System.Drawing.Point(144, 155);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new System.Drawing.Size(316, 33);
			comboBox1.TabIndex = 92;
			textBox3.Enabled = false;
			textBox3.Location = new System.Drawing.Point(676, 30);
			textBox3.Name = "textBox3";
			textBox3.Size = new System.Drawing.Size(45, 33);
			textBox3.TabIndex = 96;
			textBox2.Enabled = false;
			textBox2.Location = new System.Drawing.Point(144, 115);
			textBox2.Margin = new System.Windows.Forms.Padding(2);
			textBox2.Name = "textBox2";
			textBox2.Size = new System.Drawing.Size(316, 33);
			textBox2.TabIndex = 93;
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(15, 115);
			label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(125, 25);
			label2.TabIndex = 91;
			label2.Text = "Thư mục lưu";
			btnBrowserFile.Location = new System.Drawing.Point(464, 76);
			btnBrowserFile.Margin = new System.Windows.Forms.Padding(2);
			btnBrowserFile.Name = "btnBrowserFile";
			btnBrowserFile.Size = new System.Drawing.Size(159, 33);
			btnBrowserFile.TabIndex = 90;
			btnBrowserFile.Text = "Chọn...";
			btnBrowserFile.UseVisualStyleBackColor = true;
			btnBrowserFile.Click += new System.EventHandler(btnBrowserFile_Click);
			textBox1.Enabled = false;
			textBox1.Location = new System.Drawing.Point(144, 76);
			textBox1.Margin = new System.Windows.Forms.Padding(2);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(316, 33);
			textBox1.TabIndex = 89;
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(15, 79);
			label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(122, 25);
			label1.TabIndex = 88;
			label1.Text = "Chọn tệp tin";
			btnGetData.Location = new System.Drawing.Point(464, 115);
			btnGetData.Margin = new System.Windows.Forms.Padding(2);
			btnGetData.Name = "btnGetData";
			btnGetData.Size = new System.Drawing.Size(159, 33);
			btnGetData.TabIndex = 87;
			btnGetData.Text = "Chọn...";
			btnGetData.UseVisualStyleBackColor = true;
			btnGetData.Click += new System.EventHandler(btnGetData_Click);
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(581, 33);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(89, 25);
			label3.TabIndex = 97;
			label3.Text = "Số Sheet";
			btnThoat.AutoSize = true;
			btnThoat.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
			btnThoat.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnThoat.FlatAppearance.BorderSize = 0;
			btnThoat.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnThoat.ForeColor = System.Drawing.Color.White;
			btnThoat.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnThoat.Location = new System.Drawing.Point(878, 193);
			btnThoat.Name = "btnThoat";
			btnThoat.Size = new System.Drawing.Size(111, 38);
			btnThoat.TabIndex = 14;
			btnThoat.Text = "Thoát";
			btnThoat.UseVisualStyleBackColor = false;
			btnThoat.Click += new System.EventHandler(btnThoat_Click);
			groupBox2.Controls.Add(dgvHad);
			groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			groupBox2.Location = new System.Drawing.Point(629, 245);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(364, 300);
			groupBox2.TabIndex = 30;
			groupBox2.TabStop = false;
			groupBox2.Text = "DANH SÁCH ĐÃ TỒN TẠI";
			dgvHad.AllowUserToAddRows = false;
			dgvHad.AllowUserToDeleteRows = false;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dgvHad.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
			dgvHad.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dgvHad.Columns.AddRange(dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn10, dataGridViewTextBoxColumn11, dataGridViewTextBoxColumn12, dataGridViewTextBoxColumn13, dataGridViewTextBoxColumn14, dataGridViewTextBoxColumn15, dataGridViewTextBoxColumn16, dataGridViewTextBoxColumn17, dataGridViewTextBoxColumn18, dataGridViewTextBoxColumn19, dataGridViewTextBoxColumn20);
			dgvHad.Dock = System.Windows.Forms.DockStyle.Fill;
			dgvHad.Location = new System.Drawing.Point(3, 18);
			dgvHad.Name = "dgvHad";
			dgvHad.ReadOnly = true;
			dgvHad.RowHeadersWidth = 20;
			dgvHad.Size = new System.Drawing.Size(358, 279);
			dgvHad.TabIndex = 20;
			dataGridViewTextBoxColumn1.DataPropertyName = "pationID";
			dataGridViewTextBoxColumn1.HeaderText = "Patron ID";
			dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			dataGridViewTextBoxColumn1.ReadOnly = true;
			dataGridViewTextBoxColumn1.Width = 81;
			dataGridViewTextBoxColumn2.DataPropertyName = "MaSV_O";
			dataGridViewTextBoxColumn2.HeaderText = "Mã Cán Bộ";
			dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			dataGridViewTextBoxColumn2.ReadOnly = true;
			dataGridViewTextBoxColumn2.Width = 89;
			dataGridViewTextBoxColumn3.DataPropertyName = "HoTen";
			dataGridViewTextBoxColumn3.HeaderText = "Họ Tên";
			dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
			dataGridViewTextBoxColumn3.ReadOnly = true;
			dataGridViewTextBoxColumn3.Width = 68;
			dataGridViewTextBoxColumn4.DataPropertyName = "GT";
			dataGridViewTextBoxColumn4.HeaderText = "Giới Tính";
			dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
			dataGridViewTextBoxColumn4.ReadOnly = true;
			dataGridViewTextBoxColumn4.Width = 79;
			dataGridViewTextBoxColumn5.DataPropertyName = "ngaySinh";
			dataGridViewTextBoxColumn5.HeaderText = "Ngày Sinh";
			dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
			dataGridViewTextBoxColumn5.ReadOnly = true;
			dataGridViewTextBoxColumn5.Width = 86;
			dataGridViewTextBoxColumn6.DataPropertyName = "password";
			dataGridViewTextBoxColumn6.HeaderText = "Mật Khẩu";
			dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
			dataGridViewTextBoxColumn6.ReadOnly = true;
			dataGridViewTextBoxColumn6.Width = 83;
			dataGridViewTextBoxColumn7.DataPropertyName = "phone";
			dataGridViewTextBoxColumn7.HeaderText = "Điện Thoại";
			dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
			dataGridViewTextBoxColumn7.ReadOnly = true;
			dataGridViewTextBoxColumn7.Width = 88;
			dataGridViewTextBoxColumn8.DataPropertyName = "email";
			dataGridViewTextBoxColumn8.HeaderText = "EMail";
			dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
			dataGridViewTextBoxColumn8.ReadOnly = true;
			dataGridViewTextBoxColumn8.Width = 61;
			dataGridViewTextBoxColumn9.DataPropertyName = "DiaChi";
			dataGridViewTextBoxColumn9.HeaderText = "Địa Chỉ";
			dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
			dataGridViewTextBoxColumn9.ReadOnly = true;
			dataGridViewTextBoxColumn9.Width = 69;
			dataGridViewTextBoxColumn10.DataPropertyName = "khoaHoc";
			dataGridViewTextBoxColumn10.HeaderText = "Khoa";
			dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
			dataGridViewTextBoxColumn10.ReadOnly = true;
			dataGridViewTextBoxColumn10.Visible = false;
			dataGridViewTextBoxColumn10.Width = 57;
			dataGridViewTextBoxColumn11.DataPropertyName = "khoa";
			dataGridViewTextBoxColumn11.HeaderText = "Khóa";
			dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			dataGridViewTextBoxColumn11.ReadOnly = true;
			dataGridViewTextBoxColumn11.Visible = false;
			dataGridViewTextBoxColumn11.Width = 57;
			dataGridViewTextBoxColumn12.DataPropertyName = "lopHoc";
			dataGridViewTextBoxColumn12.HeaderText = "Lớp Học";
			dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
			dataGridViewTextBoxColumn12.ReadOnly = true;
			dataGridViewTextBoxColumn12.Visible = false;
			dataGridViewTextBoxColumn12.Width = 73;
			dataGridViewTextBoxColumn13.DataPropertyName = "makh";
			dataGridViewTextBoxColumn13.HeaderText = "Phòng";
			dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
			dataGridViewTextBoxColumn13.ReadOnly = true;
			dataGridViewTextBoxColumn13.Width = 67;
			dataGridViewTextBoxColumn14.DataPropertyName = "chucVu";
			dataGridViewTextBoxColumn14.HeaderText = "Chức Vụ";
			dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
			dataGridViewTextBoxColumn14.ReadOnly = true;
			dataGridViewTextBoxColumn14.Width = 75;
			dataGridViewTextBoxColumn15.DataPropertyName = "chucDanh";
			dataGridViewTextBoxColumn15.HeaderText = "Chức Danh";
			dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
			dataGridViewTextBoxColumn15.ReadOnly = true;
			dataGridViewTextBoxColumn15.Width = 89;
			dataGridViewTextBoxColumn16.DataPropertyName = "QuocTich";
			dataGridViewTextBoxColumn16.HeaderText = "Quốc Tịch";
			dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
			dataGridViewTextBoxColumn16.ReadOnly = true;
			dataGridViewTextBoxColumn16.Width = 83;
			dataGridViewTextBoxColumn17.DataPropertyName = "hocBong";
			dataGridViewTextBoxColumn17.HeaderText = "Học Bổng";
			dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
			dataGridViewTextBoxColumn17.ReadOnly = true;
			dataGridViewTextBoxColumn17.Visible = false;
			dataGridViewTextBoxColumn17.Width = 80;
			dataGridViewTextBoxColumn18.DataPropertyName = "qdCongNhan";
			dataGridViewTextBoxColumn18.HeaderText = "QĐ Công Nhận";
			dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
			dataGridViewTextBoxColumn18.ReadOnly = true;
			dataGridViewTextBoxColumn18.Visible = false;
			dataGridViewTextBoxColumn18.Width = 105;
			dataGridViewTextBoxColumn19.DataPropertyName = "ngayHetHan";
			dataGridViewTextBoxColumn19.HeaderText = "Ngày Hết Hạn Thẻ";
			dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
			dataGridViewTextBoxColumn19.ReadOnly = true;
			dataGridViewTextBoxColumn19.Width = 127;
			dataGridViewTextBoxColumn20.DataPropertyName = "Day";
			dataGridViewTextBoxColumn20.HeaderText = "Ngày Hiện Tại";
			dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
			dataGridViewTextBoxColumn20.ReadOnly = true;
			dataGridViewTextBoxColumn20.Width = 105;
			lbCountListExcel.AutoSize = true;
			lbCountListExcel.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lbCountListExcel.Location = new System.Drawing.Point(3, 544);
			lbCountListExcel.Name = "lbCountListExcel";
			lbCountListExcel.Size = new System.Drawing.Size(76, 21);
			lbCountListExcel.TabIndex = 31;
			lbCountListExcel.Text = "Số lượng:";
			lbCountHad.AutoSize = true;
			lbCountHad.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lbCountHad.Location = new System.Drawing.Point(628, 544);
			lbCountHad.Name = "lbCountHad";
			lbCountHad.Size = new System.Drawing.Size(76, 21);
			lbCountHad.TabIndex = 32;
			lbCountHad.Text = "Số lượng:";
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(lbCountHad);
			base.Controls.Add(lbCountListExcel);
			base.Controls.Add(groupBox2);
			base.Controls.Add(groupBox3);
			base.Controls.Add(groupBox1);
			Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.Margin = new System.Windows.Forms.Padding(4);
			base.Name = "UCCanBo";
			base.Size = new System.Drawing.Size(1000, 565);
			base.Load += new System.EventHandler(UCCanBo_Load);
			groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvPatron).EndInit();
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).EndInit();
			groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvHad).EndInit();
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
