using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TNUE_Patron_Excel.API;
using TNUE_Patron_Excel.Config;
using TNUE_Patron_Excel.DBConnect;
using TNUE_Patron_Excel.Ldap;
using TNUE_Patron_Excel.Tool;

namespace TNUE_Patron_Excel
{
    public class FormStartLoading : Form
    {
        private Thread threadInput = null;
        private IContainer components = null;
        private PictureBox picLoader;
        private PictureBox pictureError;
        private Label labelError;
        private Button btnExit;
        private BackgroundWorker backgroundWorker1;
        private Button btnDB;
        private Button btnLdap;
        private Button btnAleph;
        private Label lbLoad;
        Aleph aleph = null;
        LdapField ldapField = null;
        bool validateDatabase = false;
        bool validateUrlAleph = false;
        bool validateLdap = false;
        public FormStartLoading()
        {
            InitializeComponent();

            backgroundWorker1.RunWorkerAsync();
            backgroundWorker1.WorkerSupportsCancellation = true;

            labelError.Visible = false;
            labelError.Parent = pictureError;
            labelError.BackColor = Color.Transparent;
            lbLoad.Visible = false;
            lbLoad.Parent = picLoader;
            lbLoad.BackColor = Color.Transparent;
        }

        public void Startaaa()
        {
            if (!TestConnecting() || !ValidateExistUrl() || !ValidatePingHostLdap())
            {
                
                Invoke((MethodInvoker)delegate
                {
                    pictureError.Visible = true;
                    labelError.Visible = true;
                    btnAleph.Visible = true;
                    btnDB.Visible = true;
                    btnLdap.Visible = true;
                    Cursor = Cursors.Default;

                    if (!validateDatabase)
                    {
                        MessageBox.Show("Lỗi: Không kết nối được đến Database Oracle", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!validateUrlAleph)
                    {
                        MessageBox.Show("Lỗi: Không kết nối được đến địa chỉ: " + aleph.UrlAleph, "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (!validateLdap)
                    {
                        MessageBox.Show("Lỗi: Không kết nối được đến server Ldap: " + ldapField.UrlLdap, "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                });

            }
            else
            {
                DataDBLocal.listZ308 = new QueryDB().listZ308TED();
                Invoke((MethodInvoker)delegate
                {
                    SetLoading(displayLoader: false);
                    Hide();
                    Control control = new Control();
                    control.EnabledPanl(bl: true);
                    control.LoadForm();
                    control.Show();
                });
            }
        }

        private void SetLoading(bool displayLoader)
        {
            if (displayLoader)
            {
                Invoke((MethodInvoker)delegate
                {
                    picLoader.Visible = true;
                    lbLoad.Visible = true;
                    Cursor = Cursors.WaitCursor;
                });
            }
            else
            {
                Invoke((MethodInvoker)delegate
                {
                    picLoader.Visible = false;
                    Cursor = Cursors.Default;
                });
            }
        }

        private bool TestConnecting()
        {
            bool result = false;
            try
            {
                SetLoading(displayLoader: true);
                Thread.Sleep(3000);
                DataOracle oracle = new ReadWriterConfig().ReadConfigDataBase();
                DBConnecting.conn = DBConnecting.GetDBConnection(oracle);
                DBConnecting.conn.Open();
                result = true;
                DBConnecting.conn.Close();
            }
            catch
            {

            }
            return validateDatabase = result;
        }
        private bool ValidateExistUrl()
        {
            aleph = new ReadWriterConfig().ReadConfigAleph();
            return validateUrlAleph = new CheckUrl().CheckUrlExist(aleph.UrlAleph);
        }
        private bool ValidatePingHostLdap()
        {
            ldapField = new ReadWriterConfig().ReadConfigLdap();
            int numberStart = ldapField.UrlLdap.IndexOf("LDAP://") + 7;
            int numberEnd = ldapField.UrlLdap.LastIndexOf(":");
            string hostUri = ldapField.UrlLdap.Substring(numberStart, numberEnd - numberStart);
            int portNumber = int.Parse(ldapField.UrlLdap.Substring(numberEnd + 1));
            return validateLdap = new CheckUrl().PingHost(hostUri, portNumber);
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            threadInput.Abort();
            Application.Exit();
        }
        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            threadInput = new Thread(Startaaa);
            threadInput.Start();
        }

        private void BtnDB_Click(object sender, EventArgs e)
        {
            FormProvider.sConfigDataBase.ShowDialog();
        }

        private void BtnLdap_Click(object sender, EventArgs e)
        {
            FormProvider.sConfigLdap.ShowDialog();
        }

        private void BtnAleph_Click(object sender, EventArgs e)
        {
            FormProvider.sConfigAleph.ShowDialog();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStartLoading));
            this.pictureError = new System.Windows.Forms.PictureBox();
            this.picLoader = new System.Windows.Forms.PictureBox();
            this.labelError = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.lbLoad = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.btnDB = new System.Windows.Forms.Button();
            this.btnLdap = new System.Windows.Forms.Button();
            this.btnAleph = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoader)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureError
            // 
            this.pictureError.BackColor = System.Drawing.Color.Transparent;
            this.pictureError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureError.Image = global::TNUE_Patron_Excel.Properties.Resources.tweek_error;
            this.pictureError.Location = new System.Drawing.Point(0, 0);
            this.pictureError.Name = "pictureError";
            this.pictureError.Size = new System.Drawing.Size(800, 418);
            this.pictureError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureError.TabIndex = 10;
            this.pictureError.TabStop = false;
            this.pictureError.Visible = false;
            // 
            // picLoader
            // 
            this.picLoader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picLoader.Image = global::TNUE_Patron_Excel.Properties.Resources.Simple_Loader;
            this.picLoader.Location = new System.Drawing.Point(0, 0);
            this.picLoader.Name = "picLoader";
            this.picLoader.Size = new System.Drawing.Size(800, 418);
            this.picLoader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picLoader.TabIndex = 9;
            this.picLoader.TabStop = false;
            this.picLoader.Visible = false;
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.BackColor = System.Drawing.Color.Transparent;
            this.labelError.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelError.ForeColor = System.Drawing.Color.Red;
            this.labelError.Location = new System.Drawing.Point(515, 175);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(174, 29);
            this.labelError.TabIndex = 11;
            this.labelError.Text = "Lỗi kết nối rồi!!!";
            // 
            // btnExit
            // 
            this.btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExit.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.Red;
            this.btnExit.Location = new System.Drawing.Point(759, 0);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(41, 29);
            this.btnExit.TabIndex = 12;
            this.btnExit.Text = "X";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // lbLoad
            // 
            this.lbLoad.AutoSize = true;
            this.lbLoad.BackColor = System.Drawing.Color.Transparent;
            this.lbLoad.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLoad.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbLoad.Location = new System.Drawing.Point(301, 374);
            this.lbLoad.Name = "lbLoad";
            this.lbLoad.Size = new System.Drawing.Size(200, 26);
            this.lbLoad.TabIndex = 13;
            this.lbLoad.Text = "Đang tải dữ liệu ...";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            // 
            // btnDB
            // 
            this.btnDB.Image = global::TNUE_Patron_Excel.Properties.Resources.database;
            this.btnDB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDB.Location = new System.Drawing.Point(650, 256);
            this.btnDB.Name = "btnDB";
            this.btnDB.Size = new System.Drawing.Size(150, 52);
            this.btnDB.TabIndex = 14;
            this.btnDB.Text = "Config  Database";
            this.btnDB.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnDB.UseVisualStyleBackColor = true;
            this.btnDB.Visible = false;
            this.btnDB.Click += new System.EventHandler(this.BtnDB_Click);
            // 
            // btnLdap
            // 
            this.btnLdap.Image = global::TNUE_Patron_Excel.Properties.Resources.simpleid_icon_adapt;
            this.btnLdap.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLdap.Location = new System.Drawing.Point(650, 311);
            this.btnLdap.Name = "btnLdap";
            this.btnLdap.Size = new System.Drawing.Size(150, 51);
            this.btnLdap.TabIndex = 15;
            this.btnLdap.Text = "Config  Ldap";
            this.btnLdap.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLdap.UseVisualStyleBackColor = true;
            this.btnLdap.Visible = false;
            this.btnLdap.Click += new System.EventHandler(this.BtnLdap_Click);
            // 
            // btnAleph
            // 
            this.btnAleph.Image = global::TNUE_Patron_Excel.Properties.Resources.mindtouch;
            this.btnAleph.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAleph.Location = new System.Drawing.Point(650, 365);
            this.btnAleph.Name = "btnAleph";
            this.btnAleph.Size = new System.Drawing.Size(150, 51);
            this.btnAleph.TabIndex = 16;
            this.btnAleph.Text = "Config  Aleph";
            this.btnAleph.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAleph.UseVisualStyleBackColor = true;
            this.btnAleph.Visible = false;
            this.btnAleph.Click += new System.EventHandler(this.BtnAleph_Click);
            // 
            // FormStartLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 418);
            this.Controls.Add(this.btnAleph);
            this.Controls.Add(this.btnLdap);
            this.Controls.Add(this.btnDB);
            this.Controls.Add(this.lbLoad);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.pictureError);
            this.Controls.Add(this.picLoader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormStartLoading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Start Loading Upload Patron";
            ((System.ComponentModel.ISupportInitialize)(this.pictureError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picLoader)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


    }
}
