using System;
using System.IO;
using System.Net;
using System.Text;
using TNUE_Patron_Excel.DBConnect;

namespace TNUE_Patron_Excel.API
{
	internal class AlephAPI
	{
		private Aleph aleph = null;

		private string strResponse;

		public AlephAPI()
		{
			aleph = new ReadWriterConfig().ReadConfigAleph();
		}

		public string Url(string xml)
		{
			string text = "http://catalog.dhsptn.edu.vn/X?op=update_bor&library=lsp50&update_flag=Y&xml_full_req=" + xml;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(aleph.UrlAleph);
			stringBuilder.Append("/X?op=update_bor&");
			stringBuilder.Append(aleph.Library);
			stringBuilder.Append("&");
			stringBuilder.Append(aleph.UpdateFlag);
			stringBuilder.Append("&xml_full_req=");
			stringBuilder.Append(xml);
			return RequestUrl(stringBuilder.ToString());
		}

		public string RequestUrl(string strURL)
		{
			string text = "";
			string text2 = "";
			try
			{
				WebRequest webRequest = WebRequest.Create(strURL);
				WebResponse response = webRequest.GetResponse();
				Stream responseStream = response.GetResponseStream();
				Encoding encoding = Encoding.GetEncoding("utf-8");
				StreamReader streamReader = new StreamReader(responseStream, encoding);
				strResponse = streamReader.ReadToEnd();
				text2 = GetBarcode(strURL);
				text = GetPatronID(strResponse);
				strResponse = GetError(strResponse);
				response.Close();
				webRequest.Abort();
			}
			catch (Exception ex)
			{
				strResponse = ex.Message;
			}
			return text2 + "\t" + text + "\n" + strResponse + "\n\n";
		}

		public string GetError(string str)
		{
			string result = "";
			string value = "<error>";
			string text = "</error>";
			if (str.Contains(text))
			{
				int num = str.IndexOf(value);
				int num2 = str.LastIndexOf(text);
				result = str.Substring(num, num2 - num + text.Length);
			}
			return result;
		}

		public string GetPatronID(string str)
		{
			string result = "";
			string text = "<patron-id>";
			string value = "</patron-id>";
			if (str.Contains(text))
			{
				int num = str.IndexOf(text);
				int num2 = str.IndexOf(value);
				result = str.Substring(num + text.Length, num2 - (num + text.Length));
			}
			return result;
		}

		public string GetBarcode(string str)
		{
			string result = "";
			string text = "<z308-key-data>";
			string value = "</z308-key-data>";
			if (str.Contains(text))
			{
				int num = str.LastIndexOf(text);
				int num2 = str.LastIndexOf(value);
				result = str.Substring(num + text.Length, num2 - (num + text.Length));
			}
			return result;
		}
	}
}
