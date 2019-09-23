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
	public class UCSinhVien : UserControl
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

		private DataGridView dgvPatron;

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

		private DataGridView dgvHad;

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

		public UCSinhVien()
		{
			InitializeComponent();
		}

		private void UCNhanVien_Load(object sender, EventArgs e)
		{
			listZ308 = DataDBLocal.listZ308;
			ComboxBlock();
			ComboxLoaiBanDoc();
			txtPatronId.Text = "1";
			txtLine.Text = "12";
			countP = new QueryDB().CountPatron();
			txtPatronId.Text = $"{countP + 1:000000000000}";
			CreateFolder(directoryPath);
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
				File.WriteAllText(textBox2.Text + "/PatronTNUE-SinhVien-" + tool.getDate() + ".xml", sbPatronXml.ToString());
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

		private void CreateFolder(string directoryPath)
		{
			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
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
			comboboxItem.Text = "Sinh Viên";
			comboboxItem.Value = "02";
			cbLoaiBanDoc.Items.Add(comboboxItem);
			comboboxItem = new ComboboxItem();
			comboboxItem.Text = "Cao Học";
			comboboxItem.Value = "03";
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
			//DateTime dateTime = DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy"));
			//DateTime dateTime2 = DateTime.Parse(dateTime.AddYears(4).ToString("dd/MM/yyyy"));
            string dateTime = DateTime.Now.ToString("dd/MM/yyyy");
            string dateTime2 = DateTime.Now.AddYears(4).ToString("dd/MM/yyyy");

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
                    Excel.Range range = ((Excel.Worksheet)worksheet).get_Range((object)("A" + str), (object)("Q" + count2));
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
							patron.MaSV_O = Unicode.compound2Unicode(Convert.ToString(array[j, 2]).Trim()).ToUpper().Trim();
							patron.GT = Unicode.compound2Unicode(tool.convertGender(Convert.ToString(array[j, 8])));
							string text2 = Convert.ToString(array[j, 4]) + " " + Convert.ToString(array[j, 6]);
							patron.HoTen = Unicode.compound2Unicode(text2.Trim());
							string text4 = patron.Day = tool.formatDate(dateTime.ToString());
							string str2 = Convert.ToString(array[j, 12]);
							patron.khoaHoc = Unicode.compound2Unicode(str2);
							string str3 = Convert.ToString(array[j, 11]);
							patron.lopHoc = Unicode.compound2Unicode(str3);
							patron.ngaySinh = tool.formatDate(Convert.ToString(array[j, 7]));
							patron.password = tool.formatDatePassword(Convert.ToString(array[j, 7]));
							patron.phone = Convert.ToString(array[j, 9]);
							patron.email = Convert.ToString(array[j, 10]);
							string text5 = Convert.ToString(array[j, 17]);
							patron.ngayHetHan = tool.getNgayHetHan(Convert.ToString(array[j, 17]));
							patron.makh = "";
							patron.DiaChi = "";
							string str4 = Convert.ToString(array[j, 13]);
							patron.Khoa = Unicode.compound2Unicode(str4);
							string str5 = Convert.ToString(array[j, 14]);
							patron.QuocTich = Unicode.compound2Unicode(str5);
							string str6 = Convert.ToString(array[j, 15]);
							patron.hocBong = Unicode.compound2Unicode(str6);
							string str7 = Convert.ToString(array[j, 16]);
							patron.qdCongNhan = Unicode.compound2Unicode(str7);
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
				user.telephoneNumber = item.phone.Trim();
				user.userPassword = item.password;
				user.objectClass = "OpenLDAPPerson";
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
				sbPatronXml.Append("<patron-record>");
				sbPatronXml.Append(new z303().tab3(item));
				sbPatronXml.Append(new z304().tab4(item));
				sbPatronXml.Append(new z305().tab5(item, block, status));
				sbPatronXml.Append(new z308().tab8(item));
				sbPatronXml.Append("</patron-record>");
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
				stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><p-file-20><patron-record>");
				stringBuilder.Append(new z303().tab3(item));
				stringBuilder.Append(new z304().tab4(item));
				stringBuilder.Append(new z305().tab5(item, block, status));
				stringBuilder.Append(new z308().tab8(item));
				stringBuilder.Append("</patron-record></p-file-20>");
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
				File.WriteAllText(textBox2.Text + "/DanhSachTT-SinhVien-" + tool.getDate() + ".txt", sbList.ToString());
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
			RemovePatron();
			int num = int.Parse(txtPatronId.Text);
			foreach (Patron item3 in listPatron)
			{
				item3.pationID = $"{num:000000000000}";
				num++;
			}
		}

		private void RemovePatron()
		{
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
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
			label7 = new System.Windows.Forms.Label();
			cbLoaiBanDoc = new System.Windows.Forms.ComboBox();
			pb_TaiChinh = new System.Windows.Forms.PictureBox();
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
			groupBox3.Location = new System.Drawing.Point(3, 246);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(620, 297);
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
			dgvPatron.Size = new System.Drawing.Size(614, 276);
			dgvPatron.TabIndex = 18;
			pationID.DataPropertyName = "pationID";
			pationID.HeaderText = "Patron ID";
			pationID.Name = "pationID";
			pationID.ReadOnly = true;
			pationID.Width = 81;
			MaSV_O.DataPropertyName = "MaSV_O";
			MaSV_O.HeaderText = "Mã Sinh Viên";
			MaSV_O.Name = "MaSV_O";
			MaSV_O.ReadOnly = true;
			MaSV_O.Width = 101;
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
			dataGridViewCellStyle2.Format = "d";
			dataGridViewCellStyle2.NullValue = null;
			ngaySinh.DefaultCellStyle = dataGridViewCellStyle2;
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
			khoaHoc.Width = 59;
			khoa.DataPropertyName = "khoa";
			khoa.HeaderText = "Khóa";
			khoa.Name = "khoa";
			khoa.ReadOnly = true;
			khoa.Width = 59;
			lopHoc.DataPropertyName = "lopHoc";
			lopHoc.HeaderText = "Lớp Học";
			lopHoc.Name = "lopHoc";
			lopHoc.ReadOnly = true;
			lopHoc.Width = 75;
			makh.DataPropertyName = "makh";
			makh.HeaderText = "makh";
			makh.Name = "makh";
			makh.ReadOnly = true;
			makh.Visible = false;
			makh.Width = 58;
			chucVu.DataPropertyName = "chucVu";
			chucVu.HeaderText = "Chức Vụ";
			chucVu.Name = "chucVu";
			chucVu.ReadOnly = true;
			chucVu.Visible = false;
			chucVu.Width = 73;
			chucDanh.DataPropertyName = "chucDanh";
			chucDanh.HeaderText = "Chức Danh";
			chucDanh.Name = "chucDanh";
			chucDanh.ReadOnly = true;
			chucDanh.Visible = false;
			chucDanh.Width = 86;
			QuocTich.DataPropertyName = "QuocTich";
			QuocTich.HeaderText = "Quốc Tịch";
			QuocTich.Name = "QuocTich";
			QuocTich.ReadOnly = true;
			QuocTich.Width = 83;
			hocBong.DataPropertyName = "hocBong";
			hocBong.HeaderText = "Học Bổng";
			hocBong.Name = "hocBong";
			hocBong.ReadOnly = true;
			hocBong.Width = 83;
			qdCongNhan.DataPropertyName = "qdCongNhan";
			qdCongNhan.HeaderText = "QĐ Công Nhận";
			qdCongNhan.Name = "qdCongNhan";
			qdCongNhan.ReadOnly = true;
			qdCongNhan.Width = 111;
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
			groupBox1.Controls.Add(label7);
			groupBox1.Controls.Add(cbLoaiBanDoc);
			groupBox1.Controls.Add(pb_TaiChinh);
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
			groupBox1.Text = "Sinh vien";
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(11, 199);
			label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(128, 25);
			label7.TabIndex = 109;
			label7.Text = "Loại Bạn Đọc";
			cbLoaiBanDoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbLoaiBanDoc.FormattingEnabled = true;
			cbLoaiBanDoc.Location = new System.Drawing.Point(144, 196);
			cbLoaiBanDoc.Name = "cbLoaiBanDoc";
			cbLoaiBanDoc.Size = new System.Drawing.Size(316, 33);
			cbLoaiBanDoc.TabIndex = 108;
			pb_TaiChinh.Image = TNUE_Patron_Excel.Properties.Resources.library_logo;
			pb_TaiChinh.Location = new System.Drawing.Point(795, 20);
			pb_TaiChinh.Name = "pb_TaiChinh";
			pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
			pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			pb_TaiChinh.TabIndex = 20;
			pb_TaiChinh.TabStop = false;
			btnPush.AutoSize = true;
			btnPush.BackColor = System.Drawing.Color.Green;
			btnPush.Enabled = false;
			btnPush.FlatAppearance.BorderSize = 0;
			btnPush.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnPush.ForeColor = System.Drawing.Color.White;
			btnPush.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnPush.Location = new System.Drawing.Point(464, 193);
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
			btnXml.Location = new System.Drawing.Point(628, 151);
			btnXml.Name = "btnXml";
			btnXml.Size = new System.Drawing.Size(120, 38);
			btnXml.TabIndex = 105;
			btnXml.Text = "Xuất File Xml";
			btnXml.UseVisualStyleBackColor = false;
			btnXml.Visible = false;
			btnXml.Click += new System.EventHandler(btnXml_Click);
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(403, 39);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(135, 25);
			label6.TabIndex = 104;
			label6.Text = "Dòng bắt đầu";
			btn_ldap.Enabled = false;
			btn_ldap.Location = new System.Drawing.Point(629, 194);
			btn_ldap.Margin = new System.Windows.Forms.Padding(2);
			btn_ldap.Name = "btn_ldap";
			btn_ldap.Size = new System.Drawing.Size(119, 35);
			btn_ldap.TabIndex = 102;
			btn_ldap.Text = "Ldap";
			btn_ldap.UseVisualStyleBackColor = true;
			btn_ldap.Visible = false;
			btn_ldap.Click += new System.EventHandler(btn_ldap_Click);
			btn_api.Enabled = false;
			btn_api.Location = new System.Drawing.Point(752, 194);
			btn_api.Margin = new System.Windows.Forms.Padding(2);
			btn_api.Name = "btn_api";
			btn_api.Size = new System.Drawing.Size(121, 35);
			btn_api.TabIndex = 100;
			btn_api.Text = "API";
			btn_api.UseVisualStyleBackColor = true;
			btn_api.Visible = false;
			btn_api.Click += new System.EventHandler(btn_api_Click);
			txtPatronId.Enabled = false;
			txtPatronId.Location = new System.Drawing.Point(144, 36);
			txtPatronId.Margin = new System.Windows.Forms.Padding(2);
			txtPatronId.Name = "txtPatronId";
			txtPatronId.Size = new System.Drawing.Size(252, 33);
			txtPatronId.TabIndex = 99;
			txtPatronId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPatronId_KeyPress);
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(15, 36);
			label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(96, 25);
			label4.TabIndex = 98;
			label4.Text = "Patron Id";
			txtLine.Location = new System.Drawing.Point(544, 33);
			txtLine.Name = "txtLine";
			txtLine.Size = new System.Drawing.Size(76, 33);
			txtLine.TabIndex = 103;
			txtLine.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLine_KeyPress);
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(15, 158);
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
			textBox3.Location = new System.Drawing.Point(721, 33);
			textBox3.Name = "textBox3";
			textBox3.Size = new System.Drawing.Size(45, 33);
			textBox3.TabIndex = 96;
			textBox2.Enabled = false;
			textBox2.Location = new System.Drawing.Point(144, 116);
			textBox2.Margin = new System.Windows.Forms.Padding(2);
			textBox2.Name = "textBox2";
			textBox2.Size = new System.Drawing.Size(316, 33);
			textBox2.TabIndex = 93;
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(15, 116);
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
			label3.Location = new System.Drawing.Point(626, 36);
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
			btnThoat.Size = new System.Drawing.Size(109, 38);
			btnThoat.TabIndex = 14;
			btnThoat.Text = "Thoát";
			btnThoat.UseVisualStyleBackColor = false;
			btnThoat.Click += new System.EventHandler(btnThoat_Click);
			groupBox2.Controls.Add(dgvHad);
			groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			groupBox2.Location = new System.Drawing.Point(629, 246);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(364, 297);
			groupBox2.TabIndex = 30;
			groupBox2.TabStop = false;
			groupBox2.Text = "DANH SÁCH ĐÃ TỒN TẠI";
			dgvHad.AllowUserToAddRows = false;
			dgvHad.AllowUserToDeleteRows = false;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dgvHad.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
			dgvHad.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dgvHad.Columns.AddRange(dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn10, dataGridViewTextBoxColumn11, dataGridViewTextBoxColumn12, dataGridViewTextBoxColumn13, dataGridViewTextBoxColumn14, dataGridViewTextBoxColumn15, dataGridViewTextBoxColumn16, dataGridViewTextBoxColumn17, dataGridViewTextBoxColumn18, dataGridViewTextBoxColumn19, dataGridViewTextBoxColumn20);
			dgvHad.Dock = System.Windows.Forms.DockStyle.Fill;
			dgvHad.Location = new System.Drawing.Point(3, 18);
			dgvHad.Name = "dgvHad";
			dgvHad.ReadOnly = true;
			dgvHad.RowHeadersWidth = 20;
			dgvHad.Size = new System.Drawing.Size(358, 276);
			dgvHad.TabIndex = 19;
			dataGridViewTextBoxColumn1.DataPropertyName = "pationID";
			dataGridViewTextBoxColumn1.HeaderText = "Patron ID";
			dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
			dataGridViewTextBoxColumn1.ReadOnly = true;
			dataGridViewTextBoxColumn1.Width = 81;
			dataGridViewTextBoxColumn2.DataPropertyName = "MaSV_O";
			dataGridViewTextBoxColumn2.HeaderText = "Mã Sinh Viên";
			dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
			dataGridViewTextBoxColumn2.ReadOnly = true;
			dataGridViewTextBoxColumn2.Width = 101;
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
			dataGridViewTextBoxColumn10.Width = 59;
			dataGridViewTextBoxColumn11.DataPropertyName = "khoa";
			dataGridViewTextBoxColumn11.HeaderText = "Khóa";
			dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
			dataGridViewTextBoxColumn11.ReadOnly = true;
			dataGridViewTextBoxColumn11.Width = 59;
			dataGridViewTextBoxColumn12.DataPropertyName = "lopHoc";
			dataGridViewTextBoxColumn12.HeaderText = "Lớp Học";
			dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
			dataGridViewTextBoxColumn12.ReadOnly = true;
			dataGridViewTextBoxColumn12.Width = 75;
			dataGridViewTextBoxColumn13.DataPropertyName = "makh";
			dataGridViewTextBoxColumn13.HeaderText = "makh";
			dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
			dataGridViewTextBoxColumn13.ReadOnly = true;
			dataGridViewTextBoxColumn13.Visible = false;
			dataGridViewTextBoxColumn13.Width = 58;
			dataGridViewTextBoxColumn14.DataPropertyName = "chucVu";
			dataGridViewTextBoxColumn14.HeaderText = "Chức Vụ";
			dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
			dataGridViewTextBoxColumn14.ReadOnly = true;
			dataGridViewTextBoxColumn14.Visible = false;
			dataGridViewTextBoxColumn14.Width = 73;
			dataGridViewTextBoxColumn15.DataPropertyName = "chucDanh";
			dataGridViewTextBoxColumn15.HeaderText = "Chức Danh";
			dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
			dataGridViewTextBoxColumn15.ReadOnly = true;
			dataGridViewTextBoxColumn15.Visible = false;
			dataGridViewTextBoxColumn15.Width = 86;
			dataGridViewTextBoxColumn16.DataPropertyName = "QuocTich";
			dataGridViewTextBoxColumn16.HeaderText = "Quốc Tịch";
			dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
			dataGridViewTextBoxColumn16.ReadOnly = true;
			dataGridViewTextBoxColumn16.Width = 83;
			dataGridViewTextBoxColumn17.DataPropertyName = "hocBong";
			dataGridViewTextBoxColumn17.HeaderText = "Học Bổng";
			dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
			dataGridViewTextBoxColumn17.ReadOnly = true;
			dataGridViewTextBoxColumn17.Width = 83;
			dataGridViewTextBoxColumn18.DataPropertyName = "qdCongNhan";
			dataGridViewTextBoxColumn18.HeaderText = "QĐ Công Nhận";
			dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
			dataGridViewTextBoxColumn18.ReadOnly = true;
			dataGridViewTextBoxColumn18.Width = 111;
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
			lbCountListExcel.Location = new System.Drawing.Point(3, 542);
			lbCountListExcel.Name = "lbCountListExcel";
			lbCountListExcel.Size = new System.Drawing.Size(76, 21);
			lbCountListExcel.TabIndex = 31;
			lbCountListExcel.Text = "Số lượng:";
			lbCountHad.AutoSize = true;
			lbCountHad.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lbCountHad.Location = new System.Drawing.Point(628, 542);
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
			base.Name = "UCSinhVien";
			base.Size = new System.Drawing.Size(1000, 565);
			base.Load += new System.EventHandler(UCNhanVien_Load);
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
