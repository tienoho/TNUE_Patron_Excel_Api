using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TNUE_Patron_Excel.Tool
{
	internal class ToolP
	{
		public string addNameKey(string name)
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

		public string formatDate(string str)
		{
			try
			{
				if (str == "" || str == null)
				{
					str = "19900101";
				}
				else
				{
					CultureInfo provider = new CultureInfo("vi-VN");
					str = DateTime.Parse(str, provider).ToString("yyyyMMdd");
				}
			}
			catch
			{
				str = "19900101";
			}
			return str;
		}

		public string getNgayHetHan(string str)
		{
			DateTime dateTime = DateTime.Parse(DateTime.Parse(DateTime.Now.ToString("dd/MM/yyyy")).AddYears(4).ToString("dd/MM/yyyy"));
			try
			{
				if (str == "" || str == null)
				{
					str = dateVn(dateTime.ToString());
				}
				else
				{
					CultureInfo provider = new CultureInfo("vi-VN");
					str = DateTime.Parse(str, provider).ToString("yyyyMMdd");
				}
			}
			catch
			{
				str = dateVn(dateTime.ToString());
			}
			return str;
		}

		public string dateVn(string str)
		{
			CultureInfo provider = new CultureInfo("vi-VN");
			str = DateTime.Parse(str, provider).ToString("yyyyMMdd");
			return str;
		}

		public string RemoveVietnameseMark(string str)
		{
			Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
			string input = str.Normalize(NormalizationForm.FormD);
			return regex.Replace(input, string.Empty).Replace('đ', 'd').Replace('Đ', 'D');
		}

		public string formatPatronId(string Id)
		{
			int num = Id.Count();
			if (num > 9)
			{
				Id = Id.Substring(Id.Length - 9);
			}
			return Id;
		}

		public string convertGender(string genDer)
		{
			genDer = genDer.ToUpper();
			genDer = ((!(genDer == "NAM")) ? "Ms." : "Mr.");
			return genDer;
		}

		public string bor_type(string str)
		{
			switch (str)
			{
			case "CD":
				return "AJ";
			case "CN":
				return "AG";
			case "CP":
				return "AZ";
			case "KE":
				return "AC";
			case "KT":
				return "AV";
			case "MT":
				return "AN";
			case "NH":
				return "AM";
			case "SN":
				return "BW";
			case "TY":
				return "BR";
			case "TH":
				return "AL";
			default:
				return "";
			}
		}

		public string getDate()
		{
			string text = DateTime.Now.Year.ToString();
			string text2 = DateTime.Now.Month.ToString();
			string text3 = DateTime.Now.Day.ToString();
			string text4 = DateTime.Now.Hour.ToString();
			string text5 = DateTime.Now.Minute.ToString();
			string text6 = DateTime.Now.Second.ToString();
			return "_" + text + "." + text2 + "." + text3 + "_" + text4 + "." + text5;
		}

		public string formatDatePassword(string str)
		{
			try
			{
				if (str == "" || str == null)
				{
					str = "01011990";
				}
				else
				{
					CultureInfo provider = new CultureInfo("vi-VN");
					str = DateTime.Parse(str, provider).ToString("ddMMyyyy");
				}
			}
			catch
			{
				str = "01011990";
			}
			return str;
		}
	}
}
