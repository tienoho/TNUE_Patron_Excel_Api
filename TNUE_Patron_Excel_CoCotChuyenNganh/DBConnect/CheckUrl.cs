using System.Net;

namespace TNUE_Patron_Excel.DBConnect
{
	internal class CheckUrl
	{
		public bool CheckUrlExist(string url)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.Timeout = 15000;
			httpWebRequest.Method = "HEAD";
			try
			{
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					return httpWebResponse.StatusCode == HttpStatusCode.OK;
				}
			}
			catch (WebException)
			{
				return false;
			}
		}
	}
}
