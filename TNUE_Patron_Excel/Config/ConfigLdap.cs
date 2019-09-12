using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using TNUE_Patron_Excel.DBConnect;
using TNUE_Patron_Excel.Ldap;

namespace TNUE_Patron_Excel.Config
{
	public class ConfigLdap : Form
	{
		private LdapField ldap = null;

		private IContainer components = null;

		private Button btnTest;

		private TextBox txtLdap;

		private Label label4;

		private TextBox txtBindDn;

		private Label label2;

		private TextBox txtBindLdap;

		private Button btnOk;

		private Label lbStatus;

		private Label label1;

		private TextBox txtBindCredential;

		private Label label3;

		public ConfigLdap()
		{
			InitializeComponent();
		}

		private void ConfigLdap_Load(object sender, EventArgs e)
		{
			LdapField ldapField = new ReadWriterConfig().ReadConfigLdap();
			txtLdap.Text = ldapField.UrlLdap;
			txtBindLdap.Text = ldapField.BindLdap;
			txtBindDn.Text = ldapField.BindDn;
			txtBindCredential.Text = ldapField.BindCredential;
		}

		private void btnTest_Click(object sender, EventArgs e)
		{
			if (TestConnecting())
			{
				lbStatus.ForeColor = Color.LimeGreen;
				lbStatus.Text = "Successful connection";
			}
			else
			{
				lbStatus.ForeColor = Color.Red;
				lbStatus.Text = "Connection failed";
				MessageBox.Show("Lỗi: Không thể kết nối đến địa chỉ của Aleph ", "Thông báo!");
			}
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
				LdapField ldapField = new LdapField();
				ldapField.UrlLdap = txtLdap.Text;
				ldapField.BindLdap = txtBindLdap.Text;
				ldapField.BindDn = txtBindDn.Text;
				ldapField.BindCredential = txtBindCredential.Text;
				result = new ConectLdap().checkLdapServer(ldapField);
			}
			catch (Exception ex)
			{
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
				ldap = new LdapField();
				ldap.UrlLdap = txtLdap.Text;
				ldap.BindLdap = txtBindLdap.Text;
				ldap.BindDn = txtBindDn.Text;
				ldap.BindCredential = txtBindCredential.Text;
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
						case "urlldap":
							streamWriter.WriteLine("UrlLdap=" + ldap.UrlLdap);
							break;
						case "bindldap":
							streamWriter.WriteLine("BindLdap=" + ldap.BindLdap);
							break;
						case "binddn":
							streamWriter.WriteLine("BindDn=" + ldap.BindDn);
							break;
						case "bindcredential":
							streamWriter.WriteLine("BindCredential=" + ldap.BindCredential);
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TNUE_Patron_Excel.Config.ConfigLdap));
			btnTest = new System.Windows.Forms.Button();
			txtLdap = new System.Windows.Forms.TextBox();
			label4 = new System.Windows.Forms.Label();
			txtBindDn = new System.Windows.Forms.TextBox();
			label2 = new System.Windows.Forms.Label();
			txtBindLdap = new System.Windows.Forms.TextBox();
			btnOk = new System.Windows.Forms.Button();
			lbStatus = new System.Windows.Forms.Label();
			label1 = new System.Windows.Forms.Label();
			txtBindCredential = new System.Windows.Forms.TextBox();
			label3 = new System.Windows.Forms.Label();
			SuspendLayout();
			btnTest.AutoSize = true;
			btnTest.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnTest.FlatAppearance.BorderSize = 0;
			btnTest.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnTest.ForeColor = System.Drawing.Color.White;
			btnTest.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnTest.Location = new System.Drawing.Point(90, 335);
			btnTest.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
			btnTest.Name = "btnTest";
			btnTest.Size = new System.Drawing.Size(146, 45);
			btnTest.TabIndex = 123;
			btnTest.Text = "Test Connect";
			btnTest.UseVisualStyleBackColor = false;
			btnTest.Click += new System.EventHandler(btnTest_Click);
			txtLdap.Location = new System.Drawing.Point(137, 52);
			txtLdap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			txtLdap.Name = "txtLdap";
			txtLdap.Size = new System.Drawing.Size(329, 41);
			txtLdap.TabIndex = 120;
			label4.AutoSize = true;
			label4.Location = new System.Drawing.Point(5, 55);
			label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label4.Name = "label4";
			label4.Size = new System.Drawing.Size(101, 33);
			label4.TabIndex = 119;
			label4.Text = "Url Ldap";
			txtBindDn.Location = new System.Drawing.Point(137, 167);
			txtBindDn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			txtBindDn.Name = "txtBindDn";
			txtBindDn.Size = new System.Drawing.Size(329, 41);
			txtBindDn.TabIndex = 116;
			txtBindDn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSid_KeyPress);
			label2.AutoSize = true;
			label2.Location = new System.Drawing.Point(5, 170);
			label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(89, 33);
			label2.TabIndex = 114;
			label2.Text = "BindDn";
			txtBindLdap.Location = new System.Drawing.Point(137, 109);
			txtBindLdap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			txtBindLdap.Name = "txtBindLdap";
			txtBindLdap.Size = new System.Drawing.Size(329, 41);
			txtBindLdap.TabIndex = 112;
			txtBindLdap.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPort_KeyPress);
			btnOk.AutoSize = true;
			btnOk.BackColor = System.Drawing.Color.FromArgb(52, 152, 216);
			btnOk.FlatAppearance.BorderSize = 0;
			btnOk.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			btnOk.ForeColor = System.Drawing.Color.White;
			btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			btnOk.Location = new System.Drawing.Point(248, 335);
			btnOk.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
			btnOk.Name = "btnOk";
			btnOk.Size = new System.Drawing.Size(146, 45);
			btnOk.TabIndex = 126;
			btnOk.Text = "Ok";
			btnOk.UseVisualStyleBackColor = false;
			btnOk.Click += new System.EventHandler(btnOk_Click);
			lbStatus.AutoSize = true;
			lbStatus.Font = new System.Drawing.Font("Segoe Print", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			lbStatus.ForeColor = System.Drawing.Color.LimeGreen;
			lbStatus.Location = new System.Drawing.Point(151, 284);
			lbStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			lbStatus.Name = "lbStatus";
			lbStatus.Size = new System.Drawing.Size(222, 33);
			lbStatus.TabIndex = 129;
			lbStatus.Text = "Successful connection";
			label1.AutoSize = true;
			label1.Location = new System.Drawing.Point(10, 112);
			label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(109, 33);
			label1.TabIndex = 111;
			label1.Text = "BindLdap";
			txtBindCredential.Location = new System.Drawing.Point(137, 222);
			txtBindCredential.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			txtBindCredential.Name = "txtBindCredential";
			txtBindCredential.Size = new System.Drawing.Size(329, 41);
			txtBindCredential.TabIndex = 131;
			label3.AutoSize = true;
			label3.Location = new System.Drawing.Point(5, 225);
			label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			label3.Name = "label3";
			label3.Size = new System.Drawing.Size(106, 33);
			label3.TabIndex = 130;
			label3.Text = "Password";
			base.AutoScaleDimensions = new System.Drawing.SizeF(12f, 33f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(482, 416);
			base.Controls.Add(txtBindCredential);
			base.Controls.Add(label3);
			base.Controls.Add(lbStatus);
			base.Controls.Add(btnOk);
			base.Controls.Add(btnTest);
			base.Controls.Add(txtLdap);
			base.Controls.Add(label4);
			base.Controls.Add(txtBindDn);
			base.Controls.Add(label2);
			base.Controls.Add(txtBindLdap);
			base.Controls.Add(label1);
			Font = new System.Drawing.Font("Segoe Print", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			base.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
			base.Name = "ConfigLdap";
			base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			Text = "ConfigAleph";
			base.Load += new System.EventHandler(ConfigLdap_Load);
			ResumeLayout(false);
			PerformLayout();
		}
	}
}
