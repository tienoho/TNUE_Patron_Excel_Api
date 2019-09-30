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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigLdap));
            this.btnTest = new System.Windows.Forms.Button();
            this.txtLdap = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtBindDn = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBindLdap = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.lbStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBindCredential = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
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
            this.btnTest.Location = new System.Drawing.Point(90, 335);
            this.btnTest.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(146, 45);
            this.btnTest.TabIndex = 123;
            this.btnTest.Text = "Test Connect";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // txtLdap
            // 
            this.txtLdap.Location = new System.Drawing.Point(137, 52);
            this.txtLdap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtLdap.Name = "txtLdap";
            this.txtLdap.Size = new System.Drawing.Size(329, 41);
            this.txtLdap.TabIndex = 120;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 55);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 33);
            this.label4.TabIndex = 119;
            this.label4.Text = "Url Ldap";
            // 
            // txtBindDn
            // 
            this.txtBindDn.Location = new System.Drawing.Point(137, 167);
            this.txtBindDn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBindDn.Name = "txtBindDn";
            this.txtBindDn.Size = new System.Drawing.Size(329, 41);
            this.txtBindDn.TabIndex = 116;
            this.txtBindDn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSid_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 170);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 33);
            this.label2.TabIndex = 114;
            this.label2.Text = "BindDn";
            // 
            // txtBindLdap
            // 
            this.txtBindLdap.Location = new System.Drawing.Point(137, 109);
            this.txtBindLdap.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBindLdap.Name = "txtBindLdap";
            this.txtBindLdap.Size = new System.Drawing.Size(329, 41);
            this.txtBindLdap.TabIndex = 112;
            this.txtBindLdap.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPort_KeyPress);
            // 
            // btnOk
            // 
            this.btnOk.AutoSize = true;
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(216)))));
            this.btnOk.FlatAppearance.BorderSize = 0;
            this.btnOk.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.White;
            this.btnOk.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOk.Location = new System.Drawing.Point(248, 335);
            this.btnOk.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(146, 45);
            this.btnOk.TabIndex = 126;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Font = new System.Drawing.Font("Segoe Print", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbStatus.ForeColor = System.Drawing.Color.LimeGreen;
            this.lbStatus.Location = new System.Drawing.Point(151, 284);
            this.lbStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(222, 33);
            this.lbStatus.TabIndex = 129;
            this.lbStatus.Text = "Successful connection";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 112);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 33);
            this.label1.TabIndex = 111;
            this.label1.Text = "BindLdap";
            // 
            // txtBindCredential
            // 
            this.txtBindCredential.Location = new System.Drawing.Point(137, 222);
            this.txtBindCredential.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtBindCredential.Name = "txtBindCredential";
            this.txtBindCredential.Size = new System.Drawing.Size(329, 41);
            this.txtBindCredential.TabIndex = 131;
            this.txtBindCredential.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 225);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 33);
            this.label3.TabIndex = 130;
            this.label3.Text = "Password";
            // 
            // ConfigLdap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 33F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(482, 416);
            this.Controls.Add(this.txtBindCredential);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbStatus);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.txtLdap);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtBindDn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtBindLdap);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe Print", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 8, 6, 8);
            this.Name = "ConfigLdap";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConfigAleph";
            this.Load += new System.EventHandler(this.ConfigLdap_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
	}
}
