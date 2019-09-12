using System.DirectoryServices;
using TNUE_Patron_Excel.DBConnect;

namespace TNUE_Patron_Excel.Ldap
{
	internal class ConectLdap
	{
		private string userName = "cn=Manager,dc=sso,dc=dhsptn,dc=edu,dc=vn";

		private string password = "secret";

		private LdapField ldap = null;

		private string ldapfilter = "(&(objectclass=person)(cn={0}))";

		public DirectoryEntry entry = null;

		public ConectLdap()
		{
			ldap = new ReadWriterConfig().ReadConfigLdap();
			string path = ldap.UrlLdap + "/" + ldap.BindLdap;
			entry = new DirectoryEntry(path, ldap.BindDn, ldap.BindCredential, AuthenticationTypes.FastBind);
		}

		public bool Connect()
		{
			bool result = true;
			try
			{
				ldap = new ReadWriterConfig().ReadConfigLdap();
				string path = ldap.UrlLdap + "/" + ldap.BindLdap;
				entry = new DirectoryEntry(path, ldap.BindDn, ldap.BindCredential, AuthenticationTypes.FastBind);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool Connect(LdapField ldap)
		{
			bool result = false;
			try
			{
				string path = ldap.UrlLdap + "/" + ldap.BindLdap;
				entry = new DirectoryEntry(path, ldap.BindDn, ldap.BindCredential, AuthenticationTypes.FastBind);
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool checkLdapServer(LdapField ldap)
		{
			bool result = false;
			try
			{
				string path = ldap.UrlLdap + "/" + ldap.BindLdap;
				DirectoryEntry directoryEntry = new DirectoryEntry(path, userName, password, AuthenticationTypes.FastBind);
				object nativeObject = directoryEntry.NativeObject;
				result = true;
			}
			catch (DirectoryServicesCOMException)
			{
			}
			return result;
		}
	}
}
