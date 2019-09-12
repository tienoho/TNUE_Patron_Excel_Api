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
using TNUE_Patron_Excel.Tool;
using TNUE_Patron_Excel.Z303;

namespace TNUE_Patron_Excel.ControlMember
{
	public class UCDeleteUser : UserControl
	{
		private DataTable table = null;

		private List<Z308> listZ308 = null;

		private List<ItemBlock> DSKhongTonTai = null;

		private List<ItemBlock> ListDS = null;

		private List<string> listDeleteBlock = null;

		private IContainer components = null;

		private GroupBox groupBox1;

		private Panel panelDelete;

		private Button btnSearch;

		private Button btnDelete;

		private PictureBox pb_TaiChinh;

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

		private Button btnSua;

		private Label label1;

		private TextBox txtMa;

		private Label label3;

		private TextBox txtPhone;

		private Label label2;

		private TextBox txtEmail;

		private Label label4;

		private TextBox txtPassword;

		private Button btnUnSearch;

		private Label label5;

		private TextBox txtSearch;

		private Panel panel3;

		private RadioButton rbDeleteSeries;

		private RadioButton rbDelete;

		private Panel panelDeleteSeries;

		private Label label6;

		private Button btnChooseFile;

		private Button btnDeleteSeris;

		private Panel panel1;

		private Label lbKhongTonTaiDS;

		private Label lbCountDS;

		private GroupBox groupBox3;

		private DataGridView dgvDeleteBlock;

		private GroupBox groupBox2;

		private DataGridView dgvKhongTonTai;

		private PictureBox pictureBox1;

		private TextBox txtFileExcel;

		private DataGridViewTextBoxColumn userLogin;

		private DataGridViewTextBoxColumn userMail;

		private DataGridViewTextBoxColumn telephoneNumber;

		private DataGridViewTextBoxColumn Ma;

		private DataGridViewTextBoxColumn PatornID;

		private DataGridViewTextBoxColumn HoTen;

		private DataGridViewTextBoxColumn MaDSKhongTonTai;

		private DataGridViewTextBoxColumn PatornIDKhongTonTai;

		private DataGridViewTextBoxColumn HoTenDSKhongTonTai;

		public UCDeleteUser()
		{
			InitializeComponent();
		}

		private void UCDeleteUser_Load(object sender, EventArgs e)
		{
			try
			{
				if (rbDelete.Checked)
				{
					panelDelete.Visible = true;
					panelDeleteSeries.Visible = false;
				}
				LoadUserCase();
			}
			catch
			{
			}
		}

