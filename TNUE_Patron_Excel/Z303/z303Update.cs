using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TNUE_Patron_Excel.EntityLdapPatron;
using TNUE_Patron_Excel.Tool;

namespace TNUE_Patron_Excel.Z303
{
	internal class z303Update
    {
		public string tab3(string patronId,LdapPatron p)
		{
            string dateNowUpdate = DateTime.Now.ToString("yyyyMMdd");
            
			string hoTen = p.HoTen;
			string str = "";
			if (hoTen != null && hoTen != "")
			{
				str = addNameKey(p.HoTen) + patronId;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("<z303>");
			stringBuilder.Append("<match-id-type>00</match-id-type>");
			stringBuilder.Append("<match-id>" + patronId + "</match-id>");
			stringBuilder.Append("<record-action>A</record-action>");
			stringBuilder.Append("<z303-id>" + patronId + "</z303-id>");
			stringBuilder.Append("<z303-name-key>" + str + "</z303-name-key>");
			stringBuilder.Append("<z303-update-date>" + dateNowUpdate + "</z303-update-date>");
			stringBuilder.Append("<z303-name>" + hoTen + "</z303-name>");
			stringBuilder.Append("<z303-delinq-1-update-date>" + dateNowUpdate + "</z303-delinq-1-update-date>");
			stringBuilder.Append("<z303-delinq-2-update-date>" + dateNowUpdate + "</z303-delinq-2-update-date>");
			stringBuilder.Append("<z303-delinq-3-update-date>" + dateNowUpdate + "</z303-delinq-3-update-date>");
			stringBuilder.Append("<z303-profile-id>TNUE-LSP</z303-profile-id>");
            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.Lop, "z303-field-1"));
            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.KhoaHoc, "z303-field-2"));
            stringBuilder.Append(new ToolP().WriteStringCheckNull(p.KhoaNganh, "z303-field-3"));
			stringBuilder.Append("</z303>");
			return stringBuilder.ToString();
		}

		private string addNameKey(string name)
		{
			name = name.ToLower();
			name = RemoveVietnameseMark(name);
			do
			{
				name += " ";
			}
			while (name.Count() < 38);
			return name;
		}
		private string RemoveVietnameseMark(string str)
		{
			Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			string input = str.Normalize(NormalizationForm.FormD);
			return regex.Replace(input, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
		}

		public string z303_title(string str, string GT)
		{
			if (str == "" || str == null)
			{
				return GT;
			}
			switch (str.ToUpper().Trim())
			{
			case "GĐ":
				return "GD";
			case "PGĐ":
				return "PGD";
			default:
				return "";
			}
		}
	}
}
