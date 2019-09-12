using System.Text;

namespace TNUE_Patron_Excel.Z303
{
	internal class z304
	{
		public string tab4(Patron p)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<z304>");
			stringBuilder.Append("<record-action>A</record-action>");
			stringBuilder.Append("<email-address>" + p.email + "</email-address>");
			stringBuilder.Append("<z304-id>" + p.pationID + "</z304-id>");
			stringBuilder.Append("<z304-sequence>01</z304-sequence>");
			stringBuilder.Append("<z304-address-0>" + p.GT + " " + p.HoTen + "</z304-address-0>");
			stringBuilder.Append("<z304-address-1>" + p.DiaChi + "</z304-address-1>");
			stringBuilder.Append("<z304-address-2>" + p.QuocTich + "</z304-address-2>");
			stringBuilder.Append("<z304-zip></z304-zip>");
			stringBuilder.Append("<z304-email-address>" + p.email + "</z304-email-address>");
			stringBuilder.Append("<z304-telephone>" + p.phone + "</z304-telephone>");
			stringBuilder.Append("<z304-date-from>" + p.Day + "</z304-date-from>");
			stringBuilder.Append("<z304-date-to>" + p.ngayHetHan + "</z304-date-to>");
			stringBuilder.Append("<z304-address-type>01</z304-address-type>");
			stringBuilder.Append("<z304-telephone-2></z304-telephone-2>");
			stringBuilder.Append("<z304-telephone-3></z304-telephone-3>");
			stringBuilder.Append("<z304-telephone-4></z304-telephone-4>");
			stringBuilder.Append("<z304-sms-number></z304-sms-number>");
			stringBuilder.Append("<z304-update-date>" + p.Day + "</z304-update-date>");
			stringBuilder.Append("<z304-cat-name>MASTER</z304-cat-name>");
			stringBuilder.Append("</z304>");
			return stringBuilder.ToString();
		}
	}
}