		private void BtnSua_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Bạn có chắc chắn muốn sửa?", "Xác nhân", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
			{
				EditLdap();
				MessageBox.Show("Đã sửa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
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

		private ItemBlock getUser(string ma)
		{
			ItemBlock itemBlock = null;
			try
			{
				int index = listZ308.FindIndex(delegate(Z308 dsd)
				{
					string z308_REC_KEY = dsd.Z308_REC_KEY;
					z308_REC_KEY = z308_REC_KEY.Substring(2).Trim();
					return z308_REC_KEY.Equals(ma);
				});
				itemBlock = new ItemBlock();
				itemBlock.PatronId = listZ308[index].Z308_ID;
				itemBlock.HoTen = listZ308[index].Z308_ENCRYPTION;
				itemBlock.Ma = listZ308[index].Z308_REC_KEY.Substring(2).Trim();
			}
			catch
			{
			}
			return itemBlock;
		}

		private void BtnDelete_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhân", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
			{
				if (new ModelLdap().DeleteUserLdap(txtMa.Text))
				{
					ItemBlock user = getUser(txtMa.Text);
					new AlephAPI().Url(sbPatronApi(user.PatronId, user.HoTen, "05").ToString());
					MessageBox.Show("Đã xóa thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					LoadUserCase();
				}
				else
				{
					MessageBox.Show("Xóa không thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
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

		private void OpenExcel()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "excel file |*.xls;*.xlsx";
			openFileDialog.FilterIndex = 1;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.Multiselect = false;
			openFileDialog.Title = "Chọn file excel";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				txtFileExcel.Text = openFileDialog.FileName;
			}
			if (txtFileExcel.Text != "")
			{
				Loading_FS.text = "\tĐang chuyển dữ liệu ...";
				Loading_FS.ShowSplash();
				ReadExcel(txtFileExcel.Text);
				FilterPatron();
				dgvDeleteBlock.DataSource = ListDS;
				dgvKhongTonTai.DataSource = DSKhongTonTai;
				lbCountDS.Text = dgvDeleteBlock.RowCount.ToString();
				lbKhongTonTaiDS.Text = dgvKhongTonTai.RowCount.ToString();
				Loading_FS.CloseSplash();
				btnDeleteSeris.Enabled = true;
				MessageBox.Show("Chuyển dữ liệu thành công!");
			}
			else
			{
				MessageBox.Show("Chưa chọn tệp tin");
			}
		}

		private void ReadExcel(string path)
		{
			try
			{
				Microsoft.Office.Interop.Excel.Application application = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
				application.Workbooks.Open(path, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
				listDeleteBlock = new List<string>();
				int count = application.Worksheets.Count;
				for (int i = 1; i < count + 1; i++)
				{
                    Excel.Worksheet worksheet = (Excel.Worksheet)(dynamic)application.Sheets[i];
					try
					{
						int count2 = worksheet.UsedRange.Rows.Count;
                        Excel.Range range = ((Excel.Worksheet)worksheet).get_Range((object)"A1", (object)("A" + count2));
						int count3 = range.Rows.Count;
						int count4 = range.Columns.Count;
						object[,] array = (object[,])(dynamic)range.Value2;
						for (int j = 1; j <= array.GetLength(0); j++)
						{
							string item = Convert.ToString(array[j, 1]).ToString();
							listDeleteBlock.Add(item);
						}
					}
					catch
					{
					}
				}
				application.Workbooks.Close();
				application.Quit();
				Marshal.ReleaseComObject(application);
				listDeleteBlock.Sort();
			}
			catch
			{
			}
		}

		private void FilterPatron()
		{
			DSKhongTonTai = new List<ItemBlock>();
			ListDS = new List<ItemBlock>();
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < listDeleteBlock.Count; i++)
			{
				num = 0;
				for (int j = num2; j < listZ308.Count; j++)
				{
					string text = listZ308[j].Z308_REC_KEY.Trim();
					text = text.Substring(2);
					if (listDeleteBlock[i].Trim().ToString().Equals(text))
					{
						num2++;
						num++;
						ItemBlock itemBlock = new ItemBlock();
						itemBlock.Ma = text;
						itemBlock.PatronId = listZ308[j].Z308_ID.Trim();
						itemBlock.HoTen = listZ308[j].Z308_ENCRYPTION.Trim();
						ListDS.Add(itemBlock);
						break;
					}
				}
				if (num == 0)
				{
					ItemBlock itemBlock2 = new ItemBlock();
					itemBlock2.Ma = listDeleteBlock[i].Trim().ToString();
					DSKhongTonTai.Add(itemBlock2);
					num = 0;
				}
			}
		}

		private StringBuilder sbPatronApi(string patronId, string name, string block)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
			stringBuilder.Append("<p-file-20>");
			stringBuilder.Append("<patron-record>");
			stringBuilder.Append(new z303Block().tab3(patronId));
			stringBuilder.Append(new z305Block().tab5("LSP", patronId, block));
			stringBuilder.Append(new z305Block().tab5("LSP50", patronId, block));
			stringBuilder.Append(new z305Block().tab5("ALEPH", patronId, block));
			stringBuilder.Append("</patron-record>");
			stringBuilder.Append("</p-file-20>");
			return stringBuilder;
		}

		private void LoadUserCase()
		{
			Loading_FS.text = "\tĐang cập nhập dữ liệu ...";
			Loading_FS.ShowSplash();
			listZ308 = DataDBLocal.listZ308;
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

		private void RbDelete_CheckedChanged(object sender, EventArgs e)
		{
			panelDelete.Visible = true;
			panelDeleteSeries.Visible = false;
		}

		private void RbDeleteSeries_CheckedChanged(object sender, EventArgs e)
		{
			panelDelete.Visible = false;
			panelDeleteSeries.Visible = true;
		}

		private void BtnChooseFile_Click(object sender, EventArgs e)
		{
			OpenExcel();
		}

		private void BtnDeleteSeris_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Bạn có chắc chắn muốn xóa?", "Xác nhân", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
			{
				Loading_FS.ShowSplash();
				using (StreamWriter streamWriter = new StreamWriter("log/Api-Patron-Block-Log-" + new ToolP().getDate() + ".txt"))
				{
					foreach (ItemBlock listD in ListDS)
					{
						streamWriter.WriteLine(new AlephAPI().Url(sbPatronApi(listD.PatronId, listD.HoTen, "05").ToString()));
					}
				}
				using (StreamWriter streamWriter2 = new StreamWriter("log/Ldap-Delete-Log-" + new ToolP().getDate() + ".txt"))
				{
					foreach (ItemBlock listD2 in ListDS)
					{
						if (new ModelLdap().DeleteUserLdap(listD2.Ma))
						{
							streamWriter2.WriteLine(listD2.Ma + "\t true");
						}
						else
						{
							streamWriter2.WriteLine(listD2.Ma + "\t false");
						}
					}
				}
				Loading_FS.CloseSplash();
				MessageBox.Show("Đã xóa thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				LoadUserCase();
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TNUE_Patron_Excel.ControlMember.UCDeleteUser));
			groupBox1 = new System.Windows.Forms.GroupBox();
			panelDeleteSeries = new System.Windows.Forms.Panel();
			label6 = new System.Windows.Forms.Label();
			btnChooseFile = new System.Windows.Forms.Button();
			btnDeleteSeris = new System.Windows.Forms.Button();
			panel1 = new System.Windows.Forms.Panel();
			lbKhongTonTaiDS = new System.Windows.Forms.Label();
			lbCountDS = new System.Windows.Forms.Label();
			groupBox3 = new System.Windows.Forms.GroupBox();
			dgvDeleteBlock = new System.Windows.Forms.DataGridView();
			Ma = new System.Windows.Forms.DataGridViewTextBoxColumn();
			PatornID = new System.Windows.Forms.DataGridViewTextBoxColumn();
			HoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
			groupBox2 = new System.Windows.Forms.GroupBox();
			dgvKhongTonTai = new System.Windows.Forms.DataGridView();
			MaDSKhongTonTai = new System.Windows.Forms.DataGridViewTextBoxColumn();
			PatornIDKhongTonTai = new System.Windows.Forms.DataGridViewTextBoxColumn();
			HoTenDSKhongTonTai = new System.Windows.Forms.DataGridViewTextBoxColumn();
			pictureBox1 = new System.Windows.Forms.PictureBox();
			txtFileExcel = new System.Windows.Forms.TextBox();
			panel3 = new System.Windows.Forms.Panel();
			rbDeleteSeries = new System.Windows.Forms.RadioButton();
			rbDelete = new System.Windows.Forms.RadioButton();
			panelDelete = new System.Windows.Forms.Panel();
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
			btnDelete = new System.Windows.Forms.Button();
			pb_TaiChinh = new System.Windows.Forms.PictureBox();
			groupBox1.SuspendLayout();
			panelDeleteSeries.SuspendLayout();
			panel1.SuspendLayout();
			groupBox3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvDeleteBlock).BeginInit();
			groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)dgvKhongTonTai).BeginInit();
			((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
			panel3.SuspendLayout();
			panelDelete.SuspendLayout();
			panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)bindingNavigator1).BeginInit();
			bindingNavigator1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)superGird1).BeginInit();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).BeginInit();
			SuspendLayout();
			groupBox1.Controls.Add(panelDeleteSeries);
			groupBox1.Controls.Add(panel3);
			groupBox1.Controls.Add(panelDelete);
			groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold);
			groupBox1.Location = new System.Drawing.Point(0, 3);
			groupBox1.Name = "groupBox1";
			groupBox1.Size = new System.Drawing.Size(993, 559);
			groupBox1.TabIndex = 28;
			groupBox1.TabStop = false;
			groupBox1.Text = "Xóa bạn đọc";
			panelDeleteSeries.Controls.Add(label6);
			panelDeleteSeries.Controls.Add(btnChooseFile);
			panelDeleteSeries.Controls.Add(btnDeleteSeris);
			panelDeleteSeries.Controls.Add(panel1);
			panelDeleteSeries.Controls.Add(pictureBox1);
			panelDeleteSeries.Controls.Add(txtFileExcel);
			panelDeleteSeries.Location = new System.Drawing.Point(3, 68);
			panelDeleteSeries.Name = "panelDeleteSeries";
			panelDeleteSeries.Size = new System.Drawing.Size(987, 490);
			panelDeleteSeries.TabIndex = 128;
			label6.AutoSize = true;
			label6.Location = new System.Drawing.Point(14, 19);
			label6.Name = "label6";
			label6.Size = new System.Drawing.Size(300, 25);
			label6.TabIndex = 111;
			label6.Text = "Chọn tệp excel chứa mã bạn đọc";
			btnChooseFile.AutoSize = true;
			btnChooseFile.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnChooseFile.FlatAppearance.BorderSize = 0;
			btnChooseFile.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnChooseFile.ForeColor = System.Drawing.Color.White;
			btnChooseFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnChooseFile.Location = new System.Drawing.Point(574, 48);
			btnChooseFile.Name = "btnChooseFile";
			btnChooseFile.Size = new System.Drawing.Size(103, 36);
			btnChooseFile.TabIndex = 110;
			btnChooseFile.Text = "Chọn tệp";
			btnChooseFile.UseVisualStyleBackColor = false;
			btnChooseFile.Click += new System.EventHandler(BtnChooseFile_Click);
			btnDeleteSeris.AutoSize = true;
			btnDeleteSeris.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
			btnDeleteSeris.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnDeleteSeris.Enabled = false;
			btnDeleteSeris.FlatAppearance.BorderSize = 0;
			btnDeleteSeris.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnDeleteSeris.ForeColor = System.Drawing.Color.White;
			btnDeleteSeris.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnDeleteSeris.Location = new System.Drawing.Point(683, 48);
			btnDeleteSeris.Name = "btnDeleteSeris";
			btnDeleteSeris.Size = new System.Drawing.Size(103, 36);
			btnDeleteSeris.TabIndex = 109;
			btnDeleteSeris.Text = "Xóa";
			btnDeleteSeris.UseVisualStyleBackColor = false;
			btnDeleteSeris.Click += new System.EventHandler(BtnDeleteSeris_Click);
			panel1.Controls.Add(lbKhongTonTaiDS);
			panel1.Controls.Add(lbCountDS);
			panel1.Controls.Add(groupBox3);
			panel1.Controls.Add(groupBox2);
			panel1.Location = new System.Drawing.Point(3, 163);
			panel1.Name = "panel1";
			panel1.Size = new System.Drawing.Size(973, 318);
			panel1.TabIndex = 108;
			lbKhongTonTaiDS.AutoSize = true;
			lbKhongTonTaiDS.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lbKhongTonTaiDS.Location = new System.Drawing.Point(575, 292);
			lbKhongTonTaiDS.Name = "lbKhongTonTaiDS";
			lbKhongTonTaiDS.Size = new System.Drawing.Size(57, 21);
			lbKhongTonTaiDS.TabIndex = 112;
			lbKhongTonTaiDS.Text = "Tổng: ";
			lbCountDS.AutoSize = true;
			lbCountDS.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lbCountDS.Location = new System.Drawing.Point(9, 292);
			lbCountDS.Name = "lbCountDS";
			lbCountDS.Size = new System.Drawing.Size(57, 21);
			lbCountDS.TabIndex = 111;
			lbCountDS.Text = "Tổng: ";
			groupBox3.Controls.Add(dgvDeleteBlock);
			groupBox3.Location = new System.Drawing.Point(3, 3);
			groupBox3.Name = "groupBox3";
			groupBox3.Size = new System.Drawing.Size(563, 288);
			groupBox3.TabIndex = 110;
			groupBox3.TabStop = false;
			groupBox3.Text = "Danh sách bạn đọc";
			dgvDeleteBlock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dgvDeleteBlock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvDeleteBlock.Columns.AddRange(Ma, PatornID, HoTen);
			dgvDeleteBlock.Dock = System.Windows.Forms.DockStyle.Fill;
			dgvDeleteBlock.Location = new System.Drawing.Point(3, 29);
			dgvDeleteBlock.Name = "dgvDeleteBlock";
			dgvDeleteBlock.Size = new System.Drawing.Size(557, 256);
			dgvDeleteBlock.TabIndex = 57;
			Ma.DataPropertyName = "Ma";
			Ma.HeaderText = "Mã";
			Ma.Name = "Ma";
			Ma.Width = 65;
			PatornID.DataPropertyName = "PatornId";
			PatornID.HeaderText = "PatornID";
			PatornID.Name = "PatornID";
			PatornID.Width = 118;
			HoTen.DataPropertyName = "HoTen";
			HoTen.HeaderText = "Họ tên";
			HoTen.Name = "HoTen";
			HoTen.Width = 98;
			groupBox2.Controls.Add(dgvKhongTonTai);
			groupBox2.Location = new System.Drawing.Point(569, 3);
			groupBox2.Name = "groupBox2";
			groupBox2.Size = new System.Drawing.Size(401, 288);
			groupBox2.TabIndex = 109;
			groupBox2.TabStop = false;
			groupBox2.Text = "Danh sách không tồn tại";
			dgvKhongTonTai.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			dgvKhongTonTai.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			dgvKhongTonTai.Columns.AddRange(MaDSKhongTonTai, PatornIDKhongTonTai, HoTenDSKhongTonTai);
			dgvKhongTonTai.Dock = System.Windows.Forms.DockStyle.Fill;
			dgvKhongTonTai.Location = new System.Drawing.Point(3, 29);
			dgvKhongTonTai.Name = "dgvKhongTonTai";
			dgvKhongTonTai.Size = new System.Drawing.Size(395, 256);
			dgvKhongTonTai.TabIndex = 58;
			MaDSKhongTonTai.DataPropertyName = "Ma";
			MaDSKhongTonTai.HeaderText = "Mã";
			MaDSKhongTonTai.Name = "MaDSKhongTonTai";
			MaDSKhongTonTai.Width = 65;
			PatornIDKhongTonTai.DataPropertyName = "PatornID";
			PatornIDKhongTonTai.HeaderText = "PatornID";
			PatornIDKhongTonTai.Name = "PatornIDKhongTonTai";
			PatornIDKhongTonTai.Width = 118;
			HoTenDSKhongTonTai.DataPropertyName = "HoTen";
			HoTenDSKhongTonTai.HeaderText = "Họ tên";
			HoTenDSKhongTonTai.Name = "HoTenDSKhongTonTai";
			HoTenDSKhongTonTai.Width = 98;
			pictureBox1.Image = TNUE_Patron_Excel.Properties.Resources.library_logo;
			pictureBox1.Location = new System.Drawing.Point(817, 7);
			pictureBox1.Name = "pictureBox1";
			pictureBox1.Size = new System.Drawing.Size(160, 151);
			pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			pictureBox1.TabIndex = 20;
			pictureBox1.TabStop = false;
			txtFileExcel.Location = new System.Drawing.Point(14, 50);
			txtFileExcel.Margin = new System.Windows.Forms.Padding(2);
			txtFileExcel.Name = "txtFileExcel";
			txtFileExcel.Size = new System.Drawing.Size(555, 33);
			txtFileExcel.TabIndex = 50;
			panel3.Controls.Add(rbDeleteSeries);
			panel3.Controls.Add(rbDelete);
			panel3.Dock = System.Windows.Forms.DockStyle.Top;
			panel3.Location = new System.Drawing.Point(3, 29);
			panel3.Name = "panel3";
			panel3.Size = new System.Drawing.Size(987, 37);
			panel3.TabIndex = 32;
			rbDeleteSeries.AutoSize = true;
			rbDeleteSeries.Location = new System.Drawing.Point(164, 5);
			rbDeleteSeries.Name = "rbDeleteSeries";
			rbDeleteSeries.Size = new System.Drawing.Size(153, 29);
			rbDeleteSeries.TabIndex = 1;
			rbDeleteSeries.Text = "Xóa hàng loạt";
			rbDeleteSeries.UseVisualStyleBackColor = true;
			rbDeleteSeries.CheckedChanged += new System.EventHandler(RbDeleteSeries_CheckedChanged);
			rbDelete.AutoSize = true;
			rbDelete.Checked = true;
			rbDelete.Location = new System.Drawing.Point(8, 5);
			rbDelete.Name = "rbDelete";
			rbDelete.Size = new System.Drawing.Size(64, 29);
			rbDelete.TabIndex = 0;
			rbDelete.TabStop = true;
			rbDelete.Text = "Xóa";
			rbDelete.UseVisualStyleBackColor = true;
			rbDelete.CheckedChanged += new System.EventHandler(RbDelete_CheckedChanged);
			panelDelete.Controls.Add(btnUnSearch);
			panelDelete.Controls.Add(label5);
			panelDelete.Controls.Add(txtSearch);
			panelDelete.Controls.Add(label4);
			panelDelete.Controls.Add(txtPassword);
			panelDelete.Controls.Add(label3);
			panelDelete.Controls.Add(txtPhone);
			panelDelete.Controls.Add(label2);
			panelDelete.Controls.Add(txtEmail);
			panelDelete.Controls.Add(label1);
			panelDelete.Controls.Add(txtMa);
			panelDelete.Controls.Add(btnSua);
			panelDelete.Controls.Add(panel2);
			panelDelete.Controls.Add(btnSearch);
			panelDelete.Controls.Add(btnDelete);
			panelDelete.Controls.Add(pb_TaiChinh);
			panelDelete.Location = new System.Drawing.Point(6, 72);
			panelDelete.Name = "panelDelete";
			panelDelete.Size = new System.Drawing.Size(981, 490);
			panelDelete.TabIndex = 127;
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
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(629, 11);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(125, 25);
			label5.TabIndex = 119;
			label5.Text = "Tìm kiếm mã";
			txtSearch.Location = new System.Drawing.Point(585, 39);
			txtSearch.Name = "txtSearch";
			txtSearch.Size = new System.Drawing.Size(220, 33);
			txtSearch.TabIndex = 118;
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(3, 126);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(96, 25);
			label4.TabIndex = 117;
			label4.Text = "Mật khẩu";
			txtPassword.Location = new System.Drawing.Point(143, 123);
			txtPassword.Name = "txtPassword";
			txtPassword.Size = new System.Drawing.Size(281, 33);
			txtPassword.TabIndex = 116;
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(3, 87);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(69, 25);
			label3.TabIndex = 115;
			label3.Text = "Phone";
			txtPhone.Location = new System.Drawing.Point(143, 84);
			txtPhone.Name = "txtPhone";
			txtPhone.Size = new System.Drawing.Size(281, 33);
			txtPhone.TabIndex = 114;
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(3, 48);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(59, 25);
			label2.TabIndex = 113;
			label2.Text = "Email";
			txtEmail.Location = new System.Drawing.Point(143, 45);
			txtEmail.Name = "txtEmail";
			txtEmail.Size = new System.Drawing.Size(281, 33);
			txtEmail.TabIndex = 112;
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(3, 9);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(101, 25);
			label1.TabIndex = 111;
			label1.Text = "Mã SV/CB";
			txtMa.Enabled = false;
			txtMa.Location = new System.Drawing.Point(143, 6);
			txtMa.Name = "txtMa";
			txtMa.Size = new System.Drawing.Size(281, 33);
			txtMa.TabIndex = 110;
			btnSua.AutoSize = true;
			btnSua.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnSua.FlatAppearance.BorderSize = 0;
			btnSua.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnSua.ForeColor = System.Drawing.Color.White;
			btnSua.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnSua.Location = new System.Drawing.Point(428, 84);
			btnSua.Name = "btnSua";
			btnSua.Size = new System.Drawing.Size(103, 35);
			btnSua.TabIndex = 109;
			btnSua.Text = "Sửa";
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
			btnDelete.AutoSize = true;
			btnDelete.BackColor = System.Drawing.Color.FromArgb(231, 76, 60);
			btnDelete.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			btnDelete.FlatAppearance.BorderSize = 0;
			btnDelete.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnDelete.ForeColor = System.Drawing.Color.White;
			btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnDelete.Location = new System.Drawing.Point(428, 120);
			btnDelete.Name = "btnDelete";
			btnDelete.Size = new System.Drawing.Size(103, 38);
			btnDelete.TabIndex = 14;
			btnDelete.Text = "Xóa";
			btnDelete.UseVisualStyleBackColor = false;
			btnDelete.Click += new System.EventHandler(BtnDelete_Click);
			pb_TaiChinh.Image = TNUE_Patron_Excel.Properties.Resources.library_logo;
			pb_TaiChinh.Location = new System.Drawing.Point(816, 3);
			pb_TaiChinh.Name = "pb_TaiChinh";
			pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
			pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			pb_TaiChinh.TabIndex = 20;
			pb_TaiChinh.TabStop = false;
			base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.Controls.Add(groupBox1);
			Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.Margin = new System.Windows.Forms.Padding(4);
			base.Name = "UCDeleteUser";
			base.Size = new System.Drawing.Size(1000, 565);
			base.Load += new System.EventHandler(UCDeleteUser_Load);
			groupBox1.ResumeLayout(false);
			panelDeleteSeries.ResumeLayout(false);
			panelDeleteSeries.PerformLayout();
			panel1.ResumeLayout(false);
			panel1.PerformLayout();
			groupBox3.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvDeleteBlock).EndInit();
			groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)dgvKhongTonTai).EndInit();
			((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
			panel3.ResumeLayout(false);
			panel3.PerformLayout();
			panelDelete.ResumeLayout(false);
			panelDelete.PerformLayout();
			panel2.ResumeLayout(false);
			panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)bindingNavigator1).EndInit();
			bindingNavigator1.ResumeLayout(false);
			bindingNavigator1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)superGird1).EndInit();
			((System.ComponentModel.ISupportInitialize)pb_TaiChinh).EndInit();
			ResumeLayout(false);
		}
	}
}
