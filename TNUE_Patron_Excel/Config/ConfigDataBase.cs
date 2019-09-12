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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TNUE_Patron_Excel.Config.ConfigDataBase));
			btnTest = new System.Windows.Forms.Button();
			txtHost = new System.Windows.Forms.TextBox();
			label4 = new System.Windows.Forms.Label();
			txtSid = new System.Windows.Forms.TextBox();
			label2 = new System.Windows.Forms.Label();
			txtPort = new System.Windows.Forms.TextBox();
			label1 = new System.Windows.Forms.Label();
			txtUser = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			btnOk = new System.Windows.Forms.Button();
			txtPassword = new System.Windows.Forms.TextBox();
			label5 = new System.Windows.Forms.Label();
			lbStatus = new System.Windows.Forms.Label();
			SuspendLayout();
			btnTest.AutoSize = true;
			btnTest.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnTest.FlatAppearance.BorderSize = 0;
			btnTest.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnTest.ForeColor = System.Drawing.Color.White;
			btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnTest.Location = new System.Drawing.Point(96, 357);
			btnTest.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
			btnTest.Name = "btnTest";
			btnTest.Size = new System.Drawing.Size(146, 45);
			btnTest.TabIndex = 123;
			btnTest.Text = "Test Connect";
			btnTest.UseVisualStyleBackColor = false;
			btnTest.Click += new System.EventHandler(btnTest_Click);
			txtHost.Location = new System.Drawing.Point(116, 30);
			txtHost.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			txtHost.Name = "txtHost";
			txtHost.Size = new System.Drawing.Size(358, 41);
			txtHost.TabIndex = 120;
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(18, 33);
			label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(58, 33);
			label4.TabIndex = 119;
			label4.Text = "Host";
			txtSid.Location = new System.Drawing.Point(116, 145);
			txtSid.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			txtSid.Name = "txtSid";
			txtSid.Size = new System.Drawing.Size(358, 41);
			txtSid.TabIndex = 116;
			txtSid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSid_KeyPress);
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(10, 148);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(46, 33);
			label2.TabIndex = 114;
			label2.Text = "Sid";
			txtPort.Location = new System.Drawing.Point(116, 87);
			txtPort.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			txtPort.Name = "txtPort";
			txtPort.Size = new System.Drawing.Size(358, 41);
			txtPort.TabIndex = 112;
			txtPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPort_KeyPress);
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(18, 90);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(58, 33);
			label1.TabIndex = 111;
			label1.Text = "Port";
			txtUser.Location = new System.Drawing.Point(116, 196);
			txtUser.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			txtUser.Name = "txtUser";
			txtUser.Size = new System.Drawing.Size(358, 41);
			txtUser.TabIndex = 125;
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(10, 199);
			label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(58, 33);
			label3.TabIndex = 124;
			label3.Text = "User";
			btnOk.AutoSize = true;
			btnOk.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnOk.FlatAppearance.BorderSize = 0;
			btnOk.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnOk.ForeColor = System.Drawing.Color.White;
			btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnOk.Location = new System.Drawing.Point(254, 357);
			btnOk.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
			btnOk.Name = "btnOk";
			btnOk.Size = new System.Drawing.Size(146, 45);
			btnOk.TabIndex = 126;
			btnOk.Text = "Ok";
			btnOk.UseVisualStyleBackColor = false;
			btnOk.Click += new System.EventHandler(btnOk_Click);
			txtPassword.Location = new System.Drawing.Point(116, 247);
			txtPassword.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			txtPassword.Name = "txtPassword";
			txtPassword.Size = new System.Drawing.Size(358, 41);
			txtPassword.TabIndex = 128;
			label5.AutoSize = true;
			label5.Location = new System.Drawing.Point(10, 250);
			label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label5.Name = "label5";
			label5.Size = new System.Drawing.Size(106, 33);
			label5.TabIndex = 127;
			label5.Text = "Password";
			lbStatus.AutoSize = true;
			lbStatus.Font = new System.Drawing.Font("Segoe Print", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lbStatus.ForeColor = System.Drawing.Color.LimeGreen;
			lbStatus.Location = new System.Drawing.Point(140, 307);
			lbStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbStatus.Name = "lbStatus";
			lbStatus.Size = new System.Drawing.Size(222, 33);
			lbStatus.TabIndex = 129;
			lbStatus.Text = "Successful connection";
			base.AutoScaleDimensions = new System.Drawing.SizeF(12f, 33f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(494, 416);
			base.Controls.Add(lbStatus);
			base.Controls.Add(txtPassword);
			base.Controls.Add(label5);
			base.Controls.Add(btnOk);
			base.Controls.Add(txtUser);
			base.Controls.Add(label3);
			base.Controls.Add(btnTest);
			base.Controls.Add(txtHost);
			base.Controls.Add(label4);
			base.Controls.Add(txtSid);
			base.Controls.Add(label2);
			base.Controls.Add(txtPort);
			base.Controls.Add(label1);
			Font = new System.Drawing.Font("Segoe Print", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
			base.Name = "ConfigDataBase";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "ConfigDataBase";
			base.Load += new System.EventHandler(ConfigDataBase_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
