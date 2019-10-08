using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TNUE_Patron_Excel.DBConnect;

namespace TNUE_Patron_Excel.Config
{
	public class ConfigDataBase : Form
	{
		private DataOracle oracle = null;

		private IContainer components = null;

		private Button btnTest;

		private TextBox txtHost;

		private Label label4;

		private TextBox txtSid;

		private Label label2;

		private TextBox txtPort;

		private Label label1;

		private TextBox txtUser;

		private Label label3;

		private Button btnOk;

		private TextBox txtPassword;

		private Label label5;

		private Label lbStatus;

		public ConfigDataBase()
		{
			InitializeComponent();
		}

		private void ConfigDataBase_Load(object sender, EventArgs e)
		{
			DataOracle dataOracle = new ReadWriterConfig().ReadConfigDataBase();
			txtHost.Text = dataOracle.host;
			txtPort.Text = dataOracle.port;
			txtSid.Text = dataOracle.sid;
			txtUser.Text = dataOracle.user;
			txtPassword.Text = dataOracle.password;
		}

		private void btnTest_Click(object sender, EventArgs e)
		{
			TestConnecting();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			UpdateConfigDatabase();
		}

		private bool TestConnecting()
		{
			bool result = false;
			try
			{
				DataOracle dataOracle = new DataOracle();
				dataOracle.host = txtHost.Text;
				dataOracle.port = txtPort.Text;
				dataOracle.sid = txtSid.Text;
				dataOracle.user = txtUser.Text;
				dataOracle.password = txtPassword.Text;
				DBConnecting.conn = DBConnecting.GetDBConnection(dataOracle);
				DBConnecting.conn.Open();
				lbStatus.ForeColor = Color.LimeGreen;
				lbStatus.Text = "Successful connection";
				result = true;
				DBConnecting.conn.Close();
                MessageBox.Show("Successful connection", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
			catch (Exception ex)
			{
				DBConnecting.conn.Close();
				lbStatus.ForeColor = Color.Red;
				lbStatus.Text = "Connection failed";
				MessageBox.Show("Lỗi: " + ex.Message, "Thông báo!");
			}
			return result;
		}

		private void txtPort_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void txtSid_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private void UpdateConfigDatabase()
		{
			List<string> list = new List<string>();
			using (StreamReader streamReader = new StreamReader("ConfigConnect"))
			{
				string item;
				while ((item = streamReader.ReadLine()) != null)
				{
					list.Add(item);
				}
			}
			using (StreamWriter streamWriter = new StreamWriter("ConfigConnect"))
			{
				oracle = new DataOracle();
				oracle.host = txtHost.Text;
				oracle.port = txtPort.Text;
				oracle.sid = txtSid.Text;
				oracle.user = txtUser.Text;
				oracle.password = txtPassword.Text;
				foreach (string item2 in list)
				{
					if (!item2.Contains("="))
					{
						streamWriter.WriteLine(item2);
					}
					else
					{
						string text = item2.Substring(0, item2.IndexOf("="));
						switch (text.ToLower())
						{
						case "host":
							streamWriter.WriteLine("host=" + oracle.host);
							break;
						case "port":
							streamWriter.WriteLine("port=" + oracle.port);
							break;
						case "sid":
							streamWriter.WriteLine("sid=" + oracle.sid);
							break;
						case "user":
							streamWriter.WriteLine("user=" + oracle.user);
							break;
						case "password":
							streamWriter.WriteLine("password=" + oracle.password);
							break;
						default:
							streamWriter.WriteLine(item2);
							break;
						}
					}
				}
			}
			MessageBox.Show("Lưu thành công");
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigDataBase));
            this.btnTest = new System.Windows.Forms.Button();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSid = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lbStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnTest
            // 
            this.btnTest.AutoSize = true;
            this.btnTest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.Color.White;
            this.btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTest.Location = new System.Drawing.Point(96, 357);
            this.btnTest.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(146, 45);
            this.btnTest.TabIndex = 123;
            this.btnTest.Text = "Test Connect";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(116, 30);
            this.txtHost.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(358, 41);
            this.txtHost.TabIndex = 120;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 33);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 33);
            this.label4.TabIndex = 119;
            this.label4.Text = "Host";
            // 
            // txtSid
            // 
            this.txtSid.Location = new System.Drawing.Point(116, 145);
            this.txtSid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtSid.Name = "txtSid";
            this.txtSid.Size = new System.Drawing.Size(358, 41);
            this.txtSid.TabIndex = 116;
            this.txtSid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSid_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 148);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 33);
            this.label2.TabIndex = 114;
            this.label2.Text = "Sid";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(116, 87);
            this.txtPort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(358, 41);
            this.txtPort.TabIndex = 112;
            this.txtPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPort_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 90);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 33);
            this.label1.TabIndex = 111;
            this.label1.Text = "Port";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(116, 196);
            this.txtUser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(358, 41);
            this.txtUser.TabIndex = 125;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 199);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 33);
            this.label3.TabIndex = 124;
            this.label3.Text = "User";
            // 
            // btnOk
            // 
            this.btnOk.AutoSize = true;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(254, 357);
            this.btnOk.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(146, 45);
            this.btnOk.TabIndex = 126;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(116, 247);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(358, 41);
            this.txtPassword.TabIndex = 128;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 250);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 33);
            this.label5.TabIndex = 127;
            this.label5.Text = "Password";
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Segoe Print", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbStatus.Location = new System.Drawing.Point(140, 307);
            this.lbStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(222, 33);
            this.lbStatus.TabIndex = 129;
            this.lbStatus.Text = "Successful connection";
            // 
            // ConfigDataBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 33F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 416);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtHost);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe Print", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "ConfigDataBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConfigDataBase";
            this.Load += new System.EventHandler(this.ConfigDataBase_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
