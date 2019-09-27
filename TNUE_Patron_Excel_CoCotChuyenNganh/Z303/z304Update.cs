using System;
using System.Text;
using TNUE_Patron_Excel.Ldap;
using TNUE_Patron_Excel.Tool;

namespace TNUE_Patron_Excel.Z303
{
	internal class z304Update
	{
		public string tab4(string patronId, User user)
		{
			ToolP toolP = new ToolP();
			string str = toolP.formatDate(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")).ToString());
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<z304>");
			stringBuilder.Append("<record-action>A</record-action>");
			stringBuilder.Append("<email-address>" + user.userMail + "</email-address>");
			stringBuilder.Append("<z304-id>" + patronId + "</z304-id>");
			stringBuilder.Append("<z304-sequence>01</z304-sequence>");
			stringBuilder.Append("<z304-email-address>" + user.userMail + "</z304-email-address>");
			stringBuilder.Append("<z304-telephone>" + user.telephoneNumber + "</z304-telephone>");
			stringBuilder.Append("<z304-address-type>01</z304-address-type>");
			stringBuilder.Append("<z304-update-date>" + str + "</z304-update-date>");
			stringBuilder.Append("</z304>");
			return stringBuilder.ToString();
		}
	}
}
