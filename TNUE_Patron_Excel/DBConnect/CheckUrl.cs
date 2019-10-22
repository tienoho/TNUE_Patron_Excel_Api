using System.Net;
using System.Net.Sockets;

namespace TNUE_Patron_Excel.DBConnect
{
    internal class CheckUrl
    {
        public bool CheckUrlExist(string url)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Timeout = 5000;
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
                //return false;
            }
            return false;
        }
        public bool PingHost(string hostUri, int portNumber)
        {
            try
            {
                using (var client = new TcpClient(hostUri, portNumber))
                    return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
