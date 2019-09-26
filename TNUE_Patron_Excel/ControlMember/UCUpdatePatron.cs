using FastMember;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

namespace TNUE_Patron_Excel.ControlMember
{
	public class UCUpdatePatron : UserControl
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

		private string directoryPath = System.Windows.Forms.Application.StartupPath + "\\log";

		private DataTable table = null;

		private int dem = 0;

		private IContainer components = null;

		private FolderBrowserDialog folderBrowserDialog1;

		private GroupBox groupBox4;

		private Panel panel3;

		private RadioButton rbLdap;

		private RadioButton rbAleph;

		private Panel panelLdap;

		private Button btnUnSearch;

		private TextBox txtSearch;

		private Label label8;

		private TextBox txtPassword;

		private Label label9;

		private TextBox txtPhone;

		private Label label10;

		private TextBox txtEmail;

		private Label label11;

		private TextBox txtMa;

		private Button btnSua;

		private Panel panel2;

		private BindingNavigator bindingNavigator1;

		private ToolStripButton bindingNavigatorAddNewItem;

		private ToolStripLabel bindingNavigatorCountItem;

		private ToolStripButton bindingNavigatorDeleteItem;

		private ToolStripButton bindingNavigatorMoveFirstItem;

		private ToolStripButton bindingNavigatorMovePreviousItem;

		private ToolStripSeparator bindingNavigatorSeparator;

		private ToolStripTextBox bindingNavigatorPositionItem;

		private ToolStripSeparator bindingNavigatorSeparator1;

		private ToolStripButton bindingNavigatorMoveNextItem;

		private ToolStripButton bindingNavigatorMoveLastItem;

		private ToolStripSeparator bindingNavigatorSeparator2;

		private SuperGird superGird1;

		private DataGridViewTextBoxColumn userLogin;

		private DataGridViewTextBoxColumn userMail;

		private DataGridViewTextBoxColumn telephoneNumber;

		private Button btnSearch;

		private PictureBox pictureBox2;

		private Panel panelUpdateSeris;

		private Label lbCountHad;

		private Label lbCountListExcel;

		private GroupBox groupBox2;

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

		private GroupBox groupBox3;

		private DataGridView dgvPatron;

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

		private GroupBox groupBox1;

		private Label label7;

		private ComboBox cbLoaiBanDoc;

		private PictureBox pb_TaiChinh;

		private Button btnPush;

		private Button btnConvert;

		private Label label6;

		private TextBox txtLine;

		private Label label5;

		private ComboBox comboBox1;

		private TextBox textBox3;

		private Button btnBrowserFile;

		private TextBox textBox1;

		private Label label1;

		private Label label3;

		private Button btnThoat;

		private GroupBox groupBox5;

		private RadioButton rbSinhVien;

		private RadioButton rbCanBo;

		private Label label2;

		private Label label4;

		private Button btnhien;

		public UCUpdatePatron()
		{
			InitializeComponent();
		}

