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
        private DataTable tableClone = null;

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
                int index = listZ308.FindIndex(delegate (Z308 dsd)
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
            try
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
            catch
            {
                superGird1.DataSource = null;
            }

        }

        private void BtnUnSearch_Click(object sender, EventArgs e)
        {
            superGird1.Columns.Clear();
            superGird1.SetPagedDataSource(tableClone, bindingNavigator1);
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
            Excel.Application application = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            try
            {
                workbook = application.Workbooks.Open(path);
                listDeleteBlock = new List<string>();
                int count = application.Worksheets.Count;
                for (int i = 1; i < count + 1; i++)
                {
                    worksheet = (Excel.Worksheet)(dynamic)application.Sheets[i];
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
                listDeleteBlock.Sort();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: \n" + ex.Message, "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (worksheet != null)
                {
                    Marshal.ReleaseComObject(worksheet);
                }
                if (workbook != null)
                {
                    workbook.Close(false, Type.Missing, Type.Missing);
                    Marshal.ReleaseComObject(workbook);
                }
                application.Quit();
                Marshal.ReleaseComObject(application);
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
            tableClone = table.CloneObject();
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDeleteUser));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panelDeleteSeries = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.btnChooseFile = new System.Windows.Forms.Button();
            this.btnDeleteSeris = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbKhongTonTaiDS = new System.Windows.Forms.Label();
            this.lbCountDS = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvDeleteBlock = new System.Windows.Forms.DataGridView();
            this.Ma = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PatornID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvKhongTonTai = new System.Windows.Forms.DataGridView();
            this.MaDSKhongTonTai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PatornIDKhongTonTai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.HoTenDSKhongTonTai = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtFileExcel = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbDeleteSeries = new System.Windows.Forms.RadioButton();
            this.rbDelete = new System.Windows.Forms.RadioButton();
            this.panelDelete = new System.Windows.Forms.Panel();
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
            this.btnDelete = new System.Windows.Forms.Button();
            this.pb_TaiChinh = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.panelDeleteSeries.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeleteBlock)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhongTonTai)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel3.SuspendLayout();
            this.panelDelete.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superGird1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panelDeleteSeries);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panelDelete);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(993, 559);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Xóa bạn đọc";
            // 
            // panelDeleteSeries
            // 
            this.panelDeleteSeries.Controls.Add(this.label6);
            this.panelDeleteSeries.Controls.Add(this.btnChooseFile);
            this.panelDeleteSeries.Controls.Add(this.btnDeleteSeris);
            this.panelDeleteSeries.Controls.Add(this.panel1);
            this.panelDeleteSeries.Controls.Add(this.pictureBox1);
            this.panelDeleteSeries.Controls.Add(this.txtFileExcel);
            this.panelDeleteSeries.Location = new System.Drawing.Point(3, 68);
            this.panelDeleteSeries.Name = "panelDeleteSeries";
            this.panelDeleteSeries.Size = new System.Drawing.Size(987, 490);
            this.panelDeleteSeries.TabIndex = 128;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(300, 25);
            this.label6.TabIndex = 111;
            this.label6.Text = "Chọn tệp excel chứa mã bạn đọc";
            // 
            // btnChooseFile
            // 
            this.btnChooseFile.AutoSize = true;
            this.btnChooseFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnChooseFile.FlatAppearance.BorderSize = 0;
            this.btnChooseFile.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChooseFile.ForeColor = System.Drawing.Color.White;
            this.btnChooseFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnChooseFile.Location = new System.Drawing.Point(574, 48);
            this.btnChooseFile.Name = "btnChooseFile";
            this.btnChooseFile.Size = new System.Drawing.Size(103, 36);
            this.btnChooseFile.TabIndex = 110;
            this.btnChooseFile.Text = "Chọn tệp";
            this.btnChooseFile.UseVisualStyleBackColor = false;
            this.btnChooseFile.Click += new System.EventHandler(this.BtnChooseFile_Click);
            // 
            // btnDeleteSeris
            // 
            this.btnDeleteSeris.AutoSize = true;
            this.btnDeleteSeris.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnDeleteSeris.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDeleteSeris.Enabled = false;
            this.btnDeleteSeris.FlatAppearance.BorderSize = 0;
            this.btnDeleteSeris.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteSeris.ForeColor = System.Drawing.Color.White;
            this.btnDeleteSeris.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteSeris.Location = new System.Drawing.Point(683, 48);
            this.btnDeleteSeris.Name = "btnDeleteSeris";
            this.btnDeleteSeris.Size = new System.Drawing.Size(103, 36);
            this.btnDeleteSeris.TabIndex = 109;
            this.btnDeleteSeris.Text = "Xóa";
            this.btnDeleteSeris.UseVisualStyleBackColor = false;
            this.btnDeleteSeris.Click += new System.EventHandler(this.BtnDeleteSeris_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbKhongTonTaiDS);
            this.panel1.Controls.Add(this.lbCountDS);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Location = new System.Drawing.Point(3, 163);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(973, 318);
            this.panel1.TabIndex = 108;
            // 
            // lbKhongTonTaiDS
            // 
            this.lbKhongTonTaiDS.AutoSize = true;
            this.lbKhongTonTaiDS.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbKhongTonTaiDS.Location = new System.Drawing.Point(575, 292);
            this.lbKhongTonTaiDS.Name = "lbKhongTonTaiDS";
            this.lbKhongTonTaiDS.Size = new System.Drawing.Size(57, 21);
            this.lbKhongTonTaiDS.TabIndex = 112;
            this.lbKhongTonTaiDS.Text = "Tổng: ";
            // 
            // lbCountDS
            // 
            this.lbCountDS.AutoSize = true;
            this.lbCountDS.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCountDS.Location = new System.Drawing.Point(9, 292);
            this.lbCountDS.Name = "lbCountDS";
            this.lbCountDS.Size = new System.Drawing.Size(57, 21);
            this.lbCountDS.TabIndex = 111;
            this.lbCountDS.Text = "Tổng: ";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvDeleteBlock);
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(563, 288);
            this.groupBox3.TabIndex = 110;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Danh sách bạn đọc";
            // 
            // dgvDeleteBlock
            // 
            this.dgvDeleteBlock.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDeleteBlock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeleteBlock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Ma,
            this.PatornID,
            this.HoTen});
            this.dgvDeleteBlock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDeleteBlock.Location = new System.Drawing.Point(3, 29);
            this.dgvDeleteBlock.Name = "dgvDeleteBlock";
            this.dgvDeleteBlock.Size = new System.Drawing.Size(557, 256);
            this.dgvDeleteBlock.TabIndex = 57;
            // 
            // Ma
            // 
            this.Ma.DataPropertyName = "Ma";
            this.Ma.HeaderText = "Mã";
            this.Ma.Name = "Ma";
            // 
            // PatornID
            // 
            this.PatornID.DataPropertyName = "PatornId";
            this.PatornID.HeaderText = "PatornID";
            this.PatornID.Name = "PatornID";
            // 
            // HoTen
            // 
            this.HoTen.DataPropertyName = "HoTen";
            this.HoTen.HeaderText = "Họ tên";
            this.HoTen.Name = "HoTen";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvKhongTonTai);
            this.groupBox2.Location = new System.Drawing.Point(569, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(401, 288);
            this.groupBox2.TabIndex = 109;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Danh sách không tồn tại";
            // 
            // dgvKhongTonTai
            // 
            this.dgvKhongTonTai.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvKhongTonTai.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKhongTonTai.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MaDSKhongTonTai,
            this.PatornIDKhongTonTai,
            this.HoTenDSKhongTonTai});
            this.dgvKhongTonTai.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvKhongTonTai.Location = new System.Drawing.Point(3, 29);
            this.dgvKhongTonTai.Name = "dgvKhongTonTai";
            this.dgvKhongTonTai.Size = new System.Drawing.Size(395, 256);
            this.dgvKhongTonTai.TabIndex = 58;
            // 
            // MaDSKhongTonTai
            // 
            this.MaDSKhongTonTai.DataPropertyName = "Ma";
            this.MaDSKhongTonTai.HeaderText = "Mã";
            this.MaDSKhongTonTai.Name = "MaDSKhongTonTai";
            // 
            // PatornIDKhongTonTai
            // 
            this.PatornIDKhongTonTai.DataPropertyName = "PatornID";
            this.PatornIDKhongTonTai.HeaderText = "PatornID";
            this.PatornIDKhongTonTai.Name = "PatornIDKhongTonTai";
            // 
            // HoTenDSKhongTonTai
            // 
            this.HoTenDSKhongTonTai.DataPropertyName = "HoTen";
            this.HoTenDSKhongTonTai.HeaderText = "Họ tên";
            this.HoTenDSKhongTonTai.Name = "HoTenDSKhongTonTai";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::TNUE_Patron_Excel.Properties.Resources.library_logo;
            this.pictureBox1.Location = new System.Drawing.Point(817, 7);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(160, 151);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 20;
            this.pictureBox1.TabStop = false;
            // 
            // txtFileExcel
            // 
            this.txtFileExcel.Location = new System.Drawing.Point(14, 50);
            this.txtFileExcel.Margin = new System.Windows.Forms.Padding(2);
            this.txtFileExcel.Name = "txtFileExcel";
            this.txtFileExcel.Size = new System.Drawing.Size(555, 33);
            this.txtFileExcel.TabIndex = 50;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rbDeleteSeries);
            this.panel3.Controls.Add(this.rbDelete);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 29);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(987, 37);
            this.panel3.TabIndex = 32;
            // 
            // rbDeleteSeries
            // 
            this.rbDeleteSeries.AutoSize = true;
            this.rbDeleteSeries.Location = new System.Drawing.Point(164, 5);
            this.rbDeleteSeries.Name = "rbDeleteSeries";
            this.rbDeleteSeries.Size = new System.Drawing.Size(153, 29);
            this.rbDeleteSeries.TabIndex = 1;
            this.rbDeleteSeries.Text = "Xóa hàng loạt";
            this.rbDeleteSeries.UseVisualStyleBackColor = true;
            this.rbDeleteSeries.CheckedChanged += new System.EventHandler(this.RbDeleteSeries_CheckedChanged);
            // 
            // rbDelete
            // 
            this.rbDelete.AutoSize = true;
            this.rbDelete.Checked = true;
            this.rbDelete.Location = new System.Drawing.Point(8, 5);
            this.rbDelete.Name = "rbDelete";
            this.rbDelete.Size = new System.Drawing.Size(64, 29);
            this.rbDelete.TabIndex = 0;
            this.rbDelete.TabStop = true;
            this.rbDelete.Text = "Xóa";
            this.rbDelete.UseVisualStyleBackColor = true;
            this.rbDelete.CheckedChanged += new System.EventHandler(this.RbDelete_CheckedChanged);
            // 
            // panelDelete
            // 
            this.panelDelete.Controls.Add(this.btnUnSearch);
            this.panelDelete.Controls.Add(this.label5);
            this.panelDelete.Controls.Add(this.txtSearch);
            this.panelDelete.Controls.Add(this.label4);
            this.panelDelete.Controls.Add(this.txtPassword);
            this.panelDelete.Controls.Add(this.label3);
            this.panelDelete.Controls.Add(this.txtPhone);
            this.panelDelete.Controls.Add(this.label2);
            this.panelDelete.Controls.Add(this.txtEmail);
            this.panelDelete.Controls.Add(this.label1);
            this.panelDelete.Controls.Add(this.txtMa);
            this.panelDelete.Controls.Add(this.btnSua);
            this.panelDelete.Controls.Add(this.panel2);
            this.panelDelete.Controls.Add(this.btnSearch);
            this.panelDelete.Controls.Add(this.btnDelete);
            this.panelDelete.Controls.Add(this.pb_TaiChinh);
            this.panelDelete.Location = new System.Drawing.Point(6, 72);
            this.panelDelete.Name = "panelDelete";
            this.panelDelete.Size = new System.Drawing.Size(981, 490);
            this.panelDelete.TabIndex = 127;
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(629, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 25);
            this.label5.TabIndex = 119;
            this.label5.Text = "Tìm kiếm mã";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(585, 39);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(220, 33);
            this.txtSearch.TabIndex = 118;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 126);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 25);
            this.label4.TabIndex = 117;
            this.label4.Text = "Mật khẩu";
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(143, 123);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(281, 33);
            this.txtPassword.TabIndex = 116;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 25);
            this.label3.TabIndex = 115;
            this.label3.Text = "Phone";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(143, 84);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(281, 33);
            this.txtPhone.TabIndex = 114;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 25);
            this.label2.TabIndex = 113;
            this.label2.Text = "Email";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(143, 45);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(281, 33);
            this.txtEmail.TabIndex = 112;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 25);
            this.label1.TabIndex = 111;
            this.label1.Text = "Mã SV/CB";
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
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnSua.FlatAppearance.BorderSize = 0;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSua.Location = new System.Drawing.Point(428, 84);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(103, 35);
            this.btnSua.TabIndex = 109;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = false;
            this.btnSua.Click += new System.EventHandler(this.BtnSua_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.bindingNavigator1);
            this.panel2.Controls.Add(this.superGird1);
            this.panel2.Location = new System.Drawing.Point(3, 163);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(973, 331);
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
            this.bindingNavigatorPositionItem.Font = new System.Drawing.Font("Segoe UI", 9F);
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
            this.superGird1.Size = new System.Drawing.Size(973, 290);
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
            // btnDelete
            // 
            this.btnDelete.AutoSize = true;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            this.btnDelete.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(428, 120);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(103, 38);
            this.btnDelete.TabIndex = 14;
            this.btnDelete.Text = "Xóa";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // pb_TaiChinh
            // 
            this.pb_TaiChinh.Image = global::TNUE_Patron_Excel.Properties.Resources.library_logo;
            this.pb_TaiChinh.Location = new System.Drawing.Point(816, 3);
            this.pb_TaiChinh.Name = "pb_TaiChinh";
            this.pb_TaiChinh.Size = new System.Drawing.Size(160, 151);
            this.pb_TaiChinh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pb_TaiChinh.TabIndex = 20;
            this.pb_TaiChinh.TabStop = false;
            // 
            // UCDeleteUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCDeleteUser";
            this.Size = new System.Drawing.Size(1000, 565);
            this.Load += new System.EventHandler(this.UCDeleteUser_Load);
            this.groupBox1.ResumeLayout(false);
            this.panelDeleteSeries.ResumeLayout(false);
            this.panelDeleteSeries.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeleteBlock)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvKhongTonTai)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelDelete.ResumeLayout(false);
            this.panelDelete.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.superGird1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
