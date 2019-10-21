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

        private string directoryPath = DataDBLocal.pathUserLog;

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
                if (listPatron.Count > 0)
                {
                    btnConvert.Enabled = true;
                    MessageBox.Show("Chuyển dữ liệu thành công!");
                }
                else
                {
                    MessageBox.Show("Không có dữ liệu!");
                }
               
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
            fileEx = new Excel.Application();
            Excel.Workbook workbook = null;
            Excel.Worksheet worksheet = null;
            try
            {
                workbook = fileEx.Workbooks.Open(fileName);
                string dateTime = DateTime.Now.ToString("dd/MM/yyyy");
                string dateTime2 = DateTime.Now.AddYears(4).ToString("dd/MM/yyyy");

                listPatron = new List<Patron>();
                sbList = new StringBuilder();
                int count = fileEx.Worksheets.Count;
                string str = txtLine.Text.Trim();
                int num = int.Parse(txtPatronId.Text);
                for (int i = 1; i < count + 1; i++)
                {
                    worksheet = (Excel.Worksheet)(dynamic)fileEx.Sheets[i];

                    int count2 = worksheet.UsedRange.Rows.Count;
                    Excel.Range range = ((Excel.Worksheet)worksheet).get_Range((object)("A" + str), (object)("R" + count2));
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
                            string text4 = patron.Day = tool.formatDate(dateTime.ToString());
                            string text2 = Convert.ToString(array[j, 4]) + " " + Convert.ToString(array[j, 6]);
                            patron.HoTen = Unicode.compound2Unicode(text2.Trim());
                            patron.ngaySinh = tool.formatDate(Convert.ToString(array[j, 7]));
                            patron.password = tool.formatDatePassword(Convert.ToString(array[j, 7]));
                            patron.GT = Unicode.compound2Unicode(tool.convertGender(Convert.ToString(array[j, 8])));
                            patron.phone = Convert.ToString(array[j, 9]);
                            patron.email = Convert.ToString(array[j, 10]);
                            patron.makh = "";
                            patron.DiaChi = "";
                            string str3 = Convert.ToString(array[j, 11]);
                            patron.lopHoc = Unicode.compound2Unicode(str3);
                            string str2 = Convert.ToString(array[j, 12]);
                            patron.khoaHoc = Unicode.compound2Unicode(str2);
                            string str4 = Convert.ToString(array[j, 13]);
                            patron.Khoa = Unicode.compound2Unicode(str4);
                            string str5 = Convert.ToString(array[j, 14]);
                            patron.QuocTich = Unicode.compound2Unicode(str5);
                            string str6 = Convert.ToString(array[j, 15]);
                            patron.hocBong = Unicode.compound2Unicode(str6);
                            string str7 = Convert.ToString(array[j, 16]);
                            patron.qdCongNhan = Unicode.compound2Unicode(str7);
                            string text5 = Convert.ToString(array[j, 17]);
                            patron.ngayHetHan = tool.getNgayHetHan(text5);
                            listPatron.Add(patron);
                            num++;
                        }
                    }
                }
                listPatron.RemoveAll((Patron item) => item.MaSV_O == "");

            }
            catch (Exception arg)
            {
                MessageBox.Show("Lỗi: " + arg);
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
                fileEx.Quit();
                Marshal.ReleaseComObject(fileEx);
            }
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
                MessageBox.Show("Đã thêm thành công " + listSb.Count + " bạn đọc!", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);

                Loading_FS.text = "\tĐang cập nhập lại dữ liệu ...";
                Loading_FS.ShowSplash();
                ResetFormData();
                Loading_FS.CloseSplash();

            }
            else
            {
                MessageBox.Show("Chưa chọn đường dẫn lưu !", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }
        private void ResetFormData()
        {
            DataDBLocal.listZ308 = new QueryDB().listZ308TED();
            listZ308 = DataDBLocal.listZ308.CloneObject();
            txtLine.Text = "12";
            countP = new QueryDB().CountPatron();
            txtPatronId.Text = $"{countP + 1:000000000000}";
            dgvPatron.DataSource = null;
            dgvHad.DataSource = null;
            textBox1.Clear();
            textBox2.Clear();
            btnConvert.Enabled = false;
            btnPush.Enabled = false;
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.label7 = new System.Windows.Forms.Label();
            this.cbLoaiBanDoc = new System.Windows.Forms.ComboBox();
            this.pb_TaiChinh = new System.Windows.Forms.PictureBox();
            this.btnPush = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnXml = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_ldap = new System.Windows.Forms.Button();
            this.btn_api = new System.Windows.Forms.Button();
            this.txtPatronId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLine = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowserFile = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnGetData = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnThoat = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
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
            this.lbCountListExcel = new System.Windows.Forms.Label();
            this.lbCountHad = new System.Windows.Forms.Label();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatron)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHad)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvPatron);
            this.groupBox3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(3, 246);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(620, 297);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "DANH SÁCH";
            // 
            // dgvPatron
            // 
            this.dgvPatron.AllowUserToAddRows = false;
            this.dgvPatron.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvPatron.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
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
            this.dgvPatron.Size = new System.Drawing.Size(614, 276);
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
            dataGridViewCellStyle2.Format = "d";
            dataGridViewCellStyle2.NullValue = null;
            this.ngaySinh.DefaultCellStyle = dataGridViewCellStyle2;
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
            this.makh.Width = 58;
            // 
            // chucVu
            // 
            this.chucVu.DataPropertyName = "chucVu";
            this.chucVu.HeaderText = "Chức Vụ";
            this.chucVu.Name = "chucVu";
            this.chucVu.ReadOnly = true;
            this.chucVu.Visible = false;
            this.chucVu.Width = 73;
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
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbLoaiBanDoc);
            this.groupBox1.Controls.Add(this.pb_TaiChinh);
            this.groupBox1.Controls.Add(this.btnPush);
            this.groupBox1.Controls.Add(this.btnConvert);
            this.groupBox1.Controls.Add(this.btnXml);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btn_ldap);
            this.groupBox1.Controls.Add(this.btn_api);
            this.groupBox1.Controls.Add(this.txtPatronId);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtLine);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.textBox3);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnBrowserFile);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnGetData);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnThoat);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(993, 240);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Sinh vien";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 199);
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
            this.cbLoaiBanDoc.Location = new System.Drawing.Point(144, 196);
            this.cbLoaiBanDoc.Name = "cbLoaiBanDoc";
            this.cbLoaiBanDoc.Size = new System.Drawing.Size(316, 33);
            this.cbLoaiBanDoc.TabIndex = 108;
            // 
            // pb_TaiChinh
            // 
            this.pb_TaiChinh.Image = global::TNUE_Patron_Excel.Properties.Resources.library_logo;
            this.pb_TaiChinh.Location = new System.Drawing.Point(795, 20);
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
            this.btnPush.Location = new System.Drawing.Point(464, 193);
            this.btnPush.Name = "btnPush";
            this.btnPush.Size = new System.Drawing.Size(159, 38);
            this.btnPush.TabIndex = 107;
            this.btnPush.Text = "Tạo người dùng";
            this.btnPush.UseVisualStyleBackColor = false;
            this.btnPush.Click += new System.EventHandler(this.btnPush_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.AutoSize = true;
            this.btnConvert.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnConvert.FlatAppearance.BorderSize = 0;
            this.btnConvert.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConvert.ForeColor = System.Drawing.Color.White;
            this.btnConvert.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnConvert.Location = new System.Drawing.Point(463, 153);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(159, 37);
            this.btnConvert.TabIndex = 106;
            this.btnConvert.Text = "Chuyển dữ liệu";
            this.btnConvert.UseVisualStyleBackColor = false;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnXml
            // 
            this.btnXml.AutoSize = true;
            this.btnXml.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnXml.FlatAppearance.BorderSize = 0;
            this.btnXml.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnXml.ForeColor = System.Drawing.Color.White;
            this.btnXml.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnXml.Location = new System.Drawing.Point(628, 151);
            this.btnXml.Name = "btnXml";
            this.btnXml.Size = new System.Drawing.Size(120, 38);
            this.btnXml.TabIndex = 105;
            this.btnXml.Text = "Xuất File Xml";
            this.btnXml.UseVisualStyleBackColor = false;
            this.btnXml.Visible = false;
            this.btnXml.Click += new System.EventHandler(this.btnXml_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(403, 39);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 25);
            this.label6.TabIndex = 104;
            this.label6.Text = "Dòng bắt đầu";
            // 
            // btn_ldap
            // 
            this.btn_ldap.Enabled = false;
            this.btn_ldap.Location = new System.Drawing.Point(629, 194);
            this.btn_ldap.Margin = new System.Windows.Forms.Padding(2);
            this.btn_ldap.Name = "btn_ldap";
            this.btn_ldap.Size = new System.Drawing.Size(119, 35);
            this.btn_ldap.TabIndex = 102;
            this.btn_ldap.Text = "Ldap";
            this.btn_ldap.UseVisualStyleBackColor = true;
            this.btn_ldap.Visible = false;
            this.btn_ldap.Click += new System.EventHandler(this.btn_ldap_Click);
            // 
            // btn_api
            // 
            this.btn_api.Enabled = false;
            this.btn_api.Location = new System.Drawing.Point(752, 194);
            this.btn_api.Margin = new System.Windows.Forms.Padding(2);
            this.btn_api.Name = "btn_api";
            this.btn_api.Size = new System.Drawing.Size(121, 35);
            this.btn_api.TabIndex = 100;
            this.btn_api.Text = "API";
            this.btn_api.UseVisualStyleBackColor = true;
            this.btn_api.Visible = false;
            this.btn_api.Click += new System.EventHandler(this.btn_api_Click);
            // 
            // txtPatronId
            // 
            this.txtPatronId.Enabled = false;
            this.txtPatronId.Location = new System.Drawing.Point(144, 36);
            this.txtPatronId.Margin = new System.Windows.Forms.Padding(2);
            this.txtPatronId.Name = "txtPatronId";
            this.txtPatronId.Size = new System.Drawing.Size(252, 33);
            this.txtPatronId.TabIndex = 99;
            this.txtPatronId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPatronId_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 36);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 25);
            this.label4.TabIndex = 98;
            this.label4.Text = "Patron Id";
            // 
            // txtLine
            // 
            this.txtLine.Location = new System.Drawing.Point(544, 33);
            this.txtLine.Name = "txtLine";
            this.txtLine.Size = new System.Drawing.Size(76, 33);
            this.txtLine.TabIndex = 103;
            this.txtLine.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtLine_KeyPress);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 158);
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
            this.comboBox1.Location = new System.Drawing.Point(144, 155);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(316, 33);
            this.comboBox1.TabIndex = 92;
            // 
            // textBox3
            // 
            this.textBox3.Enabled = false;
            this.textBox3.Location = new System.Drawing.Point(721, 33);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(45, 33);
            this.textBox3.TabIndex = 96;
            this.textBox3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextBox3_KeyPress);
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(144, 116);
            this.textBox2.Margin = new System.Windows.Forms.Padding(2);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(316, 33);
            this.textBox2.TabIndex = 93;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 116);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 25);
            this.label2.TabIndex = 91;
            this.label2.Text = "Thư mục lưu";
            // 
            // btnBrowserFile
            // 
            this.btnBrowserFile.Location = new System.Drawing.Point(464, 76);
            this.btnBrowserFile.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowserFile.Name = "btnBrowserFile";
            this.btnBrowserFile.Size = new System.Drawing.Size(159, 33);
            this.btnBrowserFile.TabIndex = 90;
            this.btnBrowserFile.Text = "Chọn...";
            this.btnBrowserFile.UseVisualStyleBackColor = true;
            this.btnBrowserFile.Click += new System.EventHandler(this.btnBrowserFile_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(144, 76);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(316, 33);
            this.textBox1.TabIndex = 89;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 79);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 25);
            this.label1.TabIndex = 88;
            this.label1.Text = "Chọn tệp tin";
            // 
            // btnGetData
            // 
            this.btnGetData.Location = new System.Drawing.Point(464, 115);
            this.btnGetData.Margin = new System.Windows.Forms.Padding(2);
            this.btnGetData.Name = "btnGetData";
            this.btnGetData.Size = new System.Drawing.Size(159, 33);
            this.btnGetData.TabIndex = 87;
            this.btnGetData.Text = "Chọn...";
            this.btnGetData.UseVisualStyleBackColor = true;
            this.btnGetData.Click += new System.EventHandler(this.btnGetData_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(626, 36);
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
            this.btnThoat.Location = new System.Drawing.Point(878, 193);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(109, 38);
            this.btnThoat.TabIndex = 14;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = false;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvHad);
            this.groupBox2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(629, 246);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(364, 297);
            this.groupBox2.TabIndex = 30;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DANH SÁCH ĐÃ TỒN TẠI";
            // 
            // dgvHad
            // 
            this.dgvHad.AllowUserToAddRows = false;
            this.dgvHad.AllowUserToDeleteRows = false;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvHad.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
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
            this.dgvHad.Size = new System.Drawing.Size(358, 276);
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
            this.dataGridViewTextBoxColumn13.Width = 58;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.DataPropertyName = "chucVu";
            this.dataGridViewTextBoxColumn14.HeaderText = "Chức Vụ";
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            this.dataGridViewTextBoxColumn14.Visible = false;
            this.dataGridViewTextBoxColumn14.Width = 73;
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
            // lbCountListExcel
            // 
            this.lbCountListExcel.AutoSize = true;
            this.lbCountListExcel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCountListExcel.Location = new System.Drawing.Point(3, 542);
            this.lbCountListExcel.Name = "lbCountListExcel";
            this.lbCountListExcel.Size = new System.Drawing.Size(76, 21);
            this.lbCountListExcel.TabIndex = 31;
            this.lbCountListExcel.Text = "Số lượng:";
            // 
            // lbCountHad
            // 
            this.lbCountHad.AutoSize = true;
            this.lbCountHad.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCountHad.Location = new System.Drawing.Point(628, 542);
            this.lbCountHad.Name = "lbCountHad";
            this.lbCountHad.Size = new System.Drawing.Size(76, 21);
            this.lbCountHad.TabIndex = 32;
            this.lbCountHad.Text = "Số lượng:";
            // 
            // UCSinhVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbCountHad);
            this.Controls.Add(this.lbCountListExcel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "UCSinhVien";
            this.Size = new System.Drawing.Size(1000, 565);
            this.Load += new System.EventHandler(this.UCNhanVien_Load);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPatron)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pb_TaiChinh)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHad)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
