using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using TNUE_Patron_Excel.DBConnect;

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

        private Label lbLoad;

        public FormStartLoading()
        {
            InitializeComponent();
            threadInput = new Thread(Startaaa);
            threadInput.Start();
            labelError.Visible = false;
            labelError.Parent = pictureError;
            labelError.BackColor = Color.Transparent;
            lbLoad.Visible = false;
            lbLoad.Parent = picLoader;
            lbLoad.BackColor = Color.Transparent;
        }

        public void Startaaa()
        {
            if (!TestConnecting())
            {
                Invoke((MethodInvoker)delegate
                {
                    pictureError.Visible = true;
                    labelError.Visible = true;
                    Cursor = Cursors.Default;
                });
            }
            else
            {
                Invoke((MethodInvoker)delegate
                {
                    DataDBLocal.listZ308 = new QueryDB().listZ308TED();
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
                Thread.Sleep(6000);
                DataOracle oracle = new ReadWriterConfig().ReadConfigDataBase();
                DBConnecting.conn = DBConnecting.GetDBConnection(oracle);
                DBConnecting.conn.Open();
                result = true;
                DBConnecting.conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Thông báo!");
            }
            return result;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            threadInput.Abort();
            Application.Exit();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TNUE_Patron_Excel.FormStartLoading));
            pictureError = new System.Windows.Forms.PictureBox();
            picLoader = new System.Windows.Forms.PictureBox();
            labelError = new System.Windows.Forms.Label();
            btnExit = new System.Windows.Forms.Button();
            lbLoad = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)pictureError).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picLoader).BeginInit();
            SuspendLayout();
            pictureError.BackColor = System.Drawing.Color.Transparent;
            pictureError.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureError.Image = TNUE_Patron_Excel.Properties.Resources.tweek_error;
            pictureError.Location = new System.Drawing.Point(0, 0);
            pictureError.Name = "pictureError";
            pictureError.Size = new System.Drawing.Size(800, 450);
            pictureError.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            pictureError.TabIndex = 10;
            pictureError.TabStop = false;
            pictureError.Visible = false;
            picLoader.Dock = System.Windows.Forms.DockStyle.Fill;
            picLoader.Image = TNUE_Patron_Excel.Properties.Resources.Simple_Loader;
            picLoader.Location = new System.Drawing.Point(0, 0);
            picLoader.Name = "picLoader";
            picLoader.Size = new System.Drawing.Size(800, 450);
            picLoader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            picLoader.TabIndex = 9;
            picLoader.TabStop = false;
            picLoader.Visible = false;
            labelError.AutoSize = true;
            labelError.BackColor = System.Drawing.Color.Transparent;
            labelError.Font = new System.Drawing.Font("Arial Narrow", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            labelError.ForeColor = System.Drawing.Color.Red;
            labelError.Location = new System.Drawing.Point(515, 188);
            labelError.Name = "labelError";
            labelError.Size = new System.Drawing.Size(174, 29);
            labelError.TabIndex = 11;
            labelError.Text = "Lỗi kết nối rồi!!!";
            btnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            btnExit.Font = new System.Drawing.Font("Arial", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            btnExit.ForeColor = System.Drawing.Color.Red;
            btnExit.Location = new System.Drawing.Point(759, 0);
            btnExit.Name = "btnExit";
            btnExit.Size = new System.Drawing.Size(41, 31);
            btnExit.TabIndex = 12;
            btnExit.Text = "X";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += new System.EventHandler(BtnExit_Click);
            lbLoad.AutoSize = true;
            lbLoad.BackColor = System.Drawing.Color.Transparent;
            lbLoad.Font = new System.Drawing.Font("Times New Roman", 18f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            lbLoad.ForeColor = System.Drawing.Color.LimeGreen;
            lbLoad.Location = new System.Drawing.Point(305, 403);
            lbLoad.Name = "lbLoad";
            lbLoad.Size = new System.Drawing.Size(200, 26);
            lbLoad.TabIndex = 13;
            lbLoad.Text = "Đang tải dữ liệu ...";
            base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 14f);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            base.ClientSize = new System.Drawing.Size(800, 450);
            base.Controls.Add(lbLoad);
            base.Controls.Add(btnExit);
            base.Controls.Add(labelError);
            base.Controls.Add(pictureError);
            base.Controls.Add(picLoader);
            base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            base.Name = "FormStartLoading";
            base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "FormStartLoading";
            ((System.ComponentModel.ISupportInitialize)pictureError).EndInit();
            ((System.ComponentModel.ISupportInitialize)picLoader).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
