using System.IO;
using TNUE_Patron_Excel.API;
using TNUE_Patron_Excel.Ldap;

namespace TNUE_Patron_Excel.DBConnect
{
	internal class ReadWriterConfig
	{
		public DataOracle oracle = null;

		public Aleph aleph = null;

		public LdapField ldap = null;

		public string ReadConfig()
		{
			string[] array = File.ReadAllLines("ConfigConnect");
			return array[0].Trim();
		}

		public DataOracle ReadConfigDataBase()
		{
			using (StreamReader streamReader = new StreamReader("ConfigConnect"))
			{
				oracle = new DataOracle();
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					if (text.Contains("="))
					{
						string text2 = text.Substring(0, text.IndexOf("="));
						switch (text2.ToLower())
						{
						case "host":
							oracle.host = getField(text);
							break;
						case "port":
							oracle.port = getField(text);
							break;
						case "sid":
							oracle.sid = getField(text);
							break;
						case "user":
							oracle.user = getField(text);
							break;
						case "password":
							oracle.password = getField(text);
							break;
						}
					}
				}
			}
			return oracle;
		}

		public Aleph ReadConfigAleph()
		{
			using (StreamReader streamReader = new StreamReader("ConfigConnect"))
			{
				aleph = new Aleph();
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					if (text.Contains("="))
					{
						string text2 = text.Substring(0, text.IndexOf("="));
						switch (text2.ToLower())
						{
						case "urlaleph":
							aleph.UrlAleph = getField(text);
							break;
						case "library":
							aleph.Library = getField(text);
							break;
						case "updateflag":
							aleph.UpdateFlag = getField(text);
							break;
						}
					}
				}
			}
			return aleph;
		}

		public LdapField ReadConfigLdap()
		{
			using (StreamReader streamReader = new StreamReader("ConfigConnect"))
			{
				ldap = new LdapField();
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					if (text.Contains("="))
					{
						string text2 = text.Substring(0, text.IndexOf("="));
						switch (text2.ToLower())
						{
						case "urlldap":
							ldap.UrlLdap = getField(text);
							break;
						case "bindldap":
							ldap.BindLdap = getField(text);
							break;
						case "binddn":
							ldap.BindDn = getField(text);
							break;
						case "bindcredential":
							ldap.BindCredential = getField(text);
							break;
						}
					}
				}
			}
			return ldap;
		}

		public ReadWriterConfig()
		{
			if (!File.Exists("ConfigConnect"))
			{
				using (StreamWriter streamWriter = new StreamWriter("ConfigConnect"))
				{
					streamWriter.WriteLine("#DataBase");
					streamWriter.WriteLine("host=10.2.201.15");
					streamWriter.WriteLine("port=1521");
					streamWriter.WriteLine("sid=aleph23");
					streamWriter.WriteLine("user=tedadmin");
					streamWriter.WriteLine("password=tedprodba9102");
					streamWriter.WriteLine("#Aleph");
					streamWriter.WriteLine("UrlAleph=http://catalog.dhsptn.edu.vn");
					streamWriter.WriteLine("Library=library=lsp50");
					streamWriter.WriteLine("UpdateFlag=update_flag=Y");
					streamWriter.WriteLine("#Ldap");
					streamWriter.WriteLine("UrlLdap=LDAP://10.2.201.18:389");
					streamWriter.WriteLine("BindLdap=ou=People,dc=sso,dc=dhsptn,dc=edu,dc=vn");
					streamWriter.WriteLine("BindDn=cn=Manager,dc=sso,dc=dhsptn,dc=edu,dc=vn");
					streamWriter.WriteLine("BindCredential=secret");
				}
			}
		}

		public string getField(string str)
		{
			if (str.Contains("="))
			{
				int num = str.IndexOf("=");
				str = str.Substring(num + 1, str.Length - (num + 1));
			}
			return str.Trim();
		}
	}
}


