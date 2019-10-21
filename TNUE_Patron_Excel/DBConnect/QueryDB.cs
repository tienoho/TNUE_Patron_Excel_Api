using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using TNUE_Patron_Excel.EntityLdapPatron;
using TNUE_Patron_Excel.Tool;

namespace TNUE_Patron_Excel.DBConnect
{
    internal class QueryDB
    {
        public List<Z308> listZ308TED()
        {
            List<Z308> list = new List<Z308>();
            string sql = "SELECT * FROM LSP00.Z308,LSP00.Z303 where Z308.Z308_ID=Z303.Z303_REC_KEY and Z308.Z308_REC_KEY like'01%'";
            try
            {
                using (OracleDataReader oracleDataReader = new DBConnecting().GetDataReader(sql))
                {
                    while (oracleDataReader.Read())
                    {
                        Z308 z = new Z308();
                        z.Z308_REC_KEY = oracleDataReader["Z308_REC_KEY"].ToString().Trim();
                        z.Z308_VERIFICATION = oracleDataReader["Z308_VERIFICATION"].ToString().Trim();
                        z.Z308_VERIFICATION_TYPE = oracleDataReader["Z308_VERIFICATION_TYPE"].ToString().Trim();
                        z.Z308_ID = oracleDataReader["Z308_ID"].ToString().Trim();
                        z.Z308_STATUS = oracleDataReader["Z308_STATUS"].ToString().Trim();
                        z.Z308_ENCRYPTION = oracleDataReader["Z303_NAME"].ToString().Trim();
                        z.Z303_NAME = oracleDataReader["Z303_NAME"].ToString().Trim();
                        z.Z303_BIRTH_DATE = oracleDataReader["Z303_BIRTH_DATE"].ToString().Trim();
                        z.Z303_FIELD_1 = oracleDataReader["Z303_FIELD_1"].ToString().Trim();
                        z.Z303_FIELD_2 = oracleDataReader["Z303_FIELD_2"].ToString().Trim();
                        z.Z303_FIELD_3 = oracleDataReader["Z303_FIELD_3"].ToString().Trim();
                        list.Add(z);
                    }
                    oracleDataReader.Close();
                    DBConnecting.conn.Close();
                }
            }
            catch
            {
            }
            return list;
        }
        public List<Z303Entity> listZ303()
        {
            List<Z303Entity> list = new List<Z303Entity>();
            string sql = "SELECT * FROM LSP00.Z303,LSP00.Z303 where Z308.Z308_ID=Z303.Z303_REC_KEY and Z308.Z308_REC_KEY like'01%'";
            try
            {
                using (OracleDataReader oracleDataReader = new DBConnecting().GetDataReader(sql))
                {
                    while (oracleDataReader.Read())
                    {
                        Z303Entity z = new Z303Entity();
                        //z.Z308_REC_KEY = oracleDataReader["Z308_REC_KEY"].ToString().Trim();
                        //z.Z308_VERIFICATION = oracleDataReader["Z308_VERIFICATION"].ToString().Trim();
                        //z.Z308_VERIFICATION_TYPE = oracleDataReader["Z308_VERIFICATION_TYPE"].ToString().Trim();
                        //z.Z308_ID = oracleDataReader["Z308_ID"].ToString().Trim();
                        //z.Z308_STATUS = oracleDataReader["Z308_STATUS"].ToString().Trim();
                        //z.Z308_ENCRYPTION = oracleDataReader["Z303_NAME"].ToString().Trim();
                        //z.Z308_UPD_TIME_STAMP = oracleDataReader["Z308_UPD_TIME_STAMP"].ToString().Trim();
                        list.Add(z);
                    }
                    oracleDataReader.Close();
                    DBConnecting.conn.Close();
                }
            }
            catch
            {
            }
            return list;
        }

        public int CountPatron()
        {
            int result = 0;
            string sql = "SELECT Z308_ID FROM (SELECT Z308_ID FROM LSP00.Z308 where Z308_ID like '000%' and Z308_REC_KEY like '01%' order by TO_NUMBER(Z308_ID) desc) WHERE ROWNUM <= 1";
            try
            {
                using (OracleDataReader oracleDataReader = new DBConnecting().GetDataReader(sql))
                {
                    while (oracleDataReader.Read())
                    {
                        result = int.Parse(oracleDataReader["Z308_ID"].ToString().Trim());
                    }
                    oracleDataReader.Close();
                    DBConnecting.conn.Close();
                }
            }
            catch
            {
                result = 0;
            }
            return result;
        }

        public bool CheckBarcode(string barcode)
        {
            bool result = false;
            string sql = "SELECT Z308_REC_KEY FROM LSP00.Z308 where Z308.Z308_REC_KEY like'01" + barcode + "%'";
            try
            {
                using (OracleDataReader oracleDataReader = new DBConnecting().GetDataReader(sql))
                {
                    if (oracleDataReader.Read())
                    {
                        result = true;
                    }
                    oracleDataReader.Close();
                    DBConnecting.conn.Close();
                }
            }
            catch
            {
                result = false;
            }
            return result;
        }
    }
}
