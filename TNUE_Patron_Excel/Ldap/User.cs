using System.ComponentModel;

namespace TNUE_Patron_Excel.Ldap
{
	internal class User
	{
        [DisplayName("Tài khoản")]
		public string userLogin
		{
			get;
			set;
		}

		public string userPassword
		{
			get;
			set;
		}
        [DisplayName("Email")]
        public string userMail
		{
			get;
			set;
		}

		public string objectClass
		{
			get;
			set;
		}
        [DisplayName("Số điện thoại")]
        public string telephoneNumber
		{
			get;
			set;
		}

		public string cn
		{
			get;
			set;
		}

		public string sn
		{
			get;
			set;
		}
	}
}
