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

		private IContainer components = null;

		private FolderBrowserDialog folderBrowserDialog1;

		private GroupBox groupBox4;

		private Panel panel3;

		private RadioButton rbLdap;

		private RadioButton rbAleph;

		private Panel panelLdap;

		private Button btnUnSearch;

		private Label label4;

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

		private void BtnSua_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Bạn có chắc chắn muốn sửa?", "Xác nhân", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
			{
				EditLdap();
				LoadUserCase();
				MessageBox.Show("Đã sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
		}

		private void BtnSearch_Click(object sender, EventArgs e)
		{
			string inputText = txtSearch.Text.Trim().ToUpper();
			DataTable dataSource = (from r in table.AsEnumerable()
				where r.Field<string>("userLogin").Contains(inputText)
				select r).CopyToDataTable();
			superGird1.Columns.Clear();
			superGird1.DataSource = dataSource;
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
				readExcel2();
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
			listZ308 = DataDBLocal.listZ308;
			ComboxBlock();
			ComboxLoaiBanDoc();
			txtLine.Text = "12";
			CreateFolder(directoryPath);
			superGird1._pageSize = 100;
			IEnumerable<User> allListUser = new ModelLdap().GetAllListUser();
			table = new DataTable();
			using (ObjectReader reader = ObjectReader.Create(allListUser, "userLogin", "userMail", "telephoneNumber"))
			{
				table.Load(reader);
			}
			superGird1.SetPagedDataSource(table, bindingNavigator1);
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
			components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TNUE_Patron_Excel.ControlMember.UCUpdatePatron));
			folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			groupBox4 = new System.Windows.Forms.GroupBox();
			panelUpdateSeris = new System.Windows.Forms.Panel();
			lbCountHad = new System.Windows.Forms.Label();
			lbCountListExcel = new System.Windows.Forms.Label();
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
			label6 = new System.Windows.Forms.Label();
			txtLine = new System.Windows.Forms.TextBox();
			label5 = new System.Windows.Forms.Label();
			comboBox1 = new System.Windows.Forms.ComboBox();
			textBox3 = new System.Windows.Forms.TextBox();
			btnBrowserFile = new System.Windows.Forms.Button();
			textBox1 = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			label3 = new System.Windows.Forms.Label();
			btnThoat = new System.Windows.Forms.Button();
			panel3 = new System.Windows.Forms.Panel();
			rbLdap = new System.Windows.Forms.RadioButton();
			rbAleph = new System.Windows.Forms.RadioButton();
			panelLdap = new System.Windows.Forms.Panel();
			btnUnSearch = new System.Windows.Forms.Button();
			label4 = new System.Windows.Forms.Label();
			txtSearch = new System.Windows.Forms.TextBox();
			label8 = new System.Windows.Forms.Label();
			txtPassword = new System.Windows.Forms.TextBox();
			label9 = new System.Windows.Forms.Label();
			txtPhone = new System.Windows.Forms.TextBox();
			label10 = new System.Windows.Forms.Label();
			txtEmail = new System.Windows.Forms.TextBox();
			label11 = new System.Windows.Forms.Label();
			txtMa = new System.Windows.Forms.TextBox();
			btnSua = new System.Windows.Forms.Button();
			panel2 = new System.Windows.Forms.Panel();
			bindingNavigator1 = new System.Windows.Forms.BindingNavigator(components);
			bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
			bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
			bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
			bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
			bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
			bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
			bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
			bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
			bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
			bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			superGird1 = new TNUE_Patron_Excel.SuperGird();
			userLogin = new System.Windows.Forms.DataGridViewTextBoxColumn();
			userMail = new System.Windows.Forms.DataGridViewTextBoxColumn();
			telephoneNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
			btnSearch = new System.Windows.Forms.Button();
			pictureBox2 = new System.Windows.Forms.PictureBox();
			groupBox4.SuspendLayout();
			panelUpdateSeris.SuspendLayout();
			groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvHad).BeginInit();
			groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvPatron).BeginInit();
			groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).BeginInit();
			panel3.SuspendLayout();
			panelLdap.SuspendLayout();
			panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)bindingNavigator1).BeginInit();
			bindingNavigator1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)superGird1).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
			SuspendLayout();
			groupBox4.Controls.Add(panelUpdateSeris);
			groupBox4.Controls.Add(panel3);
			groupBox4.Controls.Add(panelLdap);
			groupBox4.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold);
			groupBox4.Location = new System.Drawing.Point(0, 6);
			groupBox4.Name = "groupBox4";
			groupBox4.Size = new System.Drawing.Size(993, 559);
			groupBox4.TabIndex = 33;
			groupBox4.TabStop = false;
			groupBox4.Text = "Cập nhập bạn đọc";
			panelUpdateSeris.Controls.Add(lbCountHad);
			panelUpdateSeris.Controls.Add(lbCountListExcel);
			panelUpdateSeris.Controls.Add(groupBox2);
			panelUpdateSeris.Controls.Add(groupBox3);
			panelUpdateSeris.Controls.Add(groupBox1);
			panelUpdateSeris.Location = new System.Drawing.Point(0, 69);
			panelUpdateSeris.Name = "panelUpdateSeris";
			panelUpdateSeris.Size = new System.Drawing.Size(987, 489);
			panelUpdateSeris.TabIndex = 34;
			panelUpdateSeris.Visible = false;
			lbCountHad.AutoSize = true;
			lbCountHad.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lbCountHad.Location = new System.Drawing.Point(639, 464);
			lbCountHad.Name = "lbCountHad";
			lbCountHad.Size = new System.Drawing.Size(76, 21);
			lbCountHad.TabIndex = 37;
			lbCountHad.Text = "Số lượng:";
			lbCountListExcel.AutoSize = true;
			lbCountListExcel.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			lbCountListExcel.Location = new System.Drawing.Point(4, 461);
			lbCountListExcel.Name = "lbCountListExcel";
			lbCountListExcel.Size = new System.Drawing.Size(76, 21);
			lbCountListExcel.TabIndex = 36;
			lbCountListExcel.Text = "Số lượng:";
			groupBox2.Controls.Add(dgvHad);
			groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			groupBox2.Location = new System.Drawing.Point(629, 196);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(364, 265);
			groupBox2.TabIndex = 35;
			groupBox2.TabStop = false;
			groupBox2.Text = "DANH SÁCH KHÔNG TỒN TẠI";
			dgvHad.AllowUserToAddRows = false;
			dgvHad.AllowUserToDeleteRows = false;
			dataGridViewCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dgvHad.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle;
			dgvHad.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dgvHad.Columns.AddRange(dataGridViewTextBoxColumn1, dataGridViewTextBoxColumn2, dataGridViewTextBoxColumn3, dataGridViewTextBoxColumn4, dataGridViewTextBoxColumn5, dataGridViewTextBoxColumn6, dataGridViewTextBoxColumn7, dataGridViewTextBoxColumn8, dataGridViewTextBoxColumn9, dataGridViewTextBoxColumn10, dataGridViewTextBoxColumn11, dataGridViewTextBoxColumn12, dataGridViewTextBoxColumn13, dataGridViewTextBoxColumn14, dataGridViewTextBoxColumn15, dataGridViewTextBoxColumn16, dataGridViewTextBoxColumn17, dataGridViewTextBoxColumn18, dataGridViewTextBoxColumn19, dataGridViewTextBoxColumn20);
			dgvHad.Dock = System.Windows.Forms.DockStyle.Fill;
			dgvHad.Location = new System.Drawing.Point(3, 18);
			dgvHad.Name = "dgvHad";
			dgvHad.ReadOnly = true;
			dgvHad.RowHeadersWidth = 20;
			dgvHad.Size = new System.Drawing.Size(358, 244);
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
			dataGridViewTextBoxColumn13.Width = 57;
			dataGridViewTextBoxColumn14.DataPropertyName = "chucVu";
			dataGridViewTextBoxColumn14.HeaderText = "Chức Vụ";
			dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
			dataGridViewTextBoxColumn14.ReadOnly = true;
			dataGridViewTextBoxColumn14.Visible = false;
			dataGridViewTextBoxColumn14.Width = 75;
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
			groupBox3.Controls.Add(dgvPatron);
			groupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			groupBox3.Location = new System.Drawing.Point(3, 196);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(620, 265);
			groupBox3.TabIndex = 34;
			groupBox3.TabStop = false;
			groupBox3.Text = "DANH SÁCH";
			dgvPatron.AllowUserToAddRows = false;
			dgvPatron.AllowUserToDeleteRows = false;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			dgvPatron.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle2;
			dgvPatron.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dgvPatron.Columns.AddRange(pationID, MaSV_O, HoTen, GT, ngaySinh, password, phone, email, DiaChi, khoaHoc, khoa, lopHoc, makh, chucVu, chucDanh, QuocTich, hocBong, qdCongNhan, ngayHetHan, Day);
			dgvPatron.Dock = System.Windows.Forms.DockStyle.Fill;
			dgvPatron.Location = new System.Drawing.Point(3, 18);
			dgvPatron.Name = "dgvPatron";
			dgvPatron.ReadOnly = true;
			dgvPatron.RowHeadersWidth = 20;
			dgvPatron.Size = new System.Drawing.Size(614, 244);
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
			dataGridViewCellStyle3.Format = "d";
			dataGridViewCellStyle3.NullValue = null;
			ngaySinh.DefaultCellStyle = dataGridViewCellStyle3;
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
			makh.Width = 57;
			chucVu.DataPropertyName = "chucVu";
			chucVu.HeaderText = "Chức Vụ";
			chucVu.Name = "chucVu";
			chucVu.ReadOnly = true;
			chucVu.Visible = false;
			chucVu.Width = 75;
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
			groupBox1.Controls.Add(label6);
			groupBox1.Controls.Add(txtLine);
			groupBox1.Controls.Add(label5);
			groupBox1.Controls.Add(comboBox1);
			groupBox1.Controls.Add(textBox3);
			groupBox1.Controls.Add(btnBrowserFile);
			groupBox1.Controls.Add(textBox1);
			groupBox1.Controls.Add(label1);
			groupBox1.Controls.Add(label3);
			groupBox1.Controls.Add(btnThoat);
			groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold);
			groupBox1.Location = new System.Drawing.Point(3, 7);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(987, 190);
			groupBox1.TabIndex = 33;
			groupBox1.TabStop = false;
			groupBox1.Text = "Cập nhập bạn đọc hàng loạt";
			label7.AutoSize = true;
			label7.Location = new System.Drawing.Point(6, 149);
			label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label7.Name = "label7";
			label7.Size = new System.Drawing.Size(128, 25);
			label7.TabIndex = 109;
			label7.Text = "Loại Bạn Đọc";
			cbLoaiBanDoc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			cbLoaiBanDoc.FormattingEnabled = true;
			cbLoaiBanDoc.Location = new System.Drawing.Point(147, 146);
			cbLoaiBanDoc.Name = "cbLoaiBanDoc";
			cbLoaiBanDoc.Size = new System.Drawing.Size(316, 33);
			cbLoaiBanDoc.TabIndex = 108;
			pb_TaiChinh.Image = TNUE_Patron_Excel.Properties.Resources.library_logo;
			pb_TaiChinh.Location = new System.Drawing.Point(803, 35);
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
			btnPush.Location = new System.Drawing.Point(467, 143);
			btnPush.Name = "btnPush";
			btnPush.Size = new System.Drawing.Size(159, 38);
			btnPush.TabIndex = 107;
			btnPush.Text = "Cập nhập";
			btnPush.UseVisualStyleBackColor = false;
			btnPush.Click += new System.EventHandler(BtnPush_Click_1);
			btnConvert.AutoSize = true;
			btnConvert.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnConvert.FlatAppearance.BorderSize = 0;
			btnConvert.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnConvert.ForeColor = System.Drawing.Color.White;
			btnConvert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnConvert.Location = new System.Drawing.Point(467, 102);
			btnConvert.Name = "btnConvert";
			btnConvert.Size = new System.Drawing.Size(159, 38);
			btnConvert.TabIndex = 106;
			btnConvert.Text = "Chuyển dữ liệu";
			btnConvert.UseVisualStyleBackColor = false;
			btnConvert.Click += new System.EventHandler(BtnConvert_Click_1);
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(6, 34);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(135, 25);
			label6.TabIndex = 104;
			label6.Text = "Dòng bắt đầu";
			txtLine.Location = new System.Drawing.Point(147, 28);
			txtLine.Name = "txtLine";
			txtLine.Size = new System.Drawing.Size(161, 33);
			txtLine.TabIndex = 103;
			txtLine.KeyPress += new System.Windows.Forms.KeyPressEventHandler(TxtLine_KeyPress_1);
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(6, 108);
			label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(102, 25);
			label5.TabIndex = 94;
			label5.Text = "Trạng thái";
			comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			comboBox1.FormattingEnabled = true;
			comboBox1.Location = new System.Drawing.Point(147, 105);
			comboBox1.Name = "comboBox1";
			comboBox1.Size = new System.Drawing.Size(316, 33);
			comboBox1.TabIndex = 92;
			textBox3.Enabled = false;
			textBox3.Location = new System.Drawing.Point(410, 31);
			textBox3.Name = "textBox3";
			textBox3.Size = new System.Drawing.Size(45, 33);
			textBox3.TabIndex = 96;
			btnBrowserFile.BackColor = System.Drawing.SystemColors.Control;
			btnBrowserFile.ForeColor = System.Drawing.SystemColors.ControlText;
			btnBrowserFile.Location = new System.Drawing.Point(467, 67);
			btnBrowserFile.Margin = new System.Windows.Forms.Padding(2);
			btnBrowserFile.Name = "btnBrowserFile";
			btnBrowserFile.Size = new System.Drawing.Size(159, 33);
			btnBrowserFile.TabIndex = 90;
			btnBrowserFile.Text = "Chọn...";
			btnBrowserFile.UseVisualStyleBackColor = false;
			btnBrowserFile.Click += new System.EventHandler(BtnBrowserFile_Click_1);
			textBox1.Enabled = false;
			textBox1.Location = new System.Drawing.Point(147, 67);
			textBox1.Margin = new System.Windows.Forms.Padding(2);
			textBox1.Name = "textBox1";
			textBox1.Size = new System.Drawing.Size(316, 33);
			textBox1.TabIndex = 89;
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(6, 70);
			label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(122, 25);
			label1.TabIndex = 88;
			label1.Text = "Chọn tệp tin";
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(315, 34);
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
			btnThoat.Location = new System.Drawing.Point(629, 143);
			btnThoat.Name = "btnThoat";
			btnThoat.Size = new System.Drawing.Size(109, 38);
			btnThoat.TabIndex = 14;
			btnThoat.Text = "Thoát";
			btnThoat.UseVisualStyleBackColor = false;
			btnThoat.Click += new System.EventHandler(BtnThoat_Click_1);
			panel3.Controls.Add(rbLdap);
			panel3.Controls.Add(rbAleph);
			panel3.Dock = System.Windows.Forms.DockStyle.Top;
			panel3.Location = new System.Drawing.Point(3, 29);
			panel3.Name = "panel3";
			panel3.Size = new System.Drawing.Size(987, 37);
			panel3.TabIndex = 32;
			rbLdap.AutoSize = true;
			rbLdap.Checked = true;
			rbLdap.Location = new System.Drawing.Point(15, 3);
			rbLdap.Name = "rbLdap";
			rbLdap.Size = new System.Drawing.Size(246, 29);
			rbLdap.TabIndex = 1;
			rbLdap.TabStop = true;
			rbLdap.Text = "Cập nhập thông tin ldap";
			rbLdap.UseVisualStyleBackColor = true;
			rbLdap.CheckedChanged += new System.EventHandler(RbLdap_CheckedChanged);
			rbAleph.AutoSize = true;
			rbAleph.Location = new System.Drawing.Point(264, 3);
			rbAleph.Name = "rbAleph";
			rbAleph.Size = new System.Drawing.Size(203, 29);
			rbAleph.TabIndex = 0;
			rbAleph.Text = "Cập nhập hàng loạt";
			rbAleph.UseVisualStyleBackColor = true;
			rbAleph.CheckedChanged += new System.EventHandler(RbAleph_CheckedChanged);
			panelLdap.Controls.Add(btnUnSearch);
			panelLdap.Controls.Add(label4);
			panelLdap.Controls.Add(txtSearch);
			panelLdap.Controls.Add(label8);
			panelLdap.Controls.Add(txtPassword);
			panelLdap.Controls.Add(label9);
			panelLdap.Controls.Add(txtPhone);
			panelLdap.Controls.Add(label10);
			panelLdap.Controls.Add(txtEmail);
			panelLdap.Controls.Add(label11);
			panelLdap.Controls.Add(txtMa);
			panelLdap.Controls.Add(btnSua);
			panelLdap.Controls.Add(panel2);
			panelLdap.Controls.Add(btnSearch);
			panelLdap.Controls.Add(pictureBox2);
			panelLdap.Dock = System.Windows.Forms.DockStyle.Bottom;
			panelLdap.Location = new System.Drawing.Point(3, 66);
			panelLdap.Name = "panelLdap";
			panelLdap.Size = new System.Drawing.Size(987, 490);
			panelLdap.TabIndex = 127;
			btnUnSearch.AutoSize = true;
			btnUnSearch.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
			btnUnSearch.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnUnSearch.FlatAppearance.BorderSize = 0;
			btnUnSearch.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnUnSearch.ForeColor = System.Drawing.Color.White;
			btnUnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnUnSearch.Location = new System.Drawing.Point(702, 82);
			btnUnSearch.Name = "btnUnSearch";
			btnUnSearch.Size = new System.Drawing.Size(103, 38);
			btnUnSearch.TabIndex = 120;
			btnUnSearch.Text = "Bỏ tìm";
			btnUnSearch.UseVisualStyleBackColor = false;
			btnUnSearch.Click += new System.EventHandler(BtnUnSearch_Click);
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(629, 11);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(125, 25);
			label4.TabIndex = 119;
			label4.Text = "Tìm kiếm mã";
			txtSearch.Location = new System.Drawing.Point(585, 39);
			txtSearch.Name = "txtSearch";
			txtSearch.Size = new System.Drawing.Size(220, 33);
			txtSearch.TabIndex = 118;
			label8.AutoSize = true;
			label8.Location = new System.Drawing.Point(3, 126);
			label8.Name = "label8";
			label8.Size = new System.Drawing.Size(96, 25);
			label8.TabIndex = 117;
			label8.Text = "Mật khẩu";
			txtPassword.Location = new System.Drawing.Point(143, 123);
			txtPassword.Name = "txtPassword";
			txtPassword.Size = new System.Drawing.Size(281, 33);
			txtPassword.TabIndex = 116;
			label9.AutoSize = true;
			label9.Location = new System.Drawing.Point(3, 87);
			label9.Name = "label9";
			label9.Size = new System.Drawing.Size(69, 25);
			label9.TabIndex = 115;
			label9.Text = "Phone";
			txtPhone.Location = new System.Drawing.Point(143, 84);
			txtPhone.Name = "txtPhone";
			txtPhone.Size = new System.Drawing.Size(281, 33);
			txtPhone.TabIndex = 114;
			label10.AutoSize = true;
			label10.Location = new System.Drawing.Point(3, 48);
			label10.Name = "label10";
			label10.Size = new System.Drawing.Size(59, 25);
			label10.TabIndex = 113;
			label10.Text = "Email";
			txtEmail.Location = new System.Drawing.Point(143, 45);
			txtEmail.Name = "txtEmail";
			txtEmail.Size = new System.Drawing.Size(281, 33);
			txtEmail.TabIndex = 112;
			label11.AutoSize = true;
			label11.Location = new System.Drawing.Point(3, 9);
			label11.Name = "label11";
			label11.Size = new System.Drawing.Size(101, 25);
			label11.TabIndex = 111;
			label11.Text = "Mã SV/CB";
			txtMa.Enabled = false;
			txtMa.Location = new System.Drawing.Point(143, 6);
			txtMa.Name = "txtMa";
			txtMa.Size = new System.Drawing.Size(281, 33);
			txtMa.TabIndex = 110;
			btnSua.AutoSize = true;
			btnSua.BackColor = System.Drawing.Color.Green;
			btnSua.FlatAppearance.BorderSize = 0;
			btnSua.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnSua.ForeColor = System.Drawing.Color.White;
			btnSua.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnSua.Location = new System.Drawing.Point(430, 122);
			btnSua.Name = "btnSua";
			btnSua.Size = new System.Drawing.Size(103, 35);
			btnSua.TabIndex = 109;
			btnSua.Text = "Cập nhập";
			btnSua.UseVisualStyleBackColor = false;
			btnSua.Click += new System.EventHandler(BtnSua_Click);
			panel2.Controls.Add(bindingNavigator1);
			panel2.Controls.Add(superGird1);
			panel2.Location = new System.Drawing.Point(3, 163);
			panel2.Name = "panel2";
			panel2.Size = new System.Drawing.Size(973, 331);
			panel2.TabIndex = 108;
			bindingNavigator1.AddNewItem = bindingNavigatorAddNewItem;
			bindingNavigator1.CountItem = bindingNavigatorCountItem;
			bindingNavigator1.DeleteItem = bindingNavigatorDeleteItem;
			bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
			{
				bindingNavigatorMoveFirstItem,
				bindingNavigatorMovePreviousItem,
				bindingNavigatorSeparator,
				bindingNavigatorPositionItem,
				bindingNavigatorCountItem,
				bindingNavigatorSeparator1,
				bindingNavigatorMoveNextItem,
				bindingNavigatorMoveLastItem,
				bindingNavigatorSeparator2,
				bindingNavigatorAddNewItem,
				bindingNavigatorDeleteItem
			});
			bindingNavigator1.Location = new System.Drawing.Point(0, 0);
			bindingNavigator1.MoveFirstItem = bindingNavigatorMoveFirstItem;
			bindingNavigator1.MoveLastItem = bindingNavigatorMoveLastItem;
			bindingNavigator1.MoveNextItem = bindingNavigatorMoveNextItem;
			bindingNavigator1.MovePreviousItem = bindingNavigatorMovePreviousItem;
			bindingNavigator1.Name = "bindingNavigator1";
			bindingNavigator1.PositionItem = bindingNavigatorPositionItem;
			bindingNavigator1.Size = new System.Drawing.Size(973, 25);
			bindingNavigator1.TabIndex = 31;
			bindingNavigator1.Text = "bindingNavigator1";
			bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			bindingNavigatorAddNewItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorAddNewItem.Image");
			bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
			bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
			bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
			bindingNavigatorAddNewItem.Text = "Add new";
			bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
			bindingNavigatorCountItem.Size = new System.Drawing.Size(34, 22);
			bindingNavigatorCountItem.Text = "of {0}";
			bindingNavigatorCountItem.ToolTipText = "Total number of items";
			bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			bindingNavigatorDeleteItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorDeleteItem.Image");
			bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
			bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
			bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
			bindingNavigatorDeleteItem.Text = "Delete";
			bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			bindingNavigatorMoveFirstItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMoveFirstItem.Image");
			bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
			bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
			bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
			bindingNavigatorMoveFirstItem.Text = "Move first";
			bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			bindingNavigatorMovePreviousItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMovePreviousItem.Image");
			bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
			bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
			bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
			bindingNavigatorMovePreviousItem.Text = "Move previous";
			bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
			bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
			bindingNavigatorPositionItem.AccessibleName = "Position";
			bindingNavigatorPositionItem.AutoSize = false;
			bindingNavigatorPositionItem.Font = new System.Drawing.Font("Segoe UI", 9f);
			bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
			bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
			bindingNavigatorPositionItem.Text = "0";
			bindingNavigatorPositionItem.ToolTipText = "Current position";
			bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
			bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
			bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			bindingNavigatorMoveNextItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMoveNextItem.Image");
			bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
			bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
			bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
			bindingNavigatorMoveNextItem.Text = "Move next";
			bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			bindingNavigatorMoveLastItem.Image = (System.Drawing.Image)resources.GetObject("bindingNavigatorMoveLastItem.Image");
			bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
			bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
			bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
			bindingNavigatorMoveLastItem.Text = "Move last";
			bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
			bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
			superGird1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			superGird1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			superGird1.Columns.AddRange(userLogin, userMail, telephoneNumber);
			superGird1.Location = new System.Drawing.Point(2, 28);
			superGird1.Name = "superGird1";
			superGird1.PageSize = 10;
			superGird1.Size = new System.Drawing.Size(973, 290);
			superGird1.TabIndex = 30;
			superGird1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(SuperGird1_CellClick);
			userLogin.DataPropertyName = "userLogin";
			userLogin.HeaderText = "Mã";
			userLogin.Name = "userLogin";
			userLogin.Width = 65;
			userMail.DataPropertyName = "userMail";
			userMail.HeaderText = "Email";
			userMail.Name = "userMail";
			userMail.Width = 84;
			telephoneNumber.DataPropertyName = "telephoneNumber";
			telephoneNumber.HeaderText = "Số điện thoại";
			telephoneNumber.Name = "telephoneNumber";
			telephoneNumber.Width = 154;
			btnSearch.AutoSize = true;
			btnSearch.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnSearch.FlatAppearance.BorderSize = 0;
			btnSearch.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnSearch.ForeColor = System.Drawing.Color.White;
			btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnSearch.Location = new System.Drawing.Point(585, 82);
			btnSearch.Name = "btnSearch";
			btnSearch.Size = new System.Drawing.Size(114, 38);
			btnSearch.TabIndex = 106;
			btnSearch.Text = "Tìm kiếm";
			btnSearch.UseVisualStyleBackColor = false;
			btnSearch.Click += new System.EventHandler(BtnSearch_Click);
			pictureBox2.Image = TNUE_Patron_Excel.Properties.Resources.library_logo;
			pictureBox2.Location = new System.Drawing.Point(816, 3);
			pictureBox2.Name = "pictureBox2";
			pictureBox2.Size = new System.Drawing.Size(160, 151);
			pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			pictureBox2.TabIndex = 20;
			pictureBox2.TabStop = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox4);
			Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.Margin = new System.Windows.Forms.Padding(4);
			base.Name = "UCUpdatePatron";
			base.Size = new System.Drawing.Size(1000, 565);
			base.Load += new System.EventHandler(UCUpdatePatron_Load);
			groupBox4.ResumeLayout(false);
			panelUpdateSeris.ResumeLayout(false);
			panelUpdateSeris.PerformLayout();
			groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvHad).EndInit();
			groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvPatron).EndInit();
			groupBox1.ResumeLayout(false);
			groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).EndInit();
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			panelLdap.ResumeLayout(false);
			panelLdap.PerformLayout();
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)bindingNavigator1).EndInit();
			bindingNavigator1.ResumeLayout(false);
			bindingNavigator1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)superGird1).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
			ResumeLayout(false);
		}
	}
}