		private void UCUpdatePatron_Load(object sender, EventArgs e)
		{
			try
			{
				if (rbLdap.Checked)
				{
					panelLdap.Visible = true;
					panelUpdateSeris.Visible = false;
				}
				LoadUserCase();
			}
			catch
			{
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

		private void BtnSua_Click(object sender, EventArgs e)
		{
			if (txtMa.Text == "")
			{
				MessageBox.Show("Email không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (MessageBox.Show("Bạn có chắc chắn muốn sửa?", "Xác nhân", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
			{
				EditLdap();
				using (StreamWriter streamWriter = new StreamWriter(directoryPath + "/Api-Patron-Update-Log-" + tool.getDate() + ".txt"))
				{
					streamWriter.WriteLine(new AlephAPI().Url(UpdatePatronEmailSdtPassword(SearchPatronId(txtMa.Text.Trim()))));
				}
				LoadUserCase();
				MessageBox.Show("Đã sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void BtnSearch_Click(object sender, EventArgs e)
		{
			if (txtSearch.Text.Trim() != "")
			{
				string inputText = txtSearch.Text.Trim().ToUpper();
				DataTable dataSource = (from r in table.AsEnumerable()
					where r.Field<string>("userLogin").Contains(inputText)
					select r).CopyToDataTable();
				superGird1.Columns.Clear();
				superGird1.DataSource = dataSource;
			}
		}

		private void BtnUnSearch_Click(object sender, EventArgs e)
		{
			superGird1.Columns.Clear();
			superGird1.SetPagedDataSource(table, bindingNavigator1);
		}

		private void SuperGird1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			int rowIndex = e.RowIndex;
			try
			{
				txtMa.Text = superGird1.Rows[rowIndex].Cells[0].Value.ToString();
				txtEmail.Text = superGird1.Rows[rowIndex].Cells[1].Value.ToString();
				txtPhone.Text = superGird1.Rows[rowIndex].Cells[2].Value.ToString();
			}
			catch
			{
			}
		}

		private void BtnBrowserFile_Click_1(object sender, EventArgs e)
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
				if (rbSinhVien.Checked)
				{
					readExcelSinhVien();
				}
				else if (rbCanBo.Checked)
				{
					readExcelCanBo();
				}
				btnConvert.Enabled = true;
				MessageBox.Show("Chuyển dữ liệu thành công!");
			}
		}

		private void BtnConvert_Click_1(object sender, EventArgs e)
		{
			if (textBox1.Text != "")
			{
				try
				{
					Loading_FS.text = "\tĐang chuyển đổi dữ liệu ...";
					Loading_FS.ShowSplash();
					compreRemovePatron();
					WriteXML();
					WriteXmlApi();
					WriterUserLdap();
					dgvPatron.DataSource = DSTonTai;
					dgvHad.DataSource = listPatron;
					CheckDataGridView(dgvPatron, lbCountListExcel);
					CheckDataGridView(dgvHad, lbCountHad);
					Loading_FS.CloseSplash();
					btnPush.Enabled = true;
					MessageBox.Show("chuyển đổi dữ liệu thành công!", "Thông báo!");
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Lỗi!");
				}
			}
		}

		private void BtnPush_Click_1(object sender, EventArgs e)
		{
			try
			{
				if (MessageBox.Show("Bạn có chắc chắn muốn cập nhập?", "Xác nhân", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
				{
					Loading_FS.text = "\tĐang cập nhập các bạn đọc ...";
					Loading_FS.ShowSplash();
					UpdatePatron();
					Loading_FS.CloseSplash();
					MessageBox.Show("Đã cập nhập thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Lỗi!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		private void BtnThoat_Click_1(object sender, EventArgs e)
		{
			System.Windows.Forms.Application.Exit();
		}

		private void TxtLine_KeyPress_1(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void RbAleph_CheckedChanged(object sender, EventArgs e)
		{
			panelLdap.Visible = false;
			panelUpdateSeris.Visible = true;
		}

		private void RbLdap_CheckedChanged(object sender, EventArgs e)
		{
			panelLdap.Visible = true;
			panelUpdateSeris.Visible = false;
		}

		private void UpdatePatron()
		{
			using (StreamWriter streamWriter = new StreamWriter(directoryPath + "/Api-Patron-Update-Log-" + tool.getDate() + ".txt"))
			{
				foreach (StringBuilder item in listSb)
				{
					streamWriter.WriteLine(new AlephAPI().Url(item.ToString()));
				}
			}
			using (new StreamWriter(directoryPath + "/Ldap-Update-Log-" + tool.getDate() + ".txt"))
			{
				foreach (User item2 in ldapUser)
				{
					if (item2.userMail != null && item2.userMail != "")
					{
						new ModelLdap().SetAdInfo(item2.userLogin, ModelLdap.Property.mail, item2.userMail);
					}
					if (item2.telephoneNumber != null && item2.telephoneNumber != "")
					{
						new ModelLdap().SetAdInfo(item2.userLogin, ModelLdap.Property.telephoneNumber, item2.telephoneNumber);
					}
                    if (item2.userPassword != null && item2.userPassword != "")
					{
						new ModelLdap().SetAdInfo(item2.userPassword, ModelLdap.Property.userPassword, item2.userPassword);
					}
				}
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

		private void readExcelSinhVien()
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
			int num = 1;
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

		private void readExcelCanBo()
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
			int num = 1;
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
			foreach (Patron item in DSTonTai)
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
			foreach (Patron item in DSTonTai)
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
			foreach (Patron item in DSTonTai)
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

		private string SearchPatronId(string patronBarcode)
		{
			try
			{
				Z308 z = listZ308.Find((Z308 x) => x.Z308_REC_KEY.Contains(patronBarcode));
				return z.Z308_ID;
			}
			catch (Exception)
			{
				MessageBox.Show("Mã sinh viên không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			return null;
		}

		private void CheckDataGridView(DataGridView gdv, Label lb)
		{
			if (gdv.ColumnCount > 0)
			{
				lb.Text = "Số lượng: " + gdv.RowCount.ToString();
			}
		}

		private void LoadUserCase()
		{
			Loading_FS.text = "\tĐang cập nhập dữ liệu ...";
			Loading_FS.ShowSplash();
			groupBox4.Enabled = false;
			listZ308 = DataDBLocal.listZ308;
			ComboxBlock();
			ComboxLoaiBanDoc();
			txtLine.Text = "12";
			CreateFolder(directoryPath);
			superGird1._pageSize = 100;
			IEnumerable<User> allListUser = new ModelLdap().GetAllListUser();
			superGird1.DataSource = null;
			table = new DataTable();
			using (ObjectReader reader = ObjectReader.Create(allListUser, "userLogin", "userMail", "telephoneNumber"))
			{
				table.Load(reader);
			}
			superGird1.SetPagedDataSource(table, bindingNavigator1);
			groupBox4.Enabled = true;
			Loading_FS.CloseSplash();
		}

		private void LoadUserCaseList()
		{
			dgvPatron.DataSource = null;
			dgvHad.DataSource = null;
		}

		private void EditLdap()
		{
			string objectFilter = txtMa.Text.Trim();
			if (txtEmail.Text == "")
			{
				MessageBox.Show("Email không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			new ModelLdap().SetAdInfo(objectFilter, ModelLdap.Property.mail, txtEmail.Text);
			new ModelLdap().SetAdInfo(objectFilter, ModelLdap.Property.telephoneNumber, txtPhone.Text);
			if (txtPassword.Text != "")
			{
				new ModelLdap().SetAdInfo(objectFilter, ModelLdap.Property.userPassword, txtPassword.Text);
			}
		}

		private string UpdatePatronEmailSdtPassword(string patronId)
		{
			User user = new User();
			user.userMail = txtEmail.Text.Trim();
			user.telephoneNumber = txtPhone.Text.Trim();
			if (txtPassword.Text != "")
			{
				user.userPassword = txtPassword.Text.Trim();
			}
			return sbPatronApi(patronId, user).ToString();
		}

		private StringBuilder sbPatronApi(string patronId, User user)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			stringBuilder.Append("<p-file-20>");
			stringBuilder.Append("<patron-record>");
			stringBuilder.Append(new z303Block().tab3(patronId));
			stringBuilder.Append(new z304Update().tab4(patronId, user));
			if (user.userPassword != null || user.userPassword != "")
			{
				stringBuilder.Append(new z308Update().tab8(patronId, user));
			}
			stringBuilder.Append("</patron-record>");
			stringBuilder.Append("</p-file-20>");
			return stringBuilder;
		}

		private void RbSinhVien_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void RbCanBo_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void Btnhien_Click(object sender, EventArgs e)
		{
			if (dem == 0)
			{
				txtPassword.UseSystemPasswordChar = false;
				dem++;
			}
			else
			{
				txtPassword.UseSystemPasswordChar = true;
				dem--;
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCUpdatePatron));
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbLdap = new System.Windows.Forms.RadioButton();
            this.rbAleph = new System.Windows.Forms.RadioButton();
            this.panelUpdateSeris = new System.Windows.Forms.Panel();
            this.lbCountHad = new System.Windows.Forms.Label();
            this.lbCountListExcel = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvHad = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvPatron = new System.Windows.Forms.DataGridView();
            this.pationID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaSV_O = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ngaySinh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.email = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiaChi = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.khoaHoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.khoa = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lopHoc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.makh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chucVu = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chucDanh = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.QuocTich = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hocBong = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qdCongNhan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ngayHetHan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Day = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rbCanBo = new System.Windows.Forms.RadioButton();
            this.rbSinhVien = new System.Windows.Forms.RadioButton();
            this.label7 = new System.Windows.Forms.Label();
            this.cbLoaiBanDoc = new System.Windows.Forms.ComboBox();
            this.pb_TaiChinh = new System.Windows.Forms.PictureBox();
            this.btnPush = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLine = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.btnBrowserFile = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnThoat = new System.Windows.Forms.Button();
            this.panelLdap = new System.Windows.Forms.Panel();
            this.btnhien = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnUnSearch = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtMa = new System.Windows.Forms.TextBox();
            this.btnSua = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.superGird1 = new TNUE_Patron_Excel.SuperGird();
            this.userLogin = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userMail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.telephoneNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSearch = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelUpdateSeris.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHad)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatron)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).BeginInit();
            this.panelLdap.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superGird1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.panel3);
            this.groupBox4.Controls.Add(this.panelUpdateSeris);
            this.groupBox4.Controls.Add(this.panelLdap);
            this.groupBox4.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox4.Location = new System.Drawing.Point(0, 6);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(993, 559);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Cập nhập bạn đọc";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rbLdap);
            this.panel3.Controls.Add(this.rbAleph);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 29);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(987, 37);
            this.panel3.TabIndex = 32;
            // 
            // rbLdap
            // 
            this.rbLdap.AutoSize = true;
            this.rbLdap.Checked = true;
            this.rbLdap.Location = new System.Drawing.Point(15, 3);
            this.rbLdap.Name = "rbLdap";
            this.rbLdap.Size = new System.Drawing.Size(246, 29);
            this.rbLdap.TabIndex = 1;
            this.rbLdap.TabStop = true;
            this.rbLdap.Text = "Cập nhập thông tin ldap";
            this.rbLdap.UseVisualStyleBackColor = true;
            this.rbLdap.CheckedChanged += new System.EventHandler(this.RbLdap_CheckedChanged);
            // 
            // rbAleph
            // 
            this.rbAleph.AutoSize = true;
            this.rbAleph.Location = new System.Drawing.Point(264, 3);
            this.rbAleph.Name = "rbAleph";
            this.rbAleph.Size = new System.Drawing.Size(203, 29);
            this.rbAleph.TabIndex = 0;
            this.rbAleph.Text = "Cập nhập hàng loạt";
            this.rbAleph.UseVisualStyleBackColor = true;
            this.rbAleph.CheckedChanged += new System.EventHandler(this.RbAleph_CheckedChanged);
            // 
            // panelUpdateSeris
            // 
            this.panelUpdateSeris.Controls.Add(this.lbCountHad);
            this.panelUpdateSeris.Controls.Add(this.lbCountListExcel);
            this.panelUpdateSeris.Controls.Add(this.groupBox2);
            this.panelUpdateSeris.Controls.Add(this.groupBox3);
            this.panelUpdateSeris.Controls.Add(this.groupBox1);
            this.panelUpdateSeris.Location = new System.Drawing.Point(3, 69);
            this.panelUpdateSeris.Name = "panelUpdateSeris";
            this.panelUpdateSeris.Size = new System.Drawing.Size(987, 489);
            this.panelUpdateSeris.TabIndex = 34;
            this.panelUpdateSeris.Visible = false;
            // 
            // lbCountHad
            // 
            this.lbCountHad.AutoSize = true;
            this.lbCountHad.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCountHad.Location = new System.Drawing.Point(639, 464);
            this.lbCountHad.Name = "lbCountHad";
            this.lbCountHad.Size = new System.Drawing.Size(76, 21);
            this.lbCountHad.TabIndex = 37;
            this.lbCountHad.Text = "Số lượng:";
            // 
            // lbCountListExcel
            // 
            this.lbCountListExcel.AutoSize = true;
            this.lbCountListExcel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCountListExcel.Location = new System.Drawing.Point(4, 461);
            this.lbCountListExcel.Name = "lbCountListExcel";
            this.lbCountListExcel.Size = new System.Drawing.Size(76, 21);
            this.lbCountListExcel.TabIndex = 36;
            this.lbCountListExcel.Text = "Số lượng:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvHad);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(629, 196);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 265);
            this.groupBox2.TabIndex = 35;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DANH SÁCH KHÔNG TỒN TẠI";
            // 
            // dgvHad
            // 
            this.dgvHad.AllowUserToAddRows = false;
            this.dgvHad.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvHad.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvHad.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvHad.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn15,
            this.dataGridViewTextBoxColumn16,
            this.dataGridViewTextBoxColumn17,
            this.dataGridViewTextBoxColumn18,
            this.dataGridViewTextBoxColumn19,
            this.dataGridViewTextBoxColumn20});
            this.dgvHad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHad.Location = new System.Drawing.Point(3, 18);
            this.dgvHad.Name = "dgvHad";
            this.dgvHad.ReadOnly = true;
            this.dgvHad.RowHeadersWidth = 20;
            this.dgvHad.Size = new System.Drawing.Size(358, 244);
            this.dgvHad.TabIndex = 19;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "pationID";
            this.dataGridViewTextBoxColumn1.HeaderText = "Patron ID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 81;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "MaSV_O";
            this.dataGridViewTextBoxColumn2.HeaderText = "Mã Sinh Viên";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 101;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "HoTen";
            this.dataGridViewTextBoxColumn3.HeaderText = "Họ Tên";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 68;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "GT";
            this.dataGridViewTextBoxColumn4.HeaderText = "Giới Tính";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 79;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "ngaySinh";
            this.dataGridViewTextBoxColumn5.HeaderText = "Ngày Sinh";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 86;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "password";
            this.dataGridViewTextBoxColumn6.HeaderText = "Mật Khẩu";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 83;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "phone";
            this.dataGridViewTextBoxColumn7.HeaderText = "Điện Thoại";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 88;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "email";
            this.dataGridViewTextBoxColumn8.HeaderText = "EMail";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 61;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.DataPropertyName = "DiaChi";
            this.dataGridViewTextBoxColumn9.HeaderText = "Địa Chỉ";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 69;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.DataPropertyName = "khoaHoc";
            this.dataGridViewTextBoxColumn10.HeaderText = "Khoa";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            this.dataGridViewTextBoxColumn10.Width = 59;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.DataPropertyName = "khoa";
            this.dataGridViewTextBoxColumn11.HeaderText = "Khóa";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            this.dataGridViewTextBoxColumn11.Width = 59;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.DataPropertyName = "lopHoc";
            this.dataGridViewTextBoxColumn12.HeaderText = "Lớp Học";
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            this.dataGridViewTextBoxColumn12.Width = 75;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.DataPropertyName = "makh";
            this.dataGridViewTextBoxColumn13.HeaderText = "makh";
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            this.dataGridViewTextBoxColumn13.Visible = false;
            this.dataGridViewTextBoxColumn13.Width = 57;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "chucVu";
            this.dataGridViewTextBoxColumn14.HeaderText = "Chức Vụ";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Visible = false;
            this.dataGridViewTextBoxColumn14.Width = 75;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.DataPropertyName = "chucDanh";
            this.dataGridViewTextBoxColumn15.HeaderText = "Chức Danh";
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            this.dataGridViewTextBoxColumn15.Visible = false;
            this.dataGridViewTextBoxColumn15.Width = 86;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.DataPropertyName = "QuocTich";
            this.dataGridViewTextBoxColumn16.HeaderText = "Quốc Tịch";
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            this.dataGridViewTextBoxColumn16.Width = 83;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.DataPropertyName = "hocBong";
            this.dataGridViewTextBoxColumn17.HeaderText = "Học Bổng";
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            this.dataGridViewTextBoxColumn17.Width = 83;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.DataPropertyName = "qdCongNhan";
            this.dataGridViewTextBoxColumn18.HeaderText = "QĐ Công Nhận";
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.ReadOnly = true;
            this.dataGridViewTextBoxColumn18.Width = 111;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.DataPropertyName = "ngayHetHan";
            this.dataGridViewTextBoxColumn19.HeaderText = "Ngày Hết Hạn Thẻ";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.ReadOnly = true;
            this.dataGridViewTextBoxColumn19.Width = 127;
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.DataPropertyName = "Day";
            this.dataGridViewTextBoxColumn20.HeaderText = "Ngày Hiện Tại";
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            this.dataGridViewTextBoxColumn20.ReadOnly = true;
            this.dataGridViewTextBoxColumn20.Width = 105;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvPatron);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(3, 196);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(620, 265);
            this.groupBox3.TabIndex = 34;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DANH SÁCH";
            // 
            // dgvPatron
            // 
            this.dgvPatron.AllowUserToAddRows = false;
            this.dgvPatron.AllowUserToDeleteRows = false;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPatron.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvPatron.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvPatron.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pationID,
            this.MaSV_O,
            this.HoTen,
            this.GT,
            this.ngaySinh,
            this.password,
            this.phone,
            this.email,
            this.DiaChi,
            this.khoaHoc,
            this.khoa,
            this.lopHoc,
            this.makh,
            this.chucVu,
            this.chucDanh,
            this.QuocTich,
            this.hocBong,
            this.qdCongNhan,
            this.ngayHetHan,
            this.Day});
            this.dgvPatron.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPatron.Location = new System.Drawing.Point(3, 18);
            this.dgvPatron.Name = "dgvPatron";
            this.dgvPatron.ReadOnly = true;
            this.dgvPatron.RowHeadersWidth = 20;
            this.dgvPatron.Size = new System.Drawing.Size(614, 244);
            this.dgvPatron.TabIndex = 18;
            // 
            // pationID
            // 
            this.pationID.DataPropertyName = "pationID";
            this.pationID.HeaderText = "Patron ID";
            this.pationID.Name = "pationID";
            this.pationID.ReadOnly = true;
            this.pationID.Width = 81;
            // 
            // MaSV_O
            // 
            this.MaSV_O.DataPropertyName = "MaSV_O";
            this.MaSV_O.HeaderText = "Mã Sinh Viên";
            this.MaSV_O.Name = "MaSV_O";
            this.MaSV_O.ReadOnly = true;
            this.MaSV_O.Width = 101;
            // 
            // HoTen
            // 
            this.HoTen.DataPropertyName = "HoTen";
            this.HoTen.HeaderText = "Họ Tên";
            this.HoTen.Name = "HoTen";
            this.HoTen.ReadOnly = true;
            this.HoTen.Width = 68;
            // 
            // GT
            // 
            this.GT.DataPropertyName = "GT";
            this.GT.HeaderText = "Giới Tính";
            this.GT.Name = "GT";
            this.GT.ReadOnly = true;
            this.GT.Width = 79;
            // 
            // ngaySinh
            // 
            this.ngaySinh.DataPropertyName = "ngaySinh";
            dataGridViewCellStyle6.Format = "d";
            dataGridViewCellStyle6.NullValue = null;
            this.ngaySinh.DefaultCellStyle = dataGridViewCellStyle6;
            this.ngaySinh.HeaderText = "Ngày Sinh";
            this.ngaySinh.Name = "ngaySinh";
            this.ngaySinh.ReadOnly = true;
            this.ngaySinh.Width = 86;
            // 
            // password
            // 
            this.password.DataPropertyName = "password";
            this.password.HeaderText = "Mật Khẩu";
            this.password.Name = "password";
            this.password.ReadOnly = true;
            this.password.Width = 83;
            // 
            // phone
            // 
            this.phone.DataPropertyName = "phone";
            this.phone.HeaderText = "Điện Thoại";
            this.phone.Name = "phone";
            this.phone.ReadOnly = true;
            this.phone.Width = 88;
            // 
            // email
            // 
            this.email.DataPropertyName = "email";
            this.email.HeaderText = "EMail";
            this.email.Name = "email";
            this.email.ReadOnly = true;
            this.email.Width = 61;
            // 
            // DiaChi
            // 
            this.DiaChi.DataPropertyName = "DiaChi";
            this.DiaChi.HeaderText = "Địa Chỉ";
            this.DiaChi.Name = "DiaChi";
            this.DiaChi.ReadOnly = true;
            this.DiaChi.Width = 69;
            // 
            // khoaHoc
            // 
            this.khoaHoc.DataPropertyName = "khoaHoc";
            this.khoaHoc.HeaderText = "Khoa";
            this.khoaHoc.Name = "khoaHoc";
            this.khoaHoc.ReadOnly = true;
            this.khoaHoc.Width = 59;
            // 
            // khoa
            // 
            this.khoa.DataPropertyName = "khoa";
            this.khoa.HeaderText = "Khóa";
            this.khoa.Name = "khoa";
            this.khoa.ReadOnly = true;
            this.khoa.Width = 59;
            // 
            // lopHoc
            // 
            this.lopHoc.DataPropertyName = "lopHoc";
            this.lopHoc.HeaderText = "Lớp Học";
            this.lopHoc.Name = "lopHoc";
            this.lopHoc.ReadOnly = true;
            this.lopHoc.Width = 75;
            // 
            // makh
            // 
            this.makh.DataPropertyName = "makh";
            this.makh.HeaderText = "makh";
            this.makh.Name = "makh";
            this.makh.ReadOnly = true;
            this.makh.Visible = false;
            this.makh.Width = 57;
            // 
            // chucVu
            // 
            this.chucVu.DataPropertyName = "chucVu";
            this.chucVu.HeaderText = "Chức Vụ";
            this.chucVu.Name = "chucVu";
            this.chucVu.ReadOnly = true;
            this.chucVu.Visible = false;
            this.chucVu.Width = 75;
            // 
            // chucDanh
            // 
            this.chucDanh.DataPropertyName = "chucDanh";
            this.chucDanh.HeaderText = "Chức Danh";
            this.chucDanh.Name = "chucDanh";
            this.chucDanh.ReadOnly = true;
            this.chucDanh.Visible = false;
            this.chucDanh.Width = 86;
            // 
            // QuocTich
            // 
            this.QuocTich.DataPropertyName = "QuocTich";
            this.QuocTich.HeaderText = "Quốc Tịch";
            this.QuocTich.Name = "QuocTich";
            this.QuocTich.ReadOnly = true;
            this.QuocTich.Width = 83;
            // 
            // hocBong
            // 
            this.hocBong.DataPropertyName = "hocBong";
            this.hocBong.HeaderText = "Học Bổng";
            this.hocBong.Name = "hocBong";
            this.hocBong.ReadOnly = true;
            this.hocBong.Width = 83;
            // 
            // qdCongNhan
            // 
            this.qdCongNhan.DataPropertyName = "qdCongNhan";
            this.qdCongNhan.HeaderText = "QĐ Công Nhận";
            this.qdCongNhan.Name = "qdCongNhan";
            this.qdCongNhan.ReadOnly = true;
            this.qdCongNhan.Width = 111;
            // 
            // ngayHetHan
            // 
            this.ngayHetHan.DataPropertyName = "ngayHetHan";
            this.ngayHetHan.HeaderText = "Ngày Hết Hạn Thẻ";
            this.ngayHetHan.Name = "ngayHetHan";
            this.ngayHetHan.ReadOnly = true;
            this.ngayHetHan.Width = 127;
            // 
            // Day
            // 
            this.Day.DataPropertyName = "Day";
            this.Day.HeaderText = "Ngày Hiện Tại";
            this.Day.Name = "Day";
            this.Day.ReadOnly = true;
            this.Day.Width = 105;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbLoaiBanDoc);
            this.groupBox1.Controls.Add(this.pb_TaiChinh);
            this.groupBox1.Controls.Add(this.btnPush);
            this.groupBox1.Controls.Add(this.btnConvert);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtLine);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.btnBrowserFile);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnThoat);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(9, 18);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(981, 190);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cập nhập bạn đọc hàng loạt";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rbCanBo);
            this.groupBox5.Controls.Add(this.rbSinhVien);
            this.groupBox5.Location = new System.Drawing.Point(468, 14);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(232, 52);
            this.groupBox5.TabIndex = 110;
            this.groupBox5.TabStop = false;
            // 
            // rbCanBo
            // 
            this.rbCanBo.AutoSize = true;
            this.rbCanBo.Location = new System.Drawing.Point(126, 20);
            this.rbCanBo.Name = "rbCanBo";
            this.rbCanBo.Size = new System.Drawing.Size(93, 29);
            this.rbCanBo.TabIndex = 3;
            this.rbCanBo.Text = "Cán bộ";
            this.rbCanBo.UseVisualStyleBackColor = true;
            this.rbCanBo.CheckedChanged += new System.EventHandler(this.RbCanBo_CheckedChanged);
            // 
            // rbSinhVien
            // 
            this.rbSinhVien.AutoSize = true;
            this.rbSinhVien.Checked = true;
            this.rbSinhVien.Location = new System.Drawing.Point(6, 20);
            this.rbSinhVien.Name = "rbSinhVien";
            this.rbSinhVien.Size = new System.Drawing.Size(111, 29);
            this.rbSinhVien.TabIndex = 2;
            this.rbSinhVien.TabStop = true;
            this.rbSinhVien.Text = "Sinh viên";
            this.rbSinhVien.UseVisualStyleBackColor = true;
            this.rbSinhVien.CheckedChanged += new System.EventHandler(this.RbSinhVien_CheckedChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 149);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(128, 25);
            this.label7.TabIndex = 109;
            this.label7.Text = "Loại Bạn Đọc";
            // 
            // cbLoaiBanDoc
            // 
            this.cbLoaiBanDoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLoaiBanDoc.FormattingEnabled = true;
            this.cbLoaiBanDoc.Location = new System.Drawing.Point(147, 145);
            this.cbLoaiBanDoc.Name = "cbLoaiBanDoc";
            this.cbLoaiBanDoc.Size = new System.Drawing.Size(316, 33);
            this.cbLoaiBanDoc.TabIndex = 108;
            // 
            // pb_TaiChinh
            // 
            this.pb_TaiChinh.Image = global::TNUE_Patron_Excel.Properties.Resources.library_logo;
            this.pb_TaiChinh.Location = new System.Drawing.Point(803, 27);
            this.pb_TaiChinh.Name = "pb_TaiChinh";
            this.pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
            this.pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_TaiChinh.TabIndex = 20;
            this.pb_TaiChinh.TabStop = false;
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
            this.btnPush.Location = new System.Drawing.Point(467, 143);
            this.btnPush.Name = "btnPush";
            this.btnPush.Size = new System.Drawing.Size(159, 38);
            this.btnPush.TabIndex = 107;
            this.btnPush.Text = "Cập nhập";
            this.btnPush.UseVisualStyleBackColor = false;
            this.btnPush.Click += new System.EventHandler(this.BtnPush_Click_1);
            // 
            // btnConvert
            // 
            this.btnConvert.AutoSize = true;
            this.btnConvert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnConvert.FlatAppearance.BorderSize = 0;
            this.btnConvert.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConvert.ForeColor = System.Drawing.Color.White;
            this.btnConvert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConvert.Location = new System.Drawing.Point(467, 102);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(159, 38);
            this.btnConvert.TabIndex = 106;
            this.btnConvert.Text = "Chuyển dữ liệu";
            this.btnConvert.UseVisualStyleBackColor = false;
            this.btnConvert.Click += new System.EventHandler(this.BtnConvert_Click_1);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 25);
            this.label6.TabIndex = 104;
            this.label6.Text = "Dòng bắt đầu";
            // 
            // txtLine
            // 
            this.txtLine.Location = new System.Drawing.Point(147, 28);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(161, 33);
            this.txtLine.TabIndex = 103;
            this.txtLine.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtLine_KeyPress_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 108);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 25);
            this.label5.TabIndex = 94;
            this.label5.Text = "Trạng thái";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(147, 105);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(316, 33);
            this.comboBox1.TabIndex = 92;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(410, 31);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(45, 33);
            this.textBox3.TabIndex = 96;
            // 
            // btnBrowserFile
            // 
            this.btnBrowserFile.BackColor = System.Drawing.SystemColors.Control;
            this.btnBrowserFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnBrowserFile.Location = new System.Drawing.Point(467, 67);
            this.btnBrowserFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowserFile.Name = "btnBrowserFile";
            this.btnBrowserFile.Size = new System.Drawing.Size(159, 33);
            this.btnBrowserFile.TabIndex = 90;
            this.btnBrowserFile.Text = "Chọn...";
            this.btnBrowserFile.UseVisualStyleBackColor = false;
            this.btnBrowserFile.Click += new System.EventHandler(this.BtnBrowserFile_Click_1);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(147, 67);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(316, 33);
            this.textBox1.TabIndex = 89;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 70);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 25);
            this.label1.TabIndex = 88;
            this.label1.Text = "Chọn tệp tin";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(315, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 25);
            this.label3.TabIndex = 97;
            this.label3.Text = "Số Sheet";
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
            this.btnThoat.Location = new System.Drawing.Point(629, 143);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(109, 38);
            this.btnThoat.TabIndex = 14;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = false;
            this.btnThoat.Click += new System.EventHandler(this.BtnThoat_Click_1);
            // 
            // panelLdap
            // 
            this.panelLdap.Controls.Add(this.btnhien);
            this.panelLdap.Controls.Add(this.label2);
            this.panelLdap.Controls.Add(this.btnUnSearch);
            this.panelLdap.Controls.Add(this.label4);
            this.panelLdap.Controls.Add(this.txtSearch);
            this.panelLdap.Controls.Add(this.label8);
            this.panelLdap.Controls.Add(this.txtPassword);
            this.panelLdap.Controls.Add(this.label9);
            this.panelLdap.Controls.Add(this.txtPhone);
            this.panelLdap.Controls.Add(this.label10);
            this.panelLdap.Controls.Add(this.txtEmail);
            this.panelLdap.Controls.Add(this.label11);
            this.panelLdap.Controls.Add(this.txtMa);
            this.panelLdap.Controls.Add(this.btnSua);
            this.panelLdap.Controls.Add(this.panel2);
            this.panelLdap.Controls.Add(this.btnSearch);
            this.panelLdap.Controls.Add(this.pictureBox2);
            this.panelLdap.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLdap.Location = new System.Drawing.Point(3, 66);
            this.panelLdap.Name = "panelLdap";
            this.panelLdap.Size = new System.Drawing.Size(987, 490);
            this.panelLdap.TabIndex = 127;
            // 
            // btnhien
            // 
            this.btnhien.BackgroundImage = global::TNUE_Patron_Excel.Properties.Resources.Eye_icon;
            this.btnhien.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnhien.Location = new System.Drawing.Point(428, 151);
            this.btnhien.Name = "btnhien";
            this.btnhien.Size = new System.Drawing.Size(36, 27);
            this.btnhien.TabIndex = 122;
            this.btnhien.UseVisualStyleBackColor = true;
            this.btnhien.Click += new System.EventHandler(this.Btnhien_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(140, 128);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(225, 17);
            this.label2.TabIndex = 121;
            this.label2.Text = "Để trống nếu không muốn thay đổi";
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
            this.btnUnSearch.Location = new System.Drawing.Point(702, 82);
            this.btnUnSearch.Name = "btnUnSearch";
            this.btnUnSearch.Size = new System.Drawing.Size(103, 38);
            this.btnUnSearch.TabIndex = 120;
            this.btnUnSearch.Text = "Bỏ tìm";
            this.btnUnSearch.UseVisualStyleBackColor = false;
            this.btnUnSearch.Click += new System.EventHandler(this.BtnUnSearch_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(629, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 25);
            this.label4.TabIndex = 119;
            this.label4.Text = "Tìm kiếm mã";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(585, 39);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 33);
            this.txtSearch.TabIndex = 118;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 151);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(96, 25);
            this.label8.TabIndex = 117;
            this.label8.Text = "Mật khẩu";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(143, 148);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(281, 33);
            this.txtPassword.TabIndex = 116;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 87);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(69, 25);
            this.label9.TabIndex = 115;
            this.label9.Text = "Phone";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(143, 84);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(281, 33);
            this.txtPhone.TabIndex = 114;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 48);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 25);
            this.label10.TabIndex = 113;
            this.label10.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(143, 45);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(281, 33);
            this.txtEmail.TabIndex = 112;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 25);
            this.label11.TabIndex = 111;
            this.label11.Text = "Mã SV/CB";
            // 
            // txtMa
            // 
            this.txtMa.Enabled = false;
            this.txtMa.Location = new System.Drawing.Point(143, 6);
            this.txtMa.Name = "txtMa";
            this.txtMa.Size = new System.Drawing.Size(281, 33);
            this.txtMa.TabIndex = 110;
            // 
            // btnSua
            // 
            this.btnSua.AutoSize = true;
            this.btnSua.BackColor = System.Drawing.Color.Green;
            this.btnSua.FlatAppearance.BorderSize = 0;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSua.Location = new System.Drawing.Point(470, 148);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(103, 35);
            this.btnSua.TabIndex = 109;
            this.btnSua.Text = "Cập nhập";
            this.btnSua.UseVisualStyleBackColor = false;
            this.btnSua.Click += new System.EventHandler(this.BtnSua_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.bindingNavigator1);
            this.panel2.Controls.Add(this.superGird1);
            this.panel2.Location = new System.Drawing.Point(3, 206);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(973, 288);
            this.panel2.TabIndex = 108;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = this.bindingNavigatorAddNewItem;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = this.bindingNavigatorDeleteItem;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.Size = new System.Drawing.Size(973, 25);
            this.bindingNavigator1.TabIndex = 31;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorAddNewItem
            // 
            this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
            this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
            this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorAddNewItem.Text = "Add new";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // superGird1
            // 
            this.superGird1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.superGird1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.superGird1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.userLogin,
            this.userMail,
            this.telephoneNumber});
            this.superGird1.Location = new System.Drawing.Point(2, 28);
            this.superGird1.Name = "superGird1";
            this.superGird1.PageSize = 10;
            this.superGird1.Size = new System.Drawing.Size(973, 253);
            this.superGird1.TabIndex = 30;
            this.superGird1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SuperGird1_CellClick);
            // 
            // userLogin
            // 
            this.userLogin.DataPropertyName = "userLogin";
            this.userLogin.HeaderText = "Mã";
            this.userLogin.Name = "userLogin";
            this.userLogin.Width = 65;
            // 
            // userMail
            // 
            this.userMail.DataPropertyName = "userMail";
            this.userMail.HeaderText = "Email";
            this.userMail.Name = "userMail";
            this.userMail.Width = 84;
            // 
            // telephoneNumber
            // 
            this.telephoneNumber.DataPropertyName = "telephoneNumber";
            this.telephoneNumber.HeaderText = "Số điện thoại";
            this.telephoneNumber.Name = "telephoneNumber";
            this.telephoneNumber.Width = 154;
            // 
            // btnSearch
            // 
            this.btnSearch.AutoSize = true;
            this.btnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.White;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(585, 82);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(114, 38);
            this.btnSearch.TabIndex = 106;
            this.btnSearch.Text = "Tìm kiếm";
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::TNUE_Patron_Excel.Properties.Resources.library_logo;
            this.pictureBox2.Location = new System.Drawing.Point(816, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(160, 151);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 20;
            this.pictureBox2.TabStop = false;
            // 
            // UCUpdatePatron
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox4);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCUpdatePatron";
            this.Size = new System.Drawing.Size(1000, 565);
            this.Load += new System.EventHandler(this.UCUpdatePatron_Load);
            this.groupBox4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelUpdateSeris.ResumeLayout(false);
            this.panelUpdateSeris.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHad)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatron)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).EndInit();
            this.panelLdap.ResumeLayout(false);
            this.panelLdap.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superGird1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }
	}
}
